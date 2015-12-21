namespace Bandwidth.Net.Model
{
    /// <summary>
    /// MMS event
    /// </summary>
    public class MmsEvent : MessageEvent
    {
        /// <summary>
        /// URIs of media files associate with the MMS message
        /// </summary>
        public string[] Media { get; set; }
    }
}
