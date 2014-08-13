using Bandwidth.Net.Data;
using Bandwidth.Net.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Event = Bandwidth.Net.Events.Event;

namespace Bandwidth.Net.Tests.Events
{
    [TestClass]
    public class CallTimeoutTests
    {
        [TestMethod]
        public void ParseRequestBodyTest()
        {
            const string json = @"{
               ""eventType"":""timeout"",
               ""from"":""+12096626728"",
               ""to"":""+15756162105"",
               ""callId"":""c-xk5kvrqs3gqjmjleybhxxgi"",
               ""callUri"":""https://api.catapult.inetwork.com/v1/users/u-647ra4bjsnxolkyswkfy7hi/calls/c-xk5kvrqs3gqjmjleybhxxgi"",
               ""time"":""2013-11-06T14:25:58.857Z""
            }";
            var ev = Event.ParseRequestBody(json) as CallTimeout;
            Assert.IsNotNull(ev);
            Assert.AreEqual("+12096626728", ev.From);
            Assert.AreEqual("+15756162105", ev.To);
            Assert.AreEqual("c-xk5kvrqs3gqjmjleybhxxgi", ev.CallId);
        }
    }
}
