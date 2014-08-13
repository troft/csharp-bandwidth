using System;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Events
{
    public class Call: Event
    {
        public string From { get; set; }
        public string To { get; set; }
        public string CallId { get; set; }
        public Uri CallUri { get; set; }
        public CallState? CallState { get; set; }
    }
}
