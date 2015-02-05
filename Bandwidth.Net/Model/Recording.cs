using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    /// <summary>
    /// Retrieve call recordings, filtering by Id, user and/or calls.
    /// </summary>
    public class Recording: BaseModel
    {
        private const string RecordingPath = "recordings";
        private static readonly Regex TranscriptionIdExtractor = new Regex(@"/transcriptions/([\w\-_]+)$");
        
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
        /// Request the transcription process to be started for the given recording id
        /// </summary>
        /// <returns>Transcription instance</returns>
        /// <example>
        /// <code>
        /// var transcription = await recording.CreateTranscription();
        /// </code>
        /// </example>
        /// <seealso href="http://ap.bandwidth.com/docs/rest-api/recordingsidtranscriptions/#resource456"/>
        public async Task<Transcription> CreateTranscription()
        {
            using (var response = await Client.MakePostRequest(Client.ConcatUserPath(string.Format("{0}/{1}/transcriptions", RecordingPath, Id)), new object()))
            {
                var match = (response.Headers.Location != null)
                    ? TranscriptionIdExtractor.Match(response.Headers.Location.OriginalString)
                    : null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return await GetTranscription(match.Groups[1].Value);
            }
        }

        /// <summary>
        /// Get information about the transcription, regardless its state.
        /// </summary>
        /// <param name="transcriptionId">Id of the transcription</param>
        /// <returns>Transcription instance</returns>
        /// <example>
        /// <code>
        /// var transcription = await call.GetTranscription("transcriptionId");
        /// </code>
        /// </example>
        /// <seealso href="http://ap.bandwidth.com/docs/rest-api/recordingsidtranscriptions/#resource455"/>
        public async Task<Transcription> GetTranscription(string transcriptionId)
        {
            if (transcriptionId == null) throw new ArgumentNullException("transcriptionId");
            var item =
                await Client.MakeGetRequest<Transcription>(
                    Client.ConcatUserPath(string.Format("{0}/{1}/transcriptions/{2}", RecordingPath, Id, transcriptionId)));
            return item;
        }

        /// <summary>
        /// Get all the transcriptions that were made for the given recodingId
        /// </summary>
        /// <returns>List of transcriptions</returns>
        /// <example>
        /// <code>
        /// var transcription = await recording.GetTranscription("transcriptionId");
        /// </code>
        /// </example>
        /// <seealso href="http://ap.bandwidth.com/docs/rest-api/recordingsidtranscriptions/#resource457"/>
        public Task<Transcription[]> GetTranscriptions()
        {
            return Client.MakeGetRequest<Transcription[]>(Client.ConcatUserPath(string.Format("{0}/{1}/transcriptions", RecordingPath, Id)));
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
