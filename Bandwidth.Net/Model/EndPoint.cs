using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
}