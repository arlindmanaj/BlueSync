namespace BlueSync.Models.DTOs
{
    public class DeviceResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MacAddress { get; set; } = string.Empty;
        public bool IsConnected { get; set; } = false;
        
        public int Volume { get; set; }
    }

}
