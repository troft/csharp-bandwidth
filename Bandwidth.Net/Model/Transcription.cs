using System;

namespace Bandwidth.Net.Model
{
    /// <summary>
    /// Transcription data
    /// </summary>
    public class Transcription: BaseModel
    {
        /// <summary>
        /// The state of the transcription, 
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// The transcribed text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The date/time the transcription resource was create
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// The seconds between activeTime and endTime for the recording; this is the time that is going to be used to charge the resource.
        /// </summary>
        public int ChargeableDuration { get; set; }
        
        /// <summary>
        /// The size of the transcribed text. If the text is longer than 1000 characters it will be cropped; the full text can be retrieved from the url available at textUrl property.
        /// </summary>
        public int TextSize { get; set; }

        /// <summary>
        /// An url to the full text; this property is available regardless the TextSize.
        /// </summary>
        public string TextUrl { get; set; }
    }
}