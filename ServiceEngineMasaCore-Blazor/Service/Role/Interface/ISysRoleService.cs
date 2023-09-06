using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.Role.Dto;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Role.Interface
{
    public interface ISysRoleService
    {
        Task<AdminResult<SqlSugarPagedList<SysRole>>> GetSysRolePageAsync([JsonContent] PRoleInput input);
        Task<AdminResult<List<RoleOutput>>> GetSysRoleListAsync();
        Task<AdminResult<object>> SysRoleGrantDataScopeAsync([JsonContent] DataScopeRoleInput input, CancellationToken token = default);
        Task<AdminResult<int>> SetSysRoleStatusAsync([JsonContent] StatusRoleInput input);
        Task<AdminResult<object>> AddSysRoleAsync([JsonContent] AddRoleInput input);
        Task<AdminResult<object>> UpdateSysRoleAsync([JsonContent] UpdateRoleInput input);
        Task<AdminResult<object>> DeleteSysRoleAsync([JsonContent] DltRoleInput input);
        Task<AdminResult<List<long>>> GetOwnOrgListAsync(long Id);
        Task<AdminResult<List<long>>> GetOwnMenuListAsync(long Id);
    }
}
