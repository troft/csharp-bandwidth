using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Clients
{
    public class Applications
    {
        private readonly Client _client;

        internal Applications(Client client)
        {
            _client = client;
        }

        private const string ApplicationsPath = "applications";

        private readonly Regex _appIdExtractor = new Regex(@"/" + ApplicationsPath + @"/([\w\-_]+)$");
        public async Task<string> Create(Application application)
        {
            using (var response = await _client.MakePostRequest(_client.ConcatUserPath(ApplicationsPath), application))
            {
                var match = (response.Headers.Location != null) ? _appIdExtractor.Match(response.Headers.Location.OriginalString) : null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return match.Groups[1].Value;
            }
        }

        public Task Update(string applicationId, Application changedData)
        {
            if (applicationId == null) throw new ArgumentNullException("applicationId");
            return _client.MakePostRequest(_client.ConcatUserPath(string.Format("{0}/{1}", ApplicationsPath, applicationId)), changedData, true);
        }

        public Task<Application> Get(string applicationId)
        {
            if (applicationId == null) throw new ArgumentNullException("applicationId");
            return _client.MakeGetRequest<Application>(_client.ConcatUserPath(ApplicationsPath), null, applicationId);
        }

        public Task<Application[]> GetAll(ApplicationQuery query = null)
        {
            query = query ?? new ApplicationQuery();
            return _client.MakeGetRequest<Application[]>(_client.ConcatUserPath(ApplicationsPath), query.ToDictionary());
        }

        public Task Remove(string applicationId)
        {
            if (applicationId == null) throw new ArgumentNullException("applicationId");
            return _client.MakeDeleteRequest(_client.ConcatUserPath(string.Format("{0}/{1}", ApplicationsPath, applicationId)));
        }

    }
}