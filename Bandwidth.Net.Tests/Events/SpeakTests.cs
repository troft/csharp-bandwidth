using Bandwidth.Net.Data;
using Bandwidth.Net.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Event = Bandwidth.Net.Events.Event;

namespace Bandwidth.Net.Tests.Events
{
    [TestClass]
    public class SpeakTests
    {
        [TestMethod]
        public void ParseRequestBodyTest()
        {
            const string json = @"{
               ""callId"": ""c-zrt5sfz2ladmhyncwgpeoda"", 
               ""callUri"": ""https://api.catapult.inetwork.com/v1/users/u-ndh7ecxejswersdu5g8zngvca/calls/c-zrt5sfz2ladmhyncwgpeoda"",
               ""eventType"": ""speak"", 
               ""state"": ""PLAYBACK_STOP"", 
               ""status"": ""done"", 
               ""time"": ""2013-06-26T17:55:46.768Z"", 
               ""type"": ""speak""
            }";
            var ev = Event.ParseRequestBody(json) as Speak;
            Assert.IsNotNull(ev);
            Assert.AreEqual("c-zrt5sfz2ladmhyncwgpeoda", ev.CallId);
            Assert.AreEqual(SpeakStatus.Done, ev.Status);
            Assert.AreEqual("speak", ev.Type);
        }
    }
}
