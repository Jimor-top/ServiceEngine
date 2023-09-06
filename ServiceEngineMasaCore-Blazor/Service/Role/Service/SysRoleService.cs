using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.Role.Callers;
using ServiceEngineMasaCore.Blazor.Service.Role.Dto;
using ServiceEngineMasaCore.Blazor.Service.Role.Interface;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Callers;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Role.Service
{
    public class SysRoleService : BaseService, ISysRoleService
    {
        private readonly IRoleClient _client;

        public SysRoleService(IRoleClient client, IPopupService popup) : base(popup)
            => _client = client;

        public Task<AdminResult<object>> AddSysRoleAsync([JsonContent] AddRoleInput input)
            => HandleErrorAsync(_client.AddSysRoleAsync(input));

        public Task<AdminResult<object>> DeleteSysRoleAsync([JsonContent] DltRoleInput input)
            => HandleErrorAsync(_client.DeleteSysRoleAsync(input));

        public Task<AdminResult<List<long>>> GetOwnMenuListAsync(long Id)
            => HandleErrorAsync(_client.GetOwnMenuListAsync(Id));

        public Task<AdminResult<List<long>>> GetOwnOrgListAsync(long Id)
               => HandleErrorAsync(_client.GetOwnOrgListAsync(Id));

        public Task<AdminResult<List<RoleOutput>>> GetSysRoleListAsync()
            => HandleErrorAsync(_client.GetSysRoleListAsync());

        public Task<AdminResult<SqlSugarPagedList<SysRole>>> GetSysRolePageAsync([JsonContent] PRoleInput input)
            => HandleErrorAsync(_client.GetSysRolePageAsync(input));

        public Task<AdminResult<int>> SetSysRoleStatusAsync([JsonContent] StatusRoleInput input)
            => HandleErrorAsync(_client.SetSysRoleStatusAsync(input));

        public Task<AdminResult<object>> SysRoleGrantDataScopeAsync([JsonContent] DataScopeRoleInput input, CancellationToken token = default)
        => HandleErrorAsync(_client.SysRoleGrantDataScopeAsync(input));

        public Task<AdminResult<object>> UpdateSysRoleAsync([JsonContent] UpdateRoleInput input)
            => HandleErrorAsync(_client.UpdateSysRoleAsync(input));
    }
}
