using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;
using ServiceEngineMasaCore.Blazor.Service.Role.Dto;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Role.Callers
{
    [JwtAuthentication]
    public interface IRoleClient : IHttpApi
    {
        /// <summary>
        /// 获取机构列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysRole/page")]
        ITask<AdminResult<SqlSugarPagedList<SysRole>>> GetSysRolePageAsync([JsonContent] PRoleInput input, CancellationToken token = default);
    }
}
