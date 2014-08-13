using System;
using System.Runtime.Serialization;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Events
{
    public class Speak: Event
    {
        public string CallId { get; set; }
        public Uri CallUri { get; set; }
        public string Tag { get; set; }
        public SpeakType? Type { get; set; }
        public SpeakStatus? Status { get; set; }
    }

    public enum SpeakType
    {
        [EnumMember(Value = "PLAYBACK_STOP")]
        PlaybackStop,
        [EnumMember(Value = "PLAYBACK_START")]
        PlaybackStart
    }

    public enum SpeakStatus
    {
        Done,
        Started
    }
}
