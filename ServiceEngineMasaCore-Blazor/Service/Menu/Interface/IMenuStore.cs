using ServiceEngine.Core.Service;

namespace ServiceEngineMasaCore.Blazor.Service.Menu.Interface
{
    public interface IMenuStore
    {
        public List<MenuOutput>? GetMenu();
        public void SaveMenu(List<MenuOutput> menuOutputs);
    }
}
