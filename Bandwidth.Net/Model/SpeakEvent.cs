namespace Bandwidth.Net.Model
{
    public class SpeakEvent : BaseEvent
    {
        public string CallId { get; set; }
        public string CallUri { get; set; }
        public string Status { get; set; }
        public string State { get; set; }
    }
}
