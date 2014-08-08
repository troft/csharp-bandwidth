using System;
using System.Collections.Generic;

namespace Bandwidth.Net.Data
{
    public class Message
    {
        public string Id { get; set; }
        public MessageDirection? Direction { get; set; }
        public Uri CallbackUrl { get; set; }
        public int? CallbackTimeout { get; set; }
        public Uri FallbackUrl { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public MessageState? State { get; set; }
        public DateTime? Time { get; set; }
        public string Text { get; set; }
        public int? Page { get; set; }
        public int? Size { get; set; }
        public string Tag { get; set; }
    }

    public class MessageQuery : Query
    {
        public string From { get; set; }
        public string To { get; set; }
        public override IDictionary<string, string> ToDictionary()
        {
            var query = base.ToDictionary();
            if (From != null)
            {
                query.Add("from", From);
            }
            if (To != null)
            {
                query.Add("to", To);
            }
            return query;
        }
    }

    public class SendMessageStatus
    {
        public string Id { get; set; }
        public Uri Location { get; set; }
        public SendMessageResult Result { get; set; }
        public string Error { get; set; }
    }

    public enum SendMessageResult
    {
        Accepted,
        Error
    }

    public enum MessageDirection
    {
        In,
        Out
    }

    public enum MessageState
    {
        Received,
        Queued,
        Sending,
        Sent,
        Error
    }
}
