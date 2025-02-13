using Azure.Core;
using BlueSync.Models.Domains;
using BlueSync.Models.DTOs;
using BlueSync.Repositories.Interfaces;
using BlueSync.Services.Interfaces;

namespace BlueSync.Services.Implementations
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepo;
        private readonly IWebSocketsService _webSocketService;

        public DeviceService(IDeviceRepository deviceRepo, IWebSocketsService webSocketService)
        {
            _deviceRepo = deviceRepo;
            _webSocketService = webSocketService;
        }

        public async Task<IEnumerable<Device>> GetDevicesAsync()
        {
            return await _deviceRepo.GetAllAsync();
        }


        public async Task<Device?> GetDeviceByIdAsync(int id)
        {
            return await _deviceRepo.GetByIdAsync(id);
        }

        public async Task AddDeviceAsync(DeviceRequestDto request)
        {
            var device = new Device
            {
                Name = request.Name,
                MacAddress = request.MacAddress,
                IsConnected = request.IsConnected,
               
                Volume = request.Volume
            };

            await _deviceRepo.AddAsync(device);

          
        }

        public async Task<DeviceResponseDto?> UpdateDeviceAsync(int id, DeviceRequestDto request)
        {
            var existingDevice = await _deviceRepo.GetByIdAsync(id);
            if (existingDevice == null) return null;

            existingDevice.Name = request.Name;
            existingDevice.MacAddress = request.MacAddress;
            existingDevice.IsConnected = request.IsConnected;
          
            existingDevice.Volume = request.Volume;

            await _deviceRepo.UpdateAsync(existingDevice);

            return new DeviceResponseDto
            {
                Id = existingDevice.Id,
                Name = existingDevice.Name,
                MacAddress = existingDevice.MacAddress,
                IsConnected = existingDevice.IsConnected,
               
                Volume = existingDevice.Volume
            };
        }

        public async Task RemoveDeviceAsync(int id)
        {
            await _deviceRepo.DeleteAsync(id);
        }

        public async Task SetVolumeAsync(int deviceId, int volume)
        {
            var device = await _deviceRepo.GetByIdAsync(deviceId);
            if (device == null) return;

            if (volume == 0) // Mute action
            {
                if (!device.IsMuted) // Only store previous volume if it's not already muted
                {
                    device.PreviousVolume = device.Volume;
                }
                device.IsMuted = true;
            }
            else
            {
                if (device.IsMuted) // If unmuting, restore previous volume if available
                {
                    volume = device.PreviousVolume ?? 50; // Default to 50 if null
                }
                device.IsMuted = false;
            }

            device.Volume = Math.Clamp(volume, 0, 100); // Ensure volume is between 0-100
            await _deviceRepo.UpdateAsync(device);

            // Notify frontend via WebSocket
            await _webSocketService.SendMessageAsync("volume_updated", new { device.Id, device.Volume });

        }
        public async Task MuteAsync(int deviceId)
        {
            var device = await _deviceRepo.GetByIdAsync(deviceId);
            if (device == null) return;

            device.IsMuted = true;
            await _deviceRepo.UpdateAsync(device);

            // Notify frontend via WebSocket
            await _webSocketService.SendMessageAsync("device_muted", new { device.Id, device.IsMuted });
        }
        public async Task UnmuteAsync(int deviceId)
        {
            var device = await _deviceRepo.GetByIdAsync(deviceId);
            if (device == null) return;

            device.IsMuted = false;
            await _deviceRepo.UpdateAsync(device);

            // Notify frontend via WebSocket
            await _webSocketService.SendMessageAsync("device_unmuted", new { device.Id, device.IsMuted });
        }

    }

}
