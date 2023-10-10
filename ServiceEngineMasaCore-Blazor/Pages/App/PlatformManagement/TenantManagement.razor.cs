using Nest;
using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.Log.Dto;
using ServiceEngineMasaCore.Blazor.Service.Role.Dto;
using ServiceEngineMasaCore.Blazor.Service.Role.Interface;
using ServiceEngineMasaCore.Blazor.Service.Tenant.Dto;
using ServiceEngineMasaCore.Blazor.Service.Tenant.Interface;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.App.PlatformManagement
{
    public partial class TenantManagement : IDisposable
    {
        [Inject]
        [NotNull]
        IPopupService? _popupService { get; set; }

        [Inject]
        [NotNull]
        ISysTenantService? _sysTenantService { get; set; }

        List<TenantOutput> _sysTenantList = new List<TenantOutput>();
        PTenantInput input = new PTenantInput();

        string _name = string.Empty;
        string _phone = string.Empty;

        int _tatolCount = 0;
        int _tatolPage = 1;
        int _currentPage = 1;
        private string _paginationSelect = "10";

        private bool _dialog { get; set; }
        private string _dialogTitle { get; set; } = string.Empty;

        UpdateTenantInput updateTenantInput = new UpdateTenantInput();

        bool _isLoading = false;
        readonly List<DataTableHeader<TenantOutput>> _headers = new List<DataTableHeader<TenantOutput>>()
        {
            new() { Text = "序号", Value = nameof(TenantOutput.Index) },
            new() { Text = "租户名称", Value = nameof(TenantOutput.Name) },
            new() { Text = "租户账户", Value = nameof(TenantOutput.AdminAccount) },
            new() { Text = "电话", Value = nameof(TenantOutput.Phone) },
            new() { Text = "租户类型", Value = nameof(TenantOutput.TenantType) },
            new() { Text = "状态", Value = nameof(TenantOutput.Status) },
            new() { Text = "数据库类型", Value = nameof(TenantOutput.DbType)},
            new() { Text = "数据库连接", Value = nameof(TenantOutput.Connection) },
            new() { Text = "排序", Value = nameof(TenantOutput.OrderNo) },
            new() { Text = "修改时间", Value = nameof(TenantOutput.UpdateTime) },
            new() { Text = "备注", Value = nameof(TenantOutput.Remark) },
            new() { Text = "操作", Value = "Action", Sortable = false, Align=DataTableHeaderAlign.Center }
        };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                _isLoading = true;
                _popupService.ShowProgressLinear();
                await LoadData();
                _popupService.HideProgressLinear();
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task LoadData()
        {
            input = new()
            {
                Page = _currentPage,
                PageSize = int.Parse(_paginationSelect),
                Name = _name,
                Phone = _phone
            };
            var res = await _sysTenantService.GetSysTenantPageAsync(input);
            if (res != null && res.Result?.Items != null)
            {
                _tatolPage = res.Result.TotalPages == 0 ? 1 : res.Result.TotalPages;
                _tatolCount = res.Result.Total;
                _sysTenantList = res.Result.Items.ToList();
                _sysTenantList = _sysTenantList.Select((item, index) => {
                    item.Index = (input.Page - 1) * input.PageSize + index + 1;
                    return item;
                }).ToList();
            }
            _isLoading = false;
        }

        private void NavigationStyleChanged(object? sender, EventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }
        private async Task OnPaginationValueChange(int value)
        {
            _currentPage = value;
            await LoadData();
        }
        private async Task OnSelectValueChange(string value)
        {
            _paginationSelect = value;
            _currentPage = 1;
            await LoadData();
        }

        private void ResetOnClick()
        {
            _name = string.Empty;
            _phone = string.Empty;
        }
        private void AddPosOnClick()
        {
            _dialog = true;
            _dialogTitle = "添加租户";
        }
        private async Task QueryOnClick()
        {
            _currentPage = 1;
            await LoadData();
        }
        private async Task EditTenantOnClick(TenantOutput tenant) {
            _dialog = true;
            _dialogTitle = "编辑租户";
        }
        private async Task GrantMenuOnClick(TenantOutput tenant) { 
            
        }
        private async Task ResetPwdOnClick(TenantOutput tenant)
        {

        }
        private async Task DltTenantOnClick(TenantOutput tenant)
        {

        }
        public void Dispose()
        {
            _sysTenantList.Clear();
        }
    }
}
