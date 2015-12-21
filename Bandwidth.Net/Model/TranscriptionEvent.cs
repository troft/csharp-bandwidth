namespace Bandwidth.Net.Model
{
    /// <summary>
    /// Transcription event
    /// </summary>
    public class TranscriptionEvent : BaseEvent
    {
        /// <summary>
        /// Url of transcription
        /// </summary>
        public string TranscriptionUri { get; set; }
        
        /// <summary>
        /// Text
        /// </summary>
        public string Text { get; set; }
        
        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }
        
        /// <summary>
        /// Id of recording
        /// </summary>
        public string RecordingId { get; set; }
        
        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; }
        
        /// <summary>
        /// Id of transcription
        /// </summary>
        public string TranscriptionId { get; set; }
    }
}
