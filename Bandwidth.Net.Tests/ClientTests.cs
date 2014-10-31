using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Bandwidth.Net.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests
{
    [TestClass]
    public class ClientTests
    {
        [TestMethod]
        public void GetInstanceTest()
        {
            Environment.SetEnvironmentVariable(Client.BandwidthUserId, "UserId");
            Environment.SetEnvironmentVariable(Client.BandwidthApiToken, "Token");
            Environment.SetEnvironmentVariable(Client.BandwidthApiSecret, "Secret");
            Environment.SetEnvironmentVariable(Client.BandwidthApiEndpoint, "EndPoint");
            Environment.SetEnvironmentVariable(Client.BandwidthApiVersion, "Version");
            Client.GetInstance();
            Client.GetInstance("userId", "token", "secret", "endpoint", "version");
            Client.GetInstance("userId", "token", "secret");
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void GetInstanceTest2()
        {
            Environment.SetEnvironmentVariable(Client.BandwidthUserId, null);
            Environment.SetEnvironmentVariable(Client.BandwidthApiToken, null);
            Environment.SetEnvironmentVariable(Client.BandwidthApiSecret, null);
            Environment.SetEnvironmentVariable(Client.BandwidthApiEndpoint, null);
            Environment.SetEnvironmentVariable(Client.BandwidthApiVersion, null);
            Client.GetInstance();
        }
        
        [TestMethod]
        public void MakeGetRequestTest()
        {
            using (var server = new HttpServer(new RequestHandler { EstimatedMethod = "GET", EstimatedPathAndQuery = "/v1/test?test1=value1&test2=value2" }))
            {
                var client = Helper.CreateClient();
                client.MakeGetRequest("test",
                             new Dictionary<string, object> { { "test1", "value1" }, { "test2", "value2" } }, null, true)
                             .Wait();
                if (server.Error != null) throw server.Error;
            }
            
        }

        [TestMethod]
        public void MakeGetRequestToObjectTest()
        {
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = "/v1/test?test1=value1&test2=value2",
                ContentToSend = Helper.CreateJsonContent(new TestItem
                {
                    Name = "Name1",
                    Flag = true
                })
            }))
            {
                var client = Helper.CreateClient();
                var item = new TestItem
                {
                    Name = "Name",
                    Flag = false
                };
                client.MakeGetRequestToObject(item, "test",
                             new Dictionary<string, object> { { "test1", "value1" }, { "test2", "value2" } })
                             .Wait();
                if (server.Error != null) throw server.Error;
                Assert.AreEqual("Name1", item.Name);
                Assert.AreEqual(true, item.Flag);
            }
        }

        [TestMethod]
        public void MakeGetRequestWithIdTest()
        {
            using (var server = new HttpServer(new RequestHandler { EstimatedMethod = "GET", EstimatedPathAndQuery = "/v1/test/id?test1=value1&test2=value2" }))
            {
                var client = Helper.CreateClient();
                {
                    client.MakeGetRequest("test",
                             new Dictionary<string, object> { { "test1", "value1" }, { "test2", "value2" } }, "id", true)
                             .Wait();
                    if (server.Error != null) throw server.Error;
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
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET", 
                EstimatedPathAndQuery = "/v1/test?test1=value1&test2=value2",
                ContentToSend =  Helper.CreateJsonContent(new TestItem
                {
                    Name = "Name",
                    Flag = true
                })
            }))
            {
                var client = Helper.CreateClient();
                {
                    var result = client.MakeGetRequest<TestItem>("test",
                        new Dictionary<string, object> { { "test1", "value1" }, { "test2", "value2" } }).Result;
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual("Name", result.Name);
                    Assert.IsTrue(result.Flag != null && result.Flag.Value);
                }
            }
        }

        [TestMethod]
        public void MakePostRequestTest()
        {
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "POST",
                EstimatedPathAndQuery = "/v1/test",
                EstimatedContent = "{\"test\":true}"
            }))
            {
                var client = Helper.CreateClient();
                {
                    client.MakePostRequest("test", new { Test = true }, true).Wait();
                    if (server.Error != null) throw server.Error;
                }
            }
        }

        [TestMethod]
        public void MakePostRequestTTest()
        {
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "POST",
                EstimatedPathAndQuery = "/v1/test",
                EstimatedContent = "{\"test\":true}",
                ContentToSend = Helper.CreateJsonContent(new TestItem
                {
                    Name = "Name",
                    Flag = true
                })
            }))
            {
                var client = Helper.CreateClient();
                {
                    var result = client.MakePostRequest<TestItem>("test", new { Test = true }).Result;
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual("Name", result.Name);
                    Assert.IsTrue(result.Flag != null && result.Flag.Value);
                }
            }

        }

        [TestMethod]
        public void MakeDeleteRequestTest()
        {
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "DELETE",
                EstimatedPathAndQuery = "/v1/test"
            }))
            {
                var client = Helper.CreateClient();
                {
                    client.MakeDeleteRequest("test").Wait();
                    if (server.Error != null) throw server.Error;
                }
            }
        }

        [TestMethod]
        public void AuthHeaderTest()
        {
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedHeaders = new Dictionary<string, string> { { "Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", Helper.ApiKey, Helper.Secret)))} }
            }))
            {
                var client = Helper.CreateClient();
                {
                    Call.List(client).Wait();
                    if (server.Error != null) throw server.Error;
                }
            }
        }
        [TestMethod]
        public void PutDataWithStreamTest()
        {
            var data = Encoding.UTF8.GetBytes("hello");
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "PUT",
                EstimatedPathAndQuery = "/v1/test",
                EstimatedContent = "hello",
                EstimatedHeaders = new Dictionary<string, string> {{"Content-Type", "media/type"}}
            }))
            {
                var client = Helper.CreateClient();
                using (var stream = new MemoryStream(data))
                {
                    client.PutData("test", stream, "media/type", true).Wait();
                    if (server.Error != null) throw server.Error;
                }
            }

        }
        [TestMethod]
        public void PutDataWithByteArrayTest()
        {
            var data = Encoding.UTF8.GetBytes("hello");
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "PUT",
                EstimatedPathAndQuery = "/v1/test",
                EstimatedContent = "hello",
                EstimatedHeaders = new Dictionary<string, string> { { "Content-Type", "media/type" } }
            }))
            {
                var client = Helper.CreateClient();
                client.PutData("test", data, "media/type", true).Wait();
                if (server.Error != null) throw server.Error;
            }

        }
    }
}
