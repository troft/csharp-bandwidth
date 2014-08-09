using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Fakes;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Bandwidth.Net.Data;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Clients
{
    [TestClass]
    public class MediaTests
    {
        [TestMethod]
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
        }

        [TestMethod]
        public void GetAllTest()
        {
            using (ShimsContext.Create())
            {
                var items = new[]
                {
                    new Media
                    {
                        MediaName = "test",
                        Content = new Uri("http://localhost/test"),
                        ContentLength = 100
                    },
                    new Media
                    {
                        MediaName = "test2",
                        Content = new Uri("http://localhost/test2"),
                        ContentLength = 200
                    }
                };
                ShimHttpClient.AllInstances.GetAsyncString = (c, url) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/media", Helper.UserId), url);
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = Helper.CreateJsonContent(items)
                    };
                    return Task.Run(() => response);
                };
                using (var client = Helper.CreateClient())
                {
                    var result = client.Media.GetAll().Result;
                    Assert.AreEqual(2, result.Length);
                    Helper.AssertObjects(items[0], result[0]);
                    Helper.AssertObjects(items[1], result[1]);
                }
            }
        }
        [TestMethod]
        public void RemoveTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.DeleteAsyncString = (c, url) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/media/test", Helper.UserId), url);
                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.OK));
                };
                using (var client = Helper.CreateClient())
                {
                    client.Media.Remove("test").Wait();
                }
            }
        }
      
    }
}
