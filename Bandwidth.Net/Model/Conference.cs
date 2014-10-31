using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public class Conference: BaseModel
    {
        internal const string ConferencePath = "conferences";
        
        private static readonly Regex ConferenceIdExtractor = new Regex(@"/" + ConferencePath + @"/([\w\-_]+)$");
        private static readonly Regex ConferenceMemberIdExtractor = new Regex(@"/members/([\w\-_]+)$");

        /// <summary>
        ///     Retrieve conference information.
        /// </summary>
        public async static Task<Conference> Get(Client client, string conferenceId)
        {
            if (conferenceId == null) throw new ArgumentNullException("conferenceId");
            var conference = await client.MakeGetRequest<Conference>(client.ConcatUserPath(ConferencePath), null, conferenceId);
            conference.Client = client;
            return conference;
        }
#if !PCL
        public static Task<Conference> Get(string conferenceId)
        {
            return Get(Client.GetInstance(), conferenceId);
        }
#endif
        /// <summary>
        ///     Create a conference.
        /// </summary>
        public static async Task<Conference> Create(Client client, IDictionary<string, object> parameters)
        {
            using (
                var response =
                    await client.MakePostRequest(client.ConcatUserPath(ConferencePath), parameters))
            {
                var match = (response.Headers.Location != null)
                    ? ConferenceIdExtractor.Match(response.Headers.Location.OriginalString)
                    : null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return await Get(client, match.Groups[1].Value);
            }
        }

#if !PCL
        public static Task<Conference> Create(IDictionary<string, object> parameters)
        {
            return Create(Client.GetInstance(), parameters);
        }
#endif
        /// <summary>
        ///     Changes properties of a conference.
        /// </summary>
        public Task Update(IDictionary<string, object> parameters)
        {
            return Client.MakePostRequest(Client.ConcatUserPath(string.Format("{0}/{1}", ConferencePath, Id)),
                parameters, true);
        }

        public Task Complete()
        {
            return Update(new Dictionary<string, object> { { "state", "completed" } });
        }

        public Task Mute()
        {
            return Update(new Dictionary<string, object> { { "mute", true } });
        }

        /// <summary>
        ///     List all members from a conference.
        /// </summary>
        public async Task<ConferenceMember[]> GetMembers()
        {
            var members = await Client.MakeGetRequest<ConferenceMember[]>(Client.ConcatUserPath(string.Format("{0}/{1}/members", ConferencePath, Id)));
            foreach (var member in members)
            {
                member.Client = Client;
                member.ConferenceId = Id;
            }
            return members;
        }

        /// <summary>
        ///     Retrieve information about a particular conference member.
        /// </summary>
        public async Task<ConferenceMember> GetMember(string memberId)
        {
            if (memberId == null) throw new ArgumentNullException("memberId");
            var member = await Client.MakeGetRequest<ConferenceMember>(
                    Client.ConcatUserPath(string.Format("{0}/{1}/members/{2}", ConferencePath, Id, memberId)));
            member.Client = Client;
            member.ConferenceId = Id;
            return member;
        }

        /// <summary>
        ///     Add a member to a conference.
        /// </summary>
        public async Task<ConferenceMember> CreateMember(IDictionary<string, object> parameters)
        {
            using (var response = await Client.MakePostRequest(Client.ConcatUserPath(string.Format("{0}/{1}/members", ConferencePath, Id)), parameters))
            {
                var match = (response.Headers.Location != null)
                    ? ConferenceMemberIdExtractor.Match(response.Headers.Location.OriginalString)
                    : null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return await GetMember(match.Groups[1].Value);
            }
        }

        /// <summary>
        ///     Play an audio/speak
        /// </summary>
        public Task PlayAudio(IDictionary<string, object> parameters)
        {
            return
                Client.MakePostRequest(
                    Client.ConcatUserPath(string.Format("{0}/{1}/audio", ConferencePath, Id)), parameters, true);
        }

        

        public int ActiveMembers { get; set; }
        public string CallbackUrl { get; set; }
        public int CallbackTimeout { get; set; }
        public string FallbackUrl { get; set; }
        public DateTime CompletedTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public string From { get; set; }
        public ConferenceState State { get; set; }
    }

    public class ConferenceMember: BaseModel
    {
        /// <summary>
        ///     Update a conference member. E.g.: remove from call, mute, hold, etc.
        /// </summary>
        public Task Update(IDictionary<string, object> parameters)
        {
            return
                Client.MakePostRequest(
                    Client.ConcatUserPath(string.Format("{0}/{1}/members/{2}", Conference.ConferencePath, ConferenceId, Id)),
                    parameters, true);
        }

        /// <summary>
        ///     Play an audio/speak a sentence to a conference member.
        /// </summary>
        public Task PlayAudio(IDictionary<string, object> parameters)
        {
            return
                Client.MakePostRequest(
                    Client.ConcatUserPath(string.Format("{0}/{1}/members/{2}/audio", Conference.ConferencePath, ConferenceId,
                        Id)), parameters, true);
        }
        internal string ConferenceId { get; set; }
        public string CallId { get; set; }
        public DateTime AddedTime { get; set; }
        public string Call { get; set; }
        public bool Hold { get; set; }
        public bool Mute { get; set; }
        public DateTime RemovedTime { get; set; }
        public bool JoinTone { get; set; }
        public bool LeavingTone { get; set; }
        public MemberState State { get; set; }
    }

    public enum ConferenceState
    {
        Created,
        Active,
        Completed
    }

    public enum MemberState
    {
        Active,
        Completed
    }
}
