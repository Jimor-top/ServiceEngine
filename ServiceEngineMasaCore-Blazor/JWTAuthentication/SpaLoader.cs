using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using ServiceEngineMasaCore.Blazor.Service.Login.Interface;
using ServiceEngineMasaCore.Blazor.Service.Menu;
using ServiceEngineMasaCore.Blazor.Service.Menu.Interface;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Interface;
using System.Diagnostics.CodeAnalysis;
using IComponent = Microsoft.AspNetCore.Components.IComponent;

namespace ServiceEngineMasaCore.Blazor.JWTAuthentication
{

    /// <summary>
    /// SPA数据加载组件
    /// </summary>
    public sealed class SpaLoader : IComponent, IHandleAfterRender, IDisposable
    {
        private readonly RenderFragment _fragment;

        private bool _initialized;
        private bool _hasCalledOnAfterRender;
        private RenderHandle _renderHandle;
        private Task<AuthenticationState>? _currentStateTask;

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Inject]
        [NotNull]
        IJSRuntime? JS { get; set; }

        [Inject]
        [NotNull]
        JwtAuthenticationStateProvider? _StateProvider { get; set; }

        [Inject]
        [NotNull]
        IMenuStore? _MenuStore { get; set; }

        [Inject]
        [NotNull]
        IUserInfoStore? _IUserInfoStore { get; set; }

        [Inject]
        [NotNull]
        ISysMenuService? _ISysMenuService { get; set; }

        public SpaLoader()
        {
            _fragment = new RenderFragment(__builder =>
            {
                if (_hasCalledOnAfterRender)
                {
                    __builder.OpenComponent<CascadingValue<Task<AuthenticationState>>>(0);
                    __builder.AddAttribute(1, "Value", _currentStateTask);
                    __builder.AddAttribute(2, "ChildContent", ChildContent);
                    __builder.CloseComponent();
                }
            });
        }

        void IComponent.Attach(RenderHandle renderHandle)
        {
            if (_renderHandle.IsInitialized)
            {
                throw new InvalidOperationException("The render handle is already set. Cannot initialize a ComponentBase more than once.");
            }

            _StateProvider.AuthenticationStateChanged += new(OnAuthenticationStateChanged);
            _renderHandle = renderHandle;
            _currentStateTask = _StateProvider.GetAuthenticationStateAsync();
        }

        Task IComponent.SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            if (!_initialized)
            {
                _initialized = true;
                _renderHandle.Render(new RenderFragment(x => { }));
            }

            return Task.CompletedTask;
        }

        async Task IHandleAfterRender.OnAfterRenderAsync()
        {
            if (!_hasCalledOnAfterRender)
            {
                _hasCalledOnAfterRender = true;

                if (!await LoadDataAsync(_currentStateTask))
                {
                    await _StateProvider.ClearAsync(true);
                }

                _renderHandle.Render(_fragment);

                //await JS.NotifyAppIsReady();
                //await JS.HideGlobalOverlay();
            }
        }

        void IDisposable.Dispose()
        {
            _StateProvider.AuthenticationStateChanged -= new(OnAuthenticationStateChanged);
        }

        private async void OnAuthenticationStateChanged(Task<AuthenticationState> newAuthStateTask)
        {
            await LoadDataAsync(newAuthStateTask);
            await _renderHandle.Dispatcher.InvokeAsync(() =>
            {
                _currentStateTask = newAuthStateTask;
                _renderHandle.Render(_fragment);
            });
        }

        private async Task<bool> LoadDataAsync(Task<AuthenticationState>? authStateTask)
        {
            await Task.Delay(3000);

            if (authStateTask is null)
            {
                return false;
            }

            var authState = await authStateTask;
            if (authState.User.Identity?.IsAuthenticated != true)
            {
                return false;
            }

            var res = await InitUserInfoAndMenuRoutes();

            return res;
        }
        private async Task<bool> InitUserInfoAndMenuRoutes()
        {
            // 触发初始化用户信息
            bool res = await _IUserInfoStore.SetUserInfos();
            if (res) {
                // 获取路由菜单数据
               var menuRes = await _ISysMenuService.GetSysMenuTreeAsync();
                if (menuRes != null)
                {
                    _MenuStore.SaveMenu(menuRes);
                }
            }
            return res;
        }
    }
}