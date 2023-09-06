using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Log.Dto;
using ServiceEngineMasaCore.Blazor.Service.Log.Interface;
using ServiceEngineMasaCore.Blazor.Service.Role.Dto;
using ServiceEngineMasaCore.Blazor.Service.Role.Interface;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Dto;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.App.SystemManagement
{
    public partial class RoleManagement : IDisposable
    {
        [Inject]
        [NotNull]
        IPopupService? _popupService { get; set; }

        [Inject]
        [NotNull]
        ISysRoleService? _sysRoleService { get; set; }

        List<SysRole> _sysRoleList = new List<SysRole>();

        PRoleInput input = new PRoleInput();

        int _tatolCount = 0;
        int _tatolPage = 1;
        int _currentPage = 1;
        private string _paginationSelect = "10";

        bool _isLoading = false;
        readonly List<DataTableHeader<SysRole>> _headers = new List<DataTableHeader<SysRole>>()
        {
            new() { Text = "序号", Value = nameof(SysRole.Index) },
            new() { Text = "角色名称", Value = nameof(SysRole.Name) },
            new() { Text = "角色编码", Value = nameof(SysRole.Code) },
            new() { Text = "数据范围", Value = nameof(SysRole.DataScope) },
            new() { Text = "排序", Value = nameof(SysRole.OrderNo) },
            new() { Text = "状态", Value = nameof(SysRole.Status) },
            new() { Text = "修改时间", Value = nameof(SysRole.UpdateTime)},
            new() { Text = "备注", Value = nameof(SysRole.Remark) },
        };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                input = new PRoleInput()
                {
                    Page = 1,
                    PageSize = int.Parse(_paginationSelect),
                };
                _isLoading = true;
                _popupService.ShowProgressLinear();
                await LoadData(input);
                _popupService.HideProgressLinear();
                _isLoading = false;
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task LoadData(PRoleInput input)
        {
            var res = await _sysRoleService.GetSysRolePageAsync(input);
            if (res != null && res.Result?.Items != null)
            {
                _tatolPage = res.Result.TotalPages == 0 ? 1 : res.Result.TotalPages;
                _tatolCount = res.Result.Total;
                _sysRoleList = res.Result.Items.ToList();
                _sysRoleList = _sysRoleList.Select((item, index) => {
                    item.Index = index + 1;
                    return item;
                }).ToList();
            }
        }

        private void NavigationStyleChanged(object? sender, EventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }
        private async Task OnPaginationValueChange(int value)
        {
            _currentPage = value;
            input = new PRoleInput()
            {
                Page = value,
                PageSize = int.Parse(_paginationSelect),
            };
            await LoadData(input);
        }
        private async Task OnSelectValueChange(string value)
        {
            _paginationSelect = value;
            _currentPage = 1;
            input = new PRoleInput()
            {
                Page = 1,
                PageSize = int.Parse(_paginationSelect),
            };
            await LoadData(input);
        }
        public void Dispose()
        {
            _sysRoleList.Clear();
        }
    }
}
