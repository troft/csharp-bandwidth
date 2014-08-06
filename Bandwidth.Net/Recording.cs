using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bandwidth.Net
{
    public class Recording
    {
        public string Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Call { get; set; }

        public string Media { get; set; }

        public RecordingState State { get; set; }
    }

    public enum RecordingState
    {
        Recording,
        Complete,
        Saving,
        Error
    }
}
