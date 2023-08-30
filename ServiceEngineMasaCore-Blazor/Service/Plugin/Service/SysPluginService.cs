using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.Org.Callers;
using ServiceEngineMasaCore.Blazor.Service.Plugin.Caller;
using ServiceEngineMasaCore.Blazor.Service.Plugin.Dto;
using ServiceEngineMasaCore.Blazor.Service.Plugin.Interface;

namespace ServiceEngineMasaCore.Blazor.Service.Plugin.Service
{
    public class SysPluginService : BaseService, ISysPluginService
    {
        private readonly IPluginClient _client;

        public SysPluginService(IPluginClient client, IPopupService popup) : base(popup)
            => _client = client;

        public Task<AdminResult<SqlSugarPagedList<SysPlugin>>> GetSysOPluginPageAsync(PPluginInput input)
            => HandleErrorAsync(_client.GetSysOPluginPageAsync(input));
    }
}
