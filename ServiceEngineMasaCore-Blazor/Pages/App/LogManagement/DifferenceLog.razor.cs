using Qiniu.CDN;
using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Log.Dto;
using ServiceEngineMasaCore.Blazor.Service.Log.Interface;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.App.LogManagement
{
    public partial class DifferenceLog : IDisposable
    {
        [Inject]
        [NotNull]
        ISysLogService? _sysLogServices { get; set; }

        List<SysLogDiff> _sysLogList = new List<SysLogDiff>();

        PLogInput input = new PLogInput();

        int _tatolCount = 0;
        int _tatolPage = 1;
        int _currentPage = 1;
        private string _paginationSelect = "10";

        bool _isLoading = false;
        readonly List<DataTableHeader<SysLogDiff>> _headers = new List<DataTableHeader<SysLogDiff>>()
        {
             new() { Text = "序号", Value = nameof(SysLogDiff.Index) },
            new() { Text = "差异操作", Value = nameof(SysLogDiff.DiffType) },
            new() { Text = "Sql语句", Value = nameof(SysLogDiff.Sql) },
            new() { Text = "参数", Value = nameof(SysLogDiff.Parameters) },
            new() { Text = "耗时(ms)", Value = nameof(SysLogDiff.Elapsed) },
            new() { Text = "操作前记录", Value = nameof(SysLogDiff.BeforeData)},
            new() { Text = "操作后记录", Value = nameof(SysLogDiff.AfterData) },
            new() { Text = "业务对象", Value = nameof(SysLogDiff.BusinessData) },
            new() { Text = "操作时间", Value = nameof(SysLogDiff.CreateTime) },
        };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                input = new PLogInput()
                {
                    Page = 1,
                    PageSize = int.Parse(_paginationSelect),
                    StartTime = DateTime.Now.AddDays(-1),
                    EndTime = DateTime.Now,
                };
                _isLoading = true;
                await LoadData(input);
                 _isLoading = false;
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
        private async Task LoadData(PLogInput input) {
            var res = await _sysLogServices.GetSysLogDiffPageAsync(input);
            if (res != null && res.Result?.Items != null)
            {
                _tatolPage = res.Result.TotalPages == 0 ? 1: res.Result.TotalPages;
                _tatolCount = res.Result.Total;
                _sysLogList = res.Result.Items.ToList();
                _sysLogList = _sysLogList.Select((item, index) => {
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
            input = new PLogInput()
            {
                Page = value,
                PageSize = int.Parse(_paginationSelect),
                StartTime = DateTime.Now.AddDays(-1),
                EndTime = DateTime.Now,
            };
            await LoadData(input);
        }
        private async Task OnSelectValueChange(string value)
        {
            _paginationSelect = value;
            _currentPage = 1;
            input = new PLogInput()
            {
                Page = 1,
                PageSize = int.Parse(value),
                StartTime = DateTime.Now.AddDays(-1),
                EndTime = DateTime.Now,
            };
            await LoadData(input);
        }
        public void Dispose()
        {
            _sysLogList.Clear();
        }
    }
}
