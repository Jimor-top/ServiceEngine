using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.File.Dto;
using ServiceEngineMasaCore.Blazor.Service.File.Interface;
using ServiceEngineMasaCore.Blazor.Service.Log.Dto;
using ServiceEngineMasaCore.Blazor.Service.Plugin.Dto;
using ServiceEngineMasaCore.Blazor.Service.Plugin.Interface;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.App.PlatformManagement
{
    public partial class DynamicPlugin : IDisposable
    {
        [Inject]
        [NotNull]
        ISysPluginService? _sysPluginService { get; set; }

        List<SysPlugin> _sysPluginList = new List<SysPlugin>();
        PPluginInput input = new PPluginInput();

        int _tatolCount = 0;
        int _tatolPage = 1;
        int _currentPage = 1;
        private string _paginationSelect = "10";

        bool _isLoading = false;
        readonly List<DataTableHeader<SysPlugin>> _headers = new List<DataTableHeader<SysPlugin>>()
        {
            new() { Text = "序号", Value = nameof(SysPlugin.Index) },
            new() { Text = "功能名称", Value = nameof(SysPlugin.Name) },
            new() { Text = "程序集名称", Value = nameof(SysPlugin.AssemblyName) },
            new() { Text = "排序", Value = nameof(SysPlugin.OrderNo) },
            new() { Text = "状态", Value = nameof(SysPlugin.Status) },
            new() { Text = "修改时间", Value = nameof(SysPlugin.UpdateTime) },
            new() { Text = "备注", Value = nameof(SysPlugin.Remark)},
        };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                _isLoading = true;

                input = new PPluginInput()
                {
                    Page = 1,
                    PageSize = int.Parse(_paginationSelect),
                };
                await LoadData(input);
                _isLoading = false;
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
        private async Task LoadData(PPluginInput input) {
            var res = await _sysPluginService.GetSysOPluginPageAsync(input);
            if (res != null && res.Result?.Items != null)
            {
                _tatolPage = res.Result.TotalPages == 0 ? 1 : res.Result.TotalPages;
                _tatolCount = res.Result.Total;
                _sysPluginList = res.Result.Items.ToList();
                _sysPluginList = _sysPluginList.Select((item, index) => {
                    item.Index = (input.Page - 1) * input.PageSize + index + 1;
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
            input = new PPluginInput()
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
            input = new PPluginInput()
            {
                Page = 1,
                PageSize = int.Parse(value),
            };
            await LoadData(input);
        }
        public void Dispose()
        {
            _sysPluginList.Clear();
        }
    }
}
