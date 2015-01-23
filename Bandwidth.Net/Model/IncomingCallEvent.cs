namespace Bandwidth.Net.Model
{
    /// <summary>
    /// Incoming call event
    /// </summary>
    public class IncomingCallEvent : CallEvent
    {
        /// <summary>
        /// Application Id
        /// </summary>
        public string ApplicationId { get; set; }
    }
}
