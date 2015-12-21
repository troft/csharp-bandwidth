using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bandwidth.Net.Model
{
    /// <summary>
    /// The Messages resource lets you send SMS text messages and view messages that were previously sent or received.
    /// </summary>
    public class Message: BaseModel
    {
        private const string MessagePath = "messages";
        
        private static readonly Regex MessageIdExtractor = new Regex(@"/" + MessagePath + @"/([\w\-_]+)$");

        /// <summary>
        /// Get a message that was sent or received
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="messageId">Id of message</param>
        /// <returns>Message info</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/messages/#GET-/v1/users/{userId}/messages/{messageId}"/>
        public static Task<Message> Get(Client client, string messageId)
        {
            if (messageId == null) throw new ArgumentNullException("messageId");
            return client.MakeGetRequest<Message>(client.ConcatUserPath(MessagePath), null, messageId);
        }
        /// <summary>
        /// Get a message that was sent or received
        /// </summary>
        /// <param name="messageId">Id of message</param>
        /// <returns>Message info</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/messages/#GET-/v1/users/{userId}/messages/{messageId}"/>
        public static Task<Message> Get(string messageId)
        {
            return Get(Client.GetInstance(), messageId);
        }

        /// <summary>
        /// Get a list of previous messages that were sent or received
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="query">Query parameters</param>
        /// <returns>List of messages</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/messages/#GET-/v1/users/{userId}/messages"/>
        public static Task<Message[]> List(Client client, IDictionary<string, object> query = null)
        {
            return client.MakeGetRequest<Message[]>(client.ConcatUserPath(MessagePath), query);
        }

        /// <summary>
        /// Get a list of previous messages that were sent or received
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="page">Page number</param>
        /// <param name="size">Page size</param>
        /// <returns>List of messages</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/messages/#GET-/v1/users/{userId}/messages"/>
        public static Task<Message[]> List(Client client, int page, int size = 25)
        {
            var query = new Dictionary<string, object> { { "page", page }, { "size", size } };
            return List(client, query);
        }
        /// <summary>
        /// Get a list of previous messages that were sent or received
        /// </summary>
        /// <param name="query">Query parameters</param>
        /// <returns>List of messages</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/messages/#GET-/v1/users/{userId}/messages"/>
        public static Task<Message[]> List(IDictionary<string, object> query = null)
        {
            return List(Client.GetInstance(), query);
        }

        /// <summary>
        /// Get a list of previous messages that were sent or received
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="size">Page size</param>
        /// <returns>List of messages</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/messages/#GET-/v1/users/{userId}/messages"/>
        public static Task<Message[]> List(int page, int size = 25)
        {
            return List(Client.GetInstance(), page, size);
        }

        ///<summary>
        /// Send a text message
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="parameters">Parameters to send message</param>
        /// <returns>Created message</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/messages/#POST-/v1/users/{userId}/messages"/>
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

        ///<summary>
        /// Send some text messages
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="messages">Messages to send</param>
        /// <returns>Result of sending of each message</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/messages/#POST-/v1/users/{userId}/messages"/>
        public static async Task<MessageResult[]> Create(Client client, params IDictionary<string, object>[] messages)
        {
            using (var response = await client.MakePostRequest(client.ConcatUserPath(MessagePath), messages))
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeAnonymousType(json, new[]{new {Result = "", Location = "", Error = new{Message = ""}}}, client.JsonSerializerSettings);
                return result.Select(i =>
                {
                    var m = new MessageResult();
                    if (i.Result == "error")
                    {
                        m.Exception = new Exception(i.Error.Message);
                    }
                    else
                    {
                        var match = (i.Location != null)? MessageIdExtractor.Match(i.Location): null;
                        if (match == null)
                        {
                            m.Exception = new Exception("Missing id in location");
                        }
                        else
                        {
                            m.MessageId = match.Groups[1].Value;
                        }
                    }
                    return m;
                }).ToArray();
            }
        }

        ///<summary>
        /// Send a text message
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="to">The phone number the message should be sent to</param>
        /// <param name="from">One of your telephone numbers the message should come from</param>
        /// <param name="text">The contents of the text message (must be 160 characters or less)</param>
        /// <returns>Created message</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/messages/#POST-/v1/users/{userId}/messages"/>
        public static Task<Message> Create(Client client, string to, string from, string text)
        {
            return Create(client, new Dictionary<string, object>
            {
                {"to", to},
                {"from", from},
                {"text", text}
            });
        }

        ///<summary>
        /// Send a text message
        /// </summary>
        /// <param name="parameters">Parameters to send message</param>
        /// <returns>Created message</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/messages/#POST-/v1/users/{userId}/messages"/>
        public static Task<Message> Create(IDictionary<string, object> parameters)
        {
            return Create(Client.GetInstance(), parameters);
        }

        ///<summary>
        /// Send some text messages
        /// </summary>
        /// <param name="messages">Messages to send</param>
        /// <returns>Result of sending of each message</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/messages/#POST-/v1/users/{userId}/messages"/>
        public static Task<MessageResult[]> Create(params IDictionary<string, object>[] messages)
        {
            return Create(Client.GetInstance(), messages);
        }
        
        ///<summary>
        /// Send a text message
        /// </summary>
        /// <param name="to">The phone number the message should be sent to</param>
        /// <param name="from">One of your telephone numbers the message should come from</param>
        /// <param name="text">The contents of the text message (must be 160 characters or less)</param>
        /// <returns>Created message</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/messages/#POST-/v1/users/{userId}/messages"/>
        public static Task<Message> Create(string to, string from, string text)
        {
            return Create(Client.GetInstance(), to, from, text);
        }

        /// <summary>
        /// The message direction (IN or OUT), used to filter.
        /// </summary>
        public MessageDirection Direction { get; set; }
        
        /// <summary>
        /// URL where the events related to the outgoing message will be posted to
        /// </summary>
        public string CallbackUrl { get; set; }

        /// <summary>
        /// Determine how long should the platform wait for callbackUrl's response before timing out in milliseconds.
        /// </summary>
        public int CallbackTimeout { get; set; }

        /// <summary>
        /// The URL used to send the callback event if the request to callbackUrl fails.
        /// </summary>
        public string FallbackUrl { get; set; }

        /// <summary>
        /// The message sender's telephone number
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Recipient
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Message state
        /// </summary>
        public MessageState State { get; set; }
        
        /// <summary>
        /// The time the message resource was created
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// The message contents
        /// </summary>
        public string Text { get; set; }
        
        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; }
        
        /// <summary>
        /// Page Size
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// A string that will be included in the callback events of the message
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// One of the request receipt option
        /// </summary>
        public string ReceiptRequested { get; set; }

        /// <summary>
        /// One of the message delivery state
        /// </summary>
        public string DeliveryState { get; set; }

        /// <summary>
        /// One of the message delivery code
        /// </summary>
        public string DeliveryCode { get; set; }

        /// <summary>
        /// Message delivery description for the respective delivery cod
        /// </summary>
        public string DeliveryDescription { get; set; }

    }

    /// <summary>
    /// Message directions
    /// </summary>
    public enum MessageDirection
    {
        In,
        Out
    }

    /// <summary>
    /// Message states
    /// </summary>
    public enum MessageState
    {
        Received,
        Queued,
        Sending,
        Sent,
        Error
    }

    /// <summary>
    /// Result of message sending 
    /// </summary>
    public class MessageResult
    {
        /// <summary>
        /// MessageId (or null on error)
        /// </summary>
        public string MessageId { get; set; }
        
        /// <summary>
        /// Error information (or null on success)
        /// </summary>
        public Exception Exception { get; set; }
    }
}
