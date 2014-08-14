using System;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Events
{
    public class Conference : Event
    {
        public string ConferenceId { get; set; }
        public Uri ConferenceUri { get; set; }
        public ConferenceState? Status { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? CompletedTime { get; set; }
    }
}