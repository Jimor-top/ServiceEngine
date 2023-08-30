using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.File.Dto;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.File.Interface
{
    public interface ISysFileService
    {
        Task<AdminResult<SqlSugarPagedList<SysFile>>> GetSysFilePageAsync(PFileInput input);
    }
}
