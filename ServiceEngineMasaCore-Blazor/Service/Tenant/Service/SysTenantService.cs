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

        public Task<AdminResult<SqlSugarPagedList<TenantOutput>>> GetSysTenantPageAsync([JsonContent] PTenantInput input)
        => HandleErrorAsync(_client.GetSysTenantPageAsync(input));
    }
}
