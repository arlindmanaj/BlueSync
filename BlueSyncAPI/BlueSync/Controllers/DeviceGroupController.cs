using BlueSync.Models.Domains;
using BlueSync.Models.DTOs;
using BlueSync.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlueSync.Controllers
{
    [ApiController]
    [Route("api/device-groups")]
    public class DeviceGroupController : ControllerBase
    {
        private readonly IDeviceGroupService _deviceGroupService;

        public DeviceGroupController(IDeviceGroupService deviceGroupService)
        {
            _deviceGroupService = deviceGroupService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var groups = await _deviceGroupService.GetGroupsAsync();
            return Ok(groups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var group = await _deviceGroupService.GetGroupByIdAsync(id);
            if (group == null) return NotFound();
            return Ok(group);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DeviceGroupRequestDto request)
        {
            await _deviceGroupService.AddGroupAsync(request);
            return CreatedAtAction(nameof(GetAll), new { });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DeviceGroupRequestDto request)
        {
            await _deviceGroupService.UpdateGroupAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _deviceGroupService.RemoveGroupAsync(id);
            return NoContent();
        }

        [HttpPost("{groupId}/assign-device/{deviceId}")]
        public async Task<IActionResult> AssignDeviceToGroup(int groupId, int deviceId)
        {
            await _deviceGroupService.AssignDeviceToGroupAsync(groupId, deviceId);
            return NoContent();
        }
    }


}
