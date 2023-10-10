using ServiceEngine.Core.Service;
using ServiceEngine.Core;
using WebApiClientCore.Attributes;
using ServiceEngineMasaCore.Blazor.Service.Tenant.Dto;

namespace ServiceEngineMasaCore.Blazor.Service.Tenant.Interface
{
    public interface ISysTenantService
    {
        Task<AdminResult<SqlSugarPagedList<TenantOutput>>> GetSysTenantPageAsync([JsonContent] PTenantInput input);
        Task<AdminResult<int>> SetSysTenantStatusAsync([JsonContent] TTenantInput input);
        Task<AdminResult<List<long>>> GetSysTenantMenuListAsync(TenantUserInput input);
        Task<AdminResult<object>> ResetPwdSysTenantAsync(TenantUserInput input);
        Task<AdminResult<object>> AddSysTenantAsync(AddTenantInput input);
        Task<AdminResult<object>> DeleteSysTenantAsync(DeleteTenantInput input);
        Task<AdminResult<object>> UpdateSysTenantAsync(UpdateTenantInput input);
        Task<AdminResult<object>> CreateSysTenantDbAsync(TTenantInput input);
    }
}
