using BlueSync.Data;
using BlueSync.Models.Domains;
using BlueSync.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlueSync.Repositories.Implementations
{
    public class AudioSessionRepository : IAudioSessionRepository
    {
        private readonly ApplicationDbContext _context;

        public AudioSessionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AudioSession>> GetAllAsync()
        {
            return await _context.AudioSessions.Include(s => s.Group).ToListAsync();
        }

        public async Task<AudioSession?> GetByIdAsync(int id)
        {
            return await _context.AudioSessions.Include(s => s.Group).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(AudioSession session)
        {
            await _context.AudioSessions.AddAsync(session);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AudioSession session)
        {
            Console.WriteLine($"[DEBUG] Updating Session {session.Id}: IsPlaying = {session.IsPlaying}");

            _context.AudioSessions.Update(session);
            await _context.SaveChangesAsync();

            // Confirm update by fetching it again
            var checkSession = await _context.AudioSessions.FindAsync(session.Id);
            Console.WriteLine($"[DEBUG] Confirming Update: Session {session.Id} - IsPlaying = {checkSession.IsPlaying}");
        }


        public async Task DeleteAsync(int id)
        {
            var session = await _context.AudioSessions.FindAsync(id);
            if (session != null)
            {
                _context.AudioSessions.Remove(session);
                await _context.SaveChangesAsync();
            }
        }
    }

}
