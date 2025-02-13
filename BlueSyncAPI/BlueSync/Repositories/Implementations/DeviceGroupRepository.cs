using BlueSync.Data;
using BlueSync.Models.Domains;
using BlueSync.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlueSync.Repositories.Implementations
{
    public class DeviceGroupRepository : IDeviceGroupRepository
    {
        private readonly ApplicationDbContext _context;

        public DeviceGroupRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DeviceGroup>> GetAllAsync()
        {
            return await _context.DeviceGroups.Include(g => g.Devices).ToListAsync();
        }

        public async Task<DeviceGroup?> GetByIdAsync(int id)
        {
            return await _context.DeviceGroups.Include(g => g.Devices).FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task AddAsync(DeviceGroup group)
        {
            await _context.DeviceGroups.AddAsync(group);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DeviceGroup group)
        {
            _context.DeviceGroups.Update(group);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var group = await _context.DeviceGroups.FindAsync(id);
            if (group != null)
            {
                _context.DeviceGroups.Remove(group);
                await _context.SaveChangesAsync();
            }
        }
    }

}
