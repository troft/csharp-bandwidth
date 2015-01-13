using System;

namespace Bandwidth.Net.Model
{
    /// <summary>
    /// Conference event
    /// </summary>
    /// <seealso href="https://catapult.inetwork.com/docs/callback-events/conference-event/"/>
    public class ConferenceEvent : BaseConferenceEvent
    {
        /// <summary>
        /// Time of creation of the conference
        /// </summary>
        public DateTime CreatedTime { get; set; }
        
        /// <summary>
        /// Time of completion of the conference
        /// </summary>
        public DateTime CompletedTime { get; set; }
    }
}
