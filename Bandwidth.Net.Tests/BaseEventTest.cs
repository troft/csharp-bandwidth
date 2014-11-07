using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bandwidth.Net.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests
{
    [TestClass]
    public class BaseEventTest
    {
        [TestMethod]
        public void CreateFromStringTest()
        {
            var ev = BaseEvent.CreateFromString("{"+
            "\"eventType\":\"incomingcall\","+
            "\"from\":\"+13233326955\","+
            "\"to\":\"+13865245000\","+
            "\"callId\":\"c-oexifypjlh5ygjr7qi4nesq\","+
            "\"callUri\": \"https://api.catapult.inetwork.com/v1/users/u-ndh7ecxejswersdu5g8zngvca/calls/c-oexifypjlh5ygjr7qi4nesq\","+
            "\"callState\":\"active\","+
            "\"applicationId\":\"a-25nh2lj6qrxznkfu4b732jy\","+
            "\"time\":\"2012-11-14T16:21:59.616Z\""+
            "}");
            Assert.IsNotNull(ev);
            var e = ev as IncomingCallEvent;
            Assert.IsNotNull(ev);
            Assert.AreEqual("incomingcall", e.EventType);
            Assert.AreEqual("+13233326955", e.From);
            Assert.AreEqual("+13865245000", e.To);
            Assert.AreEqual("c-oexifypjlh5ygjr7qi4nesq", e.CallId);
            Assert.AreEqual("https://api.catapult.inetwork.com/v1/users/u-ndh7ecxejswersdu5g8zngvca/calls/c-oexifypjlh5ygjr7qi4nesq", e.CallUri);
            Assert.AreEqual("active", e.CallState);
            Assert.AreEqual("a-25nh2lj6qrxznkfu4b732jy", e.ApplicationId);
            Assert.AreEqual("11/14/2012 16:21:59", e.Time.ToString(CultureInfo.InvariantCulture));
           
        }

        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void CreateFromString2Test()
        {
            BaseEvent.CreateFromString("{" +
            "\"eventType\":\"unknown\"," +
            "\"time\":\"2012-11-14T16:21:59.616Z\"" +
            "}");
            
        }

    }
}
