﻿using Masa.Blazor.Presets;
using ServiceEngine.Core;
using ServiceEngine.Core.Util;
using ServiceEngineMasaCore.Blazor.Service.Log.Dto;
using ServiceEngineMasaCore.Blazor.Service.Log.Interface;
using System.Diagnostics.CodeAnalysis;


namespace ServiceEngineMasaCore.Blazor.Pages.App.LogManagement
{
    public partial class OperationLog : IDisposable
    {
        private PEnqueuedSnackbars? _enqueuedSnackbars;

        [Inject]
        [NotNull]
        IPopupService? _popupService { get; set; }

        [Inject]
        [NotNull]
        DownloadFileUtil? _downloadFileUtil { get; set; }

        [Inject]
        [NotNull]
        ISysLogService? _sysLogServices { get; set; }

        List<SysLogOp> _sysLogList = new List<SysLogOp>();

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
        readonly List<DataTableHeader<SysLogOp>> _headers = new List<DataTableHeader<SysLogOp>>()
        {
             new() { Text = "序号", Value = nameof(SysLogVis.Index) },
            new() { Text = "模块名称", Value = nameof(SysLogOp.ControllerName) },
            new() { Text = "显示名称", Value = nameof(SysLogOp.DisplayTitle) },
            new() { Text = "方法名称", Value = nameof(SysLogOp.ActionName) },
            new() { Text = "请求方式", Value = nameof(SysLogOp.HttpMethod) },
            new() { Text = "请求地址", Value = nameof(SysLogOp.RemoteIp) },
            new() { Text = "级别", Value = nameof(SysLogOp.LogLevel)},
            new() { Text = "事件ID", Value = nameof(SysLogOp.EventId) },
            new() { Text = "线程ID", Value = nameof(SysLogOp.ThreadId) },
            new() { Text = "追踪ID", Value = nameof(SysLogOp.TraceId) },
            new() { Text = "账户名称", Value = nameof(SysLogOp.RealName) },
            new() { Text = "日志时间", Value = nameof(SysLogOp.LogDateTime) }
        };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                _popupService.ShowProgressLinear();
                _isLoading = true;
                await LoadData();
                _popupService.HideProgressLinear();
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
        private async Task LoadData()
        {
            input = new PLogInput()
            {
                Page = _currentPage,
                PageSize = int.Parse(_paginationSelect),
                StartTime = _startDate != null ? Convert.ToDateTime(_startDate) : null,
                EndTime = _endDate != null ? Convert.ToDateTime(_endDate) : null,
            };
            var res = await _sysLogServices.GetSysLogOpPageAsync(input);
            if (res != null && res.Result?.Items != null)
            {
                _tatolPage = res.Result.TotalPages == 0 ? 1 : res.Result.TotalPages;
                _tatolCount = res.Result.Total;
                _sysLogList = res.Result.Items.ToList();
                _sysLogList = _sysLogList.Select((item, index) => {
                    item.Index = (input.Page - 1) * input.PageSize + index + 1;
                    return item;
                }).ToList();
                _isLoading = false;
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
            var res = await _sysLogServices.ClearSysLogOpAsync();
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
        private async Task ExportOnClick() {
            input = new PLogInput()
            {
                StartTime = _startDate != null ? Convert.ToDateTime(_startDate) : null,
                EndTime = _endDate != null ? Convert.ToDateTime(_endDate) : null,
            };
            var res = await _sysLogServices.ExportLogOpAsync(input);
            if (res != null) {
                await _downloadFileUtil.DownloadFile(res);
            }
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
