using System;
using Bandwidth.Net.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Clients
{
    [TestClass]
    public class MediaTests
    {
        /*[TestMethod]
        public void SetWithByteArrayTest()
        {
            using (ShimsContext.Create())
            {
                var data = new byte[] { 1, 2, 3, 4 };
                ShimHttpClient.AllInstances.PutAsyncStringHttpContent = (c, url, content) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/media/test", Helper.UserId), url);
                    var buffer = content.ReadAsByteArrayAsync().Result;
                    Assert.AreEqual(BitConverter.ToString(data), BitConverter.ToString(buffer));
                    Assert.AreEqual("media/type", content.Headers.ContentType.MediaType);
                    Assert.AreEqual(data.Length, content.Headers.ContentLength);
                    var response = new HttpResponseMessage(HttpStatusCode.OK);
                    return Task.Run(() => response);
                };
                using (var client = Helper.CreateClient())
                {
                    client.Media.Set("test", data, "media/type").Wait();
                }
            }
        }

        [TestMethod]
        public void SetWithStreamTest()
        {
            using (ShimsContext.Create())
            {
                var data = new byte[] { 1, 2, 3, 4 };
                ShimHttpClient.AllInstances.PutAsyncStringHttpContent = (c, url, content) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/media/test", Helper.UserId), url);
                    var buffer = content.ReadAsByteArrayAsync().Result;
                    Assert.AreEqual(BitConverter.ToString(data), BitConverter.ToString(buffer));
                    Assert.AreEqual("media/type", content.Headers.ContentType.MediaType);
                    Assert.AreEqual(data.Length, content.Headers.ContentLength);
                    var response = new HttpResponseMessage(HttpStatusCode.OK);
                    return Task.Run(() => response);
                };
                using (var client = Helper.CreateClient())
                using (var stream = new MemoryStream(data))
                {
                    client.Media.Set("test", stream, "media/type").Wait();
                }
            }
        }

        
        [TestMethod]
        public void GetTest()
        {
            using (ShimsContext.Create())
            {
                var data = new byte[] { 1, 2, 3, 4 };
                ShimHttpClient.AllInstances.GetAsyncString = (c, url) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/media/test", Helper.UserId), url);
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(data)
                    };
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("media/type");
                    return Task.Run(() => response);
                };
                using (var client = Helper.CreateClient())
                {
                    using (var result = client.Media.Get("test").Result)
                    {
                        Assert.AreEqual("media/type", result.MediaType);
                        Assert.AreEqual(BitConverter.ToString(data), BitConverter.ToString(result.Buffer));
                    }
                    using (var result = client.Media.Get("test", true).Result)
                    {
                        Assert.AreEqual("media/type", result.MediaType);
                        var buffer = new byte[data.Length];
                        result.Stream.Read(buffer, 0, buffer.Length);
                        Assert.AreEqual(BitConverter.ToString(data), BitConverter.ToString(buffer));
                    }
                }
            }
        }*/

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
