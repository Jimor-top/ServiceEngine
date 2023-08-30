using ServiceEngine.Core.Service;
using ServiceEngine.Core;
using WebApiClientCore.Attributes;
using ServiceEngineMasaCore.Blazor.Service.Tenant.Dto;

namespace ServiceEngineMasaCore.Blazor.Service.Tenant.Interface
{
    public interface ISysTenantService
    {
        Task<AdminResult<SqlSugarPagedList<TenantOutput>>> GetSysTenantPageAsync([JsonContent] PTenantInput input);
    }
}
