using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public class Call
    {
        private const string CallPath = "calls";

        private static readonly Regex CallIdExtractor = new Regex(@"/" + CallPath + @"/([\w\-_]+)$");
        private Client _client;
        /// <summary>
        ///     Gets information about an active or completed call.
        /// </summary>
        public static Task<Call> Get(Client client, string callId)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            return client.MakeGetRequest<Call>(client.ConcatUserPath(CallPath), null, callId).ContinueWith(
                t =>
                {
                    var call = t.Result;
                    call._client = client;
                    return call;
                });
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
        public static Task<Call[]> List(Client client, CallQuery query = null)
        {
            query = query ?? new CallQuery{Size = 25};
            return client.MakeGetRequest<Call[]>(client.ConcatUserPath(CallPath), query.ToDictionary()).ContinueWith(
                t =>
                {
                    var calls = t.Result ?? new Call[0];
                    foreach (var call in calls)
                    {
                        call._client = client;    
                    }
                    return calls;
                });
        }

        public static Task<Call[]> List(Client client, int page, int size = 25)
        {
            return List(client, new CallQuery {Page = page, Size = size});
        }

#if !PCL        
        public static Task<Call[]> List(CallQuery query = null)
        {
            return List(Client.GetInstance(), query);
        }

        public static Task<Call[]> List(int page, int size =  25)
        {
            return List(Client.GetInstance(), page, size);
        }
#endif


        /// <summary>
        ///     Makes a phone call.
        /// </summary>
        public static async Task<Call> Create(Client client, Call call)
        {
            using (var response = await client.MakePostRequest(client.ConcatUserPath(CallPath), call))
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

        public static async Task<Call> Create(Client client, string to, string from, string callbackUrl = "none", string tag = null)
        {
            using (var response = await client.MakePostRequest(client.ConcatUserPath(CallPath), new Call
            {
                To = to,
                From = from,
                CallbackUrl = callbackUrl,
                Tag = tag
            }))
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

#if !PCL        
        public static Task<Call> Create(Call call)
        {
            return Create(Client.GetInstance(), call);
        }

        public static Task<Call> Create(string to, string from, string callbackUrl = "none", string tag = null)
        {
            return Create(Client.GetInstance(), to, from, callbackUrl, tag);
        }
#endif


        /// <summary>
        ///     Changes properties of an active phone call.
        /// </summary>
        public static Task Update(Client client, string callId, Call changedData)
        {
            if (callId == null) throw new ArgumentNullException("callId");
            return client.MakePostRequest(client.ConcatUserPath(string.Format("{0}/{1}", CallPath, callId)),
                changedData, true);
        }
#if !PCL        
        public static Task Update(string callId, Call changedData)
        {
            return Update(Client.GetInstance(), callId, changedData);
        }
#endif

        /// <summary>
        ///     Plays an audio file or speak a sentence in a phone call.
        /// </summary>
        public Task PlayAudio(Audio audio)
        {
            return _client.MakePostRequest(_client.ConcatUserPath(string.Format("{0}/{1}/audio", CallPath, Id)),
                audio, true);
        }

        public Task SpeakSentence(string sentence, string tag = null)
        {
            return PlayAudio(new Audio
            {
                Gender = Gender.Female,
                Locale = "en_US",
                Voice = "kate",
                Sentence = sentence,
                Tag = tag
            });
        }
        public Task PlayRecording(string recordingUrl)
        {
            if (recordingUrl == null) throw new ArgumentNullException("recordingUrl");
            return PlayAudio(new Audio
            {
                FileUrl = recordingUrl
            });
        }

        /// <summary>
        ///     Send DTMF.
        /// </summary>
        public Task SetDtmf(Dtmf dtmf)
        {
            return _client.MakePostRequest(_client.ConcatUserPath(string.Format("{0}/{1}/dtmf", CallPath, Id)),
                dtmf, true);
        }

        /// <summary>
        ///     Collects a series of DTMF digits from a phone call with an optional prompt. This request returns immediately. When
        ///     gather finishes, an event with the results will be posted to the callback URL.
        /// </summary>
        public Task CreateGather(CreateGather gather)
        {
            return _client.MakePostRequest(_client.ConcatUserPath(string.Format("{0}/{1}/gather", CallPath, Id)),
                gather, true);
        }

        public Task CreateGather(Client client, string promptSentence)
        {
            return CreateGather(new CreateGather
            {
                Tag = Id,
                MaxDigits = 1,
                Promt = new CreateGatherPromt
                {
                    Locale = "en_US",
                    Gender = Gender.Female,
                    Sentence = promptSentence,
                    Voice = "kate",
                    Bargeable = true
                }
            });
        }


        /// <summary>
        ///     Update the gather DTMF. The only update allowed is state:completed to stop the gather.
        /// </summary>
        public Task UpdateGather(string gatherId, Gather gather)
        {
            if (gatherId == null) throw new ArgumentNullException("gatherId");
            return
                _client.MakePostRequest(
                    _client.ConcatUserPath(string.Format("{0}/{1}/gather/{2}", CallPath, Id, gatherId)), gather,
                    true);
        }


        /// <summary>
        /// </summary>
        public Task<Gather> GetGather(string gatherId)
        {
            if (gatherId == null) throw new ArgumentNullException("gatherId");
            return
                _client.MakeGetRequest<Gather>(
                    _client.ConcatUserPath(string.Format("{0}/{1}/gather/{2}", CallPath, Id, gatherId)));
        }

        /// <summary>
        /// </summary>
        public Task<Event[]> GetEventsList()
        {
            return
                _client.MakeGetRequest<Event[]>(
                    _client.ConcatUserPath(string.Format("{0}/{1}/events", CallPath, Id)));
        }

        /// <summary>
        /// </summary>
        public Task<Event> GetEvent(string eventId)
        {
            if (eventId == null) throw new ArgumentNullException("eventId");
            return
                _client.MakeGetRequest<Event>(
                    _client.ConcatUserPath(string.Format("{0}/{1}/events/{2}", CallPath, Id, eventId)));
        }

        /// <summary>
        /// </summary>
        public Task<Recording[]> GetRecordingsList()
        {
            return
                _client.MakeGetRequest<Recording[]>(
                    _client.ConcatUserPath(string.Format("{0}/{1}/recordings", CallPath, Id)));
        }

        public string Id { get; set; }
        public CallDirection? Direction { get; set; }
        public int? CallTimeout { get; set; }
        public int? CallbackTimeout { get; set; }
        public int? ChargeableDuration { get; set; }
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
        public bool? RecordingEnabled { get; set; }
        public int? RecordingMaxDuration { get; set; }
        public CallState? State { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? ActiveTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Events { get; set; }
        public string Tag { get; set; }
        public int? Page { get; set; }
        public int? Size { get; set; }
    }

    public class CallQuery : Query
    {
        public string BridgeId { get; set; }
        public string ConferenceId { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public override IDictionary<string, string> ToDictionary()
        {
            IDictionary<string, string> query = base.ToDictionary();
            if (BridgeId != null)
            {
                query.Add("bridgeId", BridgeId);
            }
            if (ConferenceId != null)
            {
                query.Add("conferenceId", ConferenceId);
            }
            if (From != null)
            {
                query.Add("from", From);
            }
            if (To != null)
            {
                query.Add("to", To);
            }
            return query;
        }
    }
    public class WhisperAudio
    {
        public Gender? Gender { get; set; }
        public string Voice { get; set; }
        public string Sentence { get; set; }
        public string Locale { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }

    public enum CallState
    {
        Active,
        Completed,
        Started,
        Rejected,
        Transferring,
        Error
    }

    public enum CallDirection
    {
        In,
        Out
    }
}