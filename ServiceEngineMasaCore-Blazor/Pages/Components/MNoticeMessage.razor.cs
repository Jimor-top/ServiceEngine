using AngleSharp.Html.Parser;
using Blazored.LocalStorage;
using Masa.Blazor.Presets;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using ServiceEngine.Core;
using ServiceEngine.Core.Const;
using ServiceEngineMasaCore.Blazor.Service.Notice.Interface;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.Components
{
    public partial class MNoticeMessage
    {
        [Inject]
        [NotNull]
        ISysNoticeService? _sysNoticeService { get; set; }

        private PEnqueuedSnackbars? _enqueuedSnackbars;

        private HubConnection hubConnection;

        [Inject]
        [NotNull]
        private ILocalStorageService? _storage { get; set; }

        private bool _dialog { get; set; }
        string _content = string.Empty;
        private string _dialogTitle { get; set; } = string.Empty;
        List<SysNotice> _sysNoticeList { get; set; } = new List<SysNotice>();
        private StringNumber _tab = 0;

        private int _badgeMessage = 0;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var token = await _storage.GetItemAsStringAsync(StorageConst.AccessToken);
                //string hubUrl = $"http://localhost:5005/hubs/onlineUser?access_token={token}";
                //SignalRClient signalRClient = new SignalRClient(hubUrl);
                //signalRClient.MessageReceived += (user, message) =>
                //{
                //    Console.WriteLine($"Received message from {user}: {message}");
                //};
                //await signalRClient.StartAsync();


                hubConnection = new HubConnectionBuilder()
                .WithUrl($"http://localhost:5005/hubs/onlineUser?access_token={token}")
                .Build();

                hubConnection.StartAsync().ContinueWith(task =>
                {
                    if (task.Exception != null)
                    {
                        Console.WriteLine($"Failed to connect: {task.Exception.Message}");
                    }
                    else
                    {
                        Console.WriteLine("Connected");
                    }
                }).Wait();

                hubConnection.On<SysNotice>("PublicNotice", async( message) =>
                {
                    Enqueue(true, "您有一条新消息...");
                    _badgeMessage = _badgeMessage + 1;
                    _sysNoticeList.Add(message);
                    await InvokeAsync(StateHasChanged);
                });

                await LoadData();
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
        private async Task LoadData() {
            var res = await _sysNoticeService.UnReadSysNoticeAsync();
            if (res != null && res.Code == 200) {
                _sysNoticeList = res.Result;
                _badgeMessage = _sysNoticeList.Count;
            }
        }
        private async Task WatchMessageOnClick(SysNotice notice)
        {
            _content = notice.Content;
            _dialogTitle = notice.Type.GetDescription();
            _dialog = true;
            var res = await _sysNoticeService.SetReadSysNoticeAsync(new() { Id = notice.Id });
            if (res != null && res.Code == 200)
                for (int i = 0; i < _sysNoticeList.Count; i++)
                {
                    if (_sysNoticeList[i].Id == notice.Id)
                    {
                        _sysNoticeList.Remove(_sysNoticeList[i]);
                        _badgeMessage--;
                        break;
                    }
                }
        }
        private void Enqueue(bool result, string? context)
        {
            _enqueuedSnackbars?.EnqueueSnackbar(new SnackbarOptions()
            {
                Title = "提示",
                Content = context,
                Closeable = true,
                Type = AlertTypes.Info
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
        private string GetTypeDescription(NoticeTypeEnum noticeType) {
            return noticeType.GetDescription();
        }
    }
}
