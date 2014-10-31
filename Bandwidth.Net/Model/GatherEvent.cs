namespace Bandwidth.Net.Model
{
    public class GatherEvent : BaseEvent
    {
        public string CallId { get; set; }
        public string Tag { get; set; }
        public string Reason { get; set; }
        public string GatherId { get; set; }
        public string State { get; set; }
        public string Digits { get; set; }
    }
}
