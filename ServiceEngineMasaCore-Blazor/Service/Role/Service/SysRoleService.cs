using ServiceEngine.Core;
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

        public Task<AdminResult<SqlSugarPagedList<SysRole>>> GetSysRolePageAsync([JsonContent] PRoleInput input)
         => HandleErrorAsync(_client.GetSysRolePageAsync(input));
    }
}
