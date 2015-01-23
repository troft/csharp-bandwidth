namespace Bandwidth.Net.Model
{
    /// <summary>
    /// Gather event
    /// </summary>
    public class GatherEvent : BaseEvent
    {
        /// <summary>
        /// Id of related call
        /// </summary>
        public string CallId { get; set; }
        
        /// <summary>
        /// Reason
        /// </summary>
        public string Reason { get; set; }
        
        /// <summary>
        /// Id of gather
        /// </summary>
        public string GatherId { get; set; }
        
        /// <summary>
        /// State of gather
        /// </summary>
        public string State { get; set; }
        
        /// <summary>
        /// Digits
        /// </summary>
        public string Digits { get; set; }
    }
}
