using Bandwidth.Net.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConferenceMember = Bandwidth.Net.Events.ConferenceMember;
using Event = Bandwidth.Net.Events.Event;

namespace Bandwidth.Net.Tests.Events
{
    [TestClass]
    public class EventTests
    {
        [TestMethod]
        public void ParseRequestBodyTest()
        {
            const string json = "{" +
                                "\"activeMembers\": 1," +
                                "\"callId\": \"c-wf23iaxjlwdc3vrt46mnlza\"," +
                                "\"conferenceId\": \"conf-nreqnmjx4mo5y64tz6obnia\"," +
                                "\"eventType\": \"conference-member\"," +
                                "\"hold\": false," +
                                "\"memberId\": \"member-5onr7o3uxp4pngbzq65tl3q\"," +
                                "\"memberUri\": \"https://api.catapult.inetwork.com/v1/users/u-ndh7ecxejswersdu5g8zngvca/conferences/conf-nreqnmjx4mo5y64tz6obnia/members/member-5onr7o3uxp4pngbzq65tl3q\"," +
                                "\"mute\": false," +
                                "\"state\": \"active\"," +
                                "\"time\": \"2013-07-12T20:53:11.646Z\"" +
                                "}";
            var member = Event.ParseRequestBody(json) as ConferenceMember;
            Assert.IsNotNull(member);
            Assert.AreEqual(1, member.ActiveMembers);
            Assert.AreEqual("c-wf23iaxjlwdc3vrt46mnlza", member.CallId);
            Assert.AreEqual(MemberState.Active, member.State);
        }
    }
}
