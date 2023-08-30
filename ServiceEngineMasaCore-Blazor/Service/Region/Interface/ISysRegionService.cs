using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Region.Dto;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Region.Interface
{
    public interface ISysRegionService
    {
        Task<AdminResult<SqlSugarPagedList<SysRegion>>> GetSysRegionPageAsync(PRegionInput input);
        Task<AdminResult<List<SysRegion>>> GetSysRegionListAsync(long Id);
    }
}
