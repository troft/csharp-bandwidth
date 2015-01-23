namespace Bandwidth.Net.Model
{
    /// <summary>
    /// Hangup event
    /// </summary>
    public class HangupEvent : CallEvent
    {
        /// <summary>
        /// Cause of hangup
        /// </summary>
        public string Cause { get; set; } 
    }
}
