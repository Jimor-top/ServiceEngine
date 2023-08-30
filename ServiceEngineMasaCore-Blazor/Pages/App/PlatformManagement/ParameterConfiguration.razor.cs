using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Config.Dto;
using ServiceEngineMasaCore.Blazor.Service.Config.Interface;
using ServiceEngineMasaCore.Blazor.Service.Log.Dto;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.App.PlatformManagement
{
    public partial class ParameterConfiguration : IDisposable
    {
        [Inject]
        [NotNull]
        IConfigService? _configService { get; set; }

        List<SysConfig> _configList = new List<SysConfig>();
        PConfigInput input = new PConfigInput();

        int _tatolCount = 0;
        int _tatolPage = 1;
        int _currentPage = 1;
        private string _paginationSelect = "10";

        bool _isLoading = false;
        readonly List<DataTableHeader<SysConfig>> _headers = new List<DataTableHeader<SysConfig>>()
        {
             new() { Text = "序号", Value = nameof(SysConfig.Index) },
            new() { Text = "配置名称", Value = nameof(SysConfig.Name) },
            new() { Text = "配置编码", Value = nameof(SysConfig.Code) },
            new() { Text = "属性值", Value = nameof(SysConfig.Value) },
            new() { Text = "内置参数", Value = nameof(SysConfig.SysFlag) },
            new() { Text = "分组编码", Value = nameof(SysConfig.GroupCode) },
            new() { Text = "排序", Value = nameof(SysConfig.OrderNo)},
            new() { Text = "备注", Value = nameof(SysConfig.Remark) },
        };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                _isLoading = true;

                input = new()
                {
                    Page = 1,
                    PageSize = int.Parse(_paginationSelect),
                };
                await LoadData(input);

                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task LoadData(PConfigInput input) {
            var res = await _configService.GetSysConfigPageAsync(input);
            if (res != null && res.Result != null && res.Result.Items != null)
            {
                _tatolPage = res.Result.TotalPages == 0 ? 1 : res.Result.TotalPages;
                _tatolCount = res.Result.Total;
                _configList.AddRange(res.Result.Items);
                _configList = _configList.Select((item, index) => {
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
            _configList.Clear();
        }
    }
}
