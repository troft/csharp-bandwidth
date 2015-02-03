using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    /// <summary>
    /// The Conference resource allows you create conferences, add members to it, play audio, speak text, mute/unmute members, hold/unhold members and other things related to conferencing. 
    /// </summary>
    /// <seealso href="https://catapult.inetwork.com/docs/api-docs/conferences/"/>
    public class Conference: BaseModel
    {
        internal const string ConferencePath = "conferences";
        
        private static readonly Regex ConferenceIdExtractor = new Regex(@"/" + ConferencePath + @"/([\w\-_]+)$");
        private static readonly Regex ConferenceMemberIdExtractor = new Regex(@"/members/([\w\-_]+)$");

        /// <summary>
        /// Retrieve the conference information.
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="conferenceId">Id of the conference</param>
        /// <returns>Conference instance</returns>
        /// <example>
        /// <code>
        /// var conference = await Conference.Get(client, "conferenceId");
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/conferences/#GET-/v1/users/{userId}/conferences/{conferenceId}"/>
        public async static Task<Conference> Get(Client client, string conferenceId)
        {
            if (conferenceId == null) throw new ArgumentNullException("conferenceId");
            var conference = await client.MakeGetRequest<Conference>(client.ConcatUserPath(ConferencePath), null, conferenceId);
            conference.Client = client;
            return conference;
        }

        /// <summary>
        /// Retrieve the conference information.
        /// </summary>
        /// <param name="conferenceId">Id of the conference</param>
        /// <returns>Conference instance</returns>
        /// <example>
        /// <code>
        /// var conference = await Conference.Get("conferenceId");
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/conferences/#GET-/v1/users/{userId}/conferences/{conferenceId}"/>
        public static Task<Conference> Get(string conferenceId)
        {
            return Get(Client.GetInstance(), conferenceId);
        }

        /// <summary>
        /// Creates a conference with no members
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="parameters">Dictionary with keys: from, callbackUrl, callbackTimeout, fallbackUrl</param>
        /// <returns>Conference instance</returns>
        /// <example>
        /// <code>
        /// var conference = await Conference.Create(client, new Dictionary&lt;string,object&gt;{{"from", "fromNumber"}});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/conferences/#POST-/v1/users/{userId}/conferences"/>
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

        /// <summary>
        /// Creates a conference with no members
        /// </summary>
        /// <param name="parameters">Dictionary with keys: from, callbackUrl, callbackTimeout, fallbackUrl</param>
        /// <returns>Conference instance</returns>
        /// <example>
        /// <code>
        /// var conference = await Conference.Create(client, new Dictionary&lt;string,object&gt;{{"from", "fromNumber"}});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/conferences/#POST-/v1/users/{userId}/conferences"/>
        public static Task<Conference> Create(IDictionary<string, object> parameters)
        {
            return Create(Client.GetInstance(), parameters);
        }

        /// <summary>
        /// Change the conference properties/status.
        /// </summary>
        /// <param name="parameters">Changed properties</param>
        /// <example>
        /// <code>
        /// await conference.Update(new Dictionary&lt;string,object&gt;{{"state", "completed"}});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/conferences/#POST-/v1/users/{userId}/conferences/{conferenceId}"/>
        public Task Update(IDictionary<string, object> parameters)
        {
            return Client.MakePostRequest(Client.ConcatUserPath(string.Format("{0}/{1}", ConferencePath, Id)),
                parameters, true);
        }

        /// <summary>
        /// Complete the conference
        /// </summary>
        /// <example>
        /// <code>
        /// await conference.Complete();
        /// </code>
        /// </example>
        /// <seealso cref="Conference.Update"/>
        public Task Complete()
        {
            return Update(new Dictionary<string, object> { { "state", "completed" } });
        }

        /// <summary>
        /// Mute the conference
        /// </summary>
        /// <example>
        /// <code>
        /// await conference.Mute();
        /// </code>
        /// </example>
        /// <seealso cref="Conference.Update"/>
        public Task Mute()
        {
            return Update(new Dictionary<string, object> { { "mute", true } });
        }

        /// <summary>
        /// List all members from a conference
        /// </summary>
        /// <returns>Array of ConferenceMember</returns>
        /// <example>
        /// <code>
        /// var members = await conference.GetMembers();
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/conferences/#GET-/v1/users/{userId}/conferences/{conferenceId}/members"/>
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
        /// Retrieve a conference member attributes/properties
        /// </summary>
        /// <param name="memberId">Id of the memeber</param>
        /// <returns>ConferenceMember instance</returns>
        /// <example>
        /// <code>
        /// var member = await Conference.GetMember("memberId");
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/conferences/#GET-/v1/users/{userId}/conferences/{conferenceId}/members/{memberId}"/>
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
        /// Add members to a conference.
        /// </summary>
        /// <param name="parameters">Dictionary with keys: callId, joinTone, leavingTone</param>
        /// <returns>ConferenceMember instance</returns>
        /// <example>
        /// <code>
        /// var member = await conference.CreateMember(new Dictionary&lt;string,object&gt;{{"callId", "id"}});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/conferences/#POST-/v1/users/{userId}/conferences/{conferenceId}/members"/>
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
        /// Speak a text or play audio in the conference
        /// </summary>
        /// <param name="parameters">Dictionary with optional keys: fileUrl, sentence, gender, locale, voice, loopEnabled, tag</param>
        /// <example>
        /// <code>
        /// await conference.PlayAudio(new Dictionary&lt;string, object&gt;{{"fileUrl", "http://url/to/media/file"}});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/conferences/#POST-/v1/users/{userId}/conferences/{conferenceId}/audio"/>
        public Task PlayAudio(IDictionary<string, object> parameters)
        {
            return
                Client.MakePostRequest(
                    Client.ConcatUserPath(string.Format("{0}/{1}/audio", ConferencePath, Id)), parameters, true);
        }

        
        /// <summary>
        /// Number of active conference members
        /// </summary>
        public int ActiveMembers { get; set; }

        /// <summary>
        /// URL where the events related to the Conference will be posted to
        /// </summary>
        public string CallbackUrl { get; set; }

        /// <summary>
        /// Determine how long should the platform wait for callbackUrl's response before timing out in milliseconds.
        /// </summary>
        public int CallbackTimeout { get; set; }

        /// <summary>
        /// The URL used to send the callback event if the request to callbackUrl fails.
        /// </summary>
        public string FallbackUrl { get; set; }

        /// <summary>
        /// The time that the Conference was completed
        /// </summary>
        public DateTime CompletedTime { get; set; }

        /// <summary>
        /// The time that the Conference was created
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// The number that will host the conference
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Conference state
        /// </summary>
        public ConferenceState State { get; set; }
    }

    /// <summary>
    /// Member of a conference
    /// </summary>
    public class ConferenceMember: BaseModel
    {
        /// <summary>
        /// Update a member status/properties.
        /// </summary>
        /// <param name="parameters">Changed properties</param>
        /// <example>
        /// <code>
        /// var member = await conference.GetMember("id");
        /// await member.Update(new Dictionary&lt;string,object&&gt;{{"mute", true}});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/conferences/#POST-/v1/users/{userId}/conferences/{conferenceId}/members/{memberId}"/>
        public Task Update(IDictionary<string, object> parameters)
        {
            return
                Client.MakePostRequest(
                    Client.ConcatUserPath(string.Format("{0}/{1}/members/{2}", Conference.ConferencePath, ConferenceId, Id)),
                    parameters, true);
        }

        /// <summary>
        /// Speak text or play audio to a conference member
        /// </summary>
        /// <param name="parameters">Dictionary with optional keys: fileUrl, sentence, gender, locale, voice, loopEnabled, tag</param>
        /// <example>
        /// <code>
        /// await member.PlayAudio(new Dictionary&lt;string, object&gt;{{"fileUrl", "http://url/to/media/file"}});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/conferences/#POST-/v1/users/{userId}/conferences/{conferenceId}/members/{memberId}/audio"/>
        public Task PlayAudio(IDictionary<string, object> parameters)
        {
            return
                Client.MakePostRequest(
                    Client.ConcatUserPath(string.Format("{0}/{1}/members/{2}/audio", Conference.ConferencePath, ConferenceId,
                        Id)), parameters, true);
        }
        internal string ConferenceId { get; set; }
        /// <summary>
        /// Id of the cakk
        /// </summary>
        public string CallId { get; set; }
        
        /// <summary>
        /// Date when the member was added to the conference
        /// </summary>
        public DateTime AddedTime { get; set; }
        
        /// <summary>
        /// The URL used to retrieve the call of the member
        /// </summary>
        public string Call { get; set; }

        /// <summary>
        /// true - member can't hear the conference / false - member can hear the conference
        /// </summary>
        public bool Hold { get; set; }

        /// <summary>
        /// true - member can't speak in the conference / false - member can speak in the conference
        /// </summary>
        public bool Mute { get; set; }

        /// <summary>
        /// Date when member was removed from conference
        /// </summary>
        public DateTime RemovedTime { get; set; }
        
        /// <summary>
        /// true - play a tone when the new member joins the conference / false - don't play a tone when the new member joins the conference
        /// </summary>
        public bool JoinTone { get; set; }

        /// <summary>
        /// true - play a tone when the new member leaves the conference / false - don't play a tone when the new member leaves the conference
        /// </summary>
        public bool LeavingTone { get; set; }

        /// <summary>
        /// Member state
        /// </summary>
        public MemberState State { get; set; }
    }

    /// <summary>
    /// Conference states
    /// </summary>
    public enum ConferenceState
    {
        /// <summary>
        /// Created state
        /// </summary>
        Created,
        
        /// <summary>
        /// Active state
        /// </summary>
        Active,
        
        /// <summary>
        /// Completed state
        /// </summary>
        Completed
    }

    /// <summary>
    /// Conference member states
    /// </summary>
    public enum MemberState
    {
        /// <summary>
        /// Active state
        /// </summary>
        Active,

        /// <summary>
        /// Completed state
        /// </summary>
        Completed
    }
}
