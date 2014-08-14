using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Clients
{
    public class Conferences
    {
        private const string ConferencesPath = "conferences";
        private readonly Client _client;

        private readonly Regex _conferenceIdExtractor = new Regex(@"/" + ConferencesPath + @"/([\w\-_]+)$");
        private readonly Regex _conferenceMemberIdExtractor = new Regex(@"/members/([\w\-_]+)$");

        internal Conferences(Client client)
        {
            _client = client;
        }

        /// <summary>
        ///     Create a conference.
        /// </summary>
        public async Task<string> Create(Conference conference)
        {
            using (
                HttpResponseMessage response =
                    await _client.MakePostRequest(_client.ConcatUserPath(ConferencesPath), conference))
            {
                Match match = (response.Headers.Location != null)
                    ? _conferenceIdExtractor.Match(response.Headers.Location.OriginalString)
                    : null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return match.Groups[1].Value;
            }
        }

        /// <summary>
        ///     Update Conference.
        /// </summary>
        public Task Update(string conferenceId, Conference changedData)
        {
            if (conferenceId == null) throw new ArgumentNullException("conferenceId");
            return
                _client.MakePostRequest(
                    _client.ConcatUserPath(string.Format("{0}/{1}", ConferencesPath, conferenceId)), changedData, true);
        }

        /// <summary>
        ///     Retrieve conference information.
        /// </summary>
        public Task<Conference> Get(string conferenceId)
        {
            if (conferenceId == null) throw new ArgumentNullException("conferenceId");
            return _client.MakeGetRequest<Conference>(_client.ConcatUserPath(ConferencesPath), null, conferenceId);
        }

        /// <summary>
        ///     Play an audio/speak a sentence in the conference.
        /// </summary>
        public Task SetAudio(string conferenceId, Audio audio)
        {
            if (conferenceId == null) throw new ArgumentNullException("conferenceId");
            return
                _client.MakePostRequest(
                    _client.ConcatUserPath(string.Format("{0}/{1}/audio", ConferencesPath, conferenceId)), audio, true);
        }

        /// <summary>
        ///     List all members from a conference.
        /// </summary>
        public Task<ConferenceMember[]> GetAllMembers(string conferenceId)
        {
            if (conferenceId == null) throw new ArgumentNullException("conferenceId");
            return
                _client.MakeGetRequest<ConferenceMember[]>(
                    _client.ConcatUserPath(string.Format("{0}/{1}/members", ConferencesPath, conferenceId)));
        }

        /// <summary>
        ///     Retrieve information about a particular conference member.
        /// </summary>
        public Task<ConferenceMember> GetMember(string conferenceId, string memberId)
        {
            if (conferenceId == null) throw new ArgumentNullException("conferenceId");
            if (memberId == null) throw new ArgumentNullException("memberId");
            return
                _client.MakeGetRequest<ConferenceMember>(
                    _client.ConcatUserPath(string.Format("{0}/{1}/members/{2}", ConferencesPath, conferenceId, memberId)));
        }

        /// <summary>
        ///     Add a member to a conference.
        /// </summary>
        public async Task<string> CreateMember(string conferenceId, ConferenceMember member)
        {
            if (conferenceId == null) throw new ArgumentNullException("conferenceId");
            using (
                HttpResponseMessage response =
                    await
                        _client.MakePostRequest(
                            _client.ConcatUserPath(string.Format("{0}/{1}/members", ConferencesPath, conferenceId)),
                            member))
            {
                Match match = (response.Headers.Location != null)
                    ? _conferenceMemberIdExtractor.Match(response.Headers.Location.OriginalString)
                    : null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return match.Groups[1].Value;
            }
        }

        /// <summary>
        ///     Update a conference member. E.g.: remove from call, mute, hold, etc.
        /// </summary>
        public Task UpdateMember(string conferenceId, string memberId, ConferenceMember changedData)
        {
            if (conferenceId == null) throw new ArgumentNullException("conferenceId");
            if (memberId == null) throw new ArgumentNullException("memberId");
            return
                _client.MakePostRequest(
                    _client.ConcatUserPath(string.Format("{0}/{1}/members/{2}", ConferencesPath, conferenceId, memberId)),
                    changedData, true);
        }

        /// <summary>
        ///     Play an audio/speak a sentence to a conference member.
        /// </summary>
        public Task SetMemberAudio(string conferenceId, string memberId, Audio audio)
        {
            if (conferenceId == null) throw new ArgumentNullException("conferenceId");
            if (memberId == null) throw new ArgumentNullException("memberId");
            return
                _client.MakePostRequest(
                    _client.ConcatUserPath(string.Format("{0}/{1}/members/{2}/audio", ConferencesPath, conferenceId,
                        memberId)), audio, true);
        }
    }
}