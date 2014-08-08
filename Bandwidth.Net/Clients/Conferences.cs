using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Clients
{
    public class Conferences
    {
        private readonly Client _client;

        internal Conferences(Client client)
        {
            _client = client;
        }

        private const string ConferencesPath = "conferences";

        private readonly Regex _conferenceIdExtractor = new Regex(@"/" + ConferencesPath + @"/([\w\-_]+)$");
        public async Task<string> Create(Conference conference)
        {
            using (var response = await _client.MakePostRequest(_client.ConcatUserPath(ConferencesPath), conference))
            {
                var match = (response.Headers.Location != null) ? _conferenceIdExtractor.Match(response.Headers.Location.OriginalString) : null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return match.Groups[1].Value;
            }
        }

        public Task Update(string conferenceId, Conference changedData)
        {
            if (conferenceId == null) throw new ArgumentNullException("conferenceId");
            return _client.MakePostRequest(_client.ConcatUserPath(string.Format("{0}/{1}", ConferencesPath, conferenceId)), changedData, true);
        }

        public Task<Conference> Get(string conferenceId)
        {
            if (conferenceId == null) throw new ArgumentNullException("conferenceId");
            return _client.MakeGetRequest<Conference>(_client.ConcatUserPath(ConferencesPath), null, conferenceId);
        }

        public Task<Conference[]> GetAll()
        {
            return _client.MakeGetRequest<Conference[]>(_client.ConcatUserPath(ConferencesPath));
        }

        public Task SetAudio(string conferenceId, Audio audio)
        {
            if (conferenceId == null) throw new ArgumentNullException("conferenceId");
            return _client.MakePostRequest(_client.ConcatUserPath(string.Format("{0}/{1}/audio", ConferencesPath, conferenceId)), audio, true);
        }

        public Task<ConferenceMember[]> GetAllMembers(string conferenceId)
        {
            if (conferenceId == null) throw new ArgumentNullException("conferenceId");
            return _client.MakeGetRequest<ConferenceMember[]>(_client.ConcatUserPath(string.Format("{0}/{1}/members", ConferencesPath, conferenceId)));
        }
        public Task<ConferenceMember> GetMember(string conferenceId, string memberId)
        {
            if (conferenceId == null) throw new ArgumentNullException("conferenceId");
            if (memberId == null) throw new ArgumentNullException("memberId");
            return _client.MakeGetRequest<ConferenceMember>(_client.ConcatUserPath(string.Format("{0}/{1}/members/{2}", ConferencesPath, conferenceId, memberId)));
        }
        
        private readonly Regex _conferenceMemberIdExtractor = new Regex(@"/members/([\w\-_]+)$");
        public async Task<string> CreateMember(string conferenceId, ConferenceMember member)
        {
            if (conferenceId == null) throw new ArgumentNullException("conferenceId");
            using (var response = await _client.MakePostRequest(_client.ConcatUserPath(string.Format("{0}/{1}/members", ConferencesPath, conferenceId)), member))
            {
                var match = (response.Headers.Location != null) ? _conferenceMemberIdExtractor.Match(response.Headers.Location.OriginalString) : null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return match.Groups[1].Value;
            }
        }

        public Task UpdateMember(string conferenceId, string memberId, ConferenceMember changedData)
        {
            if (conferenceId == null) throw new ArgumentNullException("conferenceId");
            if (memberId == null) throw new ArgumentNullException("memberId");
            return _client.MakePostRequest(_client.ConcatUserPath(string.Format("{0}/{1}/members/{2}", ConferencesPath, conferenceId, memberId)), changedData, true);
        }

        public Task SetMemberAudio(string conferenceId, string memberId, Audio audio)
        {
            if (conferenceId == null) throw new ArgumentNullException("conferenceId");
            if (memberId == null) throw new ArgumentNullException("memberId");
            return _client.MakePostRequest(_client.ConcatUserPath(string.Format("{0}/{1}/members/{2}/audio", ConferencesPath, conferenceId, memberId)), audio, true);
        }

    }
}