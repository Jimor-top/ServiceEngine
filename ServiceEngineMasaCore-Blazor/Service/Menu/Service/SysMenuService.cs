using ServiceEngine.Core.Service;
using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Menu.Callers;
using ServiceEngineMasaCore.Blazor.Service.Menu.Interface;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Callers;

namespace ServiceEngineMasaCore.Blazor.Service.Menu.Service
{
    public class SysMenuService : BaseService, ISysMenuService
    {
        private readonly IMenuClient _client;
        public SysMenuService(IMenuClient client, IPopupService popup) : base(popup)
          => _client = client;

        public Task<AdminResult<List<SysMenu>>> GetSysMenuListAsync(string title, MenuTypeEnum? type)
           => HandleErrorAsync(_client.GetSysMenuListAsync(title,type));

        public Task<List<MenuOutput>?> GetSysMenuTreeAsync() 
            => QueryAsync(_client.GetSysMenuTreeAsync());


    }
}
