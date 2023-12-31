﻿using Masa.Blazor.Presets;
using Nest;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Qiniu.CDN;
using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Log.Dto;
using ServiceEngineMasaCore.Blazor.Service.Log.Interface;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.App.LogManagement
{
    public partial class AccessLog : IDisposable
    {
        private PEnqueuedSnackbars? _enqueuedSnackbars;

        [Inject]
        [NotNull]
        IPopupService? _popupService { get; set; }

        [Inject]
        [NotNull]
        ISysLogService? _sysLogServices { get; set; }

        List<SysLogVis> _sysLogList = new List<SysLogVis>();
        PLogInput input = new PLogInput();

        int _tatolCount = 0;
        int _tatolPage = 1;
        int _currentPage = 1;
        private string _paginationSelect = "10";

        private DateTime? _startDate;
        private DateTime? _endDate;
        bool IsDateAllowed(DateOnly date)
        {
            return date >= ( _startDate != null ? new DateOnly(_startDate.Value.Year, _startDate.Value.Month, _startDate.Value.Day):null);
        }

        bool _isLoading = false;
       
        readonly List<DataTableHeader<SysLogVis>> _headers = new List<DataTableHeader<SysLogVis>>()
        {
            new() { Text = "序号", Value = nameof(SysLogVis.Index) },
            new() { Text = "显示名称", Value = nameof(SysLogVis.DisplayTitle) },
            new() { Text = "方法名称", Value = nameof(SysLogVis.ActionName) },
            new() { Text = "账号名称", Value = nameof(SysLogVis.Account) },
            new() { Text = "真实名称", Value = nameof(SysLogVis.RealName) },
            new() { Text = "IP地址", Value = nameof(SysLogVis.RemoteIp) },
            new() { Text = "登录地址", Value = nameof(SysLogVis.Location)},
            new() { Text = "浏览器", Value = nameof(SysLogVis.Browser) },
            new() { Text = "操作系统", Value = nameof(SysLogVis.Os) },
            new() { Text = "状态", Value = nameof(SysLogVis.Status) },
            new() { Text = "耗时(ms)", Value = nameof(SysLogVis.Elapsed) },
            new() { Text = "日志时间", Value = nameof(SysLogVis.LogDateTime) }
        };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                _isLoading = true;
                _popupService.ShowProgressLinear();
                input = new PLogInput()
                {
                    Page = 1,
                    PageSize = int.Parse(_paginationSelect),
                    StartTime = _startDate != null ? Convert.ToDateTime(_startDate) : null,
                    EndTime = _endDate != null ? Convert.ToDateTime(_endDate) : null,
                };
                await LoadData(input);
                _isLoading = false;
                _popupService.HideProgressLinear();
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
        private async Task LoadData(PLogInput input) {
            
            var res = await _sysLogServices.GetSysLogVisPageAsync(input);
            if (res != null && res.Result?.Items != null)
            {
                _tatolPage = res.Result.TotalPages == 0 ? 1 : res.Result.TotalPages;
                _tatolCount = res.Result.Total;
                _sysLogList = res.Result.Items.ToList();
                _sysLogList = _sysLogList.Select((item, index) => {
                    item.Index = (input.Page-1)* input.PageSize + index + 1;
                    return item;
                }).ToList();
                StateHasChanged();
            }
        }
        private void NavigationStyleChanged(object? sender, EventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }
        private async Task OnPaginationValueChange(int value) {
            _currentPage = value;
            input = new PLogInput()
            {
                Page = value,
                PageSize = int.Parse(_paginationSelect),
                StartTime = _startDate != null ? Convert.ToDateTime(_startDate) : null,
                EndTime = _endDate != null ? Convert.ToDateTime(_endDate) : null,
            };
            await LoadData(input);
        }
        private async Task OnSelectValueChange(string value) {
            _paginationSelect = value;
            _currentPage = 1;
            input = new PLogInput()
            {
                Page = 1,
                PageSize = int.Parse(value),
                StartTime = _startDate != null ? Convert.ToDateTime(_startDate) : null,
                EndTime = _endDate != null ? Convert.ToDateTime(_endDate) : null,
            };
            await LoadData(input);
        }
        private void ResetOnClick() {
            _startDate = null;
            _endDate = null;
        } 
        private async Task ClearAllOnClick() { 
            var res = await _sysLogServices.ClearSysLogVisAsync();
            if (res != null && res.Result)
            {
                _sysLogList.Clear();
                Enqueue(true, "清空成功");
            }
            else {
                Enqueue(false,"清空失败");
            }
        }
        private async Task QueryOnClick()
        {
            input = new PLogInput()
            {
                Page = 1,
                PageSize = int.Parse(_paginationSelect),
                StartTime = _startDate != null ? Convert.ToDateTime(_startDate) : null,
                EndTime = _endDate != null ? Convert.ToDateTime(_endDate) : null,
            };
            await LoadData(input);
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
