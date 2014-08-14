using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Clients
{
    public class PhoneNumbers
    {
        private const string PhoneNumbersPath = "phoneNumbers";
        private readonly Client _client;

        private readonly Regex _phoneNumberIdExtractor = new Regex(@"/" + PhoneNumbersPath + @"/([\w\-_]+)$");

        internal PhoneNumbers(Client client)
        {
            _client = client;
        }

        /// <summary>
        ///     Allocate a number so user can use it
        /// </summary>
        public async Task<string> Create(PhoneNumber phoneNumber)
        {
            using (
                HttpResponseMessage response =
                    await _client.MakePostRequest(_client.ConcatUserPath(PhoneNumbersPath), phoneNumber))
            {
                Match match = (response.Headers.Location != null)
                    ? _phoneNumberIdExtractor.Match(response.Headers.Location.OriginalString)
                    : null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return match.Groups[1].Value;
            }
        }

        /// <summary>
        ///     Make changes to a number user has
        /// </summary>
        public Task Update(string phoneNumberId, PhoneNumber changedData)
        {
            if (phoneNumberId == null) throw new ArgumentNullException("phoneNumberId");
            return
                _client.MakePostRequest(
                    _client.ConcatUserPath(string.Format("{0}/{1}", PhoneNumbersPath, phoneNumberId)), changedData, true);
        }

        /// <summary>
        ///     Get information about one number (by number or id)
        /// </summary>
        public Task<PhoneNumber> Get(string phoneNumberOrId)
        {
            if (phoneNumberOrId == null) throw new ArgumentNullException("phoneNumberOrId");
            return _client.MakeGetRequest<PhoneNumber>(_client.ConcatUserPath(PhoneNumbersPath), null,
                Uri.EscapeDataString(phoneNumberOrId));
        }

        /// <summary>
        ///     Get a list of user's numbers
        /// </summary>
        public Task<PhoneNumber[]> GetAll(PhoneNumberQuery query = null)
        {
            query = query ?? new PhoneNumberQuery();
            return _client.MakeGetRequest<PhoneNumber[]>(_client.ConcatUserPath(PhoneNumbersPath), query.ToDictionary());
        }

        /// <summary>
        ///     Remove a number from user's account
        /// </summary>
        public Task Remove(string phoneNumberId)
        {
            if (phoneNumberId == null) throw new ArgumentNullException("phoneNumberId");
            return
                _client.MakeDeleteRequest(
                    _client.ConcatUserPath(string.Format("{0}/{1}", PhoneNumbersPath, phoneNumberId)));
        }
    }
}