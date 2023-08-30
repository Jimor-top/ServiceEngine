using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.Org.Callers;
using ServiceEngineMasaCore.Blazor.Service.Org.Interface;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Callers;

namespace ServiceEngineMasaCore.Blazor.Service.Org.Service
{
    public class SysOrgService : BaseService, ISysOrgService
    {
        private readonly ISysOrgClient _client;

        public SysOrgService(ISysOrgClient client, IPopupService popup) : base(popup)
            => _client = client;

        public Task<AdminResult<long>> AddSysOrg(AddOrgInput orgInput)
          => HandleErrorAsync(_client.AddSysOrg(orgInput));

        public Task<AdminResult<string>> DeleteSysOrg(AddOrgInput orgInput)
          => HandleErrorAsync(_client.DeleteSysOrg(orgInput));

        public Task<AdminResult<List<SysOrg>>> GetSysOrgList(long Id, string Name, string Code, string OrgType)
           => HandleErrorAsync(_client.GetSysOrgList(Id, Name, Code, OrgType));

        public Task<AdminResult<string>> UpdateSysOrg(AddOrgInput orgInput)
        => HandleErrorAsync(_client.UpdateSysOrg(orgInput));
    }
}
