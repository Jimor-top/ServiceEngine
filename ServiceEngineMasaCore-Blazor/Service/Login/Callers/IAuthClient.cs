using Microsoft.AspNetCore.Authorization;
using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Common;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Login.Callers
{
    [JwtAuthentication]
    public interface IAuthClient : IHttpApi
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="body"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("/api/sysAuth/login")]
        [AllowAnonymous]
        ITask<AdminResult<LoginOutput>> LoginAsync([JsonContent] LoginInput body, CancellationToken token = default);

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("/api/sysAuth/logout")]
        ITask<XnRestfulResult<string>> LogoutAsync(CancellationToken token = default);
    }
}
