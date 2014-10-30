/*
using System;
using System.Collections.Generic;
using Bandwidth.Net.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Model
{
    [TestClass]
    public class CallTests
    {
        [TestMethod]
        public void CreateTest()
        {
            var call = new Call
            {
                StartTime = DateTime.Now.AddMinutes(-10),
                EndTime = DateTime.Now.AddMinutes(-5),
                State = CallState.Active,
                From = "From",
                To = "To",
                Direction = CallDirection.Out
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "POST",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls", Helper.UserId),
                EstimatedContent = Helper.ToJsonString(call),
                HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/calls/1", Helper.UserId) } }
            }))
            {
                var client = Helper.CreateClient();
                var cl = Call.Create(client, call).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual("1", cl.Id);
            }
        }

        [TestMethod]
        public void UpdateTest()
        {
            var call = new Call
            {
                State = CallState.Completed,
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "POST",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1", Helper.UserId),
                EstimatedContent = Helper.ToJsonString(call),
                HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/calls/1", Helper.UserId) } }
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    client.Calls.Update("1", call).Wait();
                    if (server.Error != null) throw server.Error;
                }
            }
        }

        [TestMethod]
        public void GetTest()
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
                using (var client = Helper.CreateClient())
                {
                    var result = client.Calls.Get("1").Result;
                    if (server.Error != null) throw server.Error;
                    Helper.AssertObjects(call, result);
                }
            }
        }

        [TestMethod]
        public void GetAllTest()
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
                using (var client = Helper.CreateClient())
                {
                    var result = client.Calls.GetAll().Result;
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual(2, result.Length);
                    Helper.AssertObjects(calls[0], result[0]);
                    Helper.AssertObjects(calls[1], result[1]);
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
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1/audio", Helper.UserId),
                EstimatedContent = Helper.ToJsonString(audio)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    client.Calls.SetAudio("1", audio).Wait();
                    if (server.Error != null) throw server.Error;
                }
            }
        }

        [TestMethod]
        public void SetDtmfTest()
        {
            var dtmf = new Dtmf
            {
                DtmfOut = "Test"
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "POST",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1/dtmf", Helper.UserId),
                EstimatedContent = Helper.ToJsonString(dtmf)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    client.Calls.SetDtmf("1", dtmf).Wait();
                    if (server.Error != null) throw server.Error;
                }
            }
        }

        [TestMethod]
        public void CreateGatherTest()
        {
            var gather = new CreateGather
            {
                TerminatingDigits = "00"
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "POST",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1/gather", Helper.UserId),
                EstimatedContent = Helper.ToJsonString(gather)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    client.Calls.CreateGather("1", gather).Wait();
                    if (server.Error != null) throw server.Error;
                }
            }
        }

        [TestMethod]
        public void UpdateGatherTest()
        {
            var gather = new Gather
            {
                Reason = "Reason"
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "POST",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1/gather/11", Helper.UserId),
                EstimatedContent = Helper.ToJsonString(gather)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    client.Calls.UpdateGather("1", "11", gather).Wait();
                    if (server.Error != null) throw server.Error;
                }
            }
        }

        [TestMethod]
        public void GetGatherTest()
        {
            var gather = new Gather
            {
                Reason = "Reason"
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1/gather/11", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(gather)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var result = client.Calls.GetGather("1", "11").Result;
                    if (server.Error != null) throw server.Error;
                    Helper.AssertObjects(gather, result);
                }
            }
        }

        [TestMethod]
        public void GetEventTest()
        {
            var ev = new Event
            {
                Name = "Event",
                Id = "11"
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1/events/11", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(ev)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var result = client.Calls.GetEvent("1", "11").Result;
                    if (server.Error != null) throw server.Error;
                    Helper.AssertObjects(ev, result);
                }
            }
        }

        [TestMethod]
        public void GetEventsTest()
        {
            var events = new []{
                new Event
                {
                    Name = "Event",
                    Id = "11"
                },
                new Event
                {
                    Name = "Event2",
                    Id = "12"
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1/events", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(events)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var result = client.Calls.GetEvents("1").Result;
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual(2, result.Length);
                    Helper.AssertObjects(events[0], result[0]);
                    Helper.AssertObjects(events[1], result[1]);
                }
            }
        }

        [TestMethod]
        public void GetRecordingsTest()
        {
            var events = new[]{
                new Recording
                {
                    Id = "11",
                    Media = "Media"
                },
                new Recording
                {
                    Id = "12",
                    Media = "Media2"
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/calls/1/recordings", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(events)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var result = client.Calls.GetRecordings("1").Result;
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual(2, result.Length);
                    Helper.AssertObjects(events[0], result[0]);
                    Helper.AssertObjects(events[1], result[1]);
                }
            }
        }
        
    }
}
*/
