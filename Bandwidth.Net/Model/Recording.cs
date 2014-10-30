using System;

namespace Bandwidth.Net.Model
{
    public class Recording: BaseModel
    {
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
