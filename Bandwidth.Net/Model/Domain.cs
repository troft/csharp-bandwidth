using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public class Domain: BaseModel
    {
        private static readonly Regex EndpointIdExtractor = new Regex(@"/" + EndPoint.EndPointPath + @"/([\w\-_]+)$");
        private static readonly Regex DomainIdExtractor = new Regex(@"/" + DomainPath + @"/([\w\-_]+)$");
        
        internal const string DomainPath = "domains";

        /// <summary>
        ///  Gets a list of created domains. 
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <returns>Array of Domain</returns>
        /// <example>
        /// <code>
        /// var domains = await Domain.List(client);
        /// </code>
        /// </example>
        /// <seealso href="http://ap.bandwidth.com/docs/rest-api/domains-2/#resource745"/>
        public static async Task<Domain[]> List(Client client)
        {
            var domains = await client.MakeGetRequest<Domain[]>(client.ConcatUserPath(DomainPath)) ?? new Domain[0];
            foreach (var domain in domains)
            {
                domain.Client = client;
            }
            return domains;
        }

#if !PCL
        /// <summary>
        ///  Gets a list of created domains. 
        /// </summary>
        /// <returns>Array of Domain</returns>
        /// <example>
        /// <code>
        /// var domains = await Domain.List(client);
        /// </code>
        /// </example>
        /// <seealso href="http://ap.bandwidth.com/docs/rest-api/domains-2/#resource745"/>
        public static Task<Domain[]> List()
        {
            return List(Client.GetInstance());
        }

#endif


        /// <summary>
        /// Create a domain.
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="parameters">Parameters to create domain</param>
        /// <returns>Created domain</returns>
        /// <example>
        /// <code>
        /// var domain = await Domain.Create(client, new Dictionary&lt;string,object&gt;{{"name", "domain1"}}); 
        /// </code>
        /// </example>
        /// <seealso href="http://ap.bandwidth.com/docs/rest-api/domains-2/#resource746"/>
        public static async Task<Domain> Create(Client client, IDictionary<string, object> parameters)
        {
            using (var response = await client.MakePostRequest(client.ConcatUserPath(DomainPath), parameters))
            {
                var match = (response.Headers.Location != null)
                    ? DomainIdExtractor.Match(response.Headers.Location.OriginalString)
                    : null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return new Domain { Id = match.Groups[1].Value, Name = parameters["name"] as string, Description = parameters["description"] as string, Client = client};
            }
        }

#if !PCL
        /// <summary>
        /// Create a domain.
        /// </summary>
        /// <param name="parameters">Parameters to create domain</param>
        /// <returns>Created domain</returns>
        /// <example>
        /// <code>
        /// var domain = await Domain.Create(client, new Dictionary&lt;string,object&gt;{{"name", "domain1"}}); 
        /// </code>
        /// </example>
        /// <seealso href="http://ap.bandwidth.com/docs/rest-api/domains-2/#resource746"/>
        public static Task<Domain> Create(IDictionary<string, object> parameters)
        {
            return Create(Client.GetInstance(), parameters);
        }

#endif
        /// <summary>
        /// Remove a domain
        /// </summary>
        /// <example>
        /// <code>
        /// await domain.Delete();
        /// </code>
        /// </example>
        /// <seealso href="http://ap.bandwidth.com/docs/rest-api/domains-2/#resource747"/>
        public Task Delete()
        {
            return Client.MakeDeleteRequest(Client.ConcatUserPath(string.Format("{0}/{1}", DomainPath, Id)));
        }
        
        /// <summary>
        /// This returns a list of all endpoints associated with a domain.
        /// </summary>
        /// <returns>List of endpoints</returns>
        /// <example>
        /// <code>
        /// var endPoints = await domain.GetEndPoints();
        /// </code>
        /// </example>
        /// <seealso href="http://ap.bandwidth.com/docs/rest-api/endpoints-2/#resource751"/>
        public async Task<EndPoint[]> GetEndPoints()
        {
            var list = await Client.MakeGetRequest<EndPoint[]>(Client.ConcatUserPath(string.Format("{0}/{1}/{2}", DomainPath, Id, EndPoint.EndPointPath))) ?? new EndPoint[0];
            foreach (var endPoint in list)
            {
                endPoint.Client = Client;
                endPoint.DomainId = Id;
            }
            return list;
        }

        /// <summary>
        /// This returns an endpoint associated with a domain.
        /// </summary>
        /// <returns>Endpoint instance</returns>
        /// <example>
        /// <code>
        /// var endPoint = await domain.GetEndPoint("id");
        /// </code>
        /// </example>
        /// <seealso href="http://ap.bandwidth.com/docs/rest-api/endpoints-2/#resource752"/>
        public async Task<EndPoint> GetEndPoint(string id)
        {
            var item = await Client.MakeGetRequest<EndPoint>(Client.ConcatUserPath(string.Format("{0}/{1}/{3}/{2}", DomainPath, Id, id, EndPoint.EndPointPath)));
            item.Client = Client;
            item.DomainId = Id;
            return item;
        }
 
        /// <summary>
        /// Create an endpoint.
        /// </summary>
        /// <param name="parameters">Parameters to create endpoint</param>
        /// <returns>Endpoint instance</returns>
        /// <example>
        /// <code>
        /// var endPoint = await domain.CreateEndPoint(new Dictionary&lt;string, object&gt;{{"name", "name"}, {"application_id", "id"}, {"enabled", false}});
        /// </code>
        /// </example>
        /// <seealso href="http://ap.bandwidth.com/docs/rest-api/endpoints-2/#resource750"/>
        public async Task<EndPoint> CreateEndPoint(Dictionary<string, object> parameters)
        {
            using (var response = await Client.MakePostRequest(Client.ConcatUserPath(string.Format("{0}/{1}/endpoints", DomainPath, Id)), parameters))
            {
                var match = (response.Headers.Location != null)
                    ? EndpointIdExtractor.Match(response.Headers.Location.OriginalString)
                    : null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return await GetEndPoint(match.Groups[1].Value);
            }
        }

        /// <summary>
        /// Remove an endpoint
        /// </summary>
        /// <example>
        /// <code>
        /// await domain.DeleteEndPoint("id");
        /// </code>
        /// </example>
        /// <seealso href="http://ap.bandwidth.com/docs/rest-api/endpoints-2/#resource754"/>
        public Task DeleteEndPoint(string id)
        {
            return (new EndPoint {Client = Client, DomainId = Id, Id = id}).Delete();
        }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

    }
}
