namespace Bandwidth.Net.Model
{
    /// <summary>
    /// Reject event
    /// </summary>
    public class RejectEvent : CallEvent
    {
        /// <summary>
        /// Cause
        /// </summary>
        public string Cause { get; set; }
    }
}
