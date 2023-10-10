using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.Role.Callers;
using ServiceEngineMasaCore.Blazor.Service.Role.Dto;
using ServiceEngineMasaCore.Blazor.Service.Role.Interface;
using ServiceEngineMasaCore.Blazor.Service.Tenant.Callers;
using ServiceEngineMasaCore.Blazor.Service.Tenant.Dto;
using ServiceEngineMasaCore.Blazor.Service.Tenant.Interface;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Tenant.Service
{
    public class SysTenantService : BaseService, ISysTenantService
    {
        private readonly ITenantClient _client;

        public SysTenantService(ITenantClient client, IPopupService popup) : base(popup)
            => _client = client;

        public Task<AdminResult<object>> AddSysTenantAsync(AddTenantInput input)
            => HandleErrorAsync(_client.AddSysTenantAsync(input));

        public Task<AdminResult<object>> CreateSysTenantDbAsync(TTenantInput input)
            => HandleErrorAsync(_client.CreateSysTenantDbAsync(input));

        public Task<AdminResult<object>> DeleteSysTenantAsync(DeleteTenantInput input)
            => HandleErrorAsync(_client.DeleteSysTenantAsync(input));

        public Task<AdminResult<List<long>>> GetSysTenantMenuListAsync(TenantUserInput input)
            => HandleErrorAsync(_client.GetSysTenantMenuListAsync(input));

        public Task<AdminResult<SqlSugarPagedList<TenantOutput>>> GetSysTenantPageAsync([JsonContent] PTenantInput input)
            => HandleErrorAsync(_client.GetSysTenantPageAsync(input));

        public Task<AdminResult<object>> ResetPwdSysTenantAsync(TenantUserInput input)
            => HandleErrorAsync(_client.ResetPwdSysTenantAsync(input));

        public Task<AdminResult<int>> SetSysTenantStatusAsync([JsonContent] TTenantInput input)
            => HandleErrorAsync(_client.SetSysTenantStatusAsync(input));

        public Task<AdminResult<object>> UpdateSysTenantAsync(UpdateTenantInput input)
            => HandleErrorAsync(_client.UpdateSysTenantAsync(input));
    }
}
