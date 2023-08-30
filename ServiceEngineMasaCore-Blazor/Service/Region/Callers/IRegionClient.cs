using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;
using ServiceEngineMasaCore.Blazor.Service.Region.Dto;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Region.Callers
{
    [JwtAuthentication]
    public interface IRegionClient : IHttpApi
    {
        /// <summary>
        /// 获取机构分面
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysRegion/page")]
        ITask<AdminResult<SqlSugarPagedList<SysRegion>>> GetSysRegionPageAsync([JsonContent] PRegionInput input, CancellationToken token = default);

        /// <summary>
        /// 获取机构列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysRegion/list")]
        ITask<AdminResult<List<SysRegion>>> GetSysRegionListAsync(long Id, CancellationToken token = default);
    }
}
