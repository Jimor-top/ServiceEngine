using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;
using WebApiClientCore;
using WebApiClientCore.Attributes;
using ServiceEngineMasaCore.Blazor.Service.Config.Dto;

namespace ServiceEngineMasaCore.Blazor.Service.Config.Callers
{
    [JwtAuthentication]
    public interface IConfigClient : IHttpApi
    {
        /// <summary>
        /// 获取登录账号
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysConfig/page")]
        ITask<AdminResult<SqlSugarPagedList<SysConfig>>> GetSysConfigPageAsync([JsonContent] PConfigInput input,CancellationToken token = default);
    }
}
