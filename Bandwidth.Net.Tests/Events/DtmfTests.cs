using Bandwidth.Net.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Event = Bandwidth.Net.Events.Event;

namespace Bandwidth.Net.Tests.Events
{
    [TestClass]
    public class DtfmTests
    {
        [TestMethod]
        public void ParseRequestBodyTest()
        {
            const string json = @"{
               ""eventType"":""dtmf"",
               ""callId"":""c-z572ntgyy2vnffwpa5bwrcy"",
               ""callUri"": ""https://api.catapult.inetwork.com/v1/users/u-ndh7ecxejswersdu5g8zngvca/calls/c-z572ntgyy2vnffwpa5bwrcy"",
               ""time"":""2012-11-14T15:56:09.276Z"",
               ""dtmfDigit"":""5"",
               ""dtmfDuration"":""1600""
            }";
            var ev = Event.ParseRequestBody(json) as Dtmf;
            Assert.IsNotNull(ev);
            Assert.AreEqual("c-z572ntgyy2vnffwpa5bwrcy", ev.CallId);
            Assert.AreEqual(5, ev.DtmfDigit);
            Assert.AreEqual(1600, ev.DtmfDuration);
        }
    }
}
