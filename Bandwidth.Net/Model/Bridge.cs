using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public class Bridge : BaseModel
    {
        private const string BridgePath = "bridges";

        private static readonly Regex BridgeIdExtractor = new Regex(@"/" + BridgePath + @"/([\w\-_]+)$");

        public static async Task<Bridge> Get(Client client, string bridgeId)
        {
            if (bridgeId == null) throw new ArgumentNullException("bridgeId");
            var bridge = await client.MakeGetRequest<Bridge>(client.ConcatUserPath(BridgePath), null, bridgeId);
            bridge.Client = client;
            return bridge;
        }
#if !PCL
        public static Task<Bridge> Get(string bridgeId)
        {
            return Get(Client.GetInstance(), bridgeId);
        }
#endif

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
        public static Task<Bridge[]> List()
        {
            return List(Client.GetInstance());
        }   
#endif

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

        public static Task<Bridge> Create(Client client, string[] callIds, bool bridgeAudio)
        {
            return Create(client, new Dictionary<string, object>
                {
                    {"callIds", callIds},
                    {"bridgeAudio", bridgeAudio}
                });
        }

#if !PCL
        public static Task<Bridge> Create(Dictionary<string, object> parameters)
        {
            return Create(Client.GetInstance(), parameters);
        }
#endif

        public Task Update(Dictionary<string, object> parameters)
        {
            return Client.MakePostRequest(Client.ConcatUserPath(BridgePath + "/" + Id), parameters, true);
        }

        public Task PlayAudio(Dictionary<string, object> parameters)
        {
            return Client.MakePostRequest(Client.ConcatUserPath(BridgePath + "/" + Id + "/audio"), parameters, true);
        }

        public async Task<Call[]> GetCalls()
        {
            var calls = await Client.MakeGetRequest<Call[]>(Client.ConcatUserPath(BridgePath + "/" + Id + "/calls"));
            foreach (var call in calls)
            {
                call.Client = Client;
            }
            return calls;
        }

        public BridgeState State { get; set; }
        public string[] CallIds { get; set; }
        public string Calls { get; set; }
        public bool BridgeAudio { get; set; }
        public DateTime CompletedTime { get; set; }
        public DateTime CreatedTime { get; set; }
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
