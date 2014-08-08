using System;
using System.Threading.Tasks;

namespace Bandwidth.Net.Clients
{
    public class NumberInfo
    {
        private readonly Client _client;

        internal NumberInfo(Client client)
        {
            _client = client;
        }
        private const string NumberInfoPath = "phoneNumbers/numberInfo"; 
        public Task<Data.NumberInfo> Get(string number)
        {
            if (number == null) throw new ArgumentNullException("number");
            return _client.MakeGetRequest<Data.NumberInfo>(_client.ConcatUserPath(NumberInfoPath), null, number);
        }
    }
}