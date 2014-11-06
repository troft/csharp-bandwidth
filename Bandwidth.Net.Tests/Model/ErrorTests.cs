using Bandwidth.Net.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Model
{
    [TestClass]
    public class ErrorTests
    {
        [TestInitialize]
        public void Setup()
        {
            Helper.SetEnvironmetVariables();
        }

        [TestMethod]
        public void GetTest()
        {
            var item = new Error
            {
                Id = "1",
                Message = "Error1",
                Category = ErrorCategory.BadRequest
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/errors/1", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(item)
            }))
            {
                var client = Helper.CreateClient();
                var result = Error.Get(client, "1").Result;
                if (server.Error != null) throw server.Error;
                Helper.AssertObjects(item, result);
            }
        }

        [TestMethod]
        public void GetWithDefaultClientTest()
        {
            var item = new Error
            {
                Id = "1",
                Message = "Error1",
                Category = ErrorCategory.BadRequest
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/errors/1", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(item)
            }))
            {
                var result = Error.Get("1").Result;
                if (server.Error != null) throw server.Error;
                Helper.AssertObjects(item, result);
            }
        }

        [TestMethod]
        public void ListTest()
        {
            var items = new[]
            {
                new Error
                {
                    Id = "1",
                    Message = "Error1",
                    Category = ErrorCategory.BadRequest
                },
                new Error
                {
                    Id = "1",
                    Message = "Error2",
                    Category = ErrorCategory.Conflict
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/errors", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(items)
            }))
            {
                var client = Helper.CreateClient();
                var result = Error.List(client).Result;
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
                new Error
                {
                    Id = "1",
                    Message = "Error1",
                    Category = ErrorCategory.BadRequest
                },
                new Error
                {
                    Id = "1",
                    Message = "Error2",
                    Category = ErrorCategory.Conflict
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/errors", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(items)
            }))
            {
                var result = Error.List().Result;
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
                new Error
                {
                    Id = "1",
                    Message = "Error1",
                    Category = ErrorCategory.BadRequest
                },
                new Error
                {
                    Id = "1",
                    Message = "Error2",
                    Category = ErrorCategory.Conflict
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/errors?page=1&size=25", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(items)
            }))
            {
                var client = Helper.CreateClient();
                var result = Error.List(client, 1).Result;
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
                new Error
                {
                    Id = "1",
                    Message = "Error1",
                    Category = ErrorCategory.BadRequest
                },
                new Error
                {
                    Id = "1",
                    Message = "Error2",
                    Category = ErrorCategory.Conflict
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/errors?page=2&size=10", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(items)
            }))
            {
                var result = Error.List(2, 10).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(items[0], result[0]);
                Helper.AssertObjects(items[1], result[1]);
            }
        }
    }
}

