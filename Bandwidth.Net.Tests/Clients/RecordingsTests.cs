using Bandwidth.Net.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Clients
{
    [TestClass]
    public class RecordingsTests
    {
        [TestMethod]
        public void GetTest()
        {
            var recording = new Recording
            {
                Id = "1",
                Media = "Media"
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/recordings/1", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(recording)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var result = client.Recordings.Get("1").Result;
                    if (server.Error != null) throw server.Error;
                    Helper.AssertObjects(recording, result);
                }
            }
        }

        [TestMethod]
        public void GetAllTest()
        {
            var recordings = new[]{
                new Recording
                {
                    Id = "1",
                    Media = "Media"
                },
                new Recording
                {
                    Id = "2",
                    Media = "Media2"
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/recordings", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(recordings)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var result = client.Recordings.GetAll().Result;
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual(2, result.Length);
                    Helper.AssertObjects(recordings[0], result[0]);
                    Helper.AssertObjects(recordings[1], result[1]);
                }
            }
        }
    }
}
