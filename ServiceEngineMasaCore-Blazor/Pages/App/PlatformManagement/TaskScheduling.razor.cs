using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.Job.Dto;
using ServiceEngineMasaCore.Blazor.Service.Job.Interface;
using ServiceEngineMasaCore.Blazor.Service.Log.Dto;
using ServiceEngineMasaCore.Blazor.Service.Tenant.Dto;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.App.PlatformManagement
{
    public partial class TaskScheduling : IDisposable
    {
        [Inject]
        [NotNull]
        IPopupService? _popupService { get; set; }

        [Inject]
        [NotNull]
        ISysJobService? _sysJobService { get; set; }

        List<SysJobDetail> _sysJobList = new List<SysJobDetail>();
        PJobInput input = new PJobInput();

        int _tatolCount = 0;
        int _tatolPage = 1;
        int _currentPage = 1;
        private string _paginationSelect = "10";

        bool _isLoading = false;
        readonly List<DataTableHeader<SysJobDetail>> _headers = new List<DataTableHeader<SysJobDetail>>()
        {
            new() { Text = "序号", Value = nameof(SysJobDetail.Index) },
            new() { Text = "作业编号", Value = nameof(SysJobDetail.JobId) },
            new() { Text = "组名称", Value = nameof(SysJobDetail.GroupName) },
            new() { Text = "类型", Value = nameof(SysJobDetail.JobType) },
            new() { Text = "描述", Value = nameof(SysJobDetail.Description) },
            new() { Text = "执行方式", Value = nameof(SysJobDetail.Concurrent) },
            new() { Text = "作业创建类型", Value = nameof(SysJobDetail.CreateType)},
            new() { Text = "额外数据", Value = nameof(SysJobDetail.Properties) },
            new() { Text = "更新时间", Value = nameof(SysJobDetail.UpdatedTime) },
        };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                _isLoading = true;
                _popupService.ShowProgressLinear();
                input = new() {
                    Page = 1,
                    PageSize = int.Parse(_paginationSelect),
                };
                await LoadData(input);
                _popupService.HideProgressLinear();
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task LoadData(PJobInput input) {
            var res = await _sysJobService.GetSysJobPageAsync(input);
            if (res != null && res.Result?.Items != null)
            {
                _tatolPage = res.Result.TotalPages == 0 ? 1 : res.Result.TotalPages;
                _tatolCount = res.Result.Total;
                _sysJobList = res.Result.Items.Select(i => i.JobDetail).ToList();
                _sysJobList = _sysJobList.Select((item, index) => {
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
            _sysJobList.Clear();
        }
    }
}
