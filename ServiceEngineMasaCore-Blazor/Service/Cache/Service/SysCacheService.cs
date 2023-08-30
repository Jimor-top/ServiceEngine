using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Cache.Callers;
using ServiceEngineMasaCore.Blazor.Service.Cache.Interface;
using ServiceEngineMasaCore.Blazor.Service.Dict.Callers;
using ServiceEngineMasaCore.Blazor.Service.Dict.Interface;

namespace ServiceEngineMasaCore.Blazor.Service.Cache.Service
{
    public class SysCacheService : BaseService, ISysCacheService
    {
        private readonly ICacheClient _client;
        public SysCacheService(ICacheClient client, IPopupService popup) : base(popup)
          => _client = client;

        public Task<AdminResult<List<string>>> GetSysCachePageAsync()
            => HandleErrorAsync(_client.GetSysCachePageAsync());

        public Task<AdminResult<object>> GetSysCacheValueAsync(string key)
            => HandleErrorAsync(_client.GetSysCacheValueAsync(key));

    }
}
