namespace BlueSync.Models.Domains
{
    public class AudioSession
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public DeviceGroup Group { get; set; }
        public string? AudioSource { get; set; } // URL or file path
        public bool IsPlaying { get; set; }
    }
}
