using Bandwidth.Net.Data;
using Bandwidth.Net.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Event = Bandwidth.Net.Events.Event;

namespace Bandwidth.Net.Tests.Events
{
    [TestClass]
    public class IncomingCallTests
    {
        [TestMethod]
        public void ParseRequestBodyTest()
        {
            const string json = @"{
               ""eventType"":""incomingcall"",
               ""from"":""+13233326955"",
               ""to"":""+13865245000"",
               ""callId"":""c-oexifypjlh5ygjr7qi4nesq"",
               ""callUri"": ""https://api.catapult.inetwork.com/v1/users/u-ndh7ecxejswersdu5g8zngvca/calls/c-oexifypjlh5ygjr7qi4nesq"",
               ""callState"":""active"",
               ""applicationId"":""a-25nh2lj6qrxznkfu4b732jy"",
               ""time"":""2012-11-14T16:21:59.616Z""
            }";
            var ev = Event.ParseRequestBody(json) as IncomingCall;
            Assert.IsNotNull(ev);
            Assert.AreEqual("+13233326955", ev.From);
            Assert.AreEqual("+13865245000", ev.To);
            Assert.AreEqual("c-oexifypjlh5ygjr7qi4nesq", ev.CallId);
            Assert.AreEqual(CallState.Active, ev.CallState);
        }
    }
}
