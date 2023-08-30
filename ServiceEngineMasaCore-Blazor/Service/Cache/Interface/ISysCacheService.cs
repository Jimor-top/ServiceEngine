using ServiceEngine.Core;

namespace ServiceEngineMasaCore.Blazor.Service.Cache.Interface
{
    public interface ISysCacheService
    {
        Task<AdminResult<List<string>>> GetSysCachePageAsync();
        Task<AdminResult<object>> GetSysCacheValueAsync(string key);
    }
}
