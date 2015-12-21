using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public class EndPoint: BaseModel
    {
        internal const string EndPointPath = "endpoints";
        

        /// <summary>
        /// Remove an endpoint
        /// </summary>
        /// <example>
        /// <code>
        /// await endPoint.Delete();
        /// </code>
        /// </example>
        /// <seealso href="http://ap.bandwidth.com/docs/rest-api/endpoints-2/#resource754"/>
        public Task Delete()
        {
            return Client.MakeDeleteRequest(Client.ConcatUserPath(string.Format("{0}/{1}/{3}/{2}", Domain.DomainPath, DomainId, Id, EndPointPath)));
        }

        /// <summary>
        /// Create auth token
        /// </summary>
        /// <example>
        /// <code>
        /// var token = await endPoint.CreateAuthToken();
        /// </code>
        /// </example>
        public Task<EndPointTokenData> CreateAuthToken(int expires = 86400)
        {
            return Client.MakePostRequest<EndPointTokenData>(Client.ConcatUserPath(string.Format("{0}/{1}/{3}/{2}/tokens", Domain.DomainPath, DomainId, Id, EndPointPath)), new Dictionary<string,object>(){{"expires", expires}});
        }

        /// <summary>
        /// Delete auth token
        /// </summary>
        /// <example>
        /// <code>
        /// await endPoint.DeleteAuthToken("token");
        /// </code>
        /// </example>
        public Task DeleteAuthToken(string token)
        {
            return Client.MakeDeleteRequest(Client.ConcatUserPath(string.Format("{0}/{1}/{3}/{2}/tokens/{4}", Domain.DomainPath, DomainId, Id, EndPointPath, token)));
        }

        /// <summary>
        /// Id of domain
        /// </summary>
        public string DomainId { get; set; }

        /// <summary>
        /// Id of application
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        
        /// <summary>
        /// Enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// SipUri
        /// </summary>
        public string SipUri { get; set; }

        /// <summary>
        /// Credentials parameters
        /// </summary>
        public Dictionary<string, string> Credentials { get; set; }
    }


    /// <summary>
    /// Result of token creation
    /// </summary>
    public class EndPointTokenData
    {
        /// <summary>
        /// Expires
        /// </summary>
        [DefaultValue(0)]
        public int Expires { get; set; }
        
        /// <summary>
        /// Token value
        /// </summary>
        public string Token { get; set; }
    }
}