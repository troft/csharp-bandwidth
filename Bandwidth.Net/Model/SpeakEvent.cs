namespace Bandwidth.Net.Model
{
    /// <summary>
    /// Speak event
    /// </summary>
    public class SpeakEvent : BaseEvent
    {
        /// <summary>
        /// Id of call
        /// </summary>
        public string CallId { get; set; }
        
        /// <summary>
        /// Url of call
        /// </summary>
        public string CallUri { get; set; }
        
        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }
        
        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; }
    }
}
