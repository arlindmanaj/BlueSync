using BlueSync.Models.Domains;
using BlueSync.Repositories.Interfaces;
using BlueSync.Services.Interfaces;

namespace BlueSync.Services.Implementations
{
    public class AudioSessionService : IAudioSessionService
    {
        private readonly IAudioSessionRepository _sessionRepo;
        private readonly IWebSocketsService _webSocketService;
        private readonly IAudioQueueManager _queueManager;
        private readonly Dictionary<int, bool> _isPlaying = new(); // Track play state

        public AudioSessionService(IAudioQueueManager queueManager, IAudioSessionRepository sessionRepo, IWebSocketsService webSocketService)
        {
            _sessionRepo = sessionRepo;
            _webSocketService = webSocketService;
            _queueManager = queueManager;
        }

        public async Task<IEnumerable<AudioSession>> GetSessionsAsync()
        {
            return await _sessionRepo.GetAllAsync();
        }

        public async Task<AudioSession?> GetSessionByIdAsync(int id)
        {
            return await _sessionRepo.GetByIdAsync(id);
        }

        public async Task StartSessionAsync(int groupId, string audioSource)
        {
            var session = new AudioSession
            {
                GroupId = groupId,
                AudioSource = audioSource,
                IsPlaying = true
            };

            await _sessionRepo.AddAsync(session);

            // Initialize queue using _queueManager (instead of _audioQueues)
            _queueManager.SetQueue(session.Id, new List<string> { audioSource });

            _isPlaying[session.Id] = true;

            // Notify frontend
            await _webSocketService.SendMessageAsync("session_started", new { session.Id, session.AudioSource });
        }

        public async Task<string> StopSessionAsync(int sessionId)
        {
            var session = await _sessionRepo.GetByIdAsync(sessionId);
            if (session == null)
            {
                return $"Session {sessionId} not found.";
            }

            await _sessionRepo.DeleteAsync(sessionId);

            _isPlaying.Remove(sessionId);
            _queueManager.RemoveQueue(sessionId); // Remove queue from memory

            await _webSocketService.SendMessageAsync("session_stopped", new { sessionId });

            return $"Successfully deleted session: {sessionId}";
        }


        public async Task TogglePlaybackAsync(int sessionId)
        {
            var session = await _sessionRepo.GetByIdAsync(sessionId);
            if (session == null) return;

            session.IsPlaying = !session.IsPlaying; // Simple toggle

            await _sessionRepo.UpdateAsync(session);

            string messageType = session.IsPlaying ? "playback_playing" : "playback_paused";
            await _webSocketService.SendMessageAsync(messageType, new { sessionId });
        }



        public async Task<List<string>> AddToQueueAsync(int sessionId, string audioSource)
        {
            _queueManager.AddToQueue(sessionId, audioSource);

            // Notify frontend
            await _webSocketService.SendMessageAsync("queue_updated", new { sessionId, Queue = _queueManager.GetQueue(sessionId) });

            return _queueManager.GetQueue(sessionId);
        }

        public async Task ChangeQueueAsync(int sessionId, List<string> newQueue)
        {
            _queueManager.SetQueue(sessionId, newQueue); // Corrected method usage

            // Notify frontend
            await _webSocketService.SendMessageAsync("queue_changed", new { sessionId, Queue = newQueue });
        }

        public async Task<List<string>> GetQueueAsync(int sessionId)
        {
            return _queueManager.GetQueue(sessionId);
        }
    }
}
