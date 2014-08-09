using System;
using System.Collections.Generic;
using Bandwidth.Net.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Clients
{
    [TestClass]
    public class ConferencesTests
    {
        [TestMethod]
        public void CreateTest()
        {
            var conference = new Conference
            {
                From = "From",
                CallbackUrl = new Uri("http://localhost/")
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "POST",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences", Helper.UserId),
                EstimatedContent = Helper.ToJsonString(conference),
                HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/conferences/1", Helper.UserId) } }
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var id = client.Conferences.Create(conference).Result;
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual("1", id);
                }
            }
        }

        [TestMethod]
        public void UpdateTest()
        {
            var conference = new Conference
            {
                From = "From",
                CallbackUrl = new Uri("http://localhost/")
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "POST",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1", Helper.UserId),
                EstimatedContent = Helper.ToJsonString(conference),
                HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/conferences/1", Helper.UserId) } }
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    client.Conferences.Update("1", conference).Wait();
                    if (server.Error != null) throw server.Error;
                }
            }
        }

        [TestMethod]
        public void GetTest()
        {
            var conference = new Conference
            {
                Id = "1",
                From = "From",
                CallbackUrl = new Uri("http://localhost/"),
                State = ConferenceState.Active
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(conference)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var result = client.Conferences.Get("1").Result;
                    if (server.Error != null) throw server.Error;
                    Helper.AssertObjects(conference, result);
                }
            }
        }
        
        [TestMethod]
        public void SetAudioTest()
        {
            var audio = new Audio
            {
                Sentence = "Sentence"
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "POST",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1/audio", Helper.UserId),
                EstimatedContent = Helper.ToJsonString(audio)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    client.Conferences.SetAudio("1", audio).Wait();
                    if (server.Error != null) throw server.Error;
                }
            }
        }

        
        [TestMethod]
        public void GetAllMembersTest()
        {
            var conferenceMembers = new[]{
                new ConferenceMember
                {
                    Id = "1",
                    Call = new Uri("http://localhost")
                },
                new ConferenceMember
                {
                    Id = "2",
                    Call = new Uri("http://localhost/2")
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1/members", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(conferenceMembers)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var result = client.Conferences.GetAllMembers("1").Result;
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual(2, result.Length);
                    Helper.AssertObjects(conferenceMembers[0], result[0]);
                    Helper.AssertObjects(conferenceMembers[1], result[1]);
                }
            }
        }

        [TestMethod]
        public void CreateMemberTest()
        {
            var conferenceMember = new ConferenceMember
            {
                Call = new Uri("http://localhost")
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "POST",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1/members", Helper.UserId),
                EstimatedContent = Helper.ToJsonString(conferenceMember),
                HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/conferences/1/members/11", Helper.UserId) } }
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var id = client.Conferences.CreateMember("1", conferenceMember).Result;
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual("11", id);
                }
            }
        }

        [TestMethod]
        public void UpdateMemberTest()
        {
            var conferenceMember = new ConferenceMember
            {
                Call = new Uri("http://localhost")
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "POST",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1/members/11", Helper.UserId),
                EstimatedContent = Helper.ToJsonString(conferenceMember)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    client.Conferences.UpdateMember("1", "11", conferenceMember).Wait();
                    if (server.Error != null) throw server.Error;
                }
            }
        }

        [TestMethod]
        public void GetMemberTest()
        {
            var conferenceMember = new ConferenceMember
            {
                Id = "11",
                Call = new Uri("http://localhost")
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1/members/11", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(conferenceMember)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var result = client.Conferences.GetMember("1", "11").Result;
                    if (server.Error != null) throw server.Error;
                    Helper.AssertObjects(conferenceMember, result);
                }
            }
        }


        [TestMethod]
        public void SetMemberAudioTest()
        {
            var audio = new Audio
            {
                Sentence = "Sentence"
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "POST",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/conferences/1/members/11/audio", Helper.UserId),
                EstimatedContent = Helper.ToJsonString(audio)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    client.Conferences.SetMemberAudio("1", "11", audio).Wait();
                    if (server.Error != null) throw server.Error;
                }
            }
        }
    }
}
