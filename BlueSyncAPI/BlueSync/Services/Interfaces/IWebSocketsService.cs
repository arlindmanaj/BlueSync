using System.Net.WebSockets;

namespace BlueSync.Services.Interfaces
{
    public interface IWebSocketsService
    {
        Task AddClient(WebSocket socket);
        Task SendMessageAsync(string message);
        Task SendMessageAsync(string messageType, object data);


    }
}
