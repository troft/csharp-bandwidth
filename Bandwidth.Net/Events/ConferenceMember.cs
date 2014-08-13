using System;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Events
{
    public class ConferenceMember : Event
    {
        public int ActiveMembers { get; set; }
        public string CallId { get; set; }
        public string ConferenceId { get; set; }
        public bool Hold { get; set; }
        public string MemberId { get; set; }
        public Uri MemberUri { get; set; }
        public bool Mute { get; set; }
        public MemberState? State { get; set; }
    }
}
