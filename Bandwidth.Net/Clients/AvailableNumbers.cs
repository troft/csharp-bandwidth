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
        private const string AvailableNumbersPath = "availableNumbers";

        public Task<AvailableNumber[]> GetAll(AvailableNumberQuery query = null)
        {
            query = query ?? new AvailableNumberQuery();
            var subPath = (query.Type == AvailableNumberType.Local) ? "local" : "TollFree";
            return _client.MakeGetRequest<AvailableNumber[]>(string.Format("{0}/{1}", AvailableNumbersPath, subPath), query.ToDictionary());
        }
    }
}