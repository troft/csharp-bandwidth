using Bandwidth.Net.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Event = Bandwidth.Net.Events.Event;

namespace Bandwidth.Net.Tests.Events
{
    [TestClass]
    public class HangupTests
    {
        [TestMethod]
        public void ParseRequestBodyTest()
        {
            const string json = @"{
               ""eventType"":""hangup"",
               ""callId"":""c-z572ntgyy2vnffwpa5bwrcy"",
               ""callUri"": ""https://api.catapult.inetwork.com/v1/users/u-ndh7ecxejswersdu5g8zngvca/calls/c-z572ntgyy2vnffwpa5bwrcy"",
               ""from"":""+13233326955"",
               ""to"":""+13233326956"",
               ""cause"":""NORMAL_CLEARING"",
               ""time"":""2012-11-14T15:56:12.636Z""
            }";
            var ev = Event.ParseRequestBody(json) as Hangup;
            Assert.IsNotNull(ev);
            Assert.AreEqual("c-z572ntgyy2vnffwpa5bwrcy", ev.CallId);
            Assert.AreEqual("+13233326955", ev.From);
            Assert.AreEqual("+13233326956", ev.To);
            Assert.AreEqual("NORMAL_CLEARING", ev.Cause);
        }
    }
}
