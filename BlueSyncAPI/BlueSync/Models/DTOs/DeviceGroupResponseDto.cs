namespace BlueSync.Models.DTOs
{
    public class DeviceGroupResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<int> DeviceIds { get; set; } = new();
    }

}
