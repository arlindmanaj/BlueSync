using BlueSync.Models.Domains;

namespace BlueSync.Repositories.Interfaces
{
    public interface IDeviceGroupRepository
    {
        Task<IEnumerable<DeviceGroup>> GetAllAsync();
        Task<DeviceGroup?> GetByIdAsync(int id);
        Task AddAsync(DeviceGroup group);
        Task UpdateAsync(DeviceGroup group);
        Task DeleteAsync(int id);
    }
}
