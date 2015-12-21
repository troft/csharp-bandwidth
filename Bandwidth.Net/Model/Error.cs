using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    /// <summary>
    /// The User Errors resource lets you see information about errors that happened in your API calls and during applications callbacks.
    /// </summary>
    /// <seealso href="https://catapult.inetwork.com/docs/api-docs/errors/"/>
    public class Error: BaseModel
    {
        private const string ErrorPath = "errors";


        /// <summary>
        /// Gets information about one user error.
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="id">Id of error</param>
        /// <returns>Error information</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/errors/#GET-/v1/users/{userId}/errors/{userErrorId}"/>
        public static async Task<Error> Get(Client client, string id)
        {
            if (id == null) throw new ArgumentNullException("id");
            var item = await client.MakeGetRequest<Error>(client.ConcatUserPath(ErrorPath), null, id);
            item.Client = client;
            return item;
        }
        /// <summary>
        /// Gets information about one user error.
        /// </summary>
        /// <param name="id">Id of error</param>
        /// <returns>Error information</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/errors/#GET-/v1/users/{userId}/errors/{userErrorId}"/>
        public static Task<Error> Get(string id)
        {
            return Get(Client.GetInstance(), id);
        }

        /// <summary>
        /// Gets the most recent user errors for the user. 
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="query">Query parameters</param>
        /// <returns>List of errors</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/errors/#GET-/v1/users/{userId}/errors"/>
        public static async Task<Error[]> List(Client client, IDictionary<string, object> query = null)
        {
            var items = await client.MakeGetRequest<Error[]>(client.ConcatUserPath(ErrorPath), query) ?? new Error[0];
            foreach (var item in items)
            {
                item.Client = client;
            }
            return items;
        }

        /// <summary>
        /// Gets the most recent user errors for the user. 
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="page">Page number</param>
        /// <param name="size">Size of page</param>
        /// <returns>List of errors</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/errors/#GET-/v1/users/{userId}/errors"/>
        public static Task<Error[]> List(Client client, int page, int size = 25)
        {
            var query = new Dictionary<string, object> {{"page", page}, {"size", size}};
            return List(client, query);
        }

        /// <summary>
        /// Gets the most recent user errors for the user. 
        /// </summary>
        /// <param name="query">Query parameters</param>
        /// <returns>List of errors</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/errors/#GET-/v1/users/{userId}/errors"/>
        public static Task<Error[]> List(IDictionary<string, object> query = null)
        {
            return List(Client.GetInstance(), query);
        }

        /// <summary>
        /// Gets the most recent user errors for the user. 
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="size">Size of page</param>
        /// <returns>List of errors</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/errors/#GET-/v1/users/{userId}/errors"/>
        public static Task<Error[]> List(int page, int size = 25)
        {
            return List(Client.GetInstance(), page, size);
        }

        /// <summary>
        /// Error time
        /// </summary>
        public DateTime Time { get; set; }
        
        /// <summary>
        /// Error category
        /// </summary>
        public ErrorCategory Category { get; set; }
        
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// Error code
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// Error details
        /// </summary>
        public ErrorDetail[] Details { get; set; }
    }

    /// <summary>
    /// Error detail
    /// </summary>
    public class ErrorDetail
    {
        /// <summary>
        /// Name of datail
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Vallue of detail
        /// </summary>
        public string Value { get; set; }
    }

    public enum ErrorCategory
    {
        Authentication,
        Authorization,
        [EnumMember(Value = "not-found")]
        NotFound,
        [EnumMember(Value = "bad-request")]
        BadRequest,
        Conflict,
        Unavailable,
        Credit,
        Limit,
        Forbidden,
        Payment
    }
}