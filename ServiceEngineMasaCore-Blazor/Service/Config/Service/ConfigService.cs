using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Config.Callers;
using ServiceEngineMasaCore.Blazor.Service.Config.Dto;
using ServiceEngineMasaCore.Blazor.Service.Config.Interface;
using ServiceEngineMasaCore.Blazor.Service.Menu.Callers;
using ServiceEngineMasaCore.Blazor.Service.Menu.Interface;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Config.Service
{
    public class ConfigService : BaseService, IConfigService
    {
        private readonly IConfigClient _client;
        public ConfigService(IConfigClient client, IPopupService popup) : base(popup)
          => _client = client;

        public Task<AdminResult<SqlSugarPagedList<SysConfig>>> GetSysConfigPageAsync(PConfigInput input)
        => HandleErrorAsync(_client.GetSysConfigPageAsync(input));
    }
}
