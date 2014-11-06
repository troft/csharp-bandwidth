using System;
using System.Collections.Generic;
using Bandwidth.Net.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Model
{
    [TestClass]
    public class BridgeTests
    {
        [TestInitialize]
        public void Setup()
        {
            Helper.SetEnvironmetVariables();
        }

        [TestMethod]
        public void GetBridgeTest()
        {
            var bridge = new Bridge
            {
                Id = "1",
                ActivatedTime = DateTime.Now,
                State = Bridge.BridgeState.Active
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/bridges/1", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(bridge)
            }))
            {
                var client = Helper.CreateClient();
                var result = Bridge.Get(client, "1").Result;
                if (server.Error != null) throw server.Error;
                Helper.AssertObjects(bridge, result);
            }
        }

        [TestMethod]
        public void GetBridgeWithDefaultClientTest()
        {
            var bridge = new Bridge
            {
                Id = "1",
                ActivatedTime = DateTime.Now,
                State = Bridge.BridgeState.Active
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/bridges/1", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(bridge)
            }))
            {
                var result = Bridge.Get("1").Result;
                if (server.Error != null) throw server.Error;
                Helper.AssertObjects(bridge, result);
            }
        }

        [TestMethod]
        public void ListBridgeTest()
        {
            var bridges = new[]{
                new Bridge
                {
                    Id = "1",
                    ActivatedTime = DateTime.Now,
                    State = Bridge.BridgeState.Active
                },
                new Bridge
                {
                    Id = "2",
                    ActivatedTime = DateTime.Now.AddMinutes(-10),
                    State = Bridge.BridgeState.Active
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/bridges", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(bridges)
            }))
            {
                var client = Helper.CreateClient();
                var result = Bridge.List(client).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(bridges[0], result[0]);
                Helper.AssertObjects(bridges[1], result[1]);
            }
        }

        [TestMethod]
        public void ListBridgeWithDefaultClientTest()
        {
            var bridges = new[]{
                new Bridge
                {
                    Id = "1",
                    ActivatedTime = DateTime.Now,
                    State = Bridge.BridgeState.Active
                },
                new Bridge
                {
                    Id = "2",
                    ActivatedTime = DateTime.Now.AddMinutes(-10),
                    State = Bridge.BridgeState.Active
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/bridges", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(bridges)
            }))
            {
                var result = Bridge.List().Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(bridges[0], result[0]);
                Helper.AssertObjects(bridges[1], result[1]);
            }
        }

        [TestMethod]
        public void CreateBridgeTest()
        {
            var bridge = new Dictionary<string, object>
            {
                {"call",  "call"}
            };

            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/bridges", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(bridge),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/bridges/1", Helper.UserId) } }
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/bridges/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                }
            }))
            {
                var client = Helper.CreateClient();
                var b = Bridge.Create(client, bridge).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual("1", b.Id);
            }
        }

        [TestMethod]
        public void CreateBridgeWithDefaultClientTest()
        {
            var bridge = new Dictionary<string, object>
            {
                {"call",  "call"}
            };

            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/bridges", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(bridge),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/bridges/1", Helper.UserId) } }
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/bridges/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                }
            }))
            {
                var b = Bridge.Create(bridge).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual("1", b.Id);
            }
        }

        [TestMethod]
        public void PlayBridgeAudioTest()
        {
            var data = new Dictionary<string, object>
            {
                { "fileUrl", "url" }
            };

            using (var server = new HttpServer(new[]
            {
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/bridges/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object> {{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/bridges/1/audio", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(data)
                }
            }))
            {
                var client = Helper.CreateClient();
                var bridge = Bridge.Get(client, "1").Result;
                bridge.PlayAudio(data).Wait();
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void GetBridgeCallsTest()
        {
            using (var server = new HttpServer(new[]
            {
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/bridges/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object> {{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/bridges/1/calls", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new[]
                    {
                        new Dictionary<string, object> {{"id", "1"}}, 
                        new Dictionary<string, object> {{"id", "2"}}
                    })
                }
            }))
            {
                var client = Helper.CreateClient();
                var bridge = Bridge.Get(client, "1").Result;
                var calls = bridge.GetCalls().Result;
                Assert.AreEqual(2, calls.Length);
                Assert.AreEqual("1", calls[0].Id);
                Assert.AreEqual("2", calls[1].Id);
                if (server.Error != null) throw server.Error;
            }
            
        }

    }
}
