using System;
using Bandwidth.Net.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Model
{
    [TestClass]
    public class CallEventTests
    {
        [TestMethod]
        public void IncomingCallEventTest()
        {
            var incomingCallEventString = "{" +
                                          "\"eventType\": \"incomingcall\"," +
                                          "\"from\": \"from\"," +
                                          "\"to\": \"to\"," +
                                          "\"callId\": \"callId\"," +
                                          "\"callUri\": \"callUri\"," +
                                          "\"callState\": \"active\"," +
                                          "\"applicationId\": \"applicationId\"," +
                                          "\"time\": \"2012-11-14T16:21:59.616Z\"" +
                                          "}";

            var ev = BaseEvent.CreateFromString(incomingCallEventString) as IncomingCallEvent;
            Assert.IsNotNull(ev);
            Assert.AreEqual("from", ev.From);
            Assert.AreEqual("to", ev.To);
            Assert.AreEqual("callId", ev.CallId);
            Assert.AreEqual("callUri", ev.CallUri);
            Assert.AreEqual(CallState.Active, ev.CallState);
            Assert.AreEqual("applicationId", ev.ApplicationId);
        }

        [TestMethod]
        public void AnswerEventTest()
        {
            var answerEventString = "{" +
                              "\"eventType\": \"answer\"," +
                              "\"from\": \"from\"," +
                              "\"to\": \"to\"," +
                              "\"callId\": \"callId\"," +
                              "\"callUri\": \"callUri\"," +
                              "\"callState\": \"active\"," +
                              "\"time\": \"2012-11-14T16:21:59.616Z\"," +
                              "\"tag\":\"a tag\"" + 
                              "}";

            var ev = BaseEvent.CreateFromString(answerEventString) as AnswerEvent;
            Assert.AreEqual("from", ev.From);
            Assert.AreEqual("to", ev.To);
            Assert.AreEqual("callId", ev.CallId);
            Assert.AreEqual("callUri", ev.CallUri);
            Assert.AreEqual(CallState.Active, ev.CallState);
            Assert.AreEqual("a tag", ev.Tag);
        }

        [TestMethod]
        public void RejectEventTest()
        {
            var rejectEventString = "{" +
                  "\"eventType\": \"hangup\"," +
                  "\"from\": \"from\"," +
                  "\"to\": \"to\"," +
                  "\"callId\": \"callId\"," +
                  "\"callUri\": \"callUri\"," +
                  "\"callState\": \"completed\"," +
                  "\"cause\":\"CALL_REJECTED\"," +
                  "\"time\": \"2012-11-14T16:21:59.616Z\"," +
                  "}";

            var ev = BaseEvent.CreateFromString(rejectEventString) as HangupEvent;
            Assert.AreEqual("from", ev.From);
            Assert.AreEqual("to", ev.To);
            Assert.AreEqual("callId", ev.CallId);
            Assert.AreEqual("callUri", ev.CallUri);
            Assert.AreEqual(CallState.Completed, ev.CallState);
            Assert.AreEqual("CALL_REJECTED", ev.Cause);
        }

        [TestMethod]
        public void HangupEventTest()
        {
            var hangupEventString = "{" +
                  "\"eventType\": \"hangup\"," +
                  "\"from\": \"from\"," +
                  "\"to\": \"to\"," +
                  "\"callId\": \"callId\"," +
                  "\"callUri\": \"callUri\"," +
                  "\"callState\": \"completed\"," +
                  "\"cause\":\"NORMAL_CLEARING\"," +
                  "\"time\": \"2012-11-14T16:21:59.616Z\"," +
                  "}";

            var ev = BaseEvent.CreateFromString(hangupEventString) as HangupEvent;
            Assert.AreEqual("from", ev.From);
            Assert.AreEqual("to", ev.To);
            Assert.AreEqual("callId", ev.CallId);
            Assert.AreEqual("callUri", ev.CallUri);
            Assert.AreEqual(CallState.Completed, ev.CallState);
            Assert.AreEqual("NORMAL_CLEARING", ev.Cause);
        }

        [TestMethod]
        public void TimeoutEventTest()
        {
            var timeoutEventString = "{" +
                  "\"eventType\": \"timeout\"," +
                  "\"from\": \"from\"," +
                  "\"to\": \"to\"," +
                  "\"callId\": \"callId\"," +
                  "\"callUri\": \"callUri\"," +
                  "\"time\": \"2012-11-14T16:21:59.616Z\"," +
                  "}";

            var ev = BaseEvent.CreateFromString(timeoutEventString) as TimeoutEvent;
            Assert.AreEqual("from", ev.From);
            Assert.AreEqual("to", ev.To);
            Assert.AreEqual("callId", ev.CallId);
            Assert.AreEqual("callUri", ev.CallUri);

        }

        [TestMethod]
        public void SpeakEventTest()
        {
            var speakEventString = "{" +
                  "\"eventType\": \"speak\"," +
                  "\"state\": \"PLAYBACK_START\"," +
                  "\"status\": \"started\"," +
                  "\"callId\": \"callId\"," +
                  "\"callUri\": \"callUri\"," +
                  "\"time\": \"2012-11-14T16:21:59.616Z\"," +
                  "}";

            var ev = BaseEvent.CreateFromString(speakEventString) as SpeakEvent;
            Assert.IsNotNull(ev);
            Assert.AreEqual("callId", ev.CallId);
            Assert.AreEqual("callUri", ev.CallUri);
            Assert.AreEqual("PLAYBACK_START", ev.State);
            Assert.AreEqual("started", ev.Status);
        }

        [TestMethod]
        public void PlaybackEventTest()
        {
            var playbackEventString = "{" +
                  "\"eventType\": \"playback\"," +
                  "\"status\": \"started\"," +
                  "\"callId\": \"callId\"," +
                  "\"callUri\": \"callUri\"," +
                  "\"time\": \"2012-11-14T16:21:59.616Z\"," +
                  "}";

            var ev = BaseEvent.CreateFromString(playbackEventString) as PlaybackEvent;
            Assert.IsNotNull(ev);
            Assert.AreEqual("callId", ev.CallId);
            Assert.AreEqual("callUri", ev.CallUri);
            Assert.AreEqual("started", ev.Status);

        }

        [TestMethod]
        public void RecordingEventTest()
        {
            var recordingEventString = "{" +
                  "\"eventType\": \"recording\"," +
                  "\"status\": \"complete\"," +
                  "\"state\": \"complete\"," +
                  "\"callId\": \"callId\"," +
                  "\"recordingUri\": \"recordingUri\"," +
                  "\"recordingId\": \"recordingId\"," +
                  "\"startTime\": \"2012-11-14T16:21:59.616Z\"," +
                  "\"endTime\": \"2012-11-14T16:21:59.616Z\"," +
                  "}";

            var ev = BaseEvent.CreateFromString(recordingEventString) as RecordingEvent;
            Assert.IsNotNull(ev);
            Assert.AreEqual("callId", ev.CallId);
            Assert.AreEqual("recordingId", ev.RecordingId);
            Assert.AreEqual("recordingUri", ev.RecordingUri);
            Assert.AreEqual(RecordingState.Complete, ev.State);
            Assert.AreEqual("complete", ev.Status);
        }

        [TestMethod]
        public void TranscriptionEventTest()
        {
            var transcriptionEventString = "{" +
                  "\"eventType\": \"transcription\"," +
                  "\"state\": \"completed\"," +
                  "\"text\": \"Test text\"," +
                  "\"transcriptionId\": \"transcriptionId\"," +
                  "\"transcriptionUri\": \"transcriptionUri\"," +
                  "\"recordingId\": \"recordingId\"," +
                  "}";

            var ev = BaseEvent.CreateFromString(transcriptionEventString) as TranscriptionEvent;
            Assert.IsNotNull(ev);
            Assert.AreEqual("transcriptionId", ev.TranscriptionId);
            Assert.AreEqual("transcriptionUri", ev.TranscriptionUri);
            Assert.AreEqual("recordingId", ev.RecordingId);
            Assert.AreEqual("Test text", ev.Text);
            Assert.AreEqual("completed", ev.State);
        }

        [TestMethod]
        public void ConferenceEventTest()
        {
            var conferenceEventString = "{" +
                  "\"eventType\": \"conference\"," +
                  "\"status\": \"completed\"," +
                  "\"conferenceId\": \"conferenceId\"," +
                  "\"conferenceUri\": \"conferenceUri\"," +
                  "\"createdTime\":\"2012-11-14T16:21:59.616Z\"," +
                  "\"completedTime\":\"2012-11-14T16:21:59.616Z\"," +
                  "}";

            var ev = BaseEvent.CreateFromString(conferenceEventString) as ConferenceEvent;
            Assert.IsNotNull(ev);
            Assert.AreEqual("conferenceId", ev.ConferenceId);
            Assert.AreEqual("conferenceUri", ev.ConferenceUri);
            Assert.AreEqual("completed", ev.Status);
        }

        [TestMethod]
        public void ConferenceMemberTest()
        {
            var conferenceMemberEventString = "{" +
                  "\"eventType\": \"conference-member\"," +
                  "\"state\": \"completed\"," +
                  "\"callId\": \"callId\"," +
                  "\"conferenceId\": \"conferenceId\"," +
                  "\"memberUri\": \"memberUri\"," +
                  "\"memberId\": \"memberId\"," +
                  "\"time\":\"2012-11-14T16:21:59.616Z\"," +
                  "\"hold\": false," +
                  "\"mute\": false ," +
                  "\"activeMembers\":" + 1 + "," +
                  "}";

            var ev = BaseEvent.CreateFromString(conferenceMemberEventString) as ConferenceMemberEvent;
            Assert.IsNotNull(ev);
            Assert.AreEqual("callId", ev.CallId);
            Assert.AreEqual(1, ev.ActiveMembers);
            Assert.AreEqual("memberId", ev.MemberId);
            Assert.AreEqual("memberUri", ev.MemberUri);
            Assert.IsFalse(ev.Hold);
            Assert.IsFalse(ev.Mute);
            Assert.AreEqual("conferenceId", ev.ConferenceId);

        }

        [TestMethod]
        public void ConferencePlaybackTest()
        {
            var conferencePlaybackEventString = "{" +
                  "\"eventType\": \"conference-playback\"," +
                  "\"status\": \"started\"," +
                  "\"conferenceId\": \"conferenceId\"," +
                  "\"conferenceUri\": \"conferenceUri\"," +
                  "\"time\":\"2012-11-14T16:21:59.616Z\"," +
                  "}";

            var ev = BaseEvent.CreateFromString(conferencePlaybackEventString) as ConferencePlaybackEvent;
            Assert.IsNotNull(ev);
            Assert.AreEqual("conferenceId", ev.ConferenceId);
            Assert.AreEqual("conferenceUri", ev.ConferenceUri);
            Assert.AreEqual("started", ev.Status);
        }


        [TestMethod]
        public void ConferenceSpeakEventTest()
        {
            var conferenceSpeakEventString = "{" +
                  "\"eventType\": \"conference-speak\"," +
                  "\"status\": \"started\"," +
                  "\"conferenceId\": \"conferenceId\"," +
                  "\"conferenceUri\": \"conferenceUri\"," +
                  "\"time\":\"2012-11-14T16:21:59.616Z\"," +
                  "}";
            var ev = BaseEvent.CreateFromString(conferenceSpeakEventString) as ConferenceSpeakEvent;
            Assert.IsNotNull(ev);
            Assert.AreEqual("conferenceId", ev.ConferenceId);
            Assert.AreEqual("conferenceUri", ev.ConferenceUri);
            Assert.AreEqual("started", ev.Status);
        }

        [TestMethod]
        public void DtmfEventTest()
        {
            var dtmfEventString = "{" +
                  "\"eventType\": \"dtmf\"," +
                  "\"callId\": \"callId\"," +
                  "\"callUri\": \"callUri\"," +
                  "\"dtmfDigit\": \"5\"," + 
                  "\"dtmfDuration\":\"1600\"," + 
                  "\"time\": \"2012-11-14T16:21:59.616Z\"," +
                  "}";

            var ev = BaseEvent.CreateFromString(dtmfEventString) as DtmfEvent;
            Assert.IsNotNull(ev);
            Assert.AreEqual("callId", ev.CallId);
            Assert.AreEqual("callUri", ev.CallUri);
            Assert.AreEqual("5", ev.DtmfDigit);
            Assert.AreEqual("1600", ev.DtmfDuration);
        }

        [TestMethod]
        public void GatherEventTest()
        {
            var gatherEventString = "{" +
                  "\"eventType\": \"gather\"," +
                  "\"callId\": \"callId\"," +
                  "\"gatherId\":\"gatherId\"," +
                  "\"digits\": \"5\"," +
                  "\"reason\":\"max-digits\"," + 
                  "\"state\": \"completed\", " +
                  "\"time\": \"2012-11-14T16:21:59.616Z\"," +
                  "}";

            var ev = BaseEvent.CreateFromString(gatherEventString) as GatherEvent;
            Assert.IsNotNull(ev);
            Assert.AreEqual("gatherId", ev.GatherId);
            Assert.AreEqual("callId", ev.CallId);
            Assert.AreEqual("5", ev.Digits);
            Assert.AreEqual("completed", ev.State);
            Assert.AreEqual("max-digits", ev.Reason);
        }

    }
}
