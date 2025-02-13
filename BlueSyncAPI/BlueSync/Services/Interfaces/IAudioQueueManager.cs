namespace BlueSync.Services.Interfaces
{
    public interface IAudioQueueManager
    {
        List<string> GetQueue(int sessionId);
        void AddToQueue(int sessionId, string audioSource);
        void RemoveQueue(int sessionId);
        void SetQueue(int sessionId, List<string> newQueue);
    }
}
