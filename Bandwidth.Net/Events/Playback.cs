using System;

namespace Bandwidth.Net.Events
{
    public class Playback : Event
    {
        public string CallId { get; set; }
        public Uri CallUri { get; set; }
        public string Tag { get; set; }
        public PlaybackStatus? Status { get; set; }
    }

    public enum PlaybackStatus
    {
        Done,
        Started
    }
}