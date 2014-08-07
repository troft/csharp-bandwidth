using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bandwidth.Net
{
    partial class Client
    {
        private const string CallsPath = "calls";

        private readonly Regex _callIdExtractor = new Regex(@"/" + CallsPath + @"/(\w+)$");
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

    }
}