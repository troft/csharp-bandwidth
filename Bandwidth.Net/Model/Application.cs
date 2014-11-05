using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public class Application : BaseModel
    {
        private const string ApplicationsPath = "applications";

        private static readonly Regex ApplicationsIdExtractor = new Regex(@"/" + ApplicationsPath + @"/([\w\-_]+)$");

        public static async Task<Application> Get(Client client, string applicationId)
        {
            if (applicationId == null) throw new ArgumentNullException("applicationId");
            var instance = await client.MakeGetRequest<Application>(client.ConcatUserPath(ApplicationsPath), null, applicationId);
            instance.Client = client;
            return instance;
        }
#if !PCL
        public static Task<Application> Get(string applicationId)
        {
            return Get(Client.GetInstance(), applicationId);
        }
#endif

        public static async Task<Application[]> List(Client client, IDictionary<string, object> parameters = null)
        {
            var list = await client.MakeGetRequest<Application[]>(client.ConcatUserPath(ApplicationsPath), parameters);
            foreach (var instance in list)
            {
                instance.Client = client;
            }
            return list;
        }

        public static Task<Application[]> List(Client client, int page, int size = 25)
        {
            var query = new Dictionary<string, object> { { "page", page }, { "size", size } };
            return List(client, query);
        }
#if !PCL
        public static Task<Application[]> List(IDictionary<string, object> parameters = null)
        {
            return List(Client.GetInstance(), parameters);
        }

        public static Task<Application[]> List(int page, int size = 25)
        {
            return List(Client.GetInstance(), page, size);
        }
#endif

        ///<summary>
        ///     Send a text message
        /// </summary>
        public static async Task<Application> Create(Client client, IDictionary<string, object> parameters)
        {
            using (var response = await client.MakePostRequest(client.ConcatUserPath(ApplicationsPath), parameters))
            {
                var match = (response.Headers.Location != null)
                    ? ApplicationsIdExtractor.Match(response.Headers.Location.OriginalString)
                    : null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return await Get(client, match.Groups[1].Value);
            }
        }

        public static Task<Application> Create(Client client, string name)
        {
            return Create(client, new Dictionary<string, object>
            {
                {"name", name}
            });
        }

#if !PCL
        public static Task<Application> Create(IDictionary<string, object> parameters)
        {
            return Create(Client.GetInstance(), parameters);
        }

        public static Task<Application> Create(string name)
        {
            return Create(Client.GetInstance(), name);
        }
#endif

        public Task Delete()
        {
            return Client.MakeDeleteRequest(Client.ConcatUserPath(ApplicationsPath), Id);
        } 

        public string Name { get; set; }
        public string IncomingCallUrl { get; set; }
        public int IncomingCallUrlCallbackTimeout { get; set; }
        public string IncomingCallFallbackUrl { get; set; }
        public string IncomingSmsUrl { get; set; }
        public int IncomingSmsUrlCallbackTimeout { get; set; }
        public string IncomingSmsFallbackUrl { get; set; }
        public string CallbackHttpMethod { get; set; }
        public bool AutoAnswer { get; set; }
    }


}
