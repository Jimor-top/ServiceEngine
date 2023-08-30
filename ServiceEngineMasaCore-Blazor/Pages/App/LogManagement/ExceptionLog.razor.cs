using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Global;
using ServiceEngineMasaCore.Blazor.Service.Log.Dto;
using ServiceEngineMasaCore.Blazor.Service.Log.Interface;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.App.LogManagement
{
    public partial class ExceptionLog : IDisposable
    {
        [Inject]
        [NotNull]
        ISysLogService? _sysLogServices { get; set; }

        List<SysLogEx> _sysLogList = new List<SysLogEx>();

        PLogInput input = new PLogInput();

        int _tatolCount = 0;
        int _tatolPage = 1;
        int _currentPage = 1;
        private string _paginationSelect = "10";

        bool _isLoading = false;
        readonly List<DataTableHeader<SysLogEx>> _headers = new List<DataTableHeader<SysLogEx>>()
        {
            new() { Text = "序号", Value = nameof(SysLogEx.Index) },
            new() { Text = "模块名称", Value = nameof(SysLogEx.ControllerName) },
            new() { Text = "显示名称", Value = nameof(SysLogEx.DisplayTitle) },
            new() { Text = "方法名称", Value = nameof(SysLogEx.ActionName) },
            new() { Text = "请求方式", Value = nameof(SysLogEx.HttpMethod) },
            new() { Text = "请求地址", Value = nameof(SysLogEx.RemoteIp) },
            new() { Text = "级别", Value = nameof(SysLogEx.LogLevel)},
            new() { Text = "事件ID", Value = nameof(SysLogEx.EventId) },
            new() { Text = "线程ID", Value = nameof(SysLogEx.ThreadId) },
            new() { Text = "追踪ID", Value = nameof(SysLogEx.TraceId) },
            new() { Text = "真实姓名", Value = nameof(SysLogEx.RealName) },
            new() { Text = "日志时间", Value = nameof(SysLogEx.LogDateTime) }
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
            var res = await _sysLogServices.GetSysLogExPageAsync(input);
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
