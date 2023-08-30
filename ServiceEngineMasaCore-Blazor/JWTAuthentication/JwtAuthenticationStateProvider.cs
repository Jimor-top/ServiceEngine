using Blazored.LocalStorage;
using Furion.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using ServiceEngine.Core.Const;
using ServiceEngineMasaCore.Blazor.Extensions;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace ServiceEngineMasaCore.Blazor.JWTAuthentication
{

    public class JwtAuthenticationStateProvider : AuthenticationStateProvider,
        IJwtStorageService, IJSInvokerService
    {
        private static readonly AuthenticationState s_anonymous = new(new(new ClaimsIdentity()));

        private readonly Guid _id;
        private readonly HubConnection _hubConnection;

        [NotNull]
        private readonly ILocalStorageService _storage;
        private readonly ConcurrentDictionary<string, object?> _cache = new();

        private string? _authToken;
        private string? _refreshToken;
        private string? _userImageURL;
        public DateTimeOffset? _authTokenExpiration;
        private AuthenticationState? _authenticationState;

        [Inject]
        [NotNull]
        private IJSInvoker? _invoker { get; set; }
        private bool _signalRInitialized;
        private Func<HubConnection, Task>? _signalRInitializer;

        public JwtAuthenticationStateProvider(
            IJSInvoker? invoker,
            ILocalStorageService storage,
            NavigationManager navigation)
        {
            _id = Guid.NewGuid();
            _invoker = invoker;
            _storage = storage;

            var hubUrl = navigation.ToAbsoluteUri("");
            _hubConnection = new HubConnectionBuilder()
                    .WithUrl(hubUrl, x => x.AccessTokenProvider = GetAuthToken)
                    .WithAutomaticReconnect()
                    .Build();
        }

        public Guid Id => _id;

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
            => Task.FromResult(_authenticationState ?? s_anonymous);

        async Task IJSInvokerService.ConnectAsync(IJSInvoker? invoker, Func<HubConnection, Task>? signalRInitializer)
        {
            if (invoker is null)
            {
                throw new NullReferenceException("The connection from invoker to JavaScript was not established.");
            }

            _invoker = invoker;
            _signalRInitializer = signalRInitializer;

            // load token
            _authToken = await _storage.GetItemAsStringAsync(StorageConst.AccessToken);
            _refreshToken = await _storage.GetItemAsStringAsync(StorageConst.RefreshToken);
            _userImageURL = await _storage.GetItemAsStringAsync(StorageConst.UserImageURL);

            UpdateAuthenticationState();
        }

        Task IJSInvokerService.DisconnectAsync()
        {
            _invoker = null;
            return Task.CompletedTask;
        }

        string? IJwtStorageService.AccessToken => _authToken;

        string? IJwtStorageService.RefreshToken => _refreshToken;

        string? IJwtStorageService.UserImageURL => _userImageURL;

        DateTimeOffset? IJwtStorageService.AuthTokenExpiration => _authTokenExpiration;

        IDictionary<string, object?> IJwtStorageService.Storage => _cache;

        public TValue GetStorageValue<TValue>(string key, TValue defaultValue)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return defaultValue;
            }

            if (_cache.TryGetValue(key, out object? obj))
            {
                if (obj is not null && obj is TValue value)
                {
                    return value;
                }
            }

            return defaultValue;
        }

        public bool AddOrUpdateStorage<TValue>(string key, TValue value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            _cache.AddOrUpdate(key, value, UpdateValueFactory);

            return true;
        }

        bool? IJwtStorageService.IsAuthTokenExpiration(uint deadlineMinutes)
        {
            if (_authTokenExpiration is null)
            {
                return null;
            }

            var diff = _authTokenExpiration.Value - DateTime.UtcNow;
            return diff.TotalMinutes <= deadlineMinutes;
        }

        async Task IJwtStorageService.UpdateLocalStorageAsync(params (string Key, string? Value)[] pairs)
        {
            if (pairs.Length > 0)
            {
                var callbacks = pairs
                    .Where(x => !string.IsNullOrWhiteSpace(x.Key) && !string.IsNullOrWhiteSpace(x.Value))
                    .Select(x => new Func<ValueTask>(() => _storage.SetItemAsStringAsync(x.Key, x.Value)))
                    .ToArray();

                await _invoker!.ExecuteAsync(callbacks);

                if (TryGetValue(pairs, StorageConst.RefreshToken, out var refreshToken))
                {
                    _refreshToken = refreshToken;
                }

                if (TryGetValue(pairs, StorageConst.UserImageURL, out var userImageURL))
                {
                    _userImageURL = userImageURL;
                }

                if (TryGetValue(pairs, StorageConst.AccessToken, out var token))
                {
                    _authToken = token;
                    if (UpdateAuthenticationState())
                    {
                        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                    }
                }
            }
        }

        async Task IJwtStorageService.RemoveLocalStorageAsync(params string[] keys)
        {
            if (keys.Length > 0)
            {
                await _invoker!.ExecuteAsync(() => _storage.RemoveItemsAsync(keys));

                if (keys.Contains(StorageConst.RefreshToken))
                {
                    _refreshToken = null;
                }

                if (keys.Contains(StorageConst.UserImageURL))
                {
                    _userImageURL = null;
                }

                if (keys.Contains(StorageConst.AccessToken))
                {
                    _authToken = null;
                    if (UpdateAuthenticationState())
                    {
                        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                    }
                }
            }
        }

        public async Task ClearAsync(bool isAfterRender)
        {
            if (isAfterRender)
            {
                await _storage.RemoveItemsAsync(new string[] {
                StorageConst.AccessToken,
                StorageConst.RefreshToken,
                StorageConst.UserImageURL
            });
            }
            else
            {
                await _invoker!.ExecuteAsync(
                    () => _storage.RemoveItemsAsync(
                        new string[] {
                    StorageConst.AccessToken,
                    StorageConst.RefreshToken,
                    StorageConst.UserImageURL
                        })
                    );
            }

            _authToken = null;
            _refreshToken = null;
            _userImageURL = null;
            _cache.Clear();

            if (UpdateAuthenticationState())
            {
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
        }

        Task IJwtStorageService.ClearAsync() => ClearAsync(false);




        private bool UpdateAuthenticationState()
        {
            DateTimeOffset? authTokenExpiration = null;
            AuthenticationState authenticationState = s_anonymous;
            try
            {
                if (!string.IsNullOrWhiteSpace(_authToken))
                {
                    var identity = new ClaimsIdentity(GetClaimsFromJwt(_authToken), "jwt");
                    var user = new ClaimsPrincipal(identity);
                    var exp = user.FindFirst(c => c.Type.Equals("exp"))?.Value;
                    if (!string.IsNullOrWhiteSpace(exp) && long.TryParse(exp, out var unixTimeSeconds))
                    {
                        authTokenExpiration = DateTimeOffset.FromUnixTimeSeconds(unixTimeSeconds);
                    }
                    authenticationState = new AuthenticationState(user);
                }
            }
            catch
            {
                authenticationState = s_anonymous;
                authTokenExpiration = null;
            }

            var oldUser = _authenticationState?.User;
            var newUser = authenticationState.User;
            var changed =
                oldUser?.Identity?.IsAuthenticated != newUser.Identity?.IsAuthenticated ||
                oldUser?.GetUserId() != newUser.GetUserId();

            _authenticationState = authenticationState;
            _authTokenExpiration = authTokenExpiration;

            //第一次认证成功，则执行 SignalR初始器
            if (!_signalRInitialized && _authenticationState?.User.Identity?.IsAuthenticated == true)
            {
                _signalRInitialized = true;
                if (_signalRInitializer is not null)
                {
                    _signalRInitializer(_hubConnection);
                }
            }

            return changed;
        }

        private Task<string?> GetAuthToken() => Task.FromResult(_authToken);

        private static IEnumerable<Claim> GetClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            if (keyValuePairs != null)
            {
                keyValuePairs.TryGetValue(ClaimTypes.Role, out var roles);
                keyValuePairs.TryGetValue(ApplicationClaimTypes.Permission, out var permissions);

                var roleValues = roles?.ToString()?.Trim();
                var permissionValues = permissions?.ToString()?.Trim();

                if (!string.IsNullOrWhiteSpace(roleValues))
                {
                    if (roleValues.StartsWith("["))
                    {
                        var parsedRoles = JsonSerializer.Deserialize<string[]>(roleValues);
                        if (parsedRoles?.Length > 0)
                        {
                            claims.AddRange(parsedRoles.Select(role => new Claim(ClaimTypes.Role, role)));
                        }
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, roleValues));
                    }

                    keyValuePairs.Remove(ClaimTypes.Role);
                }

                if (!string.IsNullOrWhiteSpace(permissionValues))
                {
                    if (permissionValues.StartsWith("["))
                    {
                        var parsedPermissions = JsonSerializer.Deserialize<string[]>(permissionValues);
                        if (parsedPermissions?.Length > 0)
                        {
                            claims.AddRange(parsedPermissions.Select(permission => new Claim(ApplicationClaimTypes.Permission, permission)));
                        }
                    }
                    else
                    {
                        claims.Add(new Claim(ApplicationClaimTypes.Permission, permissionValues));
                    }
                    keyValuePairs.Remove(ApplicationClaimTypes.Permission);
                }

                claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value?.ToString() ?? string.Empty)));
            }
            return claims;
        }

        private static byte[] ParseBase64WithoutPadding(string payload)
        {
            payload = payload.Trim().Replace('-', '+').Replace('_', '/');
            var base64 = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
            return Convert.FromBase64String(base64);
        }

        private static bool TryGetValue((string Key, string? Value)[] pairs, string key, out string? value)
        {
            foreach (var (Key, Value) in pairs)
            {
                if (Key == key)
                {
                    value = Value;
                    return true;
                }
            }

            value = null;
            return false;
        }

        private static TValue UpdateValueFactory<TKey, TValue>(TKey key, TValue value) => value;

    }
}
