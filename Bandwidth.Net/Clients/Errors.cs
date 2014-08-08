using System;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Clients
{
    public class Errors
    {
        private readonly Client _client;

        internal Errors(Client client)
        {
            _client = client;
        }
        private const string ErrorsPath = "errors";
        public Task<Error> Get(string errorId)
        {
            if (errorId == null) throw new ArgumentNullException("recordingId");
            return _client.MakeGetRequest<Error>(_client.ConcatUserPath(ErrorsPath), null, errorId);
        }

        public Task<Error[]> GetAll(ErrorQuery query = null)
        {
            query = query ?? new ErrorQuery();
            return _client.MakeGetRequest<Error[]>(_client.ConcatUserPath(ErrorsPath), query.ToDictionary());
        }
    }
}