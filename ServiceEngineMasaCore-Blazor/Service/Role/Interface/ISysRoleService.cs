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
    }
}
