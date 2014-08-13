using Bandwidth.Net.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConferenceMember = Bandwidth.Net.Events.ConferenceMember;
using Event = Bandwidth.Net.Events.Event;

namespace Bandwidth.Net.Tests.Events
{
    [TestClass]
    public class ConferenceMemberTests
    {
        [TestMethod]
        public void ParseRequestBodyTest()
        {
            const string json = @"{
                ""activeMembers"": 1,
                ""callId"": ""c-wf23iaxjlwdc3vrt46mnlza"",
                ""conferenceId"": ""conf-nreqnmjx4mo5y64tz6obnia"",
                ""eventType"": ""conference-member"",
                ""hold"": false,
                ""memberId"": ""member-5onr7o3uxp4pngbzq65tl3q"",
                ""memberUri"": ""https://api.catapult.inetwork.com/v1/users/u-ndh7ecxejswersdu5g8zngvca/conferences/conf-nreqnmjx4mo5y64tz6obnia/members/member-5onr7o3uxp4pngbzq65tl3q"",
                ""mute"": false,
                ""state"": ""completed"",
                ""time"": ""2013-07-12T20:55:10.100Z"" 
            }";
            var ev = Event.ParseRequestBody(json) as ConferenceMember;
            Assert.IsNotNull(ev);
            Assert.AreEqual("conf-nreqnmjx4mo5y64tz6obnia", ev.ConferenceId);
            Assert.AreEqual(MemberState.Completed, ev.State);
        }
    }
}
