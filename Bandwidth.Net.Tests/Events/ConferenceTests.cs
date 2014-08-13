using Bandwidth.Net.Data;
using Bandwidth.Net.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Conference = Bandwidth.Net.Events.Conference;
using Event = Bandwidth.Net.Events.Event;

namespace Bandwidth.Net.Tests.Events
{
    [TestClass]
    public class ConferenceTests
    {
        [TestMethod]
        public void ParseRequestBodyTest()
        {
            const string json = @"{
                ""conferenceId"": ""conf-epztvv7s56cspvamqw7rwka"",
                ""conferenceUri"": ""https://api.catapult.inetwork.com/v1/users/u-ndh7ecxejswersdu5g8zngvca/conferences/conf-epztvv7s56cspvamqw7rwka"",
                ""eventType"":""conference"",
                ""status"":""created"",
                ""createdTime"":""2013-07-12T16:26:55.685-02:00""
            }";
            var ev = Event.ParseRequestBody(json) as Conference;
            Assert.IsNotNull(ev);
            Assert.AreEqual("conf-epztvv7s56cspvamqw7rwka", ev.ConferenceId);
            Assert.AreEqual(ConferenceState.Created, ev.Status);
        }
    }
}
