using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Clients
{
    public class Calls
    {
        private readonly Client _client;

        internal Calls(Client client)
        {
            _client = client;
        }

        private const string CallsPath = "calls";

        private readonly Regex _callIdExtractor = new Regex(@"/" + CallsPath + @"/([\w\-_]+)$");
        public async Task<string> Create(Call call)
        {
            using (var response = await _client.MakePostRequest(_client.ConcatUserPath(CallsPath), call))
            {
                var match = (response.Headers.Location != null)?_callIdExtractor.Match(response.Headers.Location.OriginalString):null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return match.Groups[1].Value;
            }
        }

        public Task Update(string callId, Call changedData)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            return _client.MakePostRequest(_client.ConcatUserPath(string.Format("{0}/{1}", CallsPath, callId)), changedData, true);
        }

        public Task<Call> Get(string callId)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            return _client.MakeGetRequest<Call>(_client.ConcatUserPath(CallsPath), null, callId);
        }

        public Task<Call[]> GetAll(CallQuery query = null)
        {
            query = query ?? new CallQuery();
            return _client.MakeGetRequest<Call[]>(_client.ConcatUserPath(CallsPath), query.ToDictionary());
        }

        public Task SetAudio(string callId, Audio audio)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            return _client.MakePostRequest(_client.ConcatUserPath(string.Format("{0}/{1}/audio", CallsPath, callId)), audio, true);
        }

        public Task SetDtmf(string callId, Dtmf dtmf)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            return _client.MakePostRequest(_client.ConcatUserPath(string.Format("{0}/{1}/dtmf", CallsPath, callId)), dtmf, true);
        }

        public Task CreateGather(string callId, CreateGather gather)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            return _client.MakePostRequest(_client.ConcatUserPath(string.Format("{0}/{1}/gather", CallsPath, callId)), gather, true);
        }

        public Task UpdateGather(string callId, string gatherId, Gather gather)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            if (gatherId == null) throw new ArgumentNullException("gatherId");
            return _client.MakePostRequest(_client.ConcatUserPath(string.Format("{0}/{1}/gather/{2}", CallsPath, callId, gatherId)), gather, true);
        }

        public Task<Gather> GetGather(string callId, string gatherId)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            if (gatherId == null) throw new ArgumentNullException("gatherId");
            return _client.MakeGetRequest<Gather>(_client.ConcatUserPath(string.Format("{0}/{1}/gather/{2}", CallsPath, callId, gatherId)));
        }

        public Task<Event[]> GetEvents(string callId)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            return _client.MakeGetRequest<Event[]>(_client.ConcatUserPath(string.Format("{0}/{1}/events", CallsPath, callId)));
        }

        public Task<Event> GetEvent(string callId, string eventId)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            if (eventId == null) throw new ArgumentNullException("eventId");
            return _client.MakeGetRequest<Event>(_client.ConcatUserPath(string.Format("{0}/{1}/events/{2}", CallsPath, callId, eventId)));
        }

        public Task<Recording[]> GetRecordings(string callId)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            return _client.MakeGetRequest<Recording[]>(_client.ConcatUserPath(string.Format("{0}/{1}/recordings", CallsPath, callId)));
        }
    }
}