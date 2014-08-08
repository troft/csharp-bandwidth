using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Clients
{
    public class Bridges
    {
        private readonly Client _client;

        internal Bridges(Client client)
        {
            _client = client;
        }

        private const string BridgesPath = "bridges";

        private readonly Regex _bridgeIdExtractor = new Regex(@"/" + BridgesPath + @"/([\w\-_]+)$");
        public async Task<string> Create(Bridge bridge)
        {
            using (var response = await _client.MakePostRequest(_client.ConcatUserPath(BridgesPath), bridge))
            {
                var match = (response.Headers.Location != null) ? _bridgeIdExtractor.Match(response.Headers.Location.OriginalString) : null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return match.Groups[1].Value;
            }
        }

        public Task Update(string bridgeId, Bridge changedData)
        {
            if (bridgeId == null) throw new ArgumentNullException("bridgeId");
            return _client.MakePostRequest(_client.ConcatUserPath(string.Format("{0}/{1}", BridgesPath, bridgeId)), changedData, true);
        }

        public Task<Bridge> Get(string bridgeId)
        {
            if (bridgeId == null) throw new ArgumentNullException("bridgeId");
            return _client.MakeGetRequest<Bridge>(_client.ConcatUserPath(BridgesPath), null, bridgeId);
        }

        public Task<Bridge[]> GetAll()
        {
            return _client.MakeGetRequest<Bridge[]>(_client.ConcatUserPath(BridgesPath));
        }

        public Task SetAudio(string bridgeId, Audio audio)
        {
            if (bridgeId == null) throw new ArgumentNullException("bridgeId");
            return _client.MakePostRequest(_client.ConcatUserPath(string.Format("{0}/{1}/audio", BridgesPath, bridgeId)), audio, true);
        }

        public Task<Call[]> GetCalls(string bridgeId)
        {
            if (bridgeId == null) throw new ArgumentNullException("bridgeId");
            return _client.MakeGetRequest<Call[]>(_client.ConcatUserPath(string.Format("{0}/{1}/calls", BridgesPath, bridgeId)));
        }

    }
}