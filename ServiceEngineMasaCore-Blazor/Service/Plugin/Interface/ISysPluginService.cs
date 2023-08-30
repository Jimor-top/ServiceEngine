using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Plugin.Dto;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Plugin.Interface
{
    public interface ISysPluginService
    {
        Task<AdminResult<SqlSugarPagedList<SysPlugin>>> GetSysOPluginPageAsync(PPluginInput input);
    }
}
