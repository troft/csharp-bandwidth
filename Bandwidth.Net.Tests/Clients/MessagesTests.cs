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
    public class MessagesTests
    {
        [TestMethod]
        public void SendTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (c, url, content) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/messages", Helper.UserId), url);
                    var message = Helper.ParseJsonContent<Message>(content).Result;
                    Assert.AreEqual("From", message.From);
                    Assert.AreEqual("To", message.To);
                    var response = new HttpResponseMessage(HttpStatusCode.Created);
                    response.Headers.Add("Location", string.Format("/v1/users/{0}/messages/1", Helper.UserId));
                    return Task.Run(() => response);
                };
                using (var client = Helper.CreateClient())
                {
                    var id = client.Messages.Send(new Message
                    {
                        From = "From",
                        To = "To"
                    }).Result;
                    Assert.AreEqual("1", id);
                }
            }
        }

        

        [TestMethod]
        public void GetTest()
        {
            using (ShimsContext.Create())
            {
                var message = new Message
                {
                    Id = "1",
                    From = "From",
                    To = "To"
                };
                ShimHttpClient.AllInstances.GetAsyncString = (c, url) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/messages/1", Helper.UserId), url);
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = Helper.CreateJsonContent(message)
                    };
                    return Task.Run(() => response);
                };
                using (var client = Helper.CreateClient())
                {
                    var result = client.Messages.Get("1").Result;
                    Helper.AssertObjects(message, result);
                }
            }
        }

        [TestMethod]
        public void GetAllTest()
        {
            using (ShimsContext.Create())
            {
                var messages = new[]
                {
                    new Message
                    {
                        Id = "1",
                        From = "From",
                        To = "To"
                    },
                    new Message
                    {
                        Id = "2",
                        From = "From2",
                        To = "To2"
                    }
                };
                ShimHttpClient.AllInstances.GetAsyncString = (c, url) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/messages", Helper.UserId), url);
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = Helper.CreateJsonContent(messages)
                    };
                    return Task.Run(() => response);
                };
                using (var client = Helper.CreateClient())
                {
                    var result = client.Messages.GetAll().Result;
                    Assert.AreEqual(2, result.Length);
                    Helper.AssertObjects(messages[0], result[0]);
                    Helper.AssertObjects(messages[1], result[1]);
                }
            }
        }
    }
}
