namespace Bandwidth.Net.Model
{
    /// <summary>
    /// Bandwidth API sends this message to the application when it receives number pad tone signals during a call.
    /// </summary>
    public class DtmfEvent : BaseEvent
    {
        /// <summary>
        /// Id of the call
        /// </summary>
        public string CallId { get; set; }
        
        /// <summary>
        /// Uri of the call
        /// </summary>
        public string CallUri { get; set; }
        
        /// <summary>
        /// Dtmf digits
        /// </summary>
        public string DtmfDigit { get; set; }
        
        /// <summary>
        /// Duration of Dtmf
        /// </summary>
        public string DtmfDuration { get; set; }
    }
}
