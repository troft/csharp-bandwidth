namespace Bandwidth.Net
{
    public class Call
    {
        public string From { get; set; }
        public string To { get; set; }
        public string CallbackUrl { get; set; }
        public bool? RecordingEnabled { get; set; }
        public string BridgeId { get; set; }
    }
}