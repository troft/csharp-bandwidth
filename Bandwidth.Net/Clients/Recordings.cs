using System;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Clients
{
    public class Recordings
    {
        private readonly Client _client;

        internal Recordings(Client client)
        {
            _client = client;
        }
        private const string RecordingsPath = "recordings";
        public Task<Recording> Get(string recordingId)
        {
            if (recordingId == null) throw new ArgumentNullException("recordingId");
            return _client.MakeGetRequest<Recording>(_client.ConcatUserPath(RecordingsPath), null, recordingId);
        }

        public Task<Recording[]> GetAll(RecordingQuery query = null)
        {
            query = query ?? new RecordingQuery();
            return _client.MakeGetRequest<Recording[]>(_client.ConcatUserPath(RecordingsPath), query.ToDictionary());
        }
    }
}