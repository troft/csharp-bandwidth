using Bandwidth.Net.Data;
using Bandwidth.Net.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Event = Bandwidth.Net.Events.Event;

namespace Bandwidth.Net.Tests.Events
{
    [TestClass]
    public class SmsTests
    {
        [TestMethod]
        public void ParseRequestBodyTest()
        {
            const string json = @"{
               ""eventType"":""sms"",
               ""direction"":""out"",
               ""messageId"": ""m-kv54fk7x66fakdnb5owdk4y"",
               ""messageUri"": ""https://api.catapult.inetwork.com/v1/users/u-ndh7ecxejswersdu5g8zngvca/messages/m-kv54fk7x66fakdnb5owdk4y"",
               ""from"":""+13233326955"",
               ""to"":""+13865245000"",
               ""text"":""Example"",
               ""applicationId"":""a-25nh2lj6qrxznkfu4b732jy"",
               ""time"":""2012-11-14T16:13:06.076Z"",
               ""state"":""sent""
            }";
            var ev = Event.ParseRequestBody(json) as Sms;
            Assert.IsNotNull(ev);
            Assert.AreEqual("m-kv54fk7x66fakdnb5owdk4y", ev.MessageId);
            Assert.AreEqual("+13233326955", ev.From);
            Assert.AreEqual("+13865245000", ev.To);
            Assert.AreEqual("Example", ev.Text);
            Assert.AreEqual(MessageState.Sent, ev.State);
        }
    }
}
