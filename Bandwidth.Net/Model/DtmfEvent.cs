namespace Bandwidth.Net.Model
{
    public class DtmfEvent : BaseEvent
    {
        public string CallId { get; set; }
        public string CallUri { get; set; }
        public string DtmfDigit { get; set; }
        public string DtmfDuration { get; set; }
    }
}
