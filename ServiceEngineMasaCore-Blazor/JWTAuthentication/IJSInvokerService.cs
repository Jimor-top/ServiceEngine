
using Microsoft.AspNetCore.SignalR.Client;

namespace ServiceEngineMasaCore.Blazor.JWTAuthentication
{
    public interface IJSInvokerService
    {
        Task ConnectAsync(IJSInvoker? invoker, Func<HubConnection, Task>? signalRInitializer = null);

        Task DisconnectAsync();
    }
}
