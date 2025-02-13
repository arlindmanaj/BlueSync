using BlueSync.Models.Domains;

namespace BlueSync.Repositories.Interfaces
{
    public interface IAudioSessionRepository
    {
        Task<IEnumerable<AudioSession>> GetAllAsync();
        Task<AudioSession?> GetByIdAsync(int id);
        Task AddAsync(AudioSession session);
        Task UpdateAsync(AudioSession session);
        Task DeleteAsync(int id);
    }
}
