using ServiceEngine.Core.Service;
using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Menu.Interface;
using System.Diagnostics.CodeAnalysis;
using ServiceEngineMasaCore.Blazor.Service.Dict.Interface;
using ServiceEngineMasaCore.Blazor.Service.Dict.Dto;
using ServiceEngineMasaCore.Blazor.Service.Log.Dto;

namespace ServiceEngineMasaCore.Blazor.Pages.App.PlatformManagement
{
    public partial class DictionaryManagement : IDisposable
    {
        [Inject]
        [NotNull]
        IPopupService? _popupService { get; set; }

        [Inject]
        [NotNull]
        ISysDictDataService? _sysDictDataService { get; set; }

        List<SysDictData> _sysDictDataList = new List<SysDictData>();
        PDictDataInput input = new PDictDataInput();

        int _tatolCount = 0;
        int _tatolPage = 1;
        int _currentPage = 1;
        private string _paginationSelect = "10";

        bool _isLoading = false;
        readonly List<DataTableHeader<SysDictData>> _headers = new List<DataTableHeader<SysDictData>>()
        {
              new() { Text = "序号", Value = nameof(SysDictData.Index) },
            new() { Text = "字典名称", Value = nameof(SysDictData.Value) },
            new() { Text = "字典编码", Value = nameof(SysDictData.Code) },
            new() { Text = "状态", Value = nameof(SysDictData.Status) },
            new() { Text = "排序", Value = nameof(SysDictData.OrderNo) },
            new() { Text = "修改时间", Value = nameof(SysDictData.UpdateTime) },
            new() { Text = "备注", Value = nameof(SysDictData.Remark)},
        };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                _isLoading = true;

                input = new() {
                    Page = 1,
                    PageSize = int.Parse(_paginationSelect),
                };
                _popupService.ShowProgressLinear();
                await LoadData(input);
                _popupService.HideProgressLinear();
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task LoadData(PDictDataInput input) {
            var res = await _sysDictDataService.GetSysDictDataPageAsync(input);
            if (res != null && res.Result != null)
            {
                _tatolPage = res.Result.TotalPages == 0 ? 1 : res.Result.TotalPages;
                _tatolCount = res.Result.Total;
                _sysDictDataList.AddRange(res.Result.Items);
                _sysDictDataList = _sysDictDataList.Select((item, index) => {
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
            input = new()
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
            input = new()
            {
                Page = 1,
                PageSize = int.Parse(value),
            };
            await LoadData(input);
        }

        public void Dispose()
        {
            _sysDictDataList.Clear();
        }
    }
}
