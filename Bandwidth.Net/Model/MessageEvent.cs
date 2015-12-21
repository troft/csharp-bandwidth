namespace Bandwidth.Net.Model
{
    /// <summary>
    /// Common event for SMS and MMS
    /// </summary>
    public abstract class MessageEvent : BaseEvent
    {
        /// <summary>
        /// Direction
        /// </summary>
        public string Direction { get; set; }
        
        /// <summary>
        /// Id of message
        /// </summary>
        public string MessageId { get; set; }
        
        /// <summary>
        /// Url of message
        /// </summary>
        public string MessageUrl { get; set; }
        
        /// <summary>
        /// From
        /// </summary>
        public string From { get; set; }
        
        /// <summary>
        /// To
        /// </summary>
        public string To { get; set; }
        
        /// <summary>
        /// Text
        /// </summary>
        public string Text { get; set; }
        
        /// <summary>
        /// Id of application
        /// </summary>
        public string ApplicationId { get; set; }
        
        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; }
    }
}
