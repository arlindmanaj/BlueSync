using BlueSync.Models.Domains;
using Microsoft.EntityFrameworkCore;

namespace BlueSync.Repositories.Interfaces
{
    public interface IDeviceRepository
    {
        Task<IEnumerable<Device>> GetAllAsync();
        Task<Device?> GetByIdAsync(int id);
        Task<List<Device>> GetByIdsAsync(List<int> ids);
        
        Task AddAsync(Device device);
        Task UpdateAsync(Device device);
        Task DeleteAsync(int id);
    }
}
