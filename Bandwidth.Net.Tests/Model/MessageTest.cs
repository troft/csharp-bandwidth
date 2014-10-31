using System;
using System.Collections.Generic;
using Bandwidth.Net.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Model
{
    [TestClass]
    public class MessageTests
    {
        [TestInitialize]
        public void Setup()
        {
            Helper.SetEnvironmetVariables();
        }

        [TestMethod]
        public void GetTest()
        {
            var message = new Message
            {
                Id = "1",
                From = "From",
                To = "To",
                Text = "Text"
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/messages/1", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(message)
            }))
            {
                var client = Helper.CreateClient();
                var result = Message.Get(client, "1").Result;
                if (server.Error != null) throw server.Error;
                Helper.AssertObjects(message, result);
            }
        }

        [TestMethod]
        public void GetWithDefaultClientTest()
        {
            var message = new Message
            {
                Id = "1",
                From = "From",
                To = "To",
                Text = "Text"
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/messages/1", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(message)
            }))
            {
                var result = Message.Get("1").Result;
                if (server.Error != null) throw server.Error;
                Helper.AssertObjects(message, result);
            }
        }

        [TestMethod]
        public void ListTest()
        {
            var messages = new[]{
                new Message
                {
                    Id = "1",
                    From = "From",
                    To = "To",
                    Text = "Text"
                },
                new Message
                {
                    Id = "2",
                    From = "From2",
                    To = "To2",
                    Text = "Text2"
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/messages", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(messages)
            }))
            {
                var client = Helper.CreateClient();
                var result = Message.List(client).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(messages[0], result[0]);
                Helper.AssertObjects(messages[1], result[1]);
            }
        }

        [TestMethod]
        public void ListWithDefaultClientTest()
        {
            var messages = new[]{
                new Message
                {
                    Id = "1",
                    From = "From",
                    To = "To",
                    Text = "Text"
                },
                new Message
                {
                    Id = "2",
                    From = "From2",
                    To = "To2",
                    Text = "Text2"
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/messages?size=10", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(messages)
            }))
            {
                var result = Message.List(new Dictionary<string, object>{{"size", 10}}).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(messages[0], result[0]);
                Helper.AssertObjects(messages[1], result[1]);
            }
        }
        [TestMethod]
        public void List2Test()
        {
            var messages = new[]{
                new Message
                {
                    Id = "1",
                    From = "From",
                    To = "To",
                    Text = "Text"
                },
                new Message
                {
                    Id = "2",
                    From = "From2",
                    To = "To2",
                    Text = "Text2"
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/messages?page=1&size=25", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(messages)
            }))
            {
                var client = Helper.CreateClient();
                var result = Message.List(client, 1).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(messages[0], result[0]);
                Helper.AssertObjects(messages[1], result[1]);
            }
        }

        [TestMethod]
        public void List2WithDefaultClientTest()
        {
            var messages = new[]{
                new Message
                {
                    Id = "1",
                    From = "From",
                    To = "To",
                    Text = "Text"
                },
                new Message
                {
                    Id = "2",
                    From = "From2",
                    To = "To2",
                    Text = "Text2"
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/messages?page=1&size=30", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(messages)
            }))
            {
                var result = Message.List(1, 30).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(messages[0], result[0]);
                Helper.AssertObjects(messages[1], result[1]);
            }
        }

        [TestMethod]
        public void CreateTest()
        {
            var message = new Dictionary<string, object>
            {
                {"to", "To"},
                {"from", "From"},
                {"text", "Text"}
            };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/messages", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(message),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/messages/1", Helper.UserId) } }
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/messages/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                }
            }))
            {
                var client = Helper.CreateClient();
                var m = Message.Create(client, message).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual("1", m.Id);
            }
        }

        [TestMethod]
        public void CreateWithDefaultClientTest()
        {
            var message = new Dictionary<string, object>
            {
                {"to", "To"},
                {"from", "From"},
                {"text", "Text"}
            };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/messages", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(message),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/messages/1", Helper.UserId) } }
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/messages/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                }
            }))
            {
                var m = Message.Create(message).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual("1", m.Id);
            }
        }
        
        [TestMethod]
        public void Create2Test()
        {
            var message = new Dictionary<string, object>
            {
                {"to", "To"},
                {"from", "From"},
                {"text", "Text"}
            };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/messages", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(message),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/messages/1", Helper.UserId) } }
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/messages/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                }
            }))
            {
                var client = Helper.CreateClient();
                var m = Message.Create(client, "To", "From", "Text").Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual("1", m.Id);
            }
        }

        [TestMethod]
        public void Create2WithDefaultClientTest()
        {
            var message = new Dictionary<string, object>
            {
                {"to", "To"},
                {"from", "From"},
                {"text", "Text"}
            };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/messages", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(message),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/messages/1", Helper.UserId) } }
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/messages/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                }
            }))
            {
                var m = Message.Create("To", "From", "Text").Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual("1", m.Id);
            }
        }
    }
}