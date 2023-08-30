using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Config.Dto;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Config.Interface
{
    public interface IConfigService
    {
        Task<AdminResult<SqlSugarPagedList<SysConfig>>> GetSysConfigPageAsync(PConfigInput input);
    }
}
