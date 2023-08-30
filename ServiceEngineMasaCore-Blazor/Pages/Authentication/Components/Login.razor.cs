using Masa.Blazor;
using ServiceEngine.Core.Service;
using ServiceEngine.Core.Util;
using ServiceEngineMasaCore.Blazor.Service.Login.Interface;
using ServiceEngineMasaCore.Blazor.Service.Menu.Interface;
using ServiceEngineMasaCore.Blazor.Service.UserInfo;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Interface;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.Authentication.Components
{
    public partial class Login
    {
        private bool _show;
        private bool _loading;
        private readonly LoginInput _model = new LoginInput() { Account = "admin", Password = "123456" };

        [Inject]
        [NotNull]
        ISysAuthService? _ISysAuthService { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; } = default!;

        [Parameter]
        public bool HideLogo { get; set; }

        [Parameter]
        public double Width { get; set; } = 410;

        [Parameter]
        public StringNumber? Elevation { get; set; }

        [Parameter]
        public string CreateAccountRoute { get; set; } = $"pages/authentication/register-v1";

        [Parameter]
        public string ForgotPasswordRoute { get; set; } = $"pages/authentication/forgot-password-v1";

        [Parameter]
        public EventCallback<MouseEventArgs> OnLogin { get; set; }

        #region Method
       
        private async Task SubmitAsync()
        {
            _loading = true;
            var result = await _ISysAuthService.LoginAsync(_model);
            if (result != null && result?.Type == "success")
            {

            }
            else {
                _loading = false;
            }
        }
        
        
        private async Task KeyHandler(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await SubmitAsync();
            }
        }
        #endregion
    }
}