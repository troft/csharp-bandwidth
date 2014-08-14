using System;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Clients
{
    public class Errors
    {
        private const string ErrorsPath = "errors";
        private readonly Client _client;

        internal Errors(Client client)
        {
            _client = client;
        }

        /// <summary>
        ///     Gets information about one user error
        /// </summary>
        public Task<Error> Get(string errorId)
        {
            if (errorId == null) throw new ArgumentNullException("recordingId");
            return _client.MakeGetRequest<Error>(_client.ConcatUserPath(ErrorsPath), null, errorId);
        }

        /// <summary>
        ///     Gets all the user errors for a user
        /// </summary>
        public Task<Error[]> GetAll(ErrorQuery query = null)
        {
            query = query ?? new ErrorQuery();
            return _client.MakeGetRequest<Error[]>(_client.ConcatUserPath(ErrorsPath), query.ToDictionary());
        }
    }
}