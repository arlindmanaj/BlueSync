using BlueSync.Models.Domains;

namespace BlueSync.Services.Interfaces
{
    public interface IAudioSessionService
    {
        Task<IEnumerable<AudioSession>> GetSessionsAsync();
        Task<AudioSession?> GetSessionByIdAsync(int id);
        Task StartSessionAsync(int groupId, string audioSource);
        Task<string> StopSessionAsync(int sessionId);
        Task TogglePlaybackAsync(int sessionId);
        Task<List<string>> AddToQueueAsync(int sessionId, string audioSource);
        Task ChangeQueueAsync(int sessionId, List<string> newQueue);
        Task<List<string>> GetQueueAsync(int sessionId);
    }
}
