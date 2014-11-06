using Bandwidth.Net.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Model
{
    [TestClass]
    public class RecordingTests
    {
        [TestInitialize]
        public void Setup()
        {
            Helper.SetEnvironmetVariables();
        }
        [TestMethod]
        public void GetTest()
        {
            var item = new Recording
            {
                Id = "1",
                Call = "call1",
                Media = "media1"
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/recordings/1", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(item)
            }))
            {
                var client = Helper.CreateClient();
                var result = Recording.Get(client, "1").Result;
                if (server.Error != null) throw server.Error;
                Helper.AssertObjects(item, result);
            }
        }

        [TestMethod]
        public void GetWithDefaultClientTest()
        {
            var item = new Recording
            {
                Id = "1",
                Call = "call1",
                Media = "media1"
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/recordings/1", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(item)
            }))
            {
                var result = Recording.Get("1").Result;
                if (server.Error != null) throw server.Error;
                Helper.AssertObjects(item, result);
            }
        }

        [TestMethod]
        public void ListTest()
        {
            var items = new[]
            {
                new Recording
                {
                    Id = "1",
                    Call = "call1",
                    Media = "media1"
                },
                new Recording
                {
                    Id = "2",
                    Call = "call2",
                    Media = "media2"
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/recordings", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(items)
            }))
            {
                var client = Helper.CreateClient();
                var result = Recording.List(client).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(items[0], result[0]);
                Helper.AssertObjects(items[1], result[1]);
            }
        }

        [TestMethod]
        public void ListWithDefaultClientTest()
        {
            var items = new[]
            {
                new Recording
                {
                    Id = "1",
                    Call = "call1",
                    Media = "media1"
                },
                new Recording
                {
                    Id = "2",
                    Call = "call2",
                    Media = "media2"
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/recordings", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(items)
            }))
            {
                var result = Recording.List().Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(items[0], result[0]);
                Helper.AssertObjects(items[1], result[1]);
            }
        }

        [TestMethod]
        public void List2Test()
        {
            var items = new[]
            {
                new Recording
                {
                    Id = "1",
                    Call = "call1",
                    Media = "media1"
                },
                new Recording
                {
                    Id = "2",
                    Call = "call2",
                    Media = "media2"
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/recordings?page=1&size=25", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(items)
            }))
            {
                var client = Helper.CreateClient();
                var result = Recording.List(client, 1).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(items[0], result[0]);
                Helper.AssertObjects(items[1], result[1]);
            }
        }

        [TestMethod]
        public void List2WithDefaultClientTest()
        {
            var items = new[]
            {
                new Recording
                {
                    Id = "1",
                    Call = "call1",
                    Media = "media1"
                },
                new Recording
                {
                    Id = "2",
                    Call = "call2",
                    Media = "media2"
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/recordings?page=2&size=10", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(items)
            }))
            {
                var result = Recording.List(2, 10).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(items[0], result[0]);
                Helper.AssertObjects(items[1], result[1]);
            }
        }
    }
}

