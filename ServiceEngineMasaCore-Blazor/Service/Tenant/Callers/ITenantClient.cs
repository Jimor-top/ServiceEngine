using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;
using ServiceEngineMasaCore.Blazor.Service.Tenant.Dto;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Tenant.Callers
{
    [JwtAuthentication]
    public interface ITenantClient : IHttpApi
    {
        /// <summary>
        /// 获取机构列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysTenant/page")]
        ITask<AdminResult<SqlSugarPagedList<TenantOutput>>> GetSysTenantPageAsync([JsonContent] PTenantInput input, CancellationToken token = default);
    }
}
