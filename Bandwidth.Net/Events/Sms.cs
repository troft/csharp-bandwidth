using System;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Events
{
    public class Sms: Event
    {
        public MessageDirection Direction { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string MessageId { get; set; }
        public Uri MessageUri { get; set; }
        public string Text { get; set; }
        public MessageState State { get; set; }
    }
}
