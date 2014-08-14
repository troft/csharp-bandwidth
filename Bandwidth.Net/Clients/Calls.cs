using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Clients
{
    public class Calls
    {
        private const string CallsPath = "calls";

        private readonly Regex _callIdExtractor = new Regex(@"/" + CallsPath + @"/([\w\-_]+)$");
        private readonly Client _client;

        internal Calls(Client client)
        {
            _client = client;
        }

        /// <summary>
        ///     Makes a phone call.
        /// </summary>
        public async Task<string> Create(Call call)
        {
            using (HttpResponseMessage response = await _client.MakePostRequest(_client.ConcatUserPath(CallsPath), call)
                )
            {
                Match match = (response.Headers.Location != null)
                    ? _callIdExtractor.Match(response.Headers.Location.OriginalString)
                    : null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return match.Groups[1].Value;
            }
        }

        /// <summary>
        ///     Changes properties of an active phone call.
        /// </summary>
        public Task Update(string callId, Call changedData)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            return _client.MakePostRequest(_client.ConcatUserPath(string.Format("{0}/{1}", CallsPath, callId)),
                changedData, true);
        }

        /// <summary>
        ///     Gets information about an active or completed call.
        /// </summary>
        public Task<Call> Get(string callId)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            return _client.MakeGetRequest<Call>(_client.ConcatUserPath(CallsPath), null, callId);
        }

        /// <summary>
        ///     Gets a list of active and historic calls user made or received.
        /// </summary>
        public Task<Call[]> GetAll(CallQuery query = null)
        {
            query = query ?? new CallQuery();
            return _client.MakeGetRequest<Call[]>(_client.ConcatUserPath(CallsPath), query.ToDictionary());
        }

        /// <summary>
        ///     Plays an audio file or speak a sentence in a phone call.
        /// </summary>
        public Task SetAudio(string callId, Audio audio)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            return _client.MakePostRequest(_client.ConcatUserPath(string.Format("{0}/{1}/audio", CallsPath, callId)),
                audio, true);
        }

        /// <summary>
        ///     Send DTMF.
        /// </summary>
        public Task SetDtmf(string callId, Dtmf dtmf)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            return _client.MakePostRequest(_client.ConcatUserPath(string.Format("{0}/{1}/dtmf", CallsPath, callId)),
                dtmf, true);
        }

        /// <summary>
        ///     Collects a series of DTMF digits from a phone call with an optional prompt. This request returns immediately. When
        ///     gather finishes, an event with the results will be posted to the callback URL.
        /// </summary>
        public Task CreateGather(string callId, CreateGather gather)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            return _client.MakePostRequest(_client.ConcatUserPath(string.Format("{0}/{1}/gather", CallsPath, callId)),
                gather, true);
        }

        /// <summary>
        ///     Update the gather DTMF. The only update allowed is state:completed to stop the gather.
        /// </summary>
        public Task UpdateGather(string callId, string gatherId, Gather gather)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            if (gatherId == null) throw new ArgumentNullException("gatherId");
            return
                _client.MakePostRequest(
                    _client.ConcatUserPath(string.Format("{0}/{1}/gather/{2}", CallsPath, callId, gatherId)), gather,
                    true);
        }

        /// <summary>
        /// </summary>
        public Task<Gather> GetGather(string callId, string gatherId)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            if (gatherId == null) throw new ArgumentNullException("gatherId");
            return
                _client.MakeGetRequest<Gather>(
                    _client.ConcatUserPath(string.Format("{0}/{1}/gather/{2}", CallsPath, callId, gatherId)));
        }

        /// <summary>
        /// </summary>
        public Task<Event[]> GetEvents(string callId)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            return
                _client.MakeGetRequest<Event[]>(
                    _client.ConcatUserPath(string.Format("{0}/{1}/events", CallsPath, callId)));
        }

        /// <summary>
        /// </summary>
        public Task<Event> GetEvent(string callId, string eventId)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            if (eventId == null) throw new ArgumentNullException("eventId");
            return
                _client.MakeGetRequest<Event>(
                    _client.ConcatUserPath(string.Format("{0}/{1}/events/{2}", CallsPath, callId, eventId)));
        }

        /// <summary>
        /// </summary>
        public Task<Recording[]> GetRecordings(string callId)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            return
                _client.MakeGetRequest<Recording[]>(
                    _client.ConcatUserPath(string.Format("{0}/{1}/recordings", CallsPath, callId)));
        }
    }
}