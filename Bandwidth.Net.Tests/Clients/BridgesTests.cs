using System;
using System.Collections.Generic;
using Bandwidth.Net.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Clients
{
    [TestClass]
    public class BridgesTests
    {
        [TestMethod]
        public void CreateTest()
        {
            var bridge = new Bridge
            {
                Calls = new Uri("http://localhost/")
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "POST",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/bridges", Helper.UserId),
                EstimatedContent = Helper.ToJsonString(bridge),
                HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/bridges/1", Helper.UserId) } }
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var id = client.Bridges.Create(bridge).Result;
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual("1", id);
                }
            }
        }

        [TestMethod]
        public void UpdateTest()
        {
            var bridge = new Bridge
            {
                Calls = new Uri("http://localhost/")
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "POST",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/bridges/1", Helper.UserId),
                EstimatedContent = Helper.ToJsonString(bridge),
                HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/bridges/1", Helper.UserId) } }
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    client.Bridges.Update("1", bridge).Wait();
                    if (server.Error != null) throw server.Error;
                }
            }
        }

        [TestMethod]
        public void GetTest()
        {
            var bridge = new Bridge
            {
                Id = "1",
                Calls = new Uri("http://localhost/")
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/bridges/1", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(bridge)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var result = client.Bridges.Get("1").Result;
                    if (server.Error != null) throw server.Error;
                    Helper.AssertObjects(bridge, result);
                }
            }
        }

        [TestMethod]
        public void GetAllTest()
        {
            var bridges = new[]{
                new Bridge
                {
                    Id = "1",
                    Calls = new Uri("http://localhost/")
                },
                new Bridge
                {
                    Id = "2",
                    Calls = new Uri("http://localhost/2")
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/bridges", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(bridges)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var result = client.Bridges.GetAll().Result;
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual(2, result.Length);
                    Helper.AssertObjects(bridges[0], result[0]);
                    Helper.AssertObjects(bridges[1], result[1]);
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
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/bridges/1/audio", Helper.UserId),
                EstimatedContent = Helper.ToJsonString(audio)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    client.Bridges.SetAudio("1", audio).Wait();
                    if (server.Error != null) throw server.Error;
                }
            }
        }

        
        [TestMethod]
        public void GetCallsTest()
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
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/bridges/1/calls", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(calls)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var result = client.Bridges.GetCalls("1").Result;
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual(2, result.Length);
                    Helper.AssertObjects(calls[0], result[0]);
                    Helper.AssertObjects(calls[1], result[1]);
                }
            }
        }
    }
}
