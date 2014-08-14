using System;
using System.Threading.Tasks;

namespace Bandwidth.Net.Clients
{
    public class NumberInfo
    {
        private const string NumberInfoPath = "phoneNumbers/numberInfo";
        private readonly Client _client;

        internal NumberInfo(Client client)
        {
            _client = client;
        }

        /// <summary>
        ///     Get the CNAM info of a number
        /// </summary>
        public Task<Data.NumberInfo> Get(string number)
        {
            if (number == null) throw new ArgumentNullException("number");
            return _client.MakeGetRequest<Data.NumberInfo>(NumberInfoPath, null, Uri.EscapeDataString(number));
        }
    }
}