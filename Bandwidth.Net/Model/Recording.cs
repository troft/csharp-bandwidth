using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    /// <summary>
    /// Retrieve call recordings, filtering by Id, user and/or calls.
    /// </summary>
    public class Recording: BaseModel
    {
        private const string RecordingPath = "recordings";
        
        /// <summary>
        /// Get recording by id
        /// </summary>
        /// <param name="client">Client iinstance</param>
        /// <param name="id">Id of recording</param>
        /// <returns>Recording info</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/recording/#GET-/v1/users/{userId}/recordings/{recordingId}"/>
        public static async Task<Recording> Get(Client client, string id)
        {
            if (id == null) throw new ArgumentNullException("id");
            var item = await client.MakeGetRequest<Recording>(client.ConcatUserPath(RecordingPath), null, id);
            item.Client = client;
            return item;
        }

        /// <summary>
        /// Get recording by id
        /// </summary>
        /// <param name="id">Id of recording</param>
        /// <returns>Recording info</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/recording/#GET-/v1/users/{userId}/recordings/{recordingId}"/>
        public static Task<Recording> Get(string id)
        {
            return Get(Client.GetInstance(), id);
        }

        /// <summary>
        /// Get recordings of user
        /// </summary>
        /// <param name="client">Client iinstance</param>
        /// <param name="query">Query parameters</param>
        /// <returns>List of recordings</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/recording/#GET-/v1/users/{userId}/recordings"/>
        public static async Task<Recording[]> List(Client client, IDictionary<string, object> query = null)
        {
            var items = await client.MakeGetRequest<Recording[]>(client.ConcatUserPath(RecordingPath), query) ?? new Recording[0];
            foreach (var item in items)
            {
                item.Client = client;
            }
            return items;
        }

        /// <summary>
        /// Get recordings of user
        /// </summary>
        /// <param name="client">Client iinstance</param>
        /// <param name="page">Page number</param>
        /// <param name="size">Page size</param>
        /// <returns>List of recordings</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/recording/#GET-/v1/users/{userId}/recordings"/>
        public static Task<Recording[]> List(Client client, int page, int size = 25)
        {
            var query = new Dictionary<string, object> { { "page", page }, { "size", size } };
            return List(client, query);
        }

        /// <summary>
        /// Get recordings of user
        /// </summary>
        /// <param name="query">Query parameters</param>
        /// <returns>List of recordings</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/recording/#GET-/v1/users/{userId}/recordings"/>
        public static Task<Recording[]> List(IDictionary<string, object> query = null)
        {
            return List(Client.GetInstance(), query);
        }

        /// <summary>
        /// Get recordings of user
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="size">Page size</param>
        /// <returns>List of recordings</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/recording/#GET-/v1/users/{userId}/recordings"/>
        public static Task<Recording[]> List(int page, int size = 25)
        {
            return List(Client.GetInstance(), page, size);
        }

        /// <summary>
        /// Date when the recording started
        /// </summary>
        public DateTime StartTime { get; set; }
        
        /// <summary>
        /// Date when the recording ended
        /// </summary>
        public DateTime EndTime { get; set; }
        
        /// <summary>
        /// Link to the recorded cal
        /// </summary>
        public string Call { get; set; }

        /// <summary>
        /// Media
        /// </summary>
        public string Media { get; set; }

        /// <summary>
        /// State
        /// </summary>
        public RecordingState State { get; set; }
    }

    /// <summary>
    /// Recording states
    /// </summary>
    public enum RecordingState
    {
        Recording,
        Complete,
        Saving,
        Error
    }

}
