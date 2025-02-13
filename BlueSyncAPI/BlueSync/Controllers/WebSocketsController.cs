using BlueSync.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlueSync.Controllers
{
    [Route("ws")]
    public class WebSocketController : ControllerBase
    {
        private readonly IWebSocketsService _webSocketService;

        public WebSocketController(IWebSocketsService webSocketService)
        {
            _webSocketService = webSocketService;
        }

        [HttpGet]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await _webSocketService.AddClient(socket);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }
    }

}
