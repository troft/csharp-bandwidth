using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bandwidth.Net.Model
{
    /// <summary>
    /// The Applications resource lets you define call and message handling applications. You write an application on your own servers and have Bandwidth API send events to it by configuring a callback URL.
    /// </summary>
    /// <seealso href="https://catapult.inetwork.com/docs/api-docs/applications/"/>
    public class Application : BaseModel
    {
        private const string ApplicationsPath = "applications";

        private static readonly Regex ApplicationsIdExtractor = new Regex(@"/" + ApplicationsPath + @"/([\w\-_]+)$");

        /// <summary>
        /// Get the specified application by ID
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="applicationId">A valid application ID</param>
        /// <returns>Application data</returns>
        /// <example>
        /// <code>
        /// var application = await Application.Get(client, "someId");
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/applications/#GET-/v1/users/{userId}/applications/{applicationId}"/>
        public static async Task<Application> Get(Client client, string applicationId)
        {
            if (applicationId == null) throw new ArgumentNullException("applicationId");
            var instance = await client.MakeGetRequest<Application>(client.ConcatUserPath(ApplicationsPath), null, applicationId);
            instance.Client = client;
            return instance;
        }
        /// <summary>
        /// Get the specified application by ID
        /// </summary>
        /// <param name="applicationId">A valid application ID</param>
        /// <returns>Application data</returns>
        /// <example>
        /// <code>
        /// var application = await Application.Get("someId");
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/applications/#GET-/v1/users/{userId}/applications/{applicationId}"/>
        public static Task<Application> Get(string applicationId)
        {
            return Get(Client.GetInstance(), applicationId);
        }

        /// <summary>
        /// List applications
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="parameters">Dictionary with optional keys: page, size</param>
        /// <returns>Array of Application</returns>
        /// <example>
        /// <code>
        /// var applications = await Application.List(client, new Dictionary&lt;string, object&gt;{{"page", 1}, {"size", 20}});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/applications/#GET-/v1/users/{userId}/applications"/>
        public static async Task<Application[]> List(Client client, IDictionary<string, object> parameters = null)
        {
            var list = await client.MakeGetRequest<Application[]>(client.ConcatUserPath(ApplicationsPath), parameters);
            foreach (var instance in list)
            {
                instance.Client = client;
            }
            return list;
        }

        /// <summary>
        /// List applications
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="page">Page number</param>
        /// <param name="size">Page size (defaults to 25)</param>
        /// <returns>Array of Application</returns>
        /// <example>
        /// <code>
        /// var applications = await Application.List(client, 1, 20);
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/applications/#GET-/v1/users/{userId}/applications"/>
        public static Task<Application[]> List(Client client, int page, int size = 25)
        {
            var query = new Dictionary<string, object> { { "page", page }, { "size", size } };
            return List(client, query);
        }

        /// <summary>
        /// List applications
        /// </summary>
        /// <param name="parameters">Dictionary with optional keys: page, size</param>
        /// <returns>Array of Application</returns>
        /// <example>
        /// <code>
        /// var applications = await Application.List(client, new Dictionary&lt;string, object&gt;{{"page", 1}, {"size", 20}});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/applications/#GET-/v1/users/{userId}/applications"/>
        public static Task<Application[]> List(IDictionary<string, object> parameters = null)
        {
            return List(Client.GetInstance(), parameters);
        }
  
        /// <summary>
        /// List applications
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="size">Page size (defaults to 25)</param>
        /// <returns>Array of Application</returns>
        /// <example>
        /// <code>
        /// var applications = await Application.List(client, 1, 20);
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/applications/#GET-/v1/users/{userId}/applications"/>
        public static Task<Application[]> List(int page, int size = 25)
        {
            return List(Client.GetInstance(), page, size);
        }

        /// <summary>
        /// Create a new application
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="parameters">Dictionary with keys: name, incomingCallUrl, incomingCallUrlCallbackTimeout, incomingCallFallbackUrl, 
        /// incomingMessageUrl, incomingMessUrlCallbackTimeout, incomingMessageFallbackUrl, callbackHttpMethod, autoAnswer</param>
        /// <returns>Created instance of Application</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/applications/#POST-/v1/users/{userId}/applications"/>
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

        /// <summary>
        /// Create a new application
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="name">Application name</param>
        /// <returns>Created instance of Application</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/applications/#POST-/v1/users/{userId}/applications"/>
        public static Task<Application> Create(Client client, string name)
        {
            return Create(client, new Dictionary<string, object>
            {
                {"name", name}
            });
        }

        /// <summary>
        /// Create a new application
        /// </summary>
        /// <param name="parameters">Dictionary with keys: name, incomingCallUrl, incomingCallUrlCallbackTimeout, incomingCallFallbackUrl, 
        /// incomingMessageUrl, incomingMessUrlCallbackTimeout, incomingMessageFallbackUrl, callbackHttpMethod, autoAnswer</param>
        /// <returns>Created instance of Application</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/applications/#POST-/v1/users/{userId}/applications"/>
        public static Task<Application> Create(IDictionary<string, object> parameters)
        {
            return Create(Client.GetInstance(), parameters);
        }

        /// <summary>
        /// Create a new application
        /// </summary>
        /// <param name="name">Application name</param>
        /// <returns>Created instance of Application</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/applications/#POST-/v1/users/{userId}/applications"/>
        public static Task<Application> Create(string name)
        {
            return Create(Client.GetInstance(), name);
        }

        /// <summary>
        /// Changes properties of an application.
        /// </summary>
        /// <param name="parameters">Dictionary of changed properties</param>
        /// <example>
        /// <code>
        /// await application.Update(new Dictionary&lt;string, object&gt;{{"incomingCallUrl", "http://host/path"}});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/applications/#POST-/v1/users/{userId}/applications/{applicationId}"/>
        public Task Update(IDictionary<string, object> parameters)
        {
            return Client.MakePostRequest(Client.ConcatUserPath(string.Format("{0}/{1}", ApplicationsPath, Id)),
                parameters, true);
        }

        /// <summary>
        /// Permanently deletes an application.
        /// </summary>
        /// <example>
        /// <code>
        /// await application.Delete();
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/applications/#DELETE-/v1/users/{userId}/applications/{applicationId}"/>
        public Task Delete()
        {
            return Client.MakeDeleteRequest(Client.ConcatUserPath(ApplicationsPath), Id);
        } 

        /// <summary>
        /// Application name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// A URL where call events will be sent for an inbound call
        /// </summary>
        public string IncomingCallUrl { get; set; }

        /// <summary>
        /// Determine how long should the platform wait for incomingCallUrl's response before timing out in milliseconds.
        /// </summary>
        public int IncomingCallUrlCallbackTimeout { get; set; }

        /// <summary>
        /// The URL used to send the callback event if the request to incomingCallUrl fails.
        /// </summary>
        public string IncomingCallFallbackUrl { get; set; }

        /// <summary>
        /// A URL where message events will be sent for an inbound message.
        /// </summary>
        public string IncomingMessageUrl { get; set; }

        /// <summary>
        /// Determine how long should the platform wait for incomingMessageUrl's response before timing out in milliseconds.
        /// </summary>
        public int IncomingMessageUrlCallbackTimeout { get; set; }

        /// <summary>
        /// The URL used to send the callback event if the request to incomingMessageUrl fails.
        /// </summary>
        public string IncomingMessageFallbackUrl { get; set; }
        
        /// <summary>
        /// Determine if the callback event should be sent via HTTP GET or HTTP POST
        /// </summary>
        public string CallbackHttpMethod { get; set; }

        /// <summary>
        /// Determines whether or not an incoming call should be automatically answered
        /// </summary>
        public bool AutoAnswer { get; set; }

        /// <summary>
        /// Deprecated member IncomingSmsUrl
        /// </summary>
        [Obsolete, JsonIgnore]
        public string IncomingSmsUrl {
            get { return IncomingMessageUrl; }
            set { IncomingMessageUrl = value; }
        }

        /// <summary>
        /// Deprecated member IncomingSmsUrlCallbackTimeout
        /// </summary>
        [Obsolete, JsonIgnore]
        public int IncomingSmsUrlCallbackTimeout
        {
            get { return IncomingMessageUrlCallbackTimeout; }
            set { IncomingMessageUrlCallbackTimeout = value; }
        }

        /// <summary>
        /// Deprecated member IncomingSmsFallbackUrl
        /// </summary>
        [Obsolete, JsonIgnore]
        public string IncomingSmsFallbackUrl
        {
            get { return IncomingMessageFallbackUrl; }
            set { IncomingMessageFallbackUrl = value; }
        }
    }


}
