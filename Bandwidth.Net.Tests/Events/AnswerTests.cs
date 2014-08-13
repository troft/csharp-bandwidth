using Bandwidth.Net.Data;
using Bandwidth.Net.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Event = Bandwidth.Net.Events.Event;

namespace Bandwidth.Net.Tests.Events
{
    [TestClass]
    public class AnswerTests
    {
        [TestMethod]
        public void ParseRequestBodyTest()
        {
            const string json = @"{
               ""eventType"":""answer"",
               ""from"":""+15753222083"",
               ""to"":""+13865245000"",
               ""callId"":""c-jjm3aiicnpngixjjelyomda"",
               ""callUri"": ""https://api.catapult.inetwork.com/v1/users/u-ndh7ecxejswersdu5g8zngvca/calls/c-jjm3aiicnpngixjjelyomda"",
               ""callState"":""active"",
               ""applicationId"":""a-25nh2lj6qrxznkfu4b732jy"",
               ""time"":""2012-11-14T16:28:31.536Z""
            }";
            var ev = Event.ParseRequestBody(json) as Answer;
            Assert.IsNotNull(ev);
            Assert.AreEqual("+15753222083", ev.From);
            Assert.AreEqual("+13865245000", ev.To);
            Assert.AreEqual("c-jjm3aiicnpngixjjelyomda", ev.CallId);
            Assert.AreEqual(CallState.Active, ev.CallState);
        }
    }
}
