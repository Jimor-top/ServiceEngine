using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.Menu.Interface;

namespace ServiceEngineMasaCore.Blazor.Service.Menu
{
    public class MenuStore : IMenuStore
    {
        List<MenuOutput> _menuStore = new List<MenuOutput>();
        public List<MenuOutput>? GetMenu()
        {
            if (_menuStore != null) { 
                return _menuStore;
            }
            return null;
        }

        public void SaveMenu(List<MenuOutput> menuOutputs)
        {
            _menuStore = menuOutputs;
        }
    }
}
