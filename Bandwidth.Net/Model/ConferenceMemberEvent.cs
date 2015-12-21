namespace Bandwidth.Net.Model
{
    /// <summary>
    /// Bandwidth API sends this message to the application when a conference member has joined / left the conference or when it as muted or put on hold
    /// </summary>
    public class ConferenceMemberEvent : BaseConferenceEvent
    {
        /// <summary>
        /// Active members count
        /// </summary>
        public int ActiveMembers { get; set; }
        
        /// <summary>
        /// Id of the Call
        /// </summary>
        public string CallId { get; set; }
        
        /// <summary>
        /// Hold
        /// </summary>
        public bool Hold { get; set; }
        
        /// <summary>
        /// Id od the member
        /// </summary>
        public string MemberId { get; set; }
        
        /// <summary>
        /// Url of the member
        /// </summary>
        public string MemberUri { get; set; }
        
        /// <summary>
        /// Mute
        /// </summary>
        public bool Mute { get; set; }
        
        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; }

    }
}
