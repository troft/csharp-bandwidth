using System.Collections.Generic;
using Bandwidth.Net.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Clients
{
    [TestClass]
    public class MessagesTests
    {
        [TestMethod]
        public void SendTest()
        {
            var message = new Message
            {
                From = "From",
                To = "To",
                Text = "Text"
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "POST",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/messages", Helper.UserId),
                EstimatedContent = Helper.ToJsonString(message),
                HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/messages/1", Helper.UserId) } }
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var id = client.Messages.Send(message).Result;
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual("1", id);
                }
            }
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
                using (var client = Helper.CreateClient())
                {
                    var result = client.Messages.Get("1").Result;
                    if (server.Error != null) throw server.Error;
                    Helper.AssertObjects(message, result);
                }
            }
        }

        [TestMethod]
        public void GetAllTest()
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
                    From = "From",
                    To = "To",
                    Text = "Text"
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/messages", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(messages)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var result = client.Messages.GetAll().Result;
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual(2, result.Length);
                    Helper.AssertObjects(messages[0], result[0]);
                    Helper.AssertObjects(messages[1], result[1]);
                }
            }
        }
    }
}
