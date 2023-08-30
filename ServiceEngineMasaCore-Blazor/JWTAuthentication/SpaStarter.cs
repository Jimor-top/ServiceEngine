using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics.CodeAnalysis;
using IComponent = Microsoft.AspNetCore.Components.IComponent;

namespace ServiceEngineMasaCore.Blazor.JWTAuthentication
{

    /// <summary>
    /// SPA启动组件
    /// </summary>
    public sealed class SpaStarter : IComponent, IHandleAfterRender, IAsyncDisposable
    {
        private readonly RenderFragment _fragment;

        private bool _initialized;
        private bool _hasCalledOnAfterRender;
        private IJSInvoker? _invoker;
        private RenderHandle _renderHandle;

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter]
        public Func<HubConnection, AuthenticationState, Task<bool>>? OnConnectSignalR { get; set; }

        public SpaStarter()
        {
            _fragment = new RenderFragment(__builder =>
            {
                __builder.OpenComponent<JSInvoker>(0);
                __builder.AddComponentReferenceCapture(1, x => _invoker = (JSInvoker)x);
                __builder.CloseComponent();
                if (_hasCalledOnAfterRender)
                {
                    __builder.OpenComponent<SpaLoader>(2);
                    __builder.AddAttribute(3, nameof(SpaLoader.ChildContent), ChildContent);
                    __builder.CloseComponent();
                }
            });
        }

        [Inject]
        [NotNull]
        IJSInvokerService? JSInvokerService { get; set; }

        void IComponent.Attach(RenderHandle renderHandle)
        {
            if (_renderHandle.IsInitialized)
            {
                throw new InvalidOperationException("The render handle is already set. Cannot initialize a ComponentBase more than once.");
            }

            _renderHandle = renderHandle;
        }

        Task IComponent.SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            if (!_initialized)
            {
                _initialized = true;
                _renderHandle.Render(_fragment);
            }

            return Task.CompletedTask;
        }

        async Task IHandleAfterRender.OnAfterRenderAsync()
        {
            if (!_hasCalledOnAfterRender)
            {
                _hasCalledOnAfterRender = true;

                await JSInvokerService.ConnectAsync(_invoker);
                _renderHandle.Render(_fragment);
            }
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await JSInvokerService.DisconnectAsync();
        }
    }
}
