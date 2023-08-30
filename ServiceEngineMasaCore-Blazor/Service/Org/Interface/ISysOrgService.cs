using ServiceEngine.Core.Service;
using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Org.Callers;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Org.Interface
{
    public interface ISysOrgService
    {
        Task<AdminResult<List<SysOrg>>> GetSysOrgList(long Id, string Name, string Code, string OrgType);
        Task<AdminResult<long>> AddSysOrg(AddOrgInput orgInput);
        Task<AdminResult<string>> UpdateSysOrg(AddOrgInput orgInput);
        Task<AdminResult<string>> DeleteSysOrg(AddOrgInput orgInput);
    }
}
