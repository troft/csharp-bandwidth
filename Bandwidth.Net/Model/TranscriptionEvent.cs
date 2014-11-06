using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public class TranscriptionEvent : BaseEvent
    {
        public string TranscriptionUri { get; set; }
        public string Text { get; set; }
        public string Status { get; set; }
        public string RecordingId { get; set; }
        public string State { get; set; }
        public string TranscriptionId { get; set; }
    }
}
