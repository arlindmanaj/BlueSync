using BlueSync.Services.Interfaces;

namespace BlueSync.Services.Implementations
{
    public class AudioQueueManager : IAudioQueueManager
    {
        private readonly Dictionary<int, List<string>> _audioQueues = new();

        public List<string> GetQueue(int sessionId)
        {
            if (!_audioQueues.ContainsKey(sessionId))
            {
                _audioQueues[sessionId] = new List<string>(); // Initialize queue
            }
            return _audioQueues[sessionId];
        }

        public void AddToQueue(int sessionId, string audioSource)
        {
            if (!_audioQueues.ContainsKey(sessionId))
            {
                _audioQueues[sessionId] = new List<string>();
            }
            _audioQueues[sessionId].Add(audioSource);
        }

        public void RemoveQueue(int sessionId)
        {
            _audioQueues.Remove(sessionId);
        }

        public void SetQueue(int sessionId, List<string> newQueue)
        {
            _audioQueues[sessionId] = newQueue;
        }

    }

}
