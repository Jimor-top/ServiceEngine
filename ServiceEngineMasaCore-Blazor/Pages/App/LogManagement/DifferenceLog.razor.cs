using Masa.Blazor.Presets;
using Qiniu.CDN;
using ServiceEngine.Core;
using ServiceEngine.Core.Util;
using ServiceEngineMasaCore.Blazor.Service.Log.Dto;
using ServiceEngineMasaCore.Blazor.Service.Log.Interface;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.App.LogManagement
{
    public partial class DifferenceLog : IDisposable
    {
        private PEnqueuedSnackbars? _enqueuedSnackbars;

        [Inject]
        [NotNull]
        IPopupService? _popupService { get; set; }

        [Inject]
        [NotNull]
        ISysLogService? _sysLogServices { get; set; }

        List<SysLogDiff> _sysLogList = new List<SysLogDiff>();

        PLogInput input = new PLogInput();

        int _tatolCount = 0;
        int _tatolPage = 1;
        int _currentPage = 1;
        private string _paginationSelect = "10";

        private DateTime? _startDate;
        private DateTime? _endDate;
        bool IsDateAllowed(DateOnly date)
        {
            return date >= (_startDate != null ? new DateOnly(_startDate.Value.Year, _startDate.Value.Month, _startDate.Value.Day) : null);
        }

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
                _isLoading = true;
                _popupService.ShowProgressLinear();
                await LoadData();
                 _isLoading = false;
                _popupService.HideProgressLinear();
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
        private async Task LoadData() {
            input = new PLogInput()
            {
                Page = _currentPage,
                PageSize = int.Parse(_paginationSelect),
                StartTime = _startDate != null ? Convert.ToDateTime(_startDate) : null,
                EndTime = _endDate != null ? Convert.ToDateTime(_endDate) : null,
            };
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
            _startDate = null;
            _endDate = null;
        }
        private async Task ClearAllOnClick()
        {
            var res = await _sysLogServices.ClearSysLogDiffAsync();
            if (res != null && res.Result)
            {
                _sysLogList.Clear();
                Enqueue(true, "清空成功");
            }
            else
            {
                Enqueue(false, "清空失败");
            }
        }
        private async Task QueryOnClick()
        {
            _currentPage = 1;
            await LoadData();
        }
        private void Enqueue(bool result, string? context)
        {
            _enqueuedSnackbars?.EnqueueSnackbar(new SnackbarOptions()
            {
                Content = context,
                Type = result ? AlertTypes.Success : AlertTypes.Error,
                Closeable = true
            });
        }
        public void Dispose()
        {
            _sysLogList.Clear();
        }
    }
}
