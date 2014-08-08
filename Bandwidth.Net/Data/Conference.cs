using System;

namespace Bandwidth.Net.Data
{
    public class Conference
    {
        public string Id { get; set; }
        public int? ActiveMembers { get; set; }
        public Uri CallbackUrl { get; set; }
        public int? CallbackTimeout { get; set; }
        public Uri FallbackUrl { get; set; }
        public DateTime? CompletedTime { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string From { get; set; }
        public ConferenceState? State { get; set; }
    }

    public class ConferenceMember
    {
        public string Id { get; set; }
        public string CallId { get; set; }
        public DateTime? AddedTime { get; set; }
        public Uri Call { get; set; }
        public bool? Hold { get; set; }
        public bool? Mute { get; set; }
        public DateTime? RemovedTime { get; set; }
        public bool? JoinTone { get; set; }
        public bool? LeavingTone { get; set; }
        public MemberState? State { get; set; }
    }

    public enum ConferenceState
    {
        Created,
        Active,
        Completed
    }

    public enum MemberState
    {
        Active,
        Completed
    }
}
