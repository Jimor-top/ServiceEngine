using Microsoft.AspNetCore.Components.Authorization;

namespace ServiceEngineMasaCore.Blazor.JWTAuthentication
{
    public interface IJwtStorageService
    {
        Guid Id { get; }

        string? AccessToken { get; }

        DateTimeOffset? AuthTokenExpiration { get; }

        string? RefreshToken { get; }

        string? UserImageURL { get; }

        IDictionary<string, object?> Storage { get; }

        TValue GetStorageValue<TValue>(string key, TValue defaultValue);

        bool AddOrUpdateStorage<TValue>(string key, TValue value);

        Task<AuthenticationState> GetAuthenticationStateAsync();

        bool? IsAuthTokenExpiration(uint deadlineMinutes);

        Task UpdateLocalStorageAsync(params (string Key, string? Value)[] pairs);

        Task RemoveLocalStorageAsync(params string[] keys);

        Task ClearAsync();
    }
}
