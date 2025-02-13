using BlueSync.Models.Domains;
using BlueSync.Models.DTOs;
using BlueSync.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlueSync.Controllers
{
    [Route("api/devices")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var devices = await _deviceService.GetDevicesAsync();
            return Ok(devices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var device = await _deviceService.GetDeviceByIdAsync(id);
            if (device == null) return NotFound();
            return Ok(device);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DeviceRequestDto request)
        {
            await _deviceService.AddDeviceAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = request.Name }, request);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DeviceRequestDto request)
        {
            if (id <= 0) return BadRequest("Invalid device ID");

            var updatedDevice = await _deviceService.UpdateDeviceAsync(id, request);
            if (updatedDevice == null) return NotFound();

            return Ok(updatedDevice);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _deviceService.RemoveDeviceAsync(id);
            return NoContent();
        }

        [HttpPut("{id}/volume/{level}")]
        public async Task<IActionResult> SetVolume(int id, int level)
        {
            await _deviceService.SetVolumeAsync(id, level);
            return NoContent();
        }


    }

}
