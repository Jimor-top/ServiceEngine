using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Job.Dto;
using ServiceEngineMasaCore.Blazor.Service.Log.Dto;
using ServiceEngineMasaCore.Blazor.Service.Print.Dto;
using ServiceEngineMasaCore.Blazor.Service.Print.Interface;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.App.PlatformManagement
{
    public partial class PrintTemplate : IDisposable
    {
        [Inject]
        [NotNull]
        ISysPrintService? _sysPrintService { get; set; }

        List<SysPrint> _sysPrintList = new List<SysPrint>();
        PPrintInput input = new PPrintInput();

        int _tatolCount = 0;
        int _tatolPage = 1;
        int _currentPage = 1;
        private string _paginationSelect = "10";

        bool _isLoading = false;
        readonly List<DataTableHeader<SysPrint>> _headers = new List<DataTableHeader<SysPrint>>()
        {
            new() { Text = "序号", Value = nameof(SysPrint.Index) },
            new() { Text = "名称", Value = nameof(SysPrint.Name) },
            new() { Text = "排序", Value = nameof(SysPrint.OrderNo) },
            new() { Text = "状态", Value = nameof(SysPrint.Status) },
            new() { Text = "修改时间", Value = nameof(SysPrint.UpdateTime) },
            new() { Text = "备注", Value = nameof(SysPrint.Remark) },
        };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                _isLoading = true;
                input = new PPrintInput()
                {
                    Page = 1,
                    PageSize = int.Parse(_paginationSelect),
                };
                await LoadData(input);
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task LoadData(PPrintInput input) {
            var res = await _sysPrintService.GetSysPrintPageAsync(input);
            if (res != null && res.Result?.Items != null)
            {
                _tatolPage = res.Result.TotalPages == 0 ? 1 : res.Result.TotalPages;
                _tatolCount = res.Result.Total;
                _sysPrintList = res.Result.Items.ToList();
                _sysPrintList = _sysPrintList.Select((item, index) => {
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
            input = new PPrintInput()
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
            input = new PPrintInput()
            {
                Page = 1,
                PageSize = int.Parse(value),
            };
            await LoadData(input);
        }
        public void Dispose()
        {
            _sysPrintList.Clear();
        }
    }
}
