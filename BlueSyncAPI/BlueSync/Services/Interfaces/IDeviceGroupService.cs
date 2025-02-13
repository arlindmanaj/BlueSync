using BlueSync.Models.Domains;
using BlueSync.Models.DTOs;

namespace BlueSync.Services.Interfaces
{
    public interface IDeviceGroupService
    {
        Task<IEnumerable<DeviceGroupResponseDto>> GetGroupsAsync();
        Task<DeviceGroupResponseDto?> GetGroupByIdAsync(int id);
        Task AddGroupAsync(DeviceGroupRequestDto request);
        Task UpdateGroupAsync(int id, DeviceGroupRequestDto request);
        Task RemoveGroupAsync(int id);
        Task AssignDeviceToGroupAsync(int deviceId, int groupId);

    }
}
