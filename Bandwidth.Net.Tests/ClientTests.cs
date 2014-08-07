using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Fakes;
using System.Threading.Tasks;
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
                    Assert.AreEqual("/test?test1=value1&test2=value2", url);
                    return Task.Run(()=> new HttpResponseMessage(HttpStatusCode.OK));
                };
                using (var client = Fake.CreateClient())
                {
                    var response = client.MakeGetRequest("/test", 
                        new Dictionary<string, string> { { "test1", "value1" }, { "test2", "value2" } }).Result;
                    Assert.IsTrue(response.IsSuccessStatusCode);
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
                    Assert.AreEqual("/test/id?test1=value1&test2=value2", url);
                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.OK));
                };
                using (var client = Fake.CreateClient())
                {
                    var response = client.MakeGetRequest("/test",
                        new Dictionary<string, string> { { "test1", "value1" }, { "test2", "value2" } }, "id").Result;
                    Assert.IsTrue(response.IsSuccessStatusCode);
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
                    Assert.AreEqual("/test", url);
                    var json = content.ReadAsStringAsync().Result;
                    Assert.AreEqual("{\"test\":true}", json);
                    Assert.AreEqual("application/json", content.Headers.ContentType.MediaType);
                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.OK));
                };
                using (var client = Fake.CreateClient())
                {
                    var response = client.MakePostRequest("/test", new {Test = true}).Result;
                    Assert.IsTrue(response.IsSuccessStatusCode);
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
                    Assert.AreEqual("/test", url);
                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.OK));
                };
                using (var client = Fake.CreateClient())
                {
                    var response = client.MakeDeleteRequest("/test").Result;
                    Assert.IsTrue(response.IsSuccessStatusCode);
                }
            }

        }

    }
}
