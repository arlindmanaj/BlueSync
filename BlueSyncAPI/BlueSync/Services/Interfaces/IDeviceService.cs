using BlueSync.Models.Domains;
using BlueSync.Models.DTOs;

namespace BlueSync.Services.Interfaces
{
    public interface IDeviceService
    {
        Task<IEnumerable<Device>> GetDevicesAsync();
        Task<Device?> GetDeviceByIdAsync(int id);
        Task AddDeviceAsync(DeviceRequestDto deviceRequestDto);
        Task<DeviceResponseDto?> UpdateDeviceAsync(int id, DeviceRequestDto request);
        Task RemoveDeviceAsync(int id);
        Task SetVolumeAsync(int deviceId, int volume);
        Task MuteAsync(int deviceId);
        Task UnmuteAsync(int deviceId);
    }
}
