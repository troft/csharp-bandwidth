using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    /// <summary>
    /// The Calls resource lets you make phone calls and view information about previous inbound and outbound calls.
    /// </summary>
    /// <seealso href="https://catapult.inetwork.com/docs/api-docs/calls/"/>
    public class Call: BaseModel
    {
        private const string CallPath = "calls";

        private static readonly Regex CallIdExtractor = new Regex(@"/" + CallPath + @"/([\w\-_]+)$");
        private static readonly Regex GatherIdExtractor = new Regex(@"/gather/([\w\-_]+)$");
        
        /// <summary>
        /// Gets information about an active or completed call
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="callId">Id of the call</param>
        /// <returns>Call instance</returns>
        /// <example>
        /// <code>
        /// var call = await Call.Get(client, "callId");
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/calls/#GET-/v1/users/{userId}/calls/{callId}"/>
        public static async Task<Call> Get(Client client, string callId)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            var call = await client.MakeGetRequest<Call>(client.ConcatUserPath(CallPath), null, callId);
            call.Client = client;
            return call;
        }

        /// <summary>
        /// Gets information about an active or completed call
        /// </summary>
        /// <param name="callId">Id of the call</param>
        /// <returns>Call instance</returns>
        /// <example>
        /// <code>
        /// var call = await Call.Get("callId");
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/calls/#GET-/v1/users/{userId}/calls/{callId}"/>
        public static Task<Call> Get(string callId)
        {
            return Get(Client.GetInstance(), callId);
        }

        /// <summary>
        ///  Gets a list of active and historic calls user made or received. 
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="query">Dictionary with optional keys: bridgeId, conferenceId, from, to, page, size</param>
        /// <returns>Array of Call</returns>
        /// <example>
        /// <code>
        /// var calls = await Call.List(client, new Dictionary&lt;string, object&gt;{{"page", 1}});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/calls/#GET-/v1/users/{userId}/calls"/>
        public static async Task<Call[]> List(Client client, IDictionary<string, object> query = null )
        {
            var calls = await client.MakeGetRequest<Call[]>(client.ConcatUserPath(CallPath), query) ?? new Call[0];
            foreach (var call in calls)
            {
                call.Client = client;
            }
            return calls;
        }

        /// <summary>
        ///  Gets a list of active and historic calls user made or received. 
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="page">Page number</param>
        /// <param name="size">Page size (default 25)</param>
        /// <returns>Array of Call</returns>
        /// <example>
        /// <code>
        /// var calls = await Call.List(client, 1, 100);
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/calls/#GET-/v1/users/{userId}/calls"/>
        public static Task<Call[]> List(Client client, int page, int size = 25)
        {
            var query = new Dictionary<string, object> {{"page", page}, {"size", size}};
            return List(client, query);
        }

        /// <summary>
        ///  Gets a list of active and historic calls user made or received. 
        /// </summary>
        /// <param name="query">Dictionary with optional keys: bridgeId, conferenceId, from, to, page, size</param>
        /// <returns>Array of Call</returns>
        /// <example>
        /// <code>
        /// var calls = await Call.List(new Dictionary&lt;string, object&gt;{{"page", 1}});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/calls/#GET-/v1/users/{userId}/calls"/>
        public static Task<Call[]> List(IDictionary<string, object> query = null)
        {
            return List(Client.GetInstance(), query);
        }

        /// <summary>
        ///  Gets a list of active and historic calls user made or received. 
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="size">Page size (default 25)</param>
        /// <returns>Array of Call</returns>
        /// <example>
        /// <code>
        /// var calls = await Call.List(1, 100);
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/calls/#GET-/v1/users/{userId}/calls"/>
        public static Task<Call[]> List(int page, int size =  25)
        {
            return List(Client.GetInstance(), page, size);
        }


        /// <summary>
        /// Makes a phone call.
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="parameters">Dictionary with keys: callbackUrl, from, to, recordingEnabled, bridgeId, tag</param>
        /// <returns>Created Call instance</returns>
        /// <example>
        /// <code>
        /// var call = await Call.Create(client, new Dictionary&lt;string,object&gt;{{"from", "fromNumber"}, {"to", "toNumber"}}); 
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/calls/#POST-/v1/users/{userId}/calls"/>
        public static async Task<Call> Create(Client client, IDictionary<string, object> parameters)
        {
            using (var response = await client.MakePostRequest(client.ConcatUserPath(CallPath), parameters))
            {
                var match = (response.Headers.Location != null)
                    ? CallIdExtractor.Match(response.Headers.Location.OriginalString)
                    : null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return await Get(client, match.Groups[1].Value);
            }
        }

        /// <summary>
        /// Makes a phone call.
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="to">The number to call (must be either an E.164 formated number, like +19195551212, or a valid SIP URI, like sip:someone@somewhere.com)</param>
        /// <param name="from">One of user's telephone numbers the call should come from (must be in E.164 format, like +19195551212)</param>
        /// <param name="callbackUrl">URL to send call events to</param>
        /// <param name="tag">A string that will be included in the callback events of the call.</param>
        /// <returns>Created Call instance</returns>
        /// <example>
        /// <code>
        /// var call = await Call.Create(client, "toNumber", "fromNumber", "http://host", "tag"); 
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/calls/#POST-/v1/users/{userId}/calls"/>
        public static Task<Call> Create(Client client, string to, string from, string callbackUrl = "none", string tag = null)
        {
            return Create(client, new Dictionary<string, object>
            {
                {"to", to},
                {"from", from},
                {"callbackUrl", callbackUrl},
                {"tag", tag}
            });
        }

        /// <summary>
        /// Makes a phone call.
        /// </summary>
        /// <param name="parameters">Dictionary with keys: callbackUrl, from, to, recordingEnabled, bridgeId, tag</param>
        /// <returns>Created Call instance</returns>
        /// <example>
        /// <code>
        /// var call = await Call.Create(client, new Dictionary&lt;string,object&gt;{{"from", "fromNumber"}, {"to", "toNumber"}}); 
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/calls/#POST-/v1/users/{userId}/calls"/>
        public static Task<Call> Create(IDictionary<string, object> parameters)
        {
            return Create(Client.GetInstance(), parameters);
        }

        /// <summary>
        /// Makes a phone call.
        /// </summary>
        /// <param name="to">The number to call (must be either an E.164 formated number, like +19195551212, or a valid SIP URI, like sip:someone@somewhere.com)</param>
        /// <param name="from">One of user's telephone numbers the call should come from (must be in E.164 format, like +19195551212)</param>
        /// <param name="callbackUrl">URL to send call events to</param>
        /// <param name="tag">A string that will be included in the callback events of the call.</param>
        /// <returns>Created Call instance</returns>
        /// <example>
        /// <code>
        /// var call = await Call.Create("toNumber", "fromNumber", "http://host", "tag"); 
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/calls/#POST-/v1/users/{userId}/calls"/>
        public static Task<Call> Create(string to, string from, string callbackUrl = "none", string tag = null)
        {
            return Create(Client.GetInstance(), to, from, callbackUrl, tag);
        }

        /// <summary>
        /// Changes properties of an active phone call.
        /// </summary>
        /// <param name="parameters">Changed properties</param>
        /// <example>
        /// <code>
        /// await call.Update(new Dictionary&lt;string,object&gt;{{"state", "active"}});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/calls/#POST-/v1/users/{userId}/calls/{callId}"/>
        public Task Update(IDictionary<string, object> parameters)
        {
            return Client.MakePostRequest(Client.ConcatUserPath(string.Format("{0}/{1}", CallPath, Id)),
                parameters, true);
        }

        /// <summary>
        /// Plays an audio file or speak a sentence in a phone call. 
        /// </summary>
        /// <param name="parameters">Dictionary with optional keys: fileUrl, sentence, gender, locale, voice, loopEnabled, tag</param>
        /// <example>
        /// <code>
        /// await call.PlayAudio(new Dictionary&lt;string, object&gt;{{"fileUrl", "http://url/to/media/file"}});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/calls/#POST-/v1/users/{userId}/calls/{callId}/audio"/>
        public Task PlayAudio(Dictionary<string, object> parameters)
        {
            return Client.MakePostRequest(Client.ConcatUserPath(string.Format("{0}/{1}/audio", CallPath, Id)),
                parameters, true);
        }

        /// <summary>
        /// Speak a sentence in a phone call
        /// </summary>
        /// <param name="sentence">Sentennce to speak</param>
        /// <param name="tag">Optional tag value which will be available in events</param>
        /// <example>
        /// await call.SpeakSentence("Hello");
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/calls/#POST-/v1/users/{userId}/calls/{callId}/audio"/>
        public Task SpeakSentence(string sentence, string tag = null)
        {
            return PlayAudio(new Dictionary<string, object>
            {
                {"gender", "female"},
                {"locale", "en_US"},
                {"voice", "kate"},
                {"sentence", sentence},
                {"tag", tag}
            });
        }

        /// <summary>
        /// Play external media in a phone call
        /// </summary>
        /// <param name="recordingUrl">URL to external media</param>
        /// <example>
        /// await call.PlayRecording("http://url/to/media");
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/calls/#POST-/v1/users/{userId}/calls/{callId}/audio"/>
        public Task PlayRecording(string recordingUrl)
        {
            if (recordingUrl == null) throw new ArgumentNullException("recordingUrl");
            return PlayAudio(new Dictionary<string, object>
            {
                { "fileUrl", recordingUrl}
            });
        }

        /// <summary>
        /// Send DTMF
        /// </summary>
        /// <param name="dtmf">the string containing the DTMF characters to be sent in a call</param>
        /// <example>
        /// <code>
        /// await call.SendDtmf("1234");
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/calls/#POST-/v1/users/{userId}/calls/{callId}/dtmf"/>
        public Task SendDtmf(string dtmf)
        {
            return Client.MakePostRequest(Client.ConcatUserPath(string.Format("{0}/{1}/dtmf", CallPath, Id)),
                new Dictionary<string, object> { { "dtmfOut", dtmf } }, true);
        }

        /// <summary>
        /// Collects a series of DTMF digits from a phone call with an optional prompt
        /// </summary>
        /// <param name="parameters">Dictionary with optional keys: maxDigits, interDigitTimeout, terminatingDigits, tag, prompt</param>
        /// <returns>Created Gather instance</returns>
        /// <example>
        /// <code>
        /// var gather = await call.CreateGather(new Dictionary&lt;string,object&gt;{{"maxDigits", 3}, {"interDigitTimeout", 5}, {"prompt": new Dictionary&lt;string,object&gt;{
        ///     {"sentence": "Please enter 3 digits"}
        /// }});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/calls/#POST-/v1/users/{userId}/calls/{callId}/gather"/>
        public async Task<Gather> CreateGather(IDictionary<string, object> parameters)
        {
            using (var response = await Client.MakePostRequest(Client.ConcatUserPath(string.Format("{0}/{1}/gather", CallPath, Id)),
                parameters))
            {
                var match = (response.Headers.Location != null)
                    ? GatherIdExtractor.Match(response.Headers.Location.OriginalString)
                    : null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return await GetGather(match.Groups[1].Value);
            }
        }

        /// <summary>
        /// Collects a DTMF digit from a phone call with a prompt
        /// </summary>
        /// <param name="promptSentence">prompt sentence</param>
        /// <returns>Created Gather instance</returns>
        /// <example>
        /// <code>
        /// var gather = await call.CreateGather("Please enter a digit");
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/calls/#POST-/v1/users/{userId}/calls/{callId}/gather"/>
        public Task<Gather> CreateGather(string promptSentence)
        {
            return CreateGather(new Dictionary<string, object>
            {
                {"tag", Id},
                {"maxDigits", 1},
                {"prompt", new Dictionary<string, object>
                    {
                        {"locale", "en_US"},
                        {"gender", "female"},
                        {"sentence", promptSentence},
                        {"voice", "kate"},
                        {"bargeable", true}
                    }
                 }
            });
        }


        /// <summary>
        /// Update the gather DTMF. The only update allowed is state:completed to stop the gather.
        /// </summary>
        /// <param name="gatherId">Id of the gather</param>
        /// <param name="parameters">Dictionary with key: state</param>
        /// <example>
        /// <code>
        /// await call.UpdateGather("gatherId", new Dictionary&lt;string,object&gt;{{"state", "completed"}});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/calls/#POST-/v1/users/{userId}/calls/{callId}/gather/{gatherId}"/>
        public Task UpdateGather(string gatherId, IDictionary<string, object> parameters)
        {
            if (gatherId == null) throw new ArgumentNullException("gatherId");
            return
                Client.MakePostRequest(
                    Client.ConcatUserPath(string.Format("{0}/{1}/gather/{2}", CallPath, Id, gatherId)), parameters,
                    true);
        }


        /// <summary>
        /// Get the gather DTMF parameters and results
        /// </summary>
        /// <param name="gatherId">Id og the gather</param>
        /// <returns>Gather instance</returns>
        /// <example>
        /// <code>
        /// var gather = await call.GetGather("gatherId");
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/calls/#GET-/v1/users/{userId}/calls/{callId}/gather/{gatherId}"/>
        public async Task<Gather> GetGather(string gatherId)
        {
            if (gatherId == null) throw new ArgumentNullException("gatherId");
            var item = 
                await Client.MakeGetRequest<Gather>(
                    Client.ConcatUserPath(string.Format("{0}/{1}/gather/{2}", CallPath, Id, gatherId)));
            item.Client = Client;
            return item;
        }

        /// <summary>
        /// Gets the events that occurred during the call
        /// </summary>
        /// <returns>Array of Event</returns>
        /// <example>
        /// <code>
        /// var events = await call.GetEvents();
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/calls/#GET-/v1/users/{userId}/calls/{callId}/events"/>
        public Task<Event[]> GetEvents()
        {
            return
                Client.MakeGetRequest<Event[]>(
                    Client.ConcatUserPath(string.Format("{0}/{1}/events", CallPath, Id)));
        }

        /// <summary>
        /// Gets information about one call event
        /// </summary>
        /// <param name="eventId">Id of the event</param>
        /// <returns>Event instance</returns>
        /// <example>
        /// <code>
        /// var ev = await client.GetEvent("eventId");
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/calls/#GET-/v1/users/{userId}/calls/{callId}/events/{callEventId}"/>
        public Task<Event> GetEvent(string eventId)
        {
            if (eventId == null) throw new ArgumentNullException("eventId");
            return
                Client.MakeGetRequest<Event>(
                    Client.ConcatUserPath(string.Format("{0}/{1}/events/{2}", CallPath, Id, eventId)));
        }

        /// <summary>
        /// Retrieve all recordings related to the call
        /// </summary>
        /// <returns>Array of Recording</returns>
        /// <example>
        /// <code>
        /// var recordings = await call.GetRecordings();
        /// </code>
        /// </example>
        public Task<Recording[]> GetRecordings()
        {
            return
                Client.MakeGetRequest<Recording[]>(
                    Client.ConcatUserPath(string.Format("{0}/{1}/recordings", CallPath, Id)));
        }

        /// <summary>
        /// Complete the call
        /// </summary>
        /// <example>
        /// <code>
        /// await call.HangUp();
        /// </code>
        /// </example>
        /// <seealso cref="Call.Update"/>
        public async Task HangUp()
        {
            await Update(new Dictionary<string, object> { { "state", "completed" } });
            await Reload();
        }

        /// <summary>
        /// Answer an incoming call
        /// </summary>
        /// <example>
        /// <code>
        /// await call.AnswerOnIncoming();
        /// </code>
        /// </example>
        /// <seealso cref="Call.Update"/>
        public async Task AnswerOnIncoming()
        {
            await Update(new Dictionary<string, object> { { "state", "active" } });
            await Reload();
        }

        /// <summary>
        /// Reject an incoming call
        /// </summary>
        /// <example>
        /// <code>
        /// await call.RejectIncoming();
        /// </code>
        /// </example>
        /// <seealso cref="Call.Update"/>
        public async Task RejectIncoming()
        {
            await Update(new Dictionary<string, object> { { "state", "rejected" } });
            await Reload();
        }

        /// <summary>
        /// Switch on recording of the call
        /// </summary>
        /// <example>
        /// <code>
        /// await call.RecordingOn();
        /// </code>
        /// </example>
        /// <seealso cref="Call.Update"/>
        public async Task RecordingOn()
        {
            await Update(new Dictionary<string, object> { { "recordingEnabled", true } });
            await Reload();
        }

        /// <summary>
        /// Switch off recording of the call
        /// </summary>
        /// <example>
        /// <code>
        /// await call.RecordingOff();
        /// </code>
        /// </example>
        /// <seealso cref="Call.Update"/>
        public async Task RecordingOff()
        {
            await Update(new Dictionary<string, object> { { "recordingEnabled", false } });
            await Reload();
        }

        private Task Reload()
        {
            return Client.MakeGetRequestToObject(this, Client.ConcatUserPath(CallPath), null, Id);
        }
        
        /// <summary>
        /// Call direction
        /// </summary>
        public CallDirection Direction { get; set; }
        
        /// <summary>
        /// Determine how long should the platform wait for call answer before timing out in seconds
        /// </summary>
        public int CallTimeout { get; set; }
        
        /// <summary>
        /// Determine how long should the platform wait for callbackUrl's response before timing out in milliseconds
        /// </summary>
        public int CallbackTimeout { get; set; }
        
        /// <summary>
        /// The seconds between ActiveTime and EndTime
        /// </summary>
        public int ChargeableDuration { get; set; }
        
        /// <summary>
        /// Called ID
        /// </summary>
        public string From { get; set; }
        
        /// <summary>
        /// Called ID
        /// </summary>
        public string To { get; set; }
        
        /// <summary>
        /// URL where the events related to the Call will be posted to
        /// </summary>
        public string CallbackUrl { get; set; }
        
        /// <summary>
        /// The URL used to send the callback event if the request to callbackUrl fails
        /// </summary>
        public string FailbackUrl { get; set; }
        
        /// <summary>
        /// URL of the bridge, if any, that contains the call
        /// </summary>
        public string Bridge { get; set; }

        /// <summary>
        /// Id of the bridge where the call will be added
        /// </summary>
        public string BridgeId { get; set; }

        /// <summary>
        /// Id of the conference where the call will be added
        /// </summary>
        public string ConferenceId { get; set; }

        /// <summary>
        /// Audio to be played to the number that the call will be transfered to
        /// </summary>
        public WhisperAudio WhisperAudio { get; set; }

        /// <summary>
        /// This is the caller id that will be used when the call is transferred
        /// </summary>
        public string TransferCallerId { get; set; }

        /// <summary>
        /// Number that the call is going to be transferred to
        /// </summary>
        public string TransferTo { get; set; }

        /// <summary>
        /// Indicates if the call should be recorded after being created
        /// </summary>
        public bool RecordingEnabled { get; set; }
        
        /// <summary>
        /// Indicates the maximum duration of call recording in seconds
        /// </summary>
        public int RecordingMaxDuration { get; set; }
        
        /// <summary>
        /// Call state
        /// </summary>
        public string State { get; set; }
        
        /// <summary>
        /// Date when the call was created
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Date when the call was answered
        /// </summary>
        public DateTime ActiveTime { get; set; }

        /// <summary>
        /// Date when the call ended
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// URL to retrieve the events related to the call
        /// </summary>
        public string Events { get; set; }

        /// <summary>
        /// A string that will be included in the callback events of the call
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Used for pagination to indicate the page requested for querying a list of calls
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Used for pagination to indicate the size of each page requested for querying a list of calls
        /// </summary>
        public int Size { get; set; }
    }
    
    /// <summary>
    /// Audio to be played to the number that the call will be transfered to
    /// </summary>
    public class WhisperAudio
    {
        /// <summary>
        /// The gender of the voice
        /// </summary>
        public Gender Gender { get; set; }
        
        /// <summary>
        /// The voice to speak the sentence
        /// </summary>
        public string Voice { get; set; }
        
        /// <summary>
        /// The sentence
        /// </summary>
        public string Sentence { get; set; }
        
        /// <summary>
        /// The locale used to get the accent of the voice used to synthesize the sentence
        /// </summary>
        public string Locale { get; set; }
    }

    /// <summary>
    /// Genders
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// Male
        /// </summary>
        Male,

        /// <summary>
        /// Female
        /// </summary>
        Female
    }


    /// <summary>
    /// Call direction
    /// </summary>
    public enum CallDirection
    {
        /// <summary>
        /// Incoming call 
        /// </summary>
        In,
        
        /// <summary>
        /// Outgoing call
        /// </summary>
        Out
    }
}