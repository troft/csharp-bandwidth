using System;

namespace Bandwidth.Net.Data
{
    public class Bridge
    {
        public string Id { get; set; }
        public string[] CallIds { get; set; }
        public Uri Calls { get; set; }
        public bool? BridgeAudio { get; set; }
        public DateTime? CompletedTime { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ActivatedTime { get; set; }
        public BridgeState? State { get; set; }
    }

    public enum BridgeState
    {
        Created,
        Updating,
        Hold,
        Completed,
        Error,
        Active
    }
}
