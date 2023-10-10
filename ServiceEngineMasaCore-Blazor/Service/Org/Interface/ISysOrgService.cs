using ServiceEngine.Core.Service;
using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Org.Callers;
using WebApiClientCore.Attributes;
using ServiceEngineMasaCore.Blazor.Service.Org.Dto;

namespace ServiceEngineMasaCore.Blazor.Service.Org.Interface
{
    public interface ISysOrgService
    {
        Task<AdminResult<List<SysOrg>>> GetSysOrgListAsync(long Id, string Name, string Code, string OrgType);
        Task<AdminResult<long>> AddSysOrgAsync(AddOrgInput orgInput);
        Task<AdminResult<string>> UpdateSysOrgAsync(UpdateOrgInput orgInput);
        Task<AdminResult<string>> DeleteSysOrgAsync(DltOrgInput orgInput);
    }
}
