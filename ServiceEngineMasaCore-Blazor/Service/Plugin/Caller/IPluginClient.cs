using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;
using ServiceEngineMasaCore.Blazor.Service.Plugin.Dto;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Plugin.Caller
{
    [JwtAuthentication]
    public interface IPluginClient : IHttpApi
    {
        /// <summary>
        /// 获取机构列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysPlugin/page")]
        ITask<AdminResult<SqlSugarPagedList<SysPlugin>>> GetSysOPluginPageAsync([JsonContent] PPluginInput input, CancellationToken token = default);
    }
}
