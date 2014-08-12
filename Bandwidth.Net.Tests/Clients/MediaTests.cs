using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Bandwidth.Net.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Clients
{
    [TestClass]
    public class MediaTests
    {
        [TestMethod]
        public void SetWithByteArrayTest()
        {
            var data = Encoding.UTF8.GetBytes("Hello");
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "PUT",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/media/test", Helper.UserId),
                EstimatedContent = "Hello",
                EstimatedHeaders = new Dictionary<string, string> { { "Content-Type", "media/type"} }
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    client.Media.Set("test", data, "media/type").Wait();
                    if (server.Error != null) throw server.Error;
                }
            }
        }

        [TestMethod]
        public void SetWithStreamTest()
        {
            var data = Encoding.UTF8.GetBytes("Hello");
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "PUT",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/media/test", Helper.UserId),
                EstimatedContent = "Hello",
                EstimatedHeaders = new Dictionary<string, string> { { "Content-Type", "media/type" } }
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    using (var stream = new MemoryStream(data))
                    {
                        client.Media.Set("test", stream, "media/type").Wait();
                        if (server.Error != null) throw server.Error;
                    }
                }
            }
        }

        
        [TestMethod]
        public void GetByteArrayTest()
        {
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/media/test", Helper.UserId),
                ContentToSend = new StringContent("Hello", Encoding.UTF8, "media/type")
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    using (var data = client.Media.Get("test").Result)
                    {
                        if (server.Error != null) throw server.Error;
                        Assert.AreEqual("media/type", data.MediaType);
                        Assert.AreEqual("Hello", Encoding.UTF8.GetString(data.Buffer));
                    }
                }
            }
        }

        [TestMethod]
        public void GetStreamTest()
        {
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/media/test", Helper.UserId),
                ContentToSend = new StringContent("Hello", Encoding.UTF8, "media/type")
            }))
            {
                using (var client = Helper.CreateClient())
                using (var data = client.Media.Get("test", true).Result)
                {
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual("media/type", data.MediaType);
                    using (var reader = new StreamReader(data.Stream, Encoding.UTF8))
                    {
                        Assert.AreEqual("Hello", reader.ReadToEnd());
                    }
                }
            }
        }

        [TestMethod]
        public void GetAllTest()
        {
            var media = new[]{
                new Media
                {
                    MediaName = "Media1",
                    Content = new Uri("http://localhost/media1")
                },
                new Media
                {
                    MediaName = "Media2",
                    Content = new Uri("http://localhost/media2")
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/media", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(media)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var result = client.Media.GetAll().Result;
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual(2, result.Length);
                    Helper.AssertObjects(media[0], result[0]);
                    Helper.AssertObjects(media[1], result[1]);
                }
            }
        }
        [TestMethod]
        public void RemoveTest()
        {
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "DELETE",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/media/media1", Helper.UserId)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    client.Media.Remove("media1").Wait();
                    if (server.Error != null) throw server.Error;
                }
            }
        }
      
    }
}
