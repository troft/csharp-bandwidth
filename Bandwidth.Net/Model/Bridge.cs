using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    /// <summary>
    /// Bridges resource. Bridge two calls allowing two way audio between them.
    /// </summary>
    /// <seealso href="https://catapult.inetwork.com/docs/api-docs/bridges/"/>
    public class Bridge : BaseModel
    {
        private const string BridgePath = "bridges";

        private static readonly Regex BridgeIdExtractor = new Regex(@"/" + BridgePath + @"/([\w\-_]+)$");

        /// <summary>
        /// Gets information about a specific bridge
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="bridgeId">Id of the bridge</param>
        /// <returns>Bridge instance</returns>
        /// <example>
        /// <code>
        /// var bridge = await Bridge.Get(client, "someId");
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/bridges/#GET-/v1/users/{userId}/bridges/{bridgeId}"/>
        public static async Task<Bridge> Get(Client client, string bridgeId)
        {
            if (bridgeId == null) throw new ArgumentNullException("bridgeId");
            var bridge = await client.MakeGetRequest<Bridge>(client.ConcatUserPath(BridgePath), null, bridgeId);
            bridge.Client = client;
            return bridge;
        }
#if !PCL
        /// <summary>
        /// Gets information about a specific bridge
        /// </summary>
        /// <param name="bridgeId">Id of the bridge</param>
        /// <returns>Bridge instance</returns>
        /// <example>
        /// <code>
        /// var bridge = await Bridge.Get("someId");
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/bridges/#GET-/v1/users/{userId}/bridges/{bridgeId}"/>
        public static Task<Bridge> Get(string bridgeId)
        {
            return Get(Client.GetInstance(), bridgeId);
        }
#endif
        /// <summary>
        /// Get list of bridges for a given user
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <returns>Array of Bridge</returns>
        /// <example>
        /// <code>
        /// var bridges = await Bridge.List(client);
        /// </code>
        /// </example>
        public static async Task<Bridge[]> List(Client client)
        {
            var bridges = await client.MakeGetRequest<Bridge[]>(client.ConcatUserPath(BridgePath));
            foreach (var bridge in bridges)
            {
                bridge.Client = client;
            }
            return bridges;
        }

#if !PCL
        /// <summary>
        /// Get list of bridges for a given user
        /// </summary>
        /// <returns>Array of Bridge</returns>
        /// <example>
        /// <code>
        /// var bridges = await Bridge.List(client);
        /// </code>
        /// </example>
        public static Task<Bridge[]> List()
        {
            return List(Client.GetInstance());
        }   
#endif
        /// <summary>
        /// Create a bridge.
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="parameters">Dictionary with keys: bridgeAudio, callIds</param>
        /// <returns>Created Bridge</returns>
        /// <example>
        /// <code>
        /// var bridge = await Bridge.Create(client, new Dictionary&lt;string,object&gt;{{"bridgeAudio", true}, {"callIds", new[]{"callId1", "callId2"}}});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/bridges/#POST-/v1/users/{userId}/bridges"/>
        public static async Task<Bridge> Create(Client client, Dictionary<string, object> parameters)
        {
            using (var response = await client.MakePostRequest(client.ConcatUserPath(BridgePath), parameters))
            {
                var match = (response.Headers.Location != null)
                    ? BridgeIdExtractor.Match(response.Headers.Location.OriginalString)
                    : null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return await Get(client, match.Groups[1].Value);
            }
        }

        /// <summary>
        /// Create a bridge.
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="callIds">The list of call ids in the bridge. If the list of call ids is not provided the bridge is logically created and it can be used to place calls later.</param>
        /// <param name="bridgeAudio">Enable/Disable two way audio path</param>
        /// <returns>Created Bridge</returns>
        /// <example>
        /// <code>
        /// var bridge = await Bridge.Create(client, new[]{"callId1", "callId2"}, true);
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/bridges/#POST-/v1/users/{userId}/bridges"/>
        public static Task<Bridge> Create(Client client, string[] callIds, bool bridgeAudio)
        {
            return Create(client, new Dictionary<string, object>
                {
                    {"callIds", callIds},
                    {"bridgeAudio", bridgeAudio}
                });
        }

#if !PCL
        /// <summary>
        /// Create a bridge.
        /// </summary>
        /// <param name="parameters">Dictionary with keys: bridgeAudio, callIds</param>
        /// <returns>Created Bridge</returns>
        /// <example>
        /// <code>
        /// var bridge = await Bridge.Create(new Dictionary&lt;string,object&gt;{{"bridgeAudio", true}, {"callIds", new[]{"callId1", "callId2"}}});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/bridges/#POST-/v1/users/{userId}/bridges"/>
        public static Task<Bridge> Create(Dictionary<string, object> parameters)
        {
            return Create(Client.GetInstance(), parameters);
        }
#endif
        /// <summary>
        /// Change calls in a bridge and bridge/unbridge the audio
        /// </summary>
        /// <param name="parameters">Dictionary o changed parameters</param>
        /// <example>
        /// <code>
        /// await bridge.Update(new Dictionary&lt;string,object&gt;{{"callIds", new[]{"callId1"}}}); // remove callId1 from the bridge
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/bridges/#POST-/v1/users/{userId}/bridges/{bridgeId}"/>
        public Task Update(Dictionary<string, object> parameters)
        {
            return Client.MakePostRequest(Client.ConcatUserPath(BridgePath + "/" + Id), parameters, true);
        }

        /// <summary>
        /// Play an audio file or speak a sentence in a bridge. 
        /// </summary>
        /// <param name="parameters">Dictionary with optional keys: fileUrl, sentence, gender, locale, voice, loopEnabled</param>
        /// <example>
        /// <code>
        /// await bridge.PlayAudio(new Dictionary&lt;string,object&gt;{{"fileUrl", "http://host/path/to/media/file"}});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/bridges/#POST-/v1/users/{userId}/bridges/{bridgeId}/audio"/>
        public Task PlayAudio(Dictionary<string, object> parameters)
        {
            return Client.MakePostRequest(Client.ConcatUserPath(BridgePath + "/" + Id + "/audio"), parameters, true);
        }

        /// <summary>
        /// Get the list of calls that are on the bridge
        /// </summary>
        /// <returns>Array of Call</returns>
        /// <example>
        /// <code>
        /// var calls = await bridge.GetCalls();
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/bridges/#GET-/v1/users/{userId}/bridges/{bridgeId}/calls"/>
        public async Task<Call[]> GetCalls()
        {
            var calls = await Client.MakeGetRequest<Call[]>(Client.ConcatUserPath(BridgePath + "/" + Id + "/calls"));
            foreach (var call in calls)
            {
                call.Client = Client;
            }
            return calls;
        }

        /// <summary>
        /// Bridge state
        /// </summary>
        public BridgeState State { get; set; }
        
        /// <summary>
        /// List of call Ids that will be in the bridge
        /// </summary>
        public string[] CallIds { get; set; }
        
        /// <summary>
        /// The URL used to retrieve the calls in a specific bridge
        /// </summary>
        public string Calls { get; set; }
        
        /// <summary>
        /// Enable/Disable two way audio path
        /// </summary>
        public bool BridgeAudio { get; set; }

        /// <summary>
        /// The time when the bridge was completed
        /// </summary>
        public DateTime CompletedTime { get; set; }

        /// <summary>
        /// The time that bridge was created
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// The time that the bridge got into active state
        /// </summary>
        public DateTime ActivatedTime { get; set; }

        public enum BridgeState
        {
            Created,
            Active,
            Hold,
            Completed,
            Error
        }

    }
}
