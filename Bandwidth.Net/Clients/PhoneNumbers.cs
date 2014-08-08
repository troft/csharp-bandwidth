using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Clients
{
    public class PhoneNumbers
    {
        private readonly Client _client;

        internal PhoneNumbers(Client client)
        {
            _client = client;
        }

        private const string PhoneNumbersPath = "phoneNumbers";

        private readonly Regex _phoneNumberIdExtractor = new Regex(@"/" + PhoneNumbersPath + @"/([\w\-_]+)$");
        public async Task<string> Create(PhoneNumber phoneNumber)
        {
            using (var response = await _client.MakePostRequest(_client.ConcatUserPath(PhoneNumbersPath), phoneNumber))
            {
                var match = (response.Headers.Location != null) ? _phoneNumberIdExtractor.Match(response.Headers.Location.OriginalString) : null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return match.Groups[1].Value;
            }
        }

        public Task Update(string phoneNumberId, PhoneNumber changedData)
        {
            if (phoneNumberId == null) throw new ArgumentNullException("phoneNumberId");
            return _client.MakePostRequest(_client.ConcatUserPath(string.Format("{0}/{1}", PhoneNumbersPath, phoneNumberId)), changedData, true);
        }

        public Task<PhoneNumber> Get(string phoneNumberOrId)
        {
            if (phoneNumberOrId == null) throw new ArgumentNullException("phoneNumberOrId");
            return _client.MakeGetRequest<PhoneNumber>(_client.ConcatUserPath(PhoneNumbersPath), null, Uri.EscapeDataString(phoneNumberOrId));
        }

        public Task<PhoneNumber[]> GetAll(PhoneNumberQuery query = null)
        {
            query = query ?? new PhoneNumberQuery();
            return _client.MakeGetRequest<PhoneNumber[]>(_client.ConcatUserPath(PhoneNumbersPath), query.ToDictionary());
        }

        public Task Remove(string phoneNumberId)
        {
            if (phoneNumberId == null) throw new ArgumentNullException("phoneNumberId");
            return _client.MakeDeleteRequest(_client.ConcatUserPath(string.Format("{0}/{1}", PhoneNumbersPath, phoneNumberId)));
        }
    }
}