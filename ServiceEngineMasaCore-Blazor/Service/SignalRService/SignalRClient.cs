using Furion.InstantMessaging;
using Microsoft.AspNetCore.SignalR.Client;

namespace ServiceEngineMasaCore.Blazor.Service.SignalRService
{
    [MapHub("/hubs/onlineUser")]
    public class SignalRClient
    {
        private readonly string _hubUrl;
        private HubConnection _hubConnection;

        public event Action<string, string> MessageReceived;


        public SignalRClient(string hubUrl)
        {
            _hubUrl = hubUrl;
        }

        public async Task StartAsync()
        {
            if (_hubConnection != null)
            {
                throw new InvalidOperationException("The SignalR client is already started.");
            }

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_hubUrl)
                .Build();

            _hubConnection.On<string, string>("PublicNotice", (user, message) =>
            {
                MessageReceived?.Invoke(user, message);
            });

            await _hubConnection.StartAsync();
        }

        public async Task SendMessageAsync(string user, string message)
        {
            if (_hubConnection == null)
            {
                throw new InvalidOperationException("The SignalR client is not started.");
            }

            await _hubConnection.InvokeAsync("SendMessage", user, message);
        }

        public async Task StopAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.StopAsync();
                _hubConnection = null;
            }
        }
    }
}
