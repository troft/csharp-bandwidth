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
    public class CallsTests
    {
        [TestMethod]
        public void CreateTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (c, url, content) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/calls", Fake.UserId), url);
                    var call = Fake.ParseJsonContent<Call>(content).Result;
                    Assert.AreEqual("From", call.From);
                    Assert.AreEqual("To", call.To);
                    var response = new HttpResponseMessage(HttpStatusCode.Created);
                    response.Headers.Add("Location", string.Format("/v1/users/{0}/calls/1", Fake.UserId));
                    return Task.Run(() => response);
                };
                using (var client = Fake.CreateClient())
                {
                    var id = client.Calls.Create(new Call
                    {
                        From = "From",
                        To = "To"
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
                    Assert.AreEqual(string.Format("users/{0}/calls/1", Fake.UserId), url);
                    var call = Fake.ParseJsonContent<Call>(content).Result;
                    Assert.AreEqual(CallState.Transferring, call.State);
                    Assert.AreEqual("Number", call.TransferTo);
                    Assert.AreEqual("http://localhost/", call.CallbackUrl.ToString());
                    var response = new HttpResponseMessage(HttpStatusCode.Created);
                    response.Headers.Add("Location", string.Format("/v1/users/{0}/calls/1", Fake.UserId));
                    return Task.Run(() => response);
                };
                using (var client = Fake.CreateClient())
                {
                    client.Calls.Update("1", new Call
                    {
                        State = CallState.Transferring,
                        TransferTo = "Number",
                        CallbackUrl = new Uri("http://localhost/")
                    }).Wait();
                }
            }
        }

        [TestMethod]
        public void GetTest()
        {
            using (ShimsContext.Create())
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
                ShimHttpClient.AllInstances.GetAsyncString = (c, url) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/calls/1", Fake.UserId), url);
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = Fake.CreateJsonContent(call)
                    };
                    return Task.Run(() => response);
                };
                using (var client = Fake.CreateClient())
                {
                    var result = client.Calls.Get("1").Result;
                    Fake.AssertObjects(call, result);
                }
            }
        }

        [TestMethod]
        public void GetAllTest()
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
                    Assert.AreEqual(string.Format("users/{0}/calls", Fake.UserId), url);
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = Fake.CreateJsonContent(calls)
                    };
                    return Task.Run(() => response);
                };
                using (var client = Fake.CreateClient())
                {
                    var result = client.Calls.GetAll().Result;
                    Assert.AreEqual(2, result.Length);
                    Fake.AssertObjects(calls[0], result[0]);
                    Fake.AssertObjects(calls[1], result[1]);
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
                    Assert.AreEqual(string.Format("users/{0}/calls/1/audio", Fake.UserId), url);
                    var audio = Fake.ParseJsonContent<Audio>(content).Result;
                    Assert.AreEqual("Test", audio.Sentence);
                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.OK));
                };
                using (var client = Fake.CreateClient())
                {
                    client.Calls.SetAudio("1", new Audio{Sentence = "Test"}).Wait();
                }
            }
        }

        [TestMethod]
        public void SetDtmfTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (c, url, content) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/calls/1/dtmf", Fake.UserId), url);
                    var dtmf = Fake.ParseJsonContent<Dtmf>(content).Result;
                    Assert.AreEqual("Test", dtmf.DtmfOut);
                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.OK));
                };
                using (var client = Fake.CreateClient())
                {
                    client.Calls.SetDtmf("1", new Dtmf { DtmfOut = "Test" }).Wait();
                }
            }
        }

        [TestMethod]
        public void CreateGatherTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (c, url, content) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/calls/1/gather", Fake.UserId), url);
                    var gather = Fake.ParseJsonContent<CreateGather>(content).Result;
                    Assert.AreEqual("00", gather.TerminatingDigits);
                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.Created));
                };
                using (var client = Fake.CreateClient())
                {
                    client.Calls.CreateGather("1", new CreateGather { TerminatingDigits = "00" }).Wait();
                }
            }
        }

        [TestMethod]
        public void UpdateGatherTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (c, url, content) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/calls/1/gather/11", Fake.UserId), url);
                    var gather = Fake.ParseJsonContent<Gather>(content).Result;
                    Assert.AreEqual("00", gather.Digits);
                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.Created));
                };
                using (var client = Fake.CreateClient())
                {
                    client.Calls.UpdateGather("1", "11", new Gather { Digits = "00" }).Wait();
                }
            }
        }

        [TestMethod]
        public void GetGatherTest()
        {
            using (ShimsContext.Create())
            {
                var gather = new Gather
                {
                    Id = "11",
                    Digits = "00"
                };
                ShimHttpClient.AllInstances.GetAsyncString = (c, url) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/calls/1/gather/11", Fake.UserId), url);
                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.Created){Content = Fake.CreateJsonContent(gather)});
                };
                using (var client = Fake.CreateClient())
                {
                    var result  = client.Calls.GetGather("1", "11").Result;
                    Fake.AssertObjects(gather, result);
                }
            }
        }

        [TestMethod]
        public void GetEventTest()
        {
            using (ShimsContext.Create())
            {
                var ev = new Event
                {
                    Id = "11",
                    Data = "Test"
                };
                ShimHttpClient.AllInstances.GetAsyncString = (c, url) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/calls/1/events/11", Fake.UserId), url);
                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.Created) { Content = Fake.CreateJsonContent(ev) });
                };
                using (var client = Fake.CreateClient())
                {
                    var result = client.Calls.GetEvent("1", "11").Result;
                    Fake.AssertObjects(ev, result);
                }
            }
        }

        [TestMethod]
        public void GetEventsTest()
        {
            using (ShimsContext.Create())
            {
                var events = new[]{
                    new Event
                    {
                        Id = "11",
                        Data = "Test"
                    },
                    new Event
                    {
                        Id = "22",
                        Data = "Test2"
                    }
                };
                ShimHttpClient.AllInstances.GetAsyncString = (c, url) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/calls/1/events", Fake.UserId), url);
                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.Created) { Content = Fake.CreateJsonContent(events) });
                };
                using (var client = Fake.CreateClient())
                {
                    var result = client.Calls.GetEvents("1").Result;
                    Assert.AreEqual(2, result.Length);
                    Fake.AssertObjects(events[0], result[0]);
                    Fake.AssertObjects(events[1], result[1]);
                }
            }
        }

        [TestMethod]
        public void GetRecordingsTest()
        {
            using (ShimsContext.Create())
            {
                var recordings = new[]{
                    new Recording
                    {
                        Id = "11",
                        State = RecordingState.Complete
                    },
                    new Recording
                    {
                        Id = "22",
                        State = RecordingState.Recording
                    }
                };
                ShimHttpClient.AllInstances.GetAsyncString = (c, url) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/calls/1/recordings", Fake.UserId), url);
                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.Created) { Content = Fake.CreateJsonContent(recordings) });
                };
                using (var client = Fake.CreateClient())
                {
                    var result = client.Calls.GetRecordings("1").Result;
                    Assert.AreEqual(2, result.Length);
                    Fake.AssertObjects(recordings[0], result[0]);
                    Fake.AssertObjects(recordings[1], result[1]);
                }
            }
        }
        
    }
}
