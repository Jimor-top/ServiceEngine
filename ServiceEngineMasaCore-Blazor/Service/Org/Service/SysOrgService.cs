using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.Org.Callers;
using ServiceEngineMasaCore.Blazor.Service.Org.Dto;
using ServiceEngineMasaCore.Blazor.Service.Org.Interface;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Callers;

namespace ServiceEngineMasaCore.Blazor.Service.Org.Service
{
    public class SysOrgService : BaseService, ISysOrgService
    {
        private readonly ISysOrgClient _client;

        public SysOrgService(ISysOrgClient client, IPopupService popup) : base(popup)
            => _client = client;

        public Task<AdminResult<long>> AddSysOrgAsync(AddOrgInput orgInput)
          => HandleErrorAsync(_client.AddSysOrgAsync(orgInput));

        public Task<AdminResult<string>> DeleteSysOrgAsync(DltOrgInput orgInput)
          => HandleErrorAsync(_client.DeleteSysOrgAsync(orgInput));

        public Task<AdminResult<List<SysOrg>>> GetSysOrgListAsync(long Id, string Name, string Code, string OrgType)
           => HandleErrorAsync(_client.GetSysOrgListAsync(Id, Name, Code, OrgType));

        public Task<AdminResult<string>> UpdateSysOrgAsync(UpdateOrgInput orgInput)
        => HandleErrorAsync(_client.UpdateSysOrgAsync(orgInput));
    }
}
