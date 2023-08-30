using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;
using ServiceEngineMasaCore.Blazor.Service.Login.Callers;
using ServiceEngineMasaCore.Blazor.Service.Login.Interface;

namespace ServiceEngineMasaCore.Blazor.Service.Login.Service
{
    public class SysAuthService : BaseService, ISysAuthService
    {
        private readonly RouterPagesProvider _routers;
        private readonly IAuthClient _client;
        public SysAuthService(IAuthClient client, IPopupService popup, RouterPagesProvider routers) : base(popup)
            => (_client, _routers) = (client, routers);

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<AdminResult<LoginOutput>> LoginAsync(LoginInput input)
            => HandleErrorAsync(_client.LoginAsync(input));

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>
        public async Task LogoutAsync() => await _client.LogoutAsync();
    }
}
