using AngleSharp.Html.Parser;
using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Notice.Dto;
using ServiceEngineMasaCore.Blazor.Service.Notice.Interface;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.App.Dashboard
{
    public partial class Notice : IDisposable
    {
        [Inject]
        [NotNull]
        IPopupService? _popupService { get; set; }

        [Inject]
        [NotNull]
        ISysNoticeService? _sysNoticeService { get; set; }

        List<SysNoticeUser> _sysNoticeList = new List<SysNoticeUser>();
        PNoticeInput input = new PNoticeInput();

        int _tatolCount = 0;
        int _tatolPage = 1;
        int _currentPage = 1;
        private string _paginationSelect = "10";

        string _title = string.Empty;
        NoticeTypeEnum? _type;
        List<NoticeTypeEnum> _typeList = new List<NoticeTypeEnum>();

        string _content = string.Empty;
        private bool _dialog { get; set; }
        private string _dialogTitle { get; set; } = string.Empty;

        bool _isLoading = false;
        readonly List<DataTableHeader<SysNoticeUser>> _headers = new List<DataTableHeader<SysNoticeUser>>()
        {
            new() { Text = "序号", Value = nameof(SysNoticeUser.SysNotice.Index) },
            new() { Text = "标题", Value = nameof(SysNoticeUser.SysNotice.Title) },
            new() { Text = "内容", Value = nameof(SysNoticeUser.SysNotice.Content) },
            new() { Text = "类型", Value = nameof(SysNoticeUser.SysNotice.Type) },
            new() { Text = "创建时间", Value = nameof(SysNoticeUser.SysNotice.CreateTime) },
            new() { Text = "阅读状态", Value = nameof(SysNoticeUser.ReadStatus) },
            new() { Text = "发布者", Value = nameof(SysNoticeUser.SysNotice.PublicUserName)},
            new() { Text = "发布时间", Value = nameof(SysNoticeUser.SysNotice.PublicTime) },
            new() { Text = "操作", Value = "Action", Sortable = false, Align=DataTableHeaderAlign.Center }
        };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                _isLoading = true;
                _popupService.ShowProgressLinear();
                foreach (NoticeTypeEnum typeEnum in Enum.GetValues(typeof(NoticeTypeEnum)))
                {
                    _typeList.Add(typeEnum);
                }
                await LoadData();
                _popupService.HideProgressLinear();
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task LoadData()
        {
            input.Page = _currentPage;
            input.PageSize = int.Parse(_paginationSelect);
            input.Title = _title;
            input.Type = _type;
            var res = await _sysNoticeService.ReceivedSysNoticeAsync(input);
            if (res != null && res.Result?.Items != null)
            {
                _tatolPage = res.Result.TotalPages == 0 ? 1 : res.Result.TotalPages;
                _tatolCount = res.Result.Total;
                _sysNoticeList = res.Result.Items.ToList();
                _sysNoticeList = _sysNoticeList.Select((item, index) => {
                    item.SysNotice.Index = (input.Page - 1) * input.PageSize + index + 1;
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
            _title = string.Empty;
            _type = null;
        }
        private async Task QueryOnClick()
        {
            _currentPage = 1;
            await LoadData();
        }
        private async Task WatchMessageOnClick(SysNoticeUser notice) {
            _content = notice.SysNotice.Content;
            _dialogTitle = notice.SysNotice.Type.GetDescription();
            _dialog = true;
            if (notice.ReadStatus == NoticeUserStatusEnum.UNREAD) {
                var res = await _sysNoticeService.SetReadSysNoticeAsync(new() { Id = notice.SysNotice.Id });
                if (res != null && res.Code == 200)
                    for (int i = 0; i < _sysNoticeList.Count; i++)
                    {
                        if (_sysNoticeList[i].SysNotice.Id == notice.SysNotice.Id)
                        {
                            _sysNoticeList[i].ReadStatus = NoticeUserStatusEnum.READ;
                            break;
                        }
                    }
            }
        }
        private string RemoveHtmlTags(string html)
        {
            var parser = new HtmlParser();
            var document = parser.ParseDocument(html);

            // 获取纯文本内容
            string plainText = document.Body.TextContent;

            // 返回去掉标签后的纯文本
            return plainText;
        }
        public void Dispose()
        {
            _sysNoticeList.Clear();
        }
    }
}
