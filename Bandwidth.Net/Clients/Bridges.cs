using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Clients
{
    public class Bridges
    {
        private const string BridgesPath = "bridges";

        private readonly Regex _bridgeIdExtractor = new Regex(@"/" + BridgesPath + @"/([\w\-_]+)$");
        private readonly Client _client;

        internal Bridges(Client client)
        {
            _client = client;
        }

        /// <summary>
        ///     Create a bridge.
        /// </summary>
        public async Task<string> Create(Bridge bridge)
        {
            using (
                HttpResponseMessage response =
                    await _client.MakePostRequest(_client.ConcatUserPath(BridgesPath), bridge))
            {
                Match match = (response.Headers.Location != null)
                    ? _bridgeIdExtractor.Match(response.Headers.Location.OriginalString)
                    : null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return match.Groups[1].Value;
            }
        }

        /// <summary>
        ///     Create a bridge.
        /// </summary>
        public Task Update(string bridgeId, Bridge changedData)
        {
            if (bridgeId == null) throw new ArgumentNullException("bridgeId");
            return _client.MakePostRequest(_client.ConcatUserPath(string.Format("{0}/{1}", BridgesPath, bridgeId)),
                changedData, true);
        }

        /// <summary>
        ///     Change calls in a bridge and bridge/unbridge the audio
        /// </summary>
        public Task<Bridge> Get(string bridgeId)
        {
            if (bridgeId == null) throw new ArgumentNullException("bridgeId");
            return _client.MakeGetRequest<Bridge>(_client.ConcatUserPath(BridgesPath), null, bridgeId);
        }

        /// <summary>
        ///     Get list of bridges for a given user.
        /// </summary>
        public Task<Bridge[]> GetAll()
        {
            return _client.MakeGetRequest<Bridge[]>(_client.ConcatUserPath(BridgesPath));
        }

        /// <summary>
        ///     Play an audio file or speak a sentence in a bridge.
        /// </summary>
        public Task SetAudio(string bridgeId, Audio audio)
        {
            if (bridgeId == null) throw new ArgumentNullException("bridgeId");
            return _client.MakePostRequest(
                _client.ConcatUserPath(string.Format("{0}/{1}/audio", BridgesPath, bridgeId)), audio, true);
        }

        /// <summary>
        ///     Get the list of calls that are on the bridge
        /// </summary>
        public Task<Call[]> GetCalls(string bridgeId)
        {
            if (bridgeId == null) throw new ArgumentNullException("bridgeId");
            return
                _client.MakeGetRequest<Call[]>(
                    _client.ConcatUserPath(string.Format("{0}/{1}/calls", BridgesPath, bridgeId)));
        }
    }
}