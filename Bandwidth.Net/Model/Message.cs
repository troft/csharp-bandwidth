using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public class Message: BaseModel
    {
        private const string MessagePath = "messages";
        
        private static readonly Regex MessageIdExtractor = new Regex(@"/" + MessagePath + @"/([\w\-_]+)$");

        /// <summary>
        ///     Get a message that was sent or received
        /// </summary>
        public static Task<Message> Get(Client client, string messageId)
        {
            if (messageId == null) throw new ArgumentNullException("messageId");
            return client.MakeGetRequest<Message>(client.ConcatUserPath(MessagePath), null, messageId);
        }
#if !PCL
        public static Task<Message> Get(string messageId)
        {
            return Get(Client.GetInstance(), messageId);
        }
#endif

        /// <summary>
        ///     Get a list of previous messages that were sent or received
        /// </summary>
        public static Task<Message[]> List(Client client, IDictionary<string, object> parameters = null)
        {
            return client.MakeGetRequest<Message[]>(client.ConcatUserPath(MessagePath), parameters);
        }

        public static Task<Message[]> List(Client client, int page, int size = 25)
        {
            var query = new Dictionary<string, object> { { "page", page }, { "size", size } };
            return List(client, query);
        }
#if !PCL
        public static Task<Message[]> List(IDictionary<string, object> parameters = null)
        {
            return List(Client.GetInstance(), parameters);
        }

        public static Task<Message[]> List(int page, int size = 25)
        {
            return List(Client.GetInstance(), page, size);
        }
#endif

        ///<summary>
        ///     Send a text message
        /// </summary>
        public static async Task<Message> Create(Client client, IDictionary<string, object> parameters)
        {
            using (var response = await client.MakePostRequest(client.ConcatUserPath(MessagePath), parameters))
            {
                var match = (response.Headers.Location != null)
                    ? MessageIdExtractor.Match(response.Headers.Location.OriginalString)
                    : null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return await Get(client, match.Groups[1].Value);
            }
        }

        public static Task<Message> Create(Client client, string to, string from, string text)
        {
            return Create(client, new Dictionary<string, object>
            {
                {"to", to},
                {"from", from},
                {"text", text}
            });
        }
#if !PCL
        public static Task<Message> Create(IDictionary<string, object> parameters)
        {
            return Create(Client.GetInstance(), parameters);
        }

        public static Task<Message> Create(string to, string from, string text)
        {
            return Create(Client.GetInstance(), to, from, text);
        }
#endif
        public MessageDirection Direction { get; set; }
        public string CallbackUrl { get; set; }
        public int CallbackTimeout { get; set; }
        public string FallbackUrl { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public MessageState State { get; set; }
        public DateTime Time { get; set; }
        public string Text { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public string Tag { get; set; }
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
