using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Print.Dto;

namespace ServiceEngineMasaCore.Blazor.Service.Print.Interface
{
    public interface ISysPrintService
    {
        Task<AdminResult<SqlSugarPagedList<SysPrint>>> GetSysPrintPageAsync(PPrintInput input);
    }
}
