using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Clients
{
    public class Applications
    {
        private const string ApplicationsPath = "applications";

        private readonly Regex _appIdExtractor = new Regex(@"/" + ApplicationsPath + @"/([\w\-_]+)$");
        private readonly Client _client;

        internal Applications(Client client)
        {
            _client = client;
        }

        /// <summary>
        ///     Creates an application that can handle calls and messages for one of your phone number. Many phone numbers can
        ///     share an application.
        /// </summary>
        public async Task<string> Create(Application application)
        {
            using (
                HttpResponseMessage response =
                    await _client.MakePostRequest(_client.ConcatUserPath(ApplicationsPath), application))
            {
                Match match = (response.Headers.Location != null)
                    ? _appIdExtractor.Match(response.Headers.Location.OriginalString)
                    : null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return match.Groups[1].Value;
            }
        }

        /// <summary>
        ///     Makes changes to an application.
        /// </summary>
        public Task Update(string applicationId, Application changedData)
        {
            if (applicationId == null) throw new ArgumentNullException("applicationId");
            return
                _client.MakePostRequest(
                    _client.ConcatUserPath(string.Format("{0}/{1}", ApplicationsPath, applicationId)), changedData, true);
        }

        /// <summary>
        ///     Gets information about one of your applications.
        /// </summary>
        public Task<Application> Get(string applicationId)
        {
            if (applicationId == null) throw new ArgumentNullException("applicationId");
            return _client.MakeGetRequest<Application>(_client.ConcatUserPath(ApplicationsPath), null, applicationId);
        }


        /// <summary>
        ///     Gets a list of user's applications.
        /// </summary>
        public Task<Application[]> GetAll(ApplicationQuery query = null)
        {
            query = query ?? new ApplicationQuery();
            return _client.MakeGetRequest<Application[]>(_client.ConcatUserPath(ApplicationsPath), query.ToDictionary());
        }

        /// <summary>
        ///     Permanently deletes an application.
        /// </summary>
        public Task Remove(string applicationId)
        {
            if (applicationId == null) throw new ArgumentNullException("applicationId");
            return
                _client.MakeDeleteRequest(
                    _client.ConcatUserPath(string.Format("{0}/{1}", ApplicationsPath, applicationId)));
        }
    }
}