using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Server.Callers;
using ServiceEngineMasaCore.Blazor.Service.Server.Interface;

namespace ServiceEngineMasaCore.Blazor.Service.Server.Service
{
    public class SysServerService : BaseService, ISysServerService
    {
        private readonly IServerClient _client;

        public SysServerService(IServerClient client,IPopupService popup) : base(popup)
            => _client = client;

        public Task<AdminResult<dynamic>> GetSysServerAssemblyListAsync()
            => HandleErrorAsync(_client.GetSysServerAssemblyListAsync());

        public Task<AdminResult<dynamic>> GetSysServerBaseAsync()
            => HandleErrorAsync(_client.GetSysServerBaseAsync());

        public Task<AdminResult<dynamic>> GetSysServerDiskAsync()
            => HandleErrorAsync(_client.GetSysServerDiskAsync());

        public Task<AdminResult<dynamic>> GetSysServerUsedAsync()
          => HandleErrorAsync(_client.GetSysServerUsedAsync());
    }
}
