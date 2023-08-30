using Qiniu.CDN;
using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Log.Dto;
using ServiceEngineMasaCore.Blazor.Service.Log.Interface;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.App.LogManagement
{
    public partial class OperationLog : IDisposable
    {
        [Inject]
        [NotNull]
        ISysLogService? _sysLogServices { get; set; }

        List<SysLogOp> _sysLogList = new List<SysLogOp>();

        PLogInput input = new PLogInput();

        int _tatolCount = 0;
        int _tatolPage = 1;
        int _currentPage = 1;
        private string _paginationSelect = "10";

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
        private async Task LoadData(PLogInput input)
        {
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
