using System;
using System.Threading.Tasks;

namespace Bandwidth.Net
{
    partial class Client
    {
        private const string RecordingsPath = "recordings";
        public Task<Recording> GetRecording(string recordingId)
        {
            if (recordingId == null) throw new ArgumentNullException("recordingId");
            return MakeGetRequest<Recording>(ConcatUserPath(RecordingsPath), null, recordingId);
        }

        public Task<Recording[]> GetRecordings(RecordingQuery query = null)
        {
            query = query ?? new RecordingQuery();
            return MakeGetRequest<Recording[]>(ConcatUserPath(RecordingsPath), query.ToDictionary());
        }
    }
}