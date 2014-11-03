namespace Bandwidth.Net.Model
{
    public class SmsEvent : BaseEvent
    {
        public string Direction { get; set; }
        public string MessageId { get; set; }
        public string MessageUrl { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Text { get; set; }
        public string ApplicationId { get; set; }
        public string State { get; set; }
    }
}
