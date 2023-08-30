using Newtonsoft.Json;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Callers;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Shared
{
    public partial class Login
    {
        [Inject]
        [NotNull]
        IJwtStorageService? _StorageService { get; set; }

        [Inject]
        [NotNull]
        IUserClient? _IUserClient { get; set; }

        bool _dialog = false;
        bool _loading = false;
        private void LogoutDialog() {
            _dialog = true;
        }
        private async Task LogoutSubmit() {
            _loading = true;
            try
            {
                var res = await _IUserClient.SysAuthUserlogoutAsync();
                if (res.Type == "success")
                {
                    await _StorageService.ClearAsync();
                }
            }
            catch
            {
                await _StorageService.ClearAsync();
                _loading = false;
            }
        }
    }
}
