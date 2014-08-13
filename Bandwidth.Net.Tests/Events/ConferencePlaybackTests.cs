using Bandwidth.Net.Data;
using Bandwidth.Net.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConferenceMember = Bandwidth.Net.Events.ConferenceMember;
using Event = Bandwidth.Net.Events.Event;

namespace Bandwidth.Net.Tests.Events
{
    [TestClass]
    public class ConferencePlaybackTests
    {
        [TestMethod]
        public void ParseRequestBodyTest()
        {
            const string json = @"{
                ""eventType"":""conference-playback"",
                ""conferenceId"":""conf-nreqnmjx4mo5y64tz6obnia"",
                ""conferenceUri"": ""https://api.catapult.inetwork.com/v1/users/u-ndh7ecxejswersdu5g8zngvca/conferences/conf-nreqnmjx4mo5y64tz6obnia"",
                ""status"":""started"",
                ""time"":""2013-07-12T21:18:19.966Z"" 
            }";
            var ev = Event.ParseRequestBody(json) as ConferencePlayback;
            Assert.IsNotNull(ev);
            Assert.AreEqual("conf-nreqnmjx4mo5y64tz6obnia", ev.ConferenceId);
            Assert.AreEqual(PlaybackStatus.Started, ev.Status);
        }
    }
}
