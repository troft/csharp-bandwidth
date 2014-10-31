namespace Bandwidth.Net.Model
{
    public abstract class CallEvent: BaseEvent
    {
        public string CallId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string CallUri { get; set; }
        public CallState CallState { get; set; }
    }

}
