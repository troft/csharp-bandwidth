using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bandwidth.Net
{
    partial class Client
    {
        private const string CallsPath = "calls";

        private readonly Regex _callIdExtractor = new Regex(@"/" + CallsPath + @"/([\w\-_]+)$");
        public async Task<string> CreateCall(Call call)
        {
            var response = await MakePostRequest(ConcatUserPath(CallsPath), call);
            var match = _callIdExtractor.Match(response.Headers.Location.LocalPath);
            if (match == null)
            {
                throw new Exception("Missing id in response");
            }
            return match.Groups[1].Value;
        }

        public Task UpdateCall(string callId, Call changedData)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            return MakePostRequest(ConcatUserPath(string.Format("{0}/{1}", CallsPath, callId)), changedData);
        }

        public Task<Call> GetCall(string callId)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            return MakeGetRequest<Call>(ConcatUserPath(CallsPath), null, callId);
        }

        public Task<Call[]> GetCalls(CallQuery query = null)
        {
            query = query ?? new CallQuery();
            return MakeGetRequest<Call[]>(ConcatUserPath(CallsPath), query.ToDictionary());
        }

        public Task SetCallAudio(string callId, Audio audio)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            return MakePostRequest(ConcatUserPath(string.Format("{0}/{1}/audio", CallsPath, callId)), audio);
        }

        public Task SetCallDtmf(string callId, Dtmf dtmf)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            return MakePostRequest(ConcatUserPath(string.Format("{0}/{1}/dtmf", CallsPath, callId)), dtmf);
        }

        public Task CreateCallGather(string callId, CreateGather gather)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            return MakePostRequest(ConcatUserPath(string.Format("{0}/{1}/gather", CallsPath, callId)), gather);
        }

        public Task UpdateCallGather(string callId, string gatherId, Gather gather)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            if (gatherId == null) throw new ArgumentNullException("gatherId");
            return MakePostRequest(ConcatUserPath(string.Format("{0}/{1}/gather/{2}", CallsPath, callId, gatherId)), gather);
        }

        public Task<Gather> GetCallGather(string callId, string gatherId)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            if (gatherId == null) throw new ArgumentNullException("gatherId");
            return MakeGetRequest<Gather>(ConcatUserPath(string.Format("{0}/{1}/gather/{2}", CallsPath, callId, gatherId)));
        }

        public Task<Event[]> GetCallEvents(string callId)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            return MakeGetRequest<Event[]>(ConcatUserPath(string.Format("{0}/{1}/events", CallsPath, callId)));
        }

        public Task<Event> GetCallEvent(string callId, string eventId)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            if (eventId == null) throw new ArgumentNullException("eventId");
            return MakeGetRequest<Event>(ConcatUserPath(string.Format("{0}/{1}/events/{2}", CallsPath, callId, eventId)));
        }

        public Task<Recording[]> GetCallRecordings(string callId)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            return MakeGetRequest<Recording[]>(ConcatUserPath(string.Format("{0}/{1}/recordings", CallsPath, callId)));
        }
    }
}