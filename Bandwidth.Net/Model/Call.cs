using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public class Call: BaseModel
    {
        private const string CallPath = "calls";

        private static readonly Regex CallIdExtractor = new Regex(@"/" + CallPath + @"/([\w\-_]+)$");
        private static readonly Regex GatherIdExtractor = new Regex(@"/gather/([\w\-_]+)$");
        /// <summary>
        ///     Gets information about an active or completed call.
        /// </summary>
        public static async Task<Call> Get(Client client, string callId)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            var call = await client.MakeGetRequest<Call>(client.ConcatUserPath(CallPath), null, callId);
            call.Client = client;
            return call;
        }
#if !PCL        
        public static Task<Call> Get(string callId)
        {
            return Get(Client.GetInstance(), callId);
        }
#endif

        /// <summary>
        ///     Gets a list of active and historic calls user made or received.
        /// </summary>
        public static async Task<Call[]> List(Client client, IDictionary<string, object> query = null )
        {
            var calls = await client.MakeGetRequest<Call[]>(client.ConcatUserPath(CallPath), query) ?? new Call[0];
            foreach (var call in calls)
            {
                call.Client = client;
            }
            return calls;
        }

        public static Task<Call[]> List(Client client, int page, int size = 25)
        {
            var query = new Dictionary<string, object> {{"page", page}, {"size", size}};
            return List(client, query);
        }

#if !PCL        
        public static Task<Call[]> List(IDictionary<string, object> parameters = null)
        {
            return List(Client.GetInstance(), parameters);
        }

        public static Task<Call[]> List(int page, int size =  25)
        {
            return List(Client.GetInstance(), page, size);
        }
#endif


        /// <summary>
        ///     Makes a phone call.
        /// </summary>
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

#if !PCL        
        public static Task<Call> Create(IDictionary<string, object> parameters)
        {
            return Create(Client.GetInstance(), parameters);
        }

        public static Task<Call> Create(string to, string from, string callbackUrl = "none", string tag = null)
        {
            return Create(Client.GetInstance(), to, from, callbackUrl, tag);
        }
#endif



        /// <summary>
        ///     Changes properties of an active phone call.
        /// </summary>
        public Task Update(IDictionary<string, object> parameters)
        {
            return Client.MakePostRequest(Client.ConcatUserPath(string.Format("{0}/{1}", CallPath, Id)),
                parameters, true);
        }

        /// <summary>
        ///     Plays an audio file or speak a sentence in a phone call.
        /// </summary>
        public Task PlayAudio(Dictionary<string, object> parameters)
        {
            return Client.MakePostRequest(Client.ConcatUserPath(string.Format("{0}/{1}/audio", CallPath, Id)),
                parameters, true);
        }

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
        public Task PlayRecording(string recordingUrl)
        {
            if (recordingUrl == null) throw new ArgumentNullException("recordingUrl");
            return PlayAudio(new Dictionary<string, object>
            {
                { "fileUrl", recordingUrl}
            });
        }

        /// <summary>
        ///     Send DTMF.
        /// </summary>
        public Task SendDtmf(string dtmf)
        {
            return Client.MakePostRequest(Client.ConcatUserPath(string.Format("{0}/{1}/dtmf", CallPath, Id)),
                new Dictionary<string, object> { { "dtmfOut", dtmf } }, true);
        }

        /// <summary>
        ///     Collects a series of DTMF digits from a phone call with an optional prompt. This request returns immediately. When
        ///     gather finishes, an event with the results will be posted to the callback URL.
        /// </summary>
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
        ///     Collects a series of DTMF digits from a phone call with an optional prompt. This request returns immediately. When
        ///     gather finishes, an event with the results will be posted to the callback URL.
        /// </summary>
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
        ///     Update the gather DTMF. The only update allowed is state:completed to stop the gather.
        /// </summary>
        public Task UpdateGather(string gatherId, IDictionary<string, object> parameters)
        {
            if (gatherId == null) throw new ArgumentNullException("gatherId");
            return
                Client.MakePostRequest(
                    Client.ConcatUserPath(string.Format("{0}/{1}/gather/{2}", CallPath, Id, gatherId)), parameters,
                    true);
        }


        /// <summary>
        /// </summary>
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
        /// </summary>
        public Task<Event[]> GetEvents()
        {
            return
                Client.MakeGetRequest<Event[]>(
                    Client.ConcatUserPath(string.Format("{0}/{1}/events", CallPath, Id)));
        }

        /// <summary>
        /// </summary>
        public Task<Event> GetEvent(string eventId)
        {
            if (eventId == null) throw new ArgumentNullException("eventId");
            return
                Client.MakeGetRequest<Event>(
                    Client.ConcatUserPath(string.Format("{0}/{1}/events/{2}", CallPath, Id, eventId)));
        }

        /// <summary>
        /// </summary>
        public Task<Recording[]> GetRecordings()
        {
            return
                Client.MakeGetRequest<Recording[]>(
                    Client.ConcatUserPath(string.Format("{0}/{1}/recordings", CallPath, Id)));
        }

        /// <summary>
        /// </summary>
        public async Task HangUp()
        {
            await Update(new Dictionary<string, object> { { "state", "completed" } });
            await Reload();
        }

        /// <summary>
        /// </summary>
        public async Task AnswerOnIncoming()
        {
            await Update(new Dictionary<string, object> { { "state", "active" } });
            await Reload();
        }

        /// <summary>
        /// </summary>
        public async Task RejectIncoming()
        {
            await Update(new Dictionary<string, object> { { "state", "rejected" } });
            await Reload();
        }

        /// <summary>
        /// </summary>
        public async Task RecordingOn()
        {
            await Update(new Dictionary<string, object> { { "recordingEnabled", true } });
            await Reload();
        }

        /// <summary>
        /// </summary>
        public async Task RecordingOff()
        {
            await Update(new Dictionary<string, object> { { "recordingEnabled", false } });
            await Reload();
        }

        private Task Reload()
        {
            return Client.MakeGetRequestToObject(this, Client.ConcatUserPath(CallPath), null, Id);
        }
        public CallDirection Direction { get; set; }
        public int CallTimeout { get; set; }
        public int CallbackTimeout { get; set; }
        public int ChargeableDuration { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string CallbackUrl { get; set; }
        public string FailbackUrl { get; set; }
        public string Bridge { get; set; }
        public string BridgeId { get; set; }
        public string ConferenceId { get; set; }
        public WhisperAudio WhisperAudio { get; set; }
        public string TransferCallerId { get; set; }
        public string TransferTo { get; set; }
        public bool RecordingEnabled { get; set; }
        public int RecordingMaxDuration { get; set; }
        public string State { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ActiveTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Events { get; set; }
        public string Tag { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
    }
    
    public class WhisperAudio
    {
        public Gender Gender { get; set; }
        public string Voice { get; set; }
        public string Sentence { get; set; }
        public string Locale { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }



    public enum CallDirection
    {
        In,
        Out
    }
}