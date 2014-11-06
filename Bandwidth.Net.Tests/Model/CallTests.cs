using System;
using System.Collections.Generic;
using Bandwidth.Net.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Model
{
    [TestClass]
    public class CallTests
    {
        [TestInitialize]
        public void Setup()
        {
            Helper.SetEnvironmetVariables();
        }

        [TestMethod]
        public void GetCallTest()
        {
            var call = new Call
            {
                Id = "1",
                StartTime = DateTime.Now.AddMinutes(-10),
                EndTime = DateTime.Now.AddMinutes(-5),
                State = CallState.Active,
                From = "From",
                To = "To",
                Direction = CallDirection.Out
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(call)
            }))
            {
                var client = Helper.CreateClient();
                var result = Call.Get(client, "1").Result;
                if (server.Error != null) throw server.Error;
                Helper.AssertObjects(call, result);
            }
        }

        [TestMethod]
        public void GetCallWithDefaultClientTest()
        {
            var call = new Call
            {
                Id = "1",
                StartTime = DateTime.Now.AddMinutes(-10),
                EndTime = DateTime.Now.AddMinutes(-5),
                State = CallState.Active,
                From = "From",
                To = "To",
                Direction = CallDirection.Out
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(call)
            }))
            {
                var result = Call.Get("1").Result;
                if (server.Error != null) throw server.Error;
                Helper.AssertObjects(call, result);
            }
        }

        [TestMethod]
        public void ListCallsTest()
        {
            var calls = new[]{
                new Call
                {
                    Id = "1",
                    StartTime = DateTime.Now.AddMinutes(-10),
                    EndTime = DateTime.Now.AddMinutes(-5),
                    State = CallState.Active,
                    From = "From",
                    To = "To",
                    Direction = CallDirection.Out
                },
                new Call
                {
                    Id = "2",
                    StartTime = DateTime.Now.AddMinutes(-15),
                    EndTime = DateTime.Now.AddMinutes(-11),
                    State = CallState.Active,
                    From = "From2",
                    To = "To2",
                    Direction = CallDirection.In
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(calls)
            }))
            {
                var client = Helper.CreateClient();
                var result = Call.List(client).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(calls[0], result[0]);
                Helper.AssertObjects(calls[1], result[1]);
            }
        }

        [TestMethod]
        public void ListCallsWithDefaultClientTest()
        {
            var calls = new[]{
                new Call
                {
                    Id = "1",
                    StartTime = DateTime.Now.AddMinutes(-10),
                    EndTime = DateTime.Now.AddMinutes(-5),
                    State = CallState.Active,
                    From = "From",
                    To = "To",
                    Direction = CallDirection.Out
                },
                new Call
                {
                    Id = "2",
                    StartTime = DateTime.Now.AddMinutes(-15),
                    EndTime = DateTime.Now.AddMinutes(-11),
                    State = CallState.Active,
                    From = "From2",
                    To = "To2",
                    Direction = CallDirection.In
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(calls)
            }))
            {
                var result = Call.List().Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(calls[0], result[0]);
                Helper.AssertObjects(calls[1], result[1]);
            }
        }

        [TestMethod]
        public void ListCalls2Test()
        {
            var calls = new[]{
                new Call
                {
                    Id = "1",
                    StartTime = DateTime.Now.AddMinutes(-10),
                    EndTime = DateTime.Now.AddMinutes(-5),
                    State = CallState.Active,
                    From = "From",
                    To = "To",
                    Direction = CallDirection.Out
                },
                new Call
                {
                    Id = "2",
                    StartTime = DateTime.Now.AddMinutes(-15),
                    EndTime = DateTime.Now.AddMinutes(-11),
                    State = CallState.Active,
                    From = "From2",
                    To = "To2",
                    Direction = CallDirection.In
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls?page=1&size=25", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(calls)
            }))
            {
                var client = Helper.CreateClient();
                var result = Call.List(client, 1).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(calls[0], result[0]);
                Helper.AssertObjects(calls[1], result[1]);
            }
        }

        [TestMethod]
        public void ListCalls2WithDefaultClientTest()
        {
            var calls = new[]{
                new Call
                {
                    Id = "1",
                    StartTime = DateTime.Now.AddMinutes(-10),
                    EndTime = DateTime.Now.AddMinutes(-5),
                    State = CallState.Active,
                    From = "From",
                    To = "To",
                    Direction = CallDirection.Out
                },
                new Call
                {
                    Id = "2",
                    StartTime = DateTime.Now.AddMinutes(-15),
                    EndTime = DateTime.Now.AddMinutes(-11),
                    State = CallState.Active,
                    From = "From2",
                    To = "To2",
                    Direction = CallDirection.In
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls?page=3&size=10", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(calls)
            }))
            {
                var result = Call.List(3, 10).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(calls[0], result[0]);
                Helper.AssertObjects(calls[1], result[1]);
            }
        }
        [TestMethod]
        public void CreateCallTest()
        {
            var call = new Dictionary<string, object>
            {
                {"startTime",  DateTime.Now.AddMinutes(-10)},
                {"endTime", DateTime.Now.AddMinutes(-5)},
                {"state", "active"},
                {"from", "From"},
                {"to", "To"},
                {"direction", "out"}
            };
            
            using (var server = new HttpServer(new []{
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(call),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/calls/1", Helper.UserId) } }
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                }
            }))
            {
                var client = Helper.CreateClient();
                var cl = Call.Create(client, call).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual("1", cl.Id);
            }
        }

        [TestMethod]
        public void CreateCallWithDefaultClientTest()
        {
            var call = new Dictionary<string, object>
            {
                {"startTime",  DateTime.Now.AddMinutes(-10)},
                {"endTime", DateTime.Now.AddMinutes(-5)},
                {"state", "active"},
                {"from", "From"},
                {"to", "To"},
                {"direction", "out"}
            };

            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(call),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/calls/1", Helper.UserId) } }
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                }
            }))
            {
                var cl = Call.Create(call).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual("1", cl.Id);
            }
        }

        [TestMethod]
        public void CreateCall2Test()
        {
            var call = new Dictionary<string, object>
            {
                {"to", "To"},
                {"from", "From"},
                {"callbackUrl", "none"},
                {"tag", null}
            };

            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(call),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/calls/1", Helper.UserId) } }
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                }
            }))
            {
                var client = Helper.CreateClient();
                var cl = Call.Create(client, "To", "From").Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual("1", cl.Id);
            }
        }

        [TestMethod]
        public void CreateCall2WithDefaultClientTest()
        {
            var call = new Dictionary<string, object>
            {
                {"to", "To"},
                {"from", "From"},
                {"callbackUrl", "Callback"},
                {"tag", "Tag"}
            };

            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(call),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/calls/1", Helper.UserId) } }
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                }
            }))
            {
                var cl = Call.Create("To", "From", "Callback", "Tag").Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual("1", cl.Id);
            }
        }

        [TestMethod]
        public void UpdateCallTest()
        {
            var data = new Dictionary<string, object> {{"state", "completed"}};
            using (var server = new HttpServer(new []{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(data),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/calls/1", Helper.UserId) } }
                }
            }))
            {
                var call = Call.Get("1").Result;
                call.Update(data).Wait();
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
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1/audio", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(data)
                }
            }))
            {
                var call = Call.Get("1").Result;
                call.PlayAudio(data).Wait();
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void SpeakSentenceTest()
        {
            var data = new Dictionary<string, object>
            {
                {"gender", "female"},
                {"locale", "en_US"},
                {"voice", "kate"},
                {"sentence", "test"},
                {"tag", "tag"}
            };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1/audio", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(data)
                }
            }))
            {
                var call = Call.Get("1").Result;
                call.SpeakSentence("test", "tag").Wait();
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void PlayRecordingTest()
        {
            var data = new Dictionary<string, object>
            {
                { "fileUrl", "url" }
            };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1/audio", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(data)
                }
            }))
            {
                var call = Call.Get("1").Result;
                call.PlayRecording("url").Wait();
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void SendDtmfTest()
        {
            var data = new Dictionary<string, object>
            {
                { "dtmfOut", "test" }
            };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1/dtmf", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(data)
                }
            }))
            {
                var call = Call.Get("1").Result;
                call.SendDtmf("test").Wait();
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void CreateGatherTest()
        {
            var data = new Dictionary<string, object>
            {
                {"tag", "tag"},
                {"maxDigits", 1}
            };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1/gather", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(data)
                }
            }))
            {
                var call = Call.Get("1").Result;
                call.CreateGather(data).Wait();
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void CreateGather2Test()
        {
            var data = new Dictionary<string, object>
            {
                {"tag", "1"},
                {"maxDigits", 1},
                {"promt", new Dictionary<string, object>
                    {
                        {"locale", "en_US"},
                        {"gender", "female"},
                        {"sentence", "test"},
                        {"voice", "kate"},
                        {"bargeable", true}
                    }
                 }

            };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1/gather", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(data)
                }
            }))
            {
                var call = Call.Get("1").Result;
                call.CreateGather("test").Wait();
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void UpdateGatherTest()
        {
            var data = new Dictionary<string, object>
            {
                {"state", "completed"}
            };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1/gather/10", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(data)
                }
            }))
            {
                var call = Call.Get("1").Result;
                call.UpdateGather("10", data).Wait();
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void GetGatherTest()
        {
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1/gather/10", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "10"}})
                }
            }))
            {
                var call = Call.Get("1").Result;
                var gather = call.GetGather("10").Result;
                Assert.AreEqual("10", gather.Id);
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void GetEventsTest()
        {
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1/events", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new[]
                    {
                        new Dictionary<string, object>{{"id", "10"}},
                        new Dictionary<string, object>{{"id", "11"}}
                    })
                }
            }))
            {
                var call = Call.Get("1").Result;
                var events = call.GetEvents().Result;
                Assert.AreEqual(2, events.Length);
                Assert.AreEqual("10", events[0].Id);
                Assert.AreEqual("11", events[1].Id);
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void GetEventTest()
        {
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1/events/10", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "10"}})
                }
            }))
            {
                var call = Call.Get("1").Result;
                var ev = call.GetEvent("10").Result;
                Assert.AreEqual("10", ev.Id);
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void GetRecordingsTest()
        {
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1/recordings", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new[]
                    {
                        new Dictionary<string, object>{{"id", "10"}},
                        new Dictionary<string, object>{{"id", "11"}}
                    })
                }
            }))
            {
                var call = Call.Get("1").Result;
                var recordings = call.GetRecordings().Result;
                Assert.AreEqual(2, recordings.Length);
                Assert.AreEqual("10", recordings[0].Id);
                Assert.AreEqual("11", recordings[1].Id);
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void HangUpTest()
        {
            var data = new Dictionary<string, object> { { "state", "completed" } };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(data),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/calls/1", Helper.UserId) } }
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}, {"state", "completed"}})
                }
            }))
            {
                var call = Call.Get("1").Result;
                call.HangUp().Wait();
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(CallState.Completed, call.State);
            }
        }
        [TestMethod]
        public void AnswerOnIncomingTest()
        {
            var data = new Dictionary<string, object> { { "state", "active" } };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(data),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/calls/1", Helper.UserId) } }
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}, {"state", "active"}})
                }
            }))
            {
                var call = Call.Get("1").Result;
                call.AnswerOnIncoming().Wait();
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(CallState.Active, call.State);
            }
        }

        [TestMethod]
        public void RejectIncomingTest()
        {
            var data = new Dictionary<string, object> { { "state", "rejected" } };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(data),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/calls/1", Helper.UserId) } }
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}, {"state", "rejected"}})
                }
            }))
            {
                var call = Call.Get("1").Result;
                call.RejectIncoming().Wait();
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(CallState.Rejected, call.State);
            }
        }

        [TestMethod]
        public void RecordingOnTest()
        {
            var data = new Dictionary<string, object> { { "recordingEnabled", true } };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(data),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/calls/1", Helper.UserId) } }
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}, { "recordingEnabled", true }})
                }
            }))
            {
                var call = Call.Get("1").Result;
                call.RecordingOn().Wait();
                if (server.Error != null) throw server.Error;
                Assert.IsTrue(call.RecordingEnabled);
            }
        }

        [TestMethod]
        public void RecordingOffTest()
        {
            var data = new Dictionary<string, object> { { "recordingEnabled", false } };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(data),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/calls/1", Helper.UserId) } }
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}, { "recordingEnabled", false }})
                }
            }))
            {
                var call = Call.Get("1").Result;
                call.RecordingOff().Wait();
                if (server.Error != null) throw server.Error;
                Assert.IsFalse(call.RecordingEnabled);
            }
        }
    }
}

