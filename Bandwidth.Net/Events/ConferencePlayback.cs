using System;

namespace Bandwidth.Net.Events
{
    public class ConferencePlayback : Event
    {
        public string ConferenceId { get; set; }
        public Uri ConferenceUri { get; set; }
        public PlaybackStatus? Status { get; set; }
    }
}