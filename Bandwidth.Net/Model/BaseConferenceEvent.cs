namespace Bandwidth.Net.Model
{
    /// <summary>
    /// Base class for all conference events
    /// </summary>
    public abstract class BaseConferenceEvent : BaseEvent
    {
        /// <summary>
        /// The unique identifier of the conference
        /// </summary>
        public string ConferenceId { get; set; }
        
        /// <summary>
        /// Uri of the condefence
        /// </summary>
        public string ConferenceUri { get; set; }
        
        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }
    }
}
