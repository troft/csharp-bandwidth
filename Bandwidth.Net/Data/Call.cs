using System;
using System.Collections.Generic;

namespace Bandwidth.Net.Data
{
    
    public class Call
    {
        public string Id { get; set; }

        public CallDirection? Direction { get; set; }
        public int? CallTimeout { get; set; }
        public int? CallbackTimeout { get; set; }
        public int? ChargeableDuration { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public Uri CallbackUrl { get; set; }
        public Uri FailbackUrl { get; set; }
        public Uri Bridge { get; set; }
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
        public Uri Events { get; set; }
        public int? Page { get; set; }
        public int? Size { get; set; }
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
        Rejected,
        Transferring
    }

    public enum CallDirection
    {
        In,
        Out
    }

    public class CallQuery : Query
    {
        public string BridgeId { get; set; }
        public string ConferenceId { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public override IDictionary<string, string> ToDictionary()
        {
            var query =  base.ToDictionary();
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
}