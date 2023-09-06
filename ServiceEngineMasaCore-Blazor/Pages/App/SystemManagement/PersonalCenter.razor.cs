using Masa.Blazor.Presets;
using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Dto;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Interface;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.App.SystemManagement
{
    public partial class PersonalCenter : IDisposable
    {
        [Inject]
        [NotNull]
        IPopupService? _popupService { get; set; }

        private PEnqueuedSnackbars? _enqueuedSnackbars;

        [Inject]
        [NotNull]
        private IUserInfoStore? _iUserInfoStore { get; set; }
        [Inject]
        [NotNull]
        private ISysUserService? _ISysUserService { get; set; }

        [NotNull]
        private SysUser _UserInfo { get; set; } = new SysUser();

        [NotNull]
        private UserInfoDto _UserInfoDto { get; set; } = new UserInfoDto();

        private StringNumber _tab = 0;

        private bool _EditEnable = false;
        private bool _TimeMenu;
        private bool _show;
        public DateOnly _Date { get; set; }

        private string CurrentPassword = string.Empty;
        private string NewPassword = string.Empty;
        private string ConfirmPassword = string.Empty;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                _popupService.ShowProgressLinear();
                _UserInfoDto = _iUserInfoStore.GetUserInfos();
                var res = await _ISysUserService.GetSysUserBaseInfoAsync();
                _UserInfo = res.Result;
                if (res.Result != null) { 
                    StateHasChanged();
                }
                _popupService.HideProgressLinear();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private void NavigationStyleChanged(object? sender, EventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }

        private void Edit_Click() {
            _EditEnable = true;
        }

        private async Task Save_Click() {
            _EditEnable = false;
            var res = await _ISysUserService.PostSysUserBaseInfoAsync(_UserInfo);
            var result = (res?.Type == "success");
            var context = result ? "更新成功" : "更新失败";
            Enqueue(result, context);
        }
        private void Enqueue(bool result,string? context)
        {
            _enqueuedSnackbars?.EnqueueSnackbar(new SnackbarOptions()
            {
                Content = context,
                Type = result ? AlertTypes.Success : AlertTypes.Error,
                Closeable = true
            });
        }
        private void Reset_Click() {
            CurrentPassword = string.Empty;
            NewPassword = string.Empty;
            ConfirmPassword = string.Empty;
        }

        private async Task Confirm_Click()
        {
            if (NewPassword != ConfirmPassword) {
                Enqueue(false, "两次密码不一致");
                return;
            }
            UserPwdDto userPwdDto = new() {
                passwordOld = CurrentPassword,
                passwordNew = NewPassword
            };
            
           var res = await _ISysUserService.ChangeSysUserPwd(userPwdDto);

            var result = (res?.Type == "success");
            var context = result ? "密码修改成功" : "密码修改失败";
            Enqueue(result, context);
        }
        private void SetDateTime() {
            _TimeMenu = false;
            _UserInfo.JoinDate = Convert.ToDateTime(_Date.ToString("yyyy-MM-dd"));
        }

        public void Dispose()
        {

        }
    }
}
