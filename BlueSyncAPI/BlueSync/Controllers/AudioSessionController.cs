using BlueSync.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlueSync.Controllers
{
    [Route("api/audio-sessions")]
    [ApiController]
    public class AudioSessionController : ControllerBase
    {
        private readonly IAudioSessionService _audioSessionService;

        public AudioSessionController(IAudioSessionService audioSessionService)
        {
            _audioSessionService = audioSessionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sessions = await _audioSessionService.GetSessionsAsync();
            return Ok(sessions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var session = await _audioSessionService.GetSessionByIdAsync(id);
            if (session == null) return NotFound();
            return Ok(session);
        }

        [HttpPost("start/{groupId}")]
        public async Task<IActionResult> StartSession(int groupId, [FromBody] string audioSource)
        {
            await _audioSessionService.StartSessionAsync(groupId, audioSource);
            return Ok();
        }

        [HttpDelete("stop/{sessionId}")]
        public async Task<IActionResult> StopSession(int sessionId)
        {
            var resultMessage = await _audioSessionService.StopSessionAsync(sessionId);
            return Ok(new { message = resultMessage });
        }


        [HttpPost("toggle-playback/{sessionId}")]
        public async Task<IActionResult> TogglePlayback(int sessionId)
        {
            await _audioSessionService.TogglePlaybackAsync(sessionId);
            return NoContent();
        }



        [HttpPost("queue/add/{sessionId}")]
        public async Task<IActionResult> AddToQueue(int sessionId, [FromBody] string audioSource)
        {
            var updatedQueue = await _audioSessionService.AddToQueueAsync(sessionId, audioSource);
            return Ok(updatedQueue);
        }

        [HttpPut("queue/change/{sessionId}")]
        public async Task<IActionResult> ChangeQueue(int sessionId, [FromBody] List<string> newQueue)
        {
            await _audioSessionService.ChangeQueueAsync(sessionId, newQueue);
            return Ok();
        }

        [HttpGet("queue/{sessionId}")]
        public async Task<IActionResult> GetQueue(int sessionId)
        {
            var queue = await _audioSessionService.GetQueueAsync(sessionId);
            return Ok(queue);
        }
    }

}
