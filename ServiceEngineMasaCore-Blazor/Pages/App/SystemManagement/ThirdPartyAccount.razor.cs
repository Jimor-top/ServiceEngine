using Masa.Blazor;
using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Log.Dto;
using ServiceEngineMasaCore.Blazor.Service.Role.Dto;
using ServiceEngineMasaCore.Blazor.Service.Role.Interface;
using ServiceEngineMasaCore.Blazor.Service.Wechat.Dto;
using ServiceEngineMasaCore.Blazor.Service.Wechat.Interface;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.App.SystemManagement
{
    public partial class ThirdPartyAccount : IDisposable
    {
        [Inject]
        [NotNull]
        IPopupService? _popupService { get; set; }

        [Inject]
        [NotNull]
        IWeChatService? _sysWeChatService { get; set; }

        List<SysWechatUser> _sysWeChatList = new List<SysWechatUser>();
        PWechatUserInput input = new PWechatUserInput();

        int _tatolCount = 0;
        int _tatolPage = 1;
        int _currentPage = 1;
        private string _paginationSelect = "10";

        bool _isLoading = false;
        readonly List<DataTableHeader<SysWechatUser>> _headers = new List<DataTableHeader<SysWechatUser>>()
        {
             new() { Text = "序号", Value = nameof(SysWechatUser.Index) },
            new() { Text = "OpenID", Value = nameof(SysWechatUser.OpenId) },
            new() { Text = "UnionID", Value = nameof(SysWechatUser.UnionId) },
            new() { Text = "平台类型", Value = nameof(SysWechatUser.PlatformType) },
            new() { Text = "昵称", Value = nameof(SysWechatUser.NickName) },
            new() { Text = "头像", Value = nameof(SysWechatUser.Avatar) },
            new() { Text = "手机号码", Value = nameof(SysWechatUser.Mobile)},
            new() { Text = "性别", Value = nameof(SysWechatUser.Sex) },
            new() { Text = "城市", Value = nameof(SysWechatUser.City) },
            new() { Text = "省", Value = nameof(SysWechatUser.Province) },
            new() { Text = "国家", Value = nameof(SysWechatUser.Country) },
        };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                _isLoading = true;
                input = new()
                {
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

        private async Task LoadData(PWechatUserInput input) {
            var res = await _sysWeChatService.GetWeChatPageAsync(input);
            if (res != null && res.Result?.Items != null)
            {
                _tatolPage = res.Result.TotalPages == 0 ? 1 : res.Result.TotalPages;
                _tatolCount = res.Result.Total;
                _sysWeChatList = res.Result.Items.ToList();
                _sysWeChatList = _sysWeChatList.Select((item, index) => {
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
            _sysWeChatList.Clear();
        }
    }
}
