using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public class ConferenceMemberEvent : BaseConferenceEvent
    {
        public int ActiveMembers { get; set; }
        public string CallId { get; set; }
        public bool Hold { get; set; }
        public string MemberId { get; set; }
        public string MemberUri { get; set; }
        public bool Mute { get; set; }
        public string State { get; set; }

    }
}
