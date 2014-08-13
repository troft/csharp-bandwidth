using Bandwidth.Net.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Event = Bandwidth.Net.Events.Event;

namespace Bandwidth.Net.Tests.Events
{
    [TestClass]
    public class PlaybackTests
    {
        [TestMethod]
        public void ParseRequestBodyTest()
        {
            const string json = @"{
               ""eventType"": ""playback"",
               ""callId"": ""c-z572ntgyy2vnffwpa5bwrcy"",
               ""callUri"": ""https://api.catapult.inetwork.com/v1/users/u-ndh7ecxejswersdu5g8zngvca/calls/c-z572ntgyy2vnffwpa5bwrcy"",
               ""status"": ""done"",
               ""time"": ""2012-11-14T15:56:06.616Z"",
               ""tag"": ""abc123""
            }";
            var ev = Event.ParseRequestBody(json) as Playback;
            Assert.IsNotNull(ev);
            Assert.AreEqual("c-z572ntgyy2vnffwpa5bwrcy", ev.CallId);
            Assert.AreEqual("abc123", ev.Tag);
            Assert.AreEqual(PlaybackStatus.Done, ev.Status);
        }
    }
}
