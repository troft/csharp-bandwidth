using System.Collections.Generic;
using Bandwidth.Net.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Model
{
    [TestClass]
    public class PhoneNumberTests
    {
        [TestInitialize]
        public void Setup()
        {
            Helper.SetEnvironmetVariables();
        }
        [TestMethod]
        public void GetTest()
        {
            var item = new PhoneNumber
            {
                Id = "1",
                Number = "111",
                NumberState = PhoneNumberState.Enabled
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/phoneNumbers/1", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(item)
            }))
            {
                var client = Helper.CreateClient();
                var result = PhoneNumber.Get(client, "1").Result;
                if (server.Error != null) throw server.Error;
                Helper.AssertObjects(item, result);
            }
        }

        [TestMethod]
        public void GetWithDefaultClientTest()
        {
            var item = new PhoneNumber
            {
                Id = "1",
                Number = "111",
                NumberState = PhoneNumberState.Enabled
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/phoneNumbers/1", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(item)
            }))
            {
                var result = PhoneNumber.Get("1").Result;
                if (server.Error != null) throw server.Error;
                Helper.AssertObjects(item, result);
            }
        }

        [TestMethod]
        public void ListTest()
        {
            var items = new[]
            {
                new PhoneNumber
                {
                    Id = "1",
                    Number = "111",
                    NumberState = PhoneNumberState.Enabled
                },
                new PhoneNumber
                {
                    Id = "2",
                    Number = "222",
                    NumberState = PhoneNumberState.Enabled
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/phoneNumbers", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(items)
            }))
            {
                var client = Helper.CreateClient();
                var result = PhoneNumber.List(client).Result;
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
                new PhoneNumber
                {
                    Id = "1",
                    Number = "111",
                    NumberState = PhoneNumberState.Enabled
                },
                new PhoneNumber
                {
                    Id = "2",
                    Number = "222",
                    NumberState = PhoneNumberState.Enabled
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/phoneNumbers", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(items)
            }))
            {
                var result = PhoneNumber.List().Result;
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
                new PhoneNumber
                {
                    Id = "1",
                    Number = "111",
                    NumberState = PhoneNumberState.Enabled
                },
                new PhoneNumber
                {
                    Id = "2",
                    Number = "222",
                    NumberState = PhoneNumberState.Enabled
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/phoneNumbers?page=2&size=10", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(items)
            }))
            {
                var client = Helper.CreateClient();
                var result = PhoneNumber.List(client, 2, 10).Result;
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
                new PhoneNumber
                {
                    Id = "1",
                    Number = "111",
                    NumberState = PhoneNumberState.Enabled
                },
                new PhoneNumber
                {
                    Id = "2",
                    Number = "222",
                    NumberState = PhoneNumberState.Enabled
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/phoneNumbers?page=1&size=25", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(items)
            }))
            {
                var result = PhoneNumber.List(1).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(items[0], result[0]);
                Helper.AssertObjects(items[1], result[1]);
            }
        }
        [TestMethod]
        public void CreateTest()
        {
            var item = new Dictionary<string, object>
            {
                {"number",  "123456"}
            };
            
            using (var server = new HttpServer(new []{
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/phoneNumbers", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(item),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/phoneNumbers/1", Helper.UserId) } }
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/phoneNumbers/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                }
            }))
            {
                var client = Helper.CreateClient();
                var it = PhoneNumber.Create(client, item).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual("1", it.Id);
            }
        }

        [TestMethod]
        public void CreateWithDefaultClientTest()
        {
            var item = new Dictionary<string, object>
            {
                {"number",  "123456"}
            };

            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/phoneNumbers", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(item),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/phoneNumbers/1", Helper.UserId) } }
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/phoneNumbers/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                }
            }))
            {
                var it = PhoneNumber.Create(item).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual("1", it.Id);
            }
        }
        [TestMethod]
        public void UpdateTest()
        {
            var data = new Dictionary<string, object> {{"state", "released"}};
            using (var server = new HttpServer(new []{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/phoneNumbers/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/phoneNumbers/1", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(data),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/phoneNumbers/1", Helper.UserId) } }
                }
            }))
            {
                var item = PhoneNumber.Get("1").Result;
                item.Update(data).Wait();
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void DeleteTest()
        {
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/phoneNumbers/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                },
                new RequestHandler
                {
                    EstimatedMethod = "DELETE",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/phoneNumbers/1", Helper.UserId)
                }
            }))
            {
                var item = PhoneNumber.Get("1").Result;
                item.Delete().Wait();
                if (server.Error != null) throw server.Error;
            }
        }


    }
}

