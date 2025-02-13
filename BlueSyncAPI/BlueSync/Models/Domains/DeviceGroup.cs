namespace BlueSync.Models.Domains
{
    public class DeviceGroup
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Device> Devices { get; set; } = new List<Device>();
    }
}
