using System;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Clients
{
    public class AvailableNumbers
    {
        private readonly Client _client;

        internal AvailableNumbers(Client client)
        {
            _client = client;
        }
        private const string ApplicationsPath = "applications";

        public Task<AvailableNumber[]> GetAll(AvailableNumberQuery query = null)
        {
            query = query ?? new AvailableNumberQuery();
            return _client.MakeGetRequest<AvailableNumber[]>(_client.ConcatUserPath(ApplicationsPath), query.ToDictionary());
        }
    }
}