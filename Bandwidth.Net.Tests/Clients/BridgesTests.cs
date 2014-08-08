using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Fakes;
using System.Threading.Tasks;
using Bandwidth.Net.Data;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Clients
{
    [TestClass]
    public class BridgesTests
    {
        [TestMethod]
        public void CreateTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (c, url, content) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/bridges", Fake.UserId), url);
                    var bridge = Fake.ParseJsonContent<Bridge>(content).Result;
                    Assert.AreEqual("http://localhost/", bridge.Calls.ToString());
                    Assert.AreEqual(BridgeState.Active, bridge.State);
                    var response = new HttpResponseMessage(HttpStatusCode.Created);
                    response.Headers.Add("Location", string.Format("/v1/users/{0}/bridges/1", Fake.UserId));
                    return Task.Run(() => response);
                };
                using (var client = Fake.CreateClient())
                {
                    var id = client.Bridges.Create(new Bridge
                    {
                        Calls = new Uri("http://localhost/"),
                        State = BridgeState.Active
                    }).Result;
                    Assert.AreEqual("1", id);
                }
            }
        }

        [TestMethod]
        public void UpdateTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (c, url, content) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/bridges/1", Fake.UserId), url);
                    var bridge = Fake.ParseJsonContent<Bridge>(content).Result;
                    Assert.AreEqual("http://localhost/", bridge.Calls.ToString());
                    Assert.AreEqual(BridgeState.Completed, bridge.State);
                    var response = new HttpResponseMessage(HttpStatusCode.Created);
                    response.Headers.Add("Location", string.Format("/v1/users/{0}/bridges/1", Fake.UserId));
                    return Task.Run(() => response);
                };
                using (var client = Fake.CreateClient())
                {
                    client.Bridges.Update("1", new Bridge
                    {
                        Calls = new Uri("http://localhost/"),
                        State = BridgeState.Completed
                    }).Wait();
                }
            }
        }

        [TestMethod]
        public void GetTest()
        {
            using (ShimsContext.Create())
            {
                var bridge = new Bridge
                {
                    Id = "1",
                    Calls = new Uri("http://localhost/"),
                    State = BridgeState.Completed
                };
                ShimHttpClient.AllInstances.GetAsyncString = (c, url) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/bridges/1", Fake.UserId), url);
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = Fake.CreateJsonContent(bridge)
                    };
                    return Task.Run(() => response);
                };
                using (var client = Fake.CreateClient())
                {
                    var result = client.Bridges.Get("1").Result;
                    Fake.AssertObjects(bridge, result);
                }
            }
        }

        [TestMethod]
        public void GetAllTest()
        {
            using (ShimsContext.Create())
            {
                var bridges = new[]
                {
                    new Bridge
                    {
                        Id = "1",
                        Calls = new Uri("http://localhost/"),
                        State = BridgeState.Completed
                    },
                    new Bridge
                    {
                        Id = "2",
                        Calls = new Uri("http://localhost2/"),
                        State = BridgeState.Active
                    }
                };
                ShimHttpClient.AllInstances.GetAsyncString = (c, url) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/bridges", Fake.UserId), url);
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = Fake.CreateJsonContent(bridges)
                    };
                    return Task.Run(() => response);
                };
                using (var client = Fake.CreateClient())
                {
                    var result = client.Bridges.GetAll().Result;
                    Assert.AreEqual(2, result.Length);
                    Fake.AssertObjects(bridges[0], result[0]);
                    Fake.AssertObjects(bridges[1], result[1]);
                }
            }
        }
        [TestMethod]
        public void SetAudioTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (c, url, content) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/bridges/1/audio", Fake.UserId), url);
                    var audio = Fake.ParseJsonContent<Audio>(content).Result;
                    Assert.AreEqual("Test", audio.Sentence);
                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.OK));
                };
                using (var client = Fake.CreateClient())
                {
                    client.Bridges.SetAudio("1", new Audio{Sentence = "Test"}).Wait();
                }
            }
        }

        
        [TestMethod]
        public void GetCallsTest()
        {
            using (ShimsContext.Create())
            {
                var calls = new[]
                {
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
                        State = CallState.Completed,
                        From = "From2",
                        To = "To2",
                        Direction = CallDirection.In
                    }
                };
                ShimHttpClient.AllInstances.GetAsyncString = (c, url) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/bridges/1/calls", Fake.UserId), url);
                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.Created) { Content = Fake.CreateJsonContent(calls) });
                };
                using (var client = Fake.CreateClient())
                {
                    var result = client.Bridges.GetCalls("1").Result;
                    Assert.AreEqual(2, result.Length);
                    Fake.AssertObjects(calls[0], result[0]);
                    Fake.AssertObjects(calls[1], result[1]);
                }
            }
        }
    }
}
