namespace BlueSync.Models.Domains
{
    public class Device
    {
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string MacAddress { get; set; } = string.Empty; 
        public bool IsConnected { get; set; } = false; 
        public int Volume { get; set; } = 50;
        public bool IsMuted { get; set; } = false;
        public int? PreviousVolume { get; set; }
    }

}
