using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Clients
{
    public class Messages
    {
        private const string MessagesPath = "messages";
        private readonly Client _client;

        private readonly Regex _messageIdExtractor = new Regex(@"/" + MessagesPath + @"/([\w\-_]+)$");

        internal Messages(Client client)
        {
            _client = client;
        }

        /// <summary>
        ///     Send a text message
        /// </summary>
        public async Task<string> Send(Message message)
        {
            using (
                HttpResponseMessage response =
                    await _client.MakePostRequest(_client.ConcatUserPath(MessagesPath), message))
            {
                Match match = (response.Headers.Location != null)
                    ? _messageIdExtractor.Match(response.Headers.Location.OriginalString)
                    : null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return match.Groups[1].Value;
            }
        }

        /// <summary>
        ///     Send text messages
        /// </summary>
        public async Task<SendMessageStatus[]> Send(Message[] messages)
        {
            SendMessageStatus[] result =
                await _client.MakePostRequest<SendMessageStatus[]>(_client.ConcatUserPath(MessagesPath), messages);
            foreach (
                SendMessageStatus item in
                    (from i in result where i.Location != null && i.Result == SendMessageResult.Accepted select i))
            {
                Match match = _messageIdExtractor.Match(item.Location.OriginalString);
                if (match != null)
                {
                    item.Id = match.Groups[1].Value;
                }
            }
            return result;
        }

        /// <summary>
        ///     Get a message that was sent or received
        /// </summary>
        public Task<Message> Get(string messageId)
        {
            if (messageId == null) throw new ArgumentNullException("messageId");
            return _client.MakeGetRequest<Message>(_client.ConcatUserPath(MessagesPath), null, messageId);
        }

        /// <summary>
        ///     Get a list of previous messages that were sent or received
        /// </summary>
        public Task<Message[]> GetAll(MessageQuery query = null)
        {
            query = query ?? new MessageQuery();
            return _client.MakeGetRequest<Message[]>(_client.ConcatUserPath(MessagesPath), query.ToDictionary());
        }
    }
}