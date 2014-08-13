using Bandwidth.Net.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Event = Bandwidth.Net.Events.Event;
using Recording = Bandwidth.Net.Events.Recording;

namespace Bandwidth.Net.Tests.Events
{
    [TestClass]
    public class RecordingTests
    {
        [TestMethod]
        public void ParseRequestBodyTest()
        {
            const string json = @"{
               ""callId"": ""c-zrt5sfz2ladmhyncwgpeoda"",
               ""eventType"": ""recording"",
               ""recordingId"": ""rec-hvlxqfj3y7zrf7w7qydhefi"",
               ""recordingUri"": ""https://api.catapult.inetwork.com/v1/users/u-ndh7ecxejswersdu5g8zngvca/recordings/rec-hvlxqfj3y7zrf7w7qydhefi"",
               ""state"": ""error"",
               ""status"": ""error"",
               ""startTime"": ""2013-08-19T16:56:57.643Z"",
               ""endTime"": ""2013-08-19T16:57:08.712Z""
            }";
            var ev = Event.ParseRequestBody(json) as Recording;
            Assert.IsNotNull(ev);
            Assert.AreEqual("c-zrt5sfz2ladmhyncwgpeoda", ev.CallId);
            Assert.AreEqual("rec-hvlxqfj3y7zrf7w7qydhefi", ev.RecordingId);
            Assert.AreEqual(RecordingState.Error, ev.Status);
        }
    }
}
