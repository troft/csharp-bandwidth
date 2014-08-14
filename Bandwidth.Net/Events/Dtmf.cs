using System;

namespace Bandwidth.Net.Events
{
    public class Dtmf : Event
    {
        public string CallId { get; set; }
        public Uri CallUri { get; set; }
        public int DtmfDigit { get; set; }
        public int DtmfDuration { get; set; }
    }
}