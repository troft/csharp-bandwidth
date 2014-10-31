namespace Bandwidth.Net.Model
{
    public class TimeoutEvent : BaseEvent
    {
        public string CallId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string CallUri { get; set; }
    }
}
