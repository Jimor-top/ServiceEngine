using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.Menu.Interface;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.App.PlatformManagement
{
    public partial class MenuManagement : IDisposable
    {
        [Inject]
        [NotNull]
        IPopupService? _popupService { get; set; }

        [Inject]
        [NotNull]
        ISysMenuService? _sysMenuService { get; set; }

        List<SysMenu> _sysMenuList = new List<SysMenu>();

        MenuInput input = new MenuInput();

        private bool _singleExpand;

        bool _isLoading = false;
        readonly List<DataTableHeader<SysMenu>> _headers = new List<DataTableHeader<SysMenu>>()
        {
            new (){ Text="",Value="data-table-expand",Sortable = false,Width="10%"},
            new() { Text = "菜单名称", Value = nameof(SysMenu.Title),Width="10%" },
            new() { Text = "类型", Value = nameof(SysMenu.Type) ,Width="10%"},
            new() { Text = "路由路径", Value = nameof(SysMenu.Path),Width="10%" },
            new() { Text = "组件路径", Value = nameof(SysMenu.Component) ,Width="15%"},
            new() { Text = "授权标识", Value = nameof(SysMenu.Permission) ,Width="10%"},
            new() { Text = "排序", Value = nameof(SysMenu.OrderNo),Width="10%"},
            new() { Text = "状态", Value = nameof(SysMenu.Status) ,Width="10%"},
            new() { Text = "修改时间", Value = nameof(SysMenu.UpdateTime) ,Width="10%"},
            new() { Text = "操作", Value = "Action", Sortable = false,Width="10%" }
        };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                _isLoading = true;
                _popupService.ShowProgressLinear();
                var res = await _sysMenuService.GetSysMenuListAsync(input.Title, input.Type);
                if (res != null && res.Result != null)
                {
                    _sysMenuList.AddRange(res.Result);
                }
                _popupService.HideProgressLinear();
                _isLoading = false;
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
        private void NavigationStyleChanged(object? sender, EventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            _sysMenuList.Clear();
        }
    }
}
