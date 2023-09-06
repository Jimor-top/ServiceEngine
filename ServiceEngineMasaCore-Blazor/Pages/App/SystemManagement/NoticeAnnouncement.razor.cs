using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.Log.Dto;
using ServiceEngineMasaCore.Blazor.Service.Notice.Dto;
using ServiceEngineMasaCore.Blazor.Service.Notice.Interface;
using ServiceEngineMasaCore.Blazor.Service.Role.Dto;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.App.SystemManagement
{
    public partial class NoticeAnnouncement : IDisposable
    {
        [Inject]
        [NotNull]
        IPopupService? _popupService { get; set; }

        [Inject]
        [NotNull]
        ISysNoticeService? _sysNoticeService { get; set; }

        List<SysNotice> _sysNoticeList = new List<SysNotice>();
        PNoticeInput input = new PNoticeInput();

        int _tatolCount = 0;
        int _tatolPage = 1;
        int _currentPage = 1;
        private string _paginationSelect = "10";

        bool _isLoading = false;
        readonly List<DataTableHeader<SysNotice>> _headers = new List<DataTableHeader<SysNotice>>()
        {
            new() { Text = "序号", Value = nameof(SysNotice.Index) },
            new() { Text = "标题", Value = nameof(SysNotice.Title) },
            new() { Text = "内容", Value = nameof(SysNotice.Content) },
            new() { Text = "类型", Value = nameof(SysNotice.Type) },
            new() { Text = "创建时间", Value = nameof(SysNotice.CreateTime) },
            new() { Text = "状态", Value = nameof(SysNotice.Status) },
            new() { Text = "发布者", Value = nameof(SysNotice.UpdateTime)},
            new() { Text = "发布时间", Value = nameof(SysNotice.PublicTime) },
        };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                _isLoading = true;

                input = new PNoticeInput() {
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

        private async Task LoadData(PNoticeInput input) {
            var res = await _sysNoticeService.GetSysNoticePageAsynct(input);
            if (res != null && res.Result?.Items != null)
            {
                _tatolPage = res.Result.TotalPages == 0 ? 1 : res.Result.TotalPages;
                _tatolCount = res.Result.Total;
                _sysNoticeList = res.Result.Items.ToList();
                _sysNoticeList = _sysNoticeList.Select((item, index) => {
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
            input = new PNoticeInput()
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
            input = new PNoticeInput()
            {
                Page = 1,
                PageSize = int.Parse(value),
            };
            await LoadData(input);
        }
        public void Dispose()
        {
            _sysNoticeList.Clear();
        }
    }
}
