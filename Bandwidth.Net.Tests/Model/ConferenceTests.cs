using System.Collections.Generic;
using Bandwidth.Net.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Model
{
    [TestClass]
    public class ConferenceTests
    {
        [TestInitialize]
        public void Setup()
        {
            Helper.SetEnvironmetVariables();
        }

        [TestMethod]
        public void GetTest()
        {
            var conference = new Conference
            {
                Id = "1",
                From = "From",
                CallbackUrl = "Url"
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(conference)
            }))
            {
                var client = Helper.CreateClient();
                var result = Conference.Get(client, "1").Result;
                if (server.Error != null) throw server.Error;
                Helper.AssertObjects(conference, result);
            }
        }

        [TestMethod]
        public void GetWithDefaultClientTest()
        {
            var conference = new Conference
            {
                Id = "1",
                From = "From",
                CallbackUrl = "Url"
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(conference)
            }))
            {
                var result = Conference.Get("1").Result;
                if (server.Error != null) throw server.Error;
                Helper.AssertObjects(conference, result);
            }
        }
        
        [TestMethod]
        public void CreateTest()
        {
            var conference = new Dictionary<string, object>
            {
                {"from", "From"},
                {"callback", "Callback"}
            };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(conference),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/conferences/1", Helper.UserId) } }
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                }
            }))
            {
                var client = Helper.CreateClient();
                var m = Conference.Create(client, conference).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual("1", m.Id);
            }
        }

        [TestMethod]
        public void CreateWithDefaultClientTest()
        {
            var conference = new Dictionary<string, object>
            {
                {"from", "From"},
                {"callback", "Callback"}
            };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(conference),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/conferences/1", Helper.UserId) } }
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                }
            }))
            {
                var m = Conference.Create(conference).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual("1", m.Id);
            }
        }

        [TestMethod]
        public void UpdateTest()
        {
            var data = new Dictionary<string, object> { { "state", "completed" } };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(data),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/conferences/1", Helper.UserId) } }
                }
            }))
            {
                var conference = Conference.Get("1").Result;
                conference.Update(data).Wait();
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void CompleteTest()
        {
            var data = new Dictionary<string, object> { { "state", "completed" } };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(data),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/conferences/1", Helper.UserId) } }
                }
            }))
            {
                var conference = Conference.Get("1").Result;
                conference.Complete().Wait();
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void MuteTest()
        {
            var data = new Dictionary<string, object> { { "mute", true } };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(data),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/conferences/1", Helper.UserId) } }
                }
            }))
            {
                var conference = Conference.Get("1").Result;
                conference.Mute().Wait();
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void PlayAudioTest()
        {
            var data = new Dictionary<string, object> { { "fileUrl", "url" } };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1/audio", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(data)
                }
            }))
            {
                var c = Conference.Get("1").Result;
                c.PlayAudio(data).Wait();
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void GetMembersTest()
        {
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1/members", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new[]
                    {
                        new Dictionary<string, object>{{"id", "10"}},
                        new Dictionary<string, object>{{"id", "11"}}
                    })
                }
            }))
            {
                var c = Conference.Get("1").Result;
                var members = c.GetMembers().Result;
                Assert.AreEqual(2, members.Length);
                Assert.AreEqual("10", members[0].Id);
                Assert.AreEqual("11", members[1].Id);
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void GetMemberTest()
        {
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1/members/10", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "10"}})
                }
            }))
            {
                var c = Conference.Get("1").Result;
                var member = c.GetMember("10").Result;
                Assert.AreEqual("10", member.Id);
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void CreateMemberTest()
        {
            var data = new Dictionary<string, object> {{"call", "Call"}};
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1/members", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(data),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/conferences/1/members/10", Helper.UserId) } }
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1/members/10", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "10"}})
                }
            }))
            {
                var c = Conference.Get("1").Result;
                var member = c.CreateMember(data).Result;
                Assert.AreEqual("10", member.Id);
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void PlayMemberAudioTest()
        {
            var data = new Dictionary<string, object> { { "fileUrl", "url" } };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1/members/10", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "10"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1/members/10/audio", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(data)
                }
            }))
            {
                var c = Conference.Get("1").Result;
                var member = c.GetMember("10").Result;
                member.PlayAudio(data).Wait();
                if (server.Error != null) throw server.Error;
            }
        }
        
    }
}