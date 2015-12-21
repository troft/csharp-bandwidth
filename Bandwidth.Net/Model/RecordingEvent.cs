using System;

namespace Bandwidth.Net.Model
{
    /// <summary>
    /// Recording event
    /// </summary>
    public class RecordingEvent : BaseEvent
    {
        /// <summary>
        /// Id of related call
        /// </summary>
        public string CallId { get; set; }
        
        /// <summary>
        /// Id of recording
        /// </summary>
        public string RecordingId { get; set; }
        
        /// <summary>
        /// Url of recording
        /// </summary>
        public string RecordingUri { get; set; }
        
        
        /// <summary>
        /// Recording state
        /// </summary>
        public RecordingState State { get; set; }
        
        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }
        
        /// <summary>
        /// Date when the recording started
        /// </summary>
        public DateTime StartTime { get; set; }
        
        /// <summary>
        /// Date when the recording ended
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}
