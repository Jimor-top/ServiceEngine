using ServiceEngine.Core;

namespace ServiceEngineMasaCore.Blazor.Service.Server.Interface
{
    public interface ISysServerService
    {
        Task<AdminResult<dynamic>> GetSysServerBaseAsync();
        Task<AdminResult<dynamic>> GetSysServerUsedAsync();
        Task<AdminResult<dynamic>> GetSysServerDiskAsync();
        Task<AdminResult<dynamic>> GetSysServerAssemblyListAsync();

    }
}
