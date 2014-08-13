using System;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Events
{
    public class Recording : Event
    {
        public string CallId { get; set; }
        public string RecordingId { get; set; }
        public Uri RecordingUri { get; set; }
        public string Tag { get; set; }
        public RecordingState? Status { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
