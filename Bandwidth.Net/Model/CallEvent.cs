namespace Bandwidth.Net.Model
{
    /// <summary>
    /// Base class for call events
    /// </summary>
    public abstract class CallEvent: BaseEvent
    {
        /// <summary>
        /// Id of call
        /// </summary>
        public string CallId { get; set; }
        
        /// <summary>
        /// "From" phone number
        /// </summary>
        public string From { get; set; }
        
        /// <summary>
        /// "To" phone number
        /// </summary>
        public string To { get; set; }
        
        /// <summary>
        /// URI of thr call
        /// </summary>
        public string CallUri { get; set; }
        
        /// <summary>
        /// State of the call
        /// </summary>
        public string CallState { get; set; }
    }

}
