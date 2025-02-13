namespace BlueSync.Models.DTOs
{
    public class DeviceGroupRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public List<int> DeviceIds { get; set; } = new(); // Only device IDs, no full objects
    }

}
