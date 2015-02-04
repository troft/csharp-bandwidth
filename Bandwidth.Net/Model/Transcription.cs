using System;

namespace Bandwidth.Net.Model
{
    public class Transcription: BaseModel
    {
        public string State { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public int ChargeableDuration { get; set; }
        public int TextSize { get; set; }
        public string TextUrl { get; set; }
    }
}