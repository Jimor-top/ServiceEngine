using ServiceEngine.Core.Service;
using ServiceEngine.Core;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Menu.Interface
{
    public interface ISysMenuService
    {
        Task<List<MenuOutput>?> GetSysMenuTreeAsync();
        Task<AdminResult<List<SysMenu>>> GetSysMenuListAsync(string title, MenuTypeEnum? type);
    }
}
