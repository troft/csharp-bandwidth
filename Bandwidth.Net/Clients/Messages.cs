using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Clients
{
    public class Messages
    {
        private readonly Client _client;

        internal Messages(Client client)
        {
            _client = client;
        }

        private const string MessagesPath = "messages";

        private readonly Regex _messageIdExtractor = new Regex(@"/" + MessagesPath + @"/([\w\-_]+)$");
        public async Task<string> Send(Message message)
        {
            using (var response = await _client.MakePostRequest(_client.ConcatUserPath(MessagesPath), message))
            {
                var match = (response.Headers.Location != null) ? _messageIdExtractor.Match(response.Headers.Location.OriginalString) : null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return match.Groups[1].Value;
            }
        }

        public async Task<SendMessageStatus[]> Send(Message[] messages)
        {
            var result =
                await _client.MakePostRequest<SendMessageStatus[]>(_client.ConcatUserPath(MessagesPath), messages);
            foreach (var item in (from i in result where i.Location != null && i.Result == SendMessageResult.Accepted select i))
            {
                var match = _messageIdExtractor.Match(item.Location.OriginalString);
                if (match != null)
                {
                    item.Id = match.Groups[1].Value;
                }
            }
            return result;
        }
      
        public Task<Message> Get(string messageId)
        {
            if (messageId == null) throw new ArgumentNullException("messageId");
            return _client.MakeGetRequest<Message>(_client.ConcatUserPath(MessagesPath), null, messageId);
        }
        public Task<Message[]> GetAll(MessageQuery query = null)
        {
            query = query ?? new MessageQuery();
            return _client.MakeGetRequest<Message[]>(_client.ConcatUserPath(MessagesPath), query.ToDictionary());
        }
    }
}