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
    public class RecordingsTests
    {
        [TestMethod]
        public void GetTest()
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
                    Assert.AreEqual(string.Format("users/{0}/recordings/1", Fake.UserId), url);
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = Fake.CreateJsonContent(recording)
                    };
                    return Task.Run(() => response);
                };
                using (var client = Fake.CreateClient())
                {
                    var result = client.Recordings.Get("1").Result;
                    Fake.AssertObjects(recording, result);
                }
            }
        }

        [TestMethod]
        public void GetAllTest()
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
                    Assert.AreEqual(string.Format("users/{0}/recordings?page=1", Fake.UserId), url);
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = Fake.CreateJsonContent(recordings)
                    };
                    return Task.Run(() => response);
                };
                using (var client = Fake.CreateClient())
                {
                    var result = client.Recordings.GetAll(new RecordingQuery{Page = 1}).Result;
                    Assert.AreEqual(2, result.Length);
                    Fake.AssertObjects(recordings[0], result[0]);
                    Fake.AssertObjects(recordings[1], result[1]);
                }
            }
        }
    }
}
