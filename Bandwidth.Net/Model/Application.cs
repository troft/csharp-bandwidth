using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public class Application : BaseModel
    {
        private const string ApplicationsPath = "applications";

        private static readonly Regex ApplicationsIdExtractor = new Regex(@"/" + ApplicationsPath + @"/([\w\-_]+)$");

        public static Task<Application> Get(Client client, string applicationId)
        {
            if (applicationId == null) throw new ArgumentNullException("applicationId");
            return client.MakeGetRequest<Application>(client.ConcatUserPath(ApplicationsPath), null, applicationId);
        }
#if !PCL
        public static Task<Application> Get(string applicationId)
        {
            return Get(Client.GetInstance(), applicationId);
        }
#endif

        public static Task<Application[]> List(Client client, IDictionary<string, object> parameters = null)
        {
            return client.MakeGetRequest<Application[]>(client.ConcatUserPath(ApplicationsPath), parameters);
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

        public static async void Delete(Client client, string applicationId)
        {
            await client.MakeDeleteRequest(client.ConcatUserPath(ApplicationsPath), applicationId);
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
