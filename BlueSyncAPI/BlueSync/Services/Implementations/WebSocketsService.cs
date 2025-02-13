namespace BlueSync.Services.Implementations
{
    using BlueSync.Services.Interfaces;
    using System.Net.WebSockets;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class WebSocketsService : IWebSocketsService
    {
        private readonly List<WebSocket> _clients = new();

        public async Task AddClient(WebSocket socket)
        {
            _clients.Add(socket);
            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                Console.WriteLine($"[WebSocket] Received: {message}");

                if (result.CloseStatus.HasValue)
                {
                    await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                    _clients.Remove(socket);
                    return;
                }

                // Echo message back for debugging
                await SendMessageAsync("echo", new { receivedMessage = message });
            }
        }

        public async Task SendMessageAsync(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(buffer);

            foreach (var client in _clients)
            {
                if (client.State == WebSocketState.Open)
                {
                    await client.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

        public async Task SendMessageAsync(string messageType, object data)
        {
            var messageObject = new { type = messageType, payload = data };
            var messageJson = JsonSerializer.Serialize(messageObject);
            var buffer = Encoding.UTF8.GetBytes(messageJson);
            var segment = new ArraySegment<byte>(buffer);

            foreach (var client in _clients)
            {
                if (client.State == WebSocketState.Open)
                {
                    await client.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}
