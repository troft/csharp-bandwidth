using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Fakes;
using System.Text;
using System.Threading.Tasks;
using Bandwidth.Net.Data;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests
{
    [TestClass]
    public class ClientTests
    {
        [TestMethod]
        public void MakeGetRequestTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.GetAsyncString = (c, url) =>
                {
                    Assert.AreEqual("test?test1=value1&test2=value2", url);
                    return Task.Run(()=> new HttpResponseMessage(HttpStatusCode.OK));
                };
                using (var client = Fake.CreateClient())
                {
                    client.MakeGetRequest("test", 
                        new Dictionary<string, string> { { "test1", "value1" }, { "test2", "value2" } }, null, true).Wait();
                }
            }
            
        }

        [TestMethod]
        public void MakeGetRequestWithIdTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.GetAsyncString = (c, url) =>
                {
                    Assert.AreEqual("test/id?test1=value1&test2=value2", url);
                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.OK));
                };
                using (var client = Fake.CreateClient())
                {
                    client.MakeGetRequest("test",
                        new Dictionary<string, string> { { "test1", "value1" }, { "test2", "value2" } }, "id", true).Wait();
                }
            }

        }

        public class TestItem
        {
            public string Name { get; set; }
            public bool? Flag { get; set; }
        }
        [TestMethod]
        public void MakeGetRequestTTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.GetAsyncString = (c, url) =>
                {
                    Assert.AreEqual("test?test1=value1&test2=value2", url);
                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.OK){Content = Fake.CreateJsonContent(new TestItem
                    {
                        Name = "Name",
                        Flag = true
                    })});
                };
                using (var client = Fake.CreateClient())
                {
                    var result = client.MakeGetRequest<TestItem>("test",
                        new Dictionary<string, string> { { "test1", "value1" }, { "test2", "value2" } }).Result;
                    Assert.AreEqual("Name", result.Name);
                    Assert.IsTrue(result.Flag != null && result.Flag.Value);
                }
            }

        }

        [TestMethod]
        public void MakePostRequestTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (c, url, content) =>
                {
                    Assert.AreEqual("test", url);
                    var json = content.ReadAsStringAsync().Result;
                    Assert.AreEqual("{\"test\":true}", json);
                    Assert.AreEqual("application/json", content.Headers.ContentType.MediaType);
                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.OK){Content = Fake.CreateJsonContent(new TestItem
                    {
                        Name = "Name",
                        Flag = true
                    })});
                };
                using (var client = Fake.CreateClient())
                {
                    var result = client.MakePostRequest<TestItem>("test", new {Test = true}).Result;
                    Assert.AreEqual("Name", result.Name);
                    Assert.IsTrue(result.Flag != null && result.Flag.Value);
                }
            }

        }

        [TestMethod]
        public void MakePostRequestTTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (c, url, content) =>
                {
                    Assert.AreEqual("test", url);
                    var json = content.ReadAsStringAsync().Result;
                    Assert.AreEqual("{\"test\":true}", json);
                    Assert.AreEqual("application/json", content.Headers.ContentType.MediaType);
                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.OK));
                };
                using (var client = Fake.CreateClient())
                {
                    client.MakePostRequest("test", new { Test = true }, true).Wait();
                }
            }

        }

        [TestMethod]
        public void MakeDeleteRequestTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.DeleteAsyncString = (c, url) =>
                {
                    Assert.AreEqual("test", url);
                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.OK));
                };
                using (var client = Fake.CreateClient())
                {
                    client.MakeDeleteRequest("test").Wait();
                }
            }

        }

        [TestMethod]
        public void AuthHeaderTest()
        {
            using (ShimsContext.Create())
            {
                var called = false;
                ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (c, url, content) =>
                {
                    Assert.AreEqual("Basic", c.DefaultRequestHeaders.Authorization.Scheme);
                    Assert.AreEqual(string.Format("{0}:{1}", Fake.ApiKey, Fake.Secret), Encoding.UTF8.GetString(Convert.FromBase64String(c.DefaultRequestHeaders.Authorization.Parameter)));
                    called = true;
                    var response = new HttpResponseMessage(HttpStatusCode.Created);
                    response.Headers.Add("Location", string.Format("/v1/users/{0}/calls/1", Fake.UserId));
                    return Task.Run(() => response);
                };
                using (var client = Fake.CreateClient())
                {
                    client.Calls.Create(new Call
                    {
                        From = "From",
                        To = "To"
                    }).Wait();
                    Assert.IsTrue(called);
                }
            }
        }

    }
}
