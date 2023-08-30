using WebApiClientCore.Attributes;
using WebApiClientCore;
using ServiceEngineMasaCore.Blazor.Extensions;
using ServiceEngine.Core.Const;

namespace ServiceEngineMasaCore.Blazor.JWTAuthentication
{

    internal class JwtAuthenticationAttribute : ApiFilterAttribute
    {
        public override async Task OnRequestAsync(ApiRequestContext context)
        {
            if (context.IsAllowAnonymous())
            {
                return;
            }

            var headers = context.HttpContext.RequestMessage.Headers;
            if (headers.Authorization is not null && headers.Authorization.Scheme != "Bearer")
            {
                return;
            }

            try
            {
                var storageService = context.HttpContext.ServiceProvider
                    .GetRequiredService<IJwtStorageService>();

                if (!string.IsNullOrWhiteSpace(storageService.AccessToken))
                {
                    headers.Authorization = new("Bearer", storageService.AccessToken);

                    if (storageService.IsAuthTokenExpiration(1) == true)
                    {
                        headers.TryAddWithoutValidation("X-Authorization", $"Bearer {storageService.RefreshToken}");
                    }
                }
            }
            catch { }

            await Task.CompletedTask;
        }

        public override async Task OnResponseAsync(ApiResponseContext context)
        {
            if (context.HttpContext.ResponseMessage is null)
            {
                return;
            }

            var headers = context.HttpContext.ResponseMessage.Headers;
            if (!headers.TryGetValues("access-token", out var accessTokens))
            {
                return;
            }

            try
            {
                var storageService = context.HttpContext.ServiceProvider
                    .GetRequiredService<IJwtStorageService>();

                if (accessTokens.Contains("invalid_token"))
                {
                    await storageService.ClearAsync();
                }
                else if (headers.TryGetValues("x-access-token", out var refreshTokens))
                {
                    await storageService.UpdateLocalStorageAsync(
                          (StorageConst.AccessToken, accessTokens.FirstOrDefault()),
                          (StorageConst.RefreshToken, refreshTokens.FirstOrDefault()));
                }
                else
                {
                    await storageService.UpdateLocalStorageAsync(
                          (StorageConst.AccessToken, accessTokens.FirstOrDefault()));
                }
            }
            catch { }
        }
    }
}
