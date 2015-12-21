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
        public void GetMessageTest()
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
        public void GetMessageWithDefaultClientTest()
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
        public void ListMessagesTest()
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
        public void ListMessagesWithDefaultClientTest()
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
        public void ListMessages2Test()
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
        public void ListMessages2WithDefaultClientTest()
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
        public void CreateMessageTest()
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
        public void CreateMessageWithDefaultClientTest()
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
        public void CreateMessage2Test()
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
        public void CreateMessage2WithDefaultClientTest()
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

        [TestMethod]
        public void CreateMessage3Test()
        {
            var messages = new[]
            {
                new Dictionary<string, object>
                {
                    {"to", "To1"},
                    {"from", "From1"},
                    {"text", "Text1"}
                },
                new Dictionary<string, object>
                {
                    {"to", "To2"},
                    {"from", "From2"},
                    {"text", "Text2"}
                }
            };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/messages", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(messages),
                    ContentToSend = Helper.CreateJsonContent(new object[]{
                        new {Result = "accepted", Location = string.Format("/v1/users/{0}/messages/1", Helper.UserId)},
                        new {Result = "error", Error = new {Message = "Error text"}}
                    })
                }
            }))
            {
                var client = Helper.CreateClient();
                var res = Message.Create(client, messages).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, res.Length);
                Assert.AreEqual("1", res[0].MessageId);
                Assert.IsNull(res[0].Exception);
                Assert.AreEqual("Error text", res[1].Exception.Message);
                Assert.IsNull(res[1].MessageId);
            }
        }

        [TestMethod]
        public void CreateMessage3WithDefaultClientTest()
        {
            var messages = new[]
            {
                new Dictionary<string, object>
                {
                    {"to", "To1"},
                    {"from", "From1"},
                    {"text", "Text1"}
                },
                new Dictionary<string, object>
                {
                    {"to", "To2"},
                    {"from", "From2"},
                    {"text", "Text2"}
                }
            };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/messages", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(messages),
                    ContentToSend = Helper.CreateJsonContent(new object[]{
                        new {Result = "accepted", Location = string.Format("/v1/users/{0}/messages/1", Helper.UserId)},
                        new {Result = "error", Error = new {Message = "Error text"}}
                    })
                }
            }))
            {
                var res = Message.Create(messages).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, res.Length);
                Assert.AreEqual("1", res[0].MessageId);
                Assert.IsNull(res[0].Exception);
                Assert.AreEqual("Error text", res[1].Exception.Message);
                Assert.IsNull(res[1].MessageId);
            }
        }
    }
}