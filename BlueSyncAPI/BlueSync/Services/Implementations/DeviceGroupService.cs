using BlueSync.Models.Domains;
using BlueSync.Repositories.Interfaces;
using BlueSync.Services.Interfaces;
using BlueSync.Models.DTOs;

public class DeviceGroupService : IDeviceGroupService
{
    private readonly IDeviceGroupRepository _groupRepo;
    private readonly IDeviceRepository _deviceRepo;

    public DeviceGroupService(IDeviceGroupRepository groupRepo, IDeviceRepository deviceRepo)
    {
        _groupRepo = groupRepo;
        _deviceRepo = deviceRepo;
    }

    public async Task<IEnumerable<DeviceGroupResponseDto>> GetGroupsAsync()
    {
        var groups = await _groupRepo.GetAllAsync();
        return groups.Select(group => new DeviceGroupResponseDto
        {
            Id = group.Id,
            Name = group.Name ?? string.Empty,
            DeviceIds = group.Devices.Select(d => d.Id).ToList()
        });
    }

    public async Task<DeviceGroupResponseDto?> GetGroupByIdAsync(int id)
    {
        var group = await _groupRepo.GetByIdAsync(id);
        if (group == null) return null;

        return new DeviceGroupResponseDto
        {
            Id = group.Id,
            Name = group.Name ?? string.Empty,
            DeviceIds = group.Devices.Select(d => d.Id).ToList()
        };
    }

    public async Task AddGroupAsync(DeviceGroupRequestDto request)
    {
        var devices = await _deviceRepo.GetByIdsAsync(request.DeviceIds);
        var group = new DeviceGroup
        {
            Name = request.Name,
            Devices = devices
        };

        await _groupRepo.AddAsync(group);
    }

    public async Task UpdateGroupAsync(int id, DeviceGroupRequestDto request)
    {
        var group = await _groupRepo.GetByIdAsync(id);
        if (group == null) return;

        var devices = await _deviceRepo.GetByIdsAsync(request.DeviceIds);
        group.Name = request.Name;
        group.Devices = devices;

        await _groupRepo.UpdateAsync(group);
    }

    public async Task RemoveGroupAsync(int id)
    {
        await _groupRepo.DeleteAsync(id);
    }

    public async Task AssignDeviceToGroupAsync(int groupId, int deviceId)
    {
        var group = await _groupRepo.GetByIdAsync(groupId);
        var device = await _deviceRepo.GetByIdAsync(deviceId);

        if (group == null || device == null) return;

        group.Devices.Add(device);
        await _groupRepo.UpdateAsync(group);
    }
}
