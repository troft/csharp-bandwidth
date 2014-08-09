using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Bandwidth.Net.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests
{
    [TestClass]
    public class ClientTests
    {
        [TestMethod]
        public void MakeGetRequestTest()
        {
            using (var server = new HttpServer(new RequestHandler { EstimatedMethod = "GET", EstimatedPathAndQuery = "/v1/test?test1=value1&test2=value2" }))
            {
                using (var client = Helper.CreateClient())
                {
                    client.MakeGetRequest("test",
                             new Dictionary<string, string> { { "test1", "value1" }, { "test2", "value2" } }, null, true)
                             .Wait();
                    if (server.Error != null) throw server.Error;
                }
            }
            
        }

        [TestMethod]
        public void MakeGetRequestWithIdTest()
        {
            using (var server = new HttpServer(new RequestHandler { EstimatedMethod = "GET", EstimatedPathAndQuery = "/v1/test/id?test1=value1&test2=value2" }))
            {
                using (var client = Helper.CreateClient())
                {
                    client.MakeGetRequest("test",
                             new Dictionary<string, string> { { "test1", "value1" }, { "test2", "value2" } }, "id", true)
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
                using (var client = Helper.CreateClient())
                {
                    var result = client.MakeGetRequest<TestItem>("test",
                        new Dictionary<string, string> { { "test1", "value1" }, { "test2", "value2" } }).Result;
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
                using (var client = Helper.CreateClient())
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
                using (var client = Helper.CreateClient())
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
                using (var client = Helper.CreateClient())
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
                EstimatedMethod = "POST",
                EstimatedHeaders = new Dictionary<string, string> { { "Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", Helper.ApiKey, Helper.Secret)))} },
                HeadersToSend = new Dictionary<string, string> { { "Location", "http://localhost/calls/1"} }
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    client.Calls.Create(new Call
                    {
                        From = "From",
                        To = "To"
                    }).Wait();
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
                using (var client = Helper.CreateClient())
                {
                    using (var stream = new MemoryStream(data))
                    {
                        client.PutData("test", stream, "media/type", true).Wait();
                        if (server.Error != null) throw server.Error;
                    }
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
                using (var client = Helper.CreateClient())
                {
                    client.PutData("test", data, "media/type", true).Wait();
                    if (server.Error != null) throw server.Error;
                }
            }

        }
    }
}
