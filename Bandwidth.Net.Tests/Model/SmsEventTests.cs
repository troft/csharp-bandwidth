using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bandwidth.Net.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Model
{
    [TestClass]
    public class SmsEventTests
    {
        [TestMethod]
        public void IncomingSmsEventTest()
        {
            var incomingSmsEventString = "{" +
                                         "\"eventType\":\"sms\"," +
                                         "\"direction\":\"in\"," +
                                         "\"messageId\": \"messageId\"," +
                                         "\"messageUri\": \"http://testurl.com\"," +
                                         "\"from\":\"from\"," +
                                         "\"to\":\"to\"," +
                                         "\"text\":\"Example\"," +
                                         "\"applicationId\":\"applicationId\"," +
                                         "\"time\":\"2012-11-14T16:13:06.076Z\"," +
                                         "\"state\":\"received\"}";
            var ev = BaseEvent.CreateFromString(incomingSmsEventString) as SmsEvent;
            Assert.IsNotNull(ev);
            Assert.AreEqual("sms", ev.EventType);
            Assert.AreEqual("in", ev.Direction);
            Assert.AreEqual("from", ev.From);
            Assert.AreEqual("to", ev.To);
            Assert.AreEqual("Example", ev.Text);
            Assert.AreEqual("applicationId", ev.ApplicationId);
            Assert.AreEqual("received", ev.State);
        }

        [TestMethod]
        public void OutgoingSmsEventTest()
        {
            var outgoingSmsEventString = "{" +
                                         "\"eventType\":\"sms\"," +
                                         "\"direction\":\"out\"," +
                                         "\"messageId\": \"messageId\"," +
                                         "\"messageUri\": \"http://testurl.com\"," +
                                         "\"from\":\"from\"," +
                                         "\"to\":\"to\"," +
                                         "\"text\":\"Example\"," +
                                         "\"applicationId\":\"applicationId\"," +
                                         "\"time\":\"2012-11-14T16:13:06.076Z\"," +
                                         "\"state\":\"sent\"}";

            var ev = BaseEvent.CreateFromString(outgoingSmsEventString) as SmsEvent;
            Assert.IsNotNull(ev);
            Assert.AreEqual("sms", ev.EventType);
            Assert.AreEqual("out", ev.Direction);
            Assert.AreEqual("from", ev.From);
            Assert.AreEqual("to", ev.To);
            Assert.AreEqual("Example", ev.Text);
            Assert.AreEqual("applicationId", ev.ApplicationId);
            Assert.AreEqual("sent", ev.State);


        }
    }
}
