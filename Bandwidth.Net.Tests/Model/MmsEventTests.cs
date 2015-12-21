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
    public class MmsEventTests
    {
        [TestMethod]
        public void IncomingSmsEventTest()
        {
            var incomingMmsEventString = "{" +
                                         "\"eventType\":\"mms\"," +
                                         "\"direction\":\"in\"," +
                                         "\"messageId\": \"messageId\"," +
                                         "\"messageUri\": \"http://testurl.com\"," +
                                         "\"from\":\"from\"," +
                                         "\"to\":\"to\"," +
                                         "\"text\":\"Example\"," +
                                         "\"applicationId\":\"applicationId\"," +
                                         "\"time\":\"2012-11-14T16:13:06.076Z\"," +
                                         "\"media\":[ \"url1\", \"url2\"]," +
                                         "\"state\":\"received\"}";
            var ev = BaseEvent.CreateFromString(incomingMmsEventString) as MmsEvent;
            Assert.IsNotNull(ev);
            Assert.AreEqual("mms", ev.EventType);
            Assert.AreEqual("in", ev.Direction);
            Assert.AreEqual("from", ev.From);
            Assert.AreEqual("to", ev.To);
            Assert.AreEqual("Example", ev.Text);
            Assert.AreEqual("applicationId", ev.ApplicationId);
            Assert.AreEqual("received", ev.State);
            CollectionAssert.AreEqual(new[] {"url1", "url2"}, ev.Media);
        }

        [TestMethod]
        public void OutgoingSmsEventTest()
        {
            var outgoingMmsEventString = "{" +
                                         "\"eventType\":\"mms\"," +
                                         "\"direction\":\"out\"," +
                                         "\"messageId\": \"messageId\"," +
                                         "\"messageUri\": \"http://testurl.com\"," +
                                         "\"from\":\"from\"," +
                                         "\"to\":\"to\"," +
                                         "\"text\":\"Example\"," +
                                         "\"applicationId\":\"applicationId\"," +
                                         "\"time\":\"2012-11-14T16:13:06.076Z\"," +
                                         "\"media\":[ \"url1\", \"url2\"]," +
                                         "\"state\":\"sent\"}";

            var ev = BaseEvent.CreateFromString(outgoingMmsEventString) as MmsEvent;
            Assert.IsNotNull(ev);
            Assert.AreEqual("mms", ev.EventType);
            Assert.AreEqual("out", ev.Direction);
            Assert.AreEqual("from", ev.From);
            Assert.AreEqual("to", ev.To);
            Assert.AreEqual("Example", ev.Text);
            Assert.AreEqual("applicationId", ev.ApplicationId);
            Assert.AreEqual("sent", ev.State);
            CollectionAssert.AreEqual(new[] { "url1", "url2" }, ev.Media);

        }
    }
}
