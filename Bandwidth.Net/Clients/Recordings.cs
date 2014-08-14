using System;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Clients
{
    public class Recordings
    {
        private const string RecordingsPath = "recordings";
        private readonly Client _client;

        internal Recordings(Client client)
        {
            _client = client;
        }

        /// <summary>
        ///     Retrieve a specific call recording information, identified by recordingId
        /// </summary>
        public Task<Recording> Get(string recordingId)
        {
            if (recordingId == null) throw new ArgumentNullException("recordingId");
            return _client.MakeGetRequest<Recording>(_client.ConcatUserPath(RecordingsPath), null, recordingId);
        }

        /// <summary>
        ///     List a user's call recordings
        /// </summary>
        public Task<Recording[]> GetAll(RecordingQuery query = null)
        {
            query = query ?? new RecordingQuery();
            return _client.MakeGetRequest<Recording[]>(_client.ConcatUserPath(RecordingsPath), query.ToDictionary());
        }
    }
}