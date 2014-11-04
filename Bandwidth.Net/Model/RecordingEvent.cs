using System;

namespace Bandwidth.Net.Model
{
    public class RecordingEvent : BaseEvent
    {
        public string CallId { get; set; }
        public string RecordingId { get; set; }
        public string RecordingUri { get; set; }
        public RecordingState State { get; set; }
        public string Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
