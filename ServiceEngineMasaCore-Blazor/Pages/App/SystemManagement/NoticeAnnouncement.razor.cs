using AngleSharp.Html.Parser;
using Masa.Blazor.Presets;
using Nest;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
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
        PEnqueuedSnackbars? _enqueuedSnackbars;

        [Inject]
        [NotNull]
        IPopupService? _popupService { get; set; }

        [Inject]
        [NotNull]
        ISysNoticeService? _sysNoticeService { get; set; }

        List<SysNotice> _sysNoticeList = new List<SysNotice>();
        PNoticeInput input = new PNoticeInput();
        UpdateNoticeInput _updateNoticeInput = new UpdateNoticeInput();

        int _tatolCount = 0;
        int _tatolPage = 1;
        int _currentPage = 1;
        private string _paginationSelect = "10";

        private bool _dialog { get; set; }
        private string _dialogTitle { get; set; } = string.Empty;

        string _title = string.Empty;
        NoticeTypeEnum? _type;
        List<NoticeTypeEnum> _typeList = new List<NoticeTypeEnum>();
        bool _isLoading = false;
        readonly List<DataTableHeader<SysNotice>> _headers = new List<DataTableHeader<SysNotice>>()
        {
            new() { Text = "序号", Value = nameof(SysNotice.Index) },
            new() { Text = "标题", Value = nameof(SysNotice.Title) },
            new() { Text = "内容", Value = nameof(SysNotice.Content) },
            new() { Text = "类型", Value = nameof(SysNotice.Type) },
            new() { Text = "创建时间", Value = nameof(SysNotice.CreateTime) },
            new() { Text = "状态", Value = nameof(SysNotice.Status) },
            new() { Text = "发布者", Value = nameof(SysNotice.PublicUserName)},
            new() { Text = "发布时间", Value = nameof(SysNotice.PublicTime) },
            new() { Text = "操作", Value = "Action", Sortable = false, Align=DataTableHeaderAlign.Center }
        };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                _isLoading = true;
                foreach (NoticeTypeEnum typeEnum in Enum.GetValues(typeof(NoticeTypeEnum)))
                {
                    _typeList.Add(typeEnum);
                }
                _popupService.ShowProgressLinear();
                await LoadData();
                _popupService.HideProgressLinear();
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task LoadData() {
            input.Page = _currentPage;
            input.PageSize = int.Parse(_paginationSelect);
            input.Title = _title;
            input.Type = _type;
            var res = await _sysNoticeService.GetSysNoticePageAsync(input);
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
            await LoadData();
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
            await LoadData();
        }
        private void ResetOnClick()
        {
            _title = string.Empty;
            _type = null;
        }
        private void AddNotiOnClick() {
            _dialogTitle = "添加通知公告";
            _updateNoticeInput = new UpdateNoticeInput();
            _dialog = true;
        }
        private async Task QueryOnClick()
        {
            _currentPage = 1;
            await LoadData();
        }
        private async Task SubmitOnClick() {
            if (_dialogTitle.Equals("添加通知公告"))
            {
                var res = await _sysNoticeService.AddSysNoticeAsync(_updateNoticeInput);
                if (res != null && res.Code == 200) {
                    Enqueue(true,"通知公告添加成功");
                    await LoadData();
                }
                else
                    Enqueue(false, "通知公告添加失败");
            }
            else {
                var res = await _sysNoticeService.UpdateSysNoticeAsync(_updateNoticeInput);
                if (res != null && res.Code == 200)
                {
                    Enqueue(true, "通知公告修改成功");
                    for (int i = 0; i < _sysNoticeList.Count; i++)
                        if (_sysNoticeList[i].Id == _updateNoticeInput.Id) { 
                            _sysNoticeList[i] = _updateNoticeInput;
                            break;
                        }
                }
                else
                    Enqueue(false, "通知公告修改失败");
            }
            _dialog = false;
        }
        private async Task PublishNoticeOnClick(SysNotice notice) {
            var confirmed = await _popupService.ConfirmAsync(param =>
            {
                param.Title = "提示";
                param.Content = $"是否发布通知公告【{notice.Title}】?发布后不可撤销！";
                param.OkText = @T("Confirm");
                param.CancelText = @T("Cancel");
            });
            if (confirmed)
            {
                NotiInput notiInput = new()
                {
                    Id = notice.Id
                };
                var res = await _sysNoticeService.PublicSysNoticeAsync(notiInput);
                if (res != null && res.Code == 200)
                {
                    Enqueue(true, "通知公告发布成功");
                    for (int i = 0; i < _sysNoticeList.Count; i++)
                        if (_sysNoticeList[i].Id == notice.Id)
                            _sysNoticeList[i].Status = NoticeStatusEnum.PUBLIC;
                }
                else
                    Enqueue(false, "通知公告发布失败");
            }
        }
        private void EditNoticeOnClick(SysNotice notice)
        {
            _dialogTitle = "编辑通知公告";
            string str = JsonConvert.SerializeObject(notice);
            var userInput = JsonConvert.DeserializeObject<UpdateNoticeInput>(str);
            _updateNoticeInput = userInput ?? new UpdateNoticeInput();
            _dialog = true;
        }
        private async Task DltNoticeOnClick(SysNotice notice)
        {
            var confirmed = await _popupService.ConfirmAsync(param =>
            {
                param.Title = "提示";
                param.Content = $"是否删除通知公告【{notice.Title}】?";
                param.OkText = @T("Confirm");
                param.CancelText = @T("Cancel");
            });
            if (confirmed)
            {
                NotiInput deleteNotice = new() { Id = notice.Id };
                var res = await _sysNoticeService.DeleteSysNoticeAsync(deleteNotice);
                if (res != null && res.Code == 200)
                {
                    Enqueue(true, "通知公告删除成功");
                    for (int i = 0; i < _sysNoticeList.Count; i++)
                        if (_sysNoticeList[i].Id == notice.Id) { 
                            _sysNoticeList.Remove(_sysNoticeList[i]);
                            break;
                        }
                }
                else
                    Enqueue(false, "通知公告删除失败");
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
