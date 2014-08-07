using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Fakes;
using System.Text;
using System.Threading.Tasks;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Bandwidth.Net.Tests.Client
{
    [TestClass]
    public class ApiTests
    {
        [TestMethod]
        public void AuthHeaderTest()
        {
            using (ShimsContext.Create())
            {
                var called = false;
                ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (c, url, content) =>
                {
                    Assert.AreEqual("Basic", c.DefaultRequestHeaders.Authorization.Scheme);
                    Assert.AreEqual(string.Format("{0}:{1}", Fake.ApiKey, Fake.Secret), Encoding.UTF8.GetString(Convert.FromBase64String(c.DefaultRequestHeaders.Authorization.Parameter)));
                    called = true;
                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.Created));
                };
                using (var client = Fake.CreateClient())
                {
                    client.CreateCall(new Call
                    {
                        From = "From",
                        To = "To"
                    }).Wait();
                    Assert.IsTrue(called);
                }
            }
        }
        
        [TestMethod]
        public void MakeCallTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (c, url, content) =>
                {
                    Assert.AreEqual(string.Format("/users/{0}/calls", Fake.UserId), url);
                    dynamic res = JObject.Parse(content.ReadAsStringAsync().Result);
                    Assert.AreEqual("From", res.from.ToString());
                    Assert.AreEqual("To", res.to.ToString());
                    var response = new HttpResponseMessage(HttpStatusCode.Created);
                    response.Headers.Add("Location", string.Format("/v1/users/{0}/calls/1", Fake.UserId));
                    return Task.Run(() => response);
                };
                using (var client = Fake.CreateClient())
                {
                    var uri = client.CreateCall(new Call
                    {
                        From = "From",
                        To = "To"
                    }).Result;
                    Assert.AreEqual(string.Format("/v1/users/{0}/calls/1", Fake.UserId), uri.ToString());
                }
            }
        }

        [TestMethod]
        public void UpdateCallTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (c, url, content) =>
                {
                    Assert.AreEqual(string.Format("/users/{0}/calls/1", Fake.UserId), url);
                    dynamic res = JObject.Parse(content.ReadAsStringAsync().Result);
                    Assert.AreEqual("transferring", res.state.ToString());
                    Assert.AreEqual("Number", res.transferTo.ToString());
                    Assert.AreEqual("http://localhost", res.callbackUrl.ToString());
                    var response = new HttpResponseMessage(HttpStatusCode.Created);
                    response.Headers.Add("Location", string.Format("/v1/users/{0}/calls/1", Fake.UserId));
                    return Task.Run(() => response);
                };
                using (var client = Fake.CreateClient())
                {
                    client.UpdateCall("1", new Call
                    {
                        State = CallState.Transferring,
                        TransferTo = "Number",
                        CallbackUrl = new Uri("http://localhost")
                    }).Wait();
                }
            }
        }

        [TestMethod]
        public void GetRecording()
        {
            using (ShimsContext.Create())
            {
                var recording = new Recording
                {
                    Id = "1",
                    StartTime = DateTime.Now.AddMinutes(-10),
                    EndTime = DateTime.Now.AddMinutes(-5),
                    State = RecordingState.Complete,
                    Call = "call",
                    Media = "media"
                };
                ShimHttpClient.AllInstances.GetAsyncString = (c, url) =>
                {
                    Assert.AreEqual(string.Format("/users/{0}/recordings/1", Fake.UserId), url);
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = Fake.CreateJsonContent(recording)
                    };
                    return Task.Run(() => response);
                };
                using (var client = Fake.CreateClient())
                {
                    var result = client.GetRecording("1").Result;
                    Fake.AssertObjects(recording, result);
                }
            }
        }

        [TestMethod]
        public void GetRecordings()
        {
            using (ShimsContext.Create())
            {
                var recordings = new[]
                {
                    new Recording
                    {
                        Id = "1",
                        StartTime = DateTime.Now.AddMinutes(-10),
                        EndTime = DateTime.Now.AddMinutes(-5),
                        State = RecordingState.Complete,
                        Call = "call",
                        Media = "media"
                    },
                    new Recording
                    {
                        Id = "2",
                        StartTime = DateTime.Now.AddMinutes(-15),
                        EndTime = DateTime.Now.AddMinutes(-13),
                        State = RecordingState.Error,
                        Call = "call2",
                        Media = "media2"
                    }
                };
                ShimHttpClient.AllInstances.GetAsyncString = (c, url) =>
                {
                    Assert.AreEqual(string.Format("/users/{0}/recordings?page=1", Fake.UserId), url);
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = Fake.CreateJsonContent(recordings)
                    };
                    return Task.Run(() => response);
                };
                using (var client = Fake.CreateClient())
                {
                    var result = client.GetRecordings().Result;
                    Assert.AreEqual(2, result.Length);
                    Fake.AssertObjects(recordings[0], result[0]);
                    Fake.AssertObjects(recordings[1], result[1]);
                }
            }
        }
    }
}
