using System;
using System.IO;
using System.Net.Http;
using System.Text;
using Bandwidth.Net.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Model
{
    [TestClass]
    public class MediaTests
    {
        [TestInitialize]
        public void Setup()
        {
            Helper.SetEnvironmetVariables();
        }

        [TestMethod]
        public void ListTest()
        {
            var items = new[]
            {
                new Media
                {
                    MediaName = "file1",
                    Content = "url1",
                    ContentLength = 1024
                },
                new Media
                {
                    MediaName = "file2",
                    Content = "url2",
                    ContentLength = 2048
                }
            }; 
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/media", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(items)
            }))
            {
                var client = Helper.CreateClient();
                var result = Media.List(client).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(items[0], result[0]);
                Helper.AssertObjects(items[1], result[1]);
            }
        }

        [TestMethod]
        public void ListWithDefaultClientTest()
        {
            var items = new[]
            {
                new Media
                {
                    MediaName = "file1",
                    Content = "url1",
                    ContentLength = 1024
                },
                new Media
                {
                    MediaName = "file2",
                    Content = "url2",
                    ContentLength = 2048
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/media", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(items)
            }))
            {
                var result = Media.List().Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(items[0], result[0]);
                Helper.AssertObjects(items[1], result[1]);
            }
        }
        [TestMethod]
        public void List2Test()
        {
            var items = new[]
            {
                new Media
                {
                    MediaName = "file1",
                    Content = "url1",
                    ContentLength = 1024
                },
                new Media
                {
                    MediaName = "file2",
                    Content = "url2",
                    ContentLength = 2048
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/media?page=1&size=25", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(items)
            }))
            {
                var client = Helper.CreateClient();
                var result = Media.List(client, 1).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(items[0], result[0]);
                Helper.AssertObjects(items[1], result[1]);
            }
        }

        [TestMethod]
        public void ListWithDefaultClient2Test()
        {
            var items = new[]
            {
                new Media
                {
                    MediaName = "file1",
                    Content = "url1",
                    ContentLength = 1024
                },
                new Media
                {
                    MediaName = "file2",
                    Content = "url2",
                    ContentLength = 2048
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/media?page=2&size=30", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(items)
            }))
            {
                var result = Media.List(2, 30).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(items[0], result[0]);
                Helper.AssertObjects(items[1], result[1]);
            }
        }

        [TestMethod]
        public void UploadTest()
        {
            const string data = "1234567890";
            var items = new[]
            {
                new Media
                {
                    MediaName = "file1",
                    Content = "url1",
                    ContentLength = 1024
                },
                new Media
                {
                    MediaName = "file2",
                    Content = "url2",
                    ContentLength = 2048
                }
            };
            using (var server = new HttpServer(new []{
                new RequestHandler
                {
                    EstimatedMethod = "PUT",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/media/file1", Helper.UserId),
                    EstimatedContent = data
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/media?page=0&size=5", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(items)
                }
            }))
            {
                var client = Helper.CreateClient();
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
                {
                    var result = Media.Upload(client, "file1", stream).Result;
                    if (server.Error != null) throw server.Error;
                    Helper.AssertObjects(items[0], result);
                }
            }
        }
        [TestMethod]
        public void UploadWithDefaultClientTest()
        {
            const string data = "1234567890";
            var items = new[]
            {
                new Media
                {
                    MediaName = "file1",
                    Content = "url1",
                    ContentLength = 1024
                },
                new Media
                {
                    MediaName = "file2",
                    Content = "url2",
                    ContentLength = 2048
                }
            };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "PUT",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/media/file1", Helper.UserId),
                    EstimatedContent = data
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/media?page=0&size=5", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(items)
                }
            }))
            {
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
                {
                    var result = Media.Upload("file1", stream).Result;
                    if (server.Error != null) throw server.Error;
                    Helper.AssertObjects(items[0], result);
                }
            }
        }
        [TestMethod]
        public void Upload2Test()
        {
            const string data = "1234567890";
            var items = new[]
            {
                new Media
                {
                    MediaName = "file1",
                    Content = "url1",
                    ContentLength = 1024
                },
                new Media
                {
                    MediaName = "file2",
                    Content = "url2",
                    ContentLength = 2048
                }
            };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "PUT",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/media/file1", Helper.UserId),
                    EstimatedContent = data
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/media?page=0&size=5", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(items)
                }
            }))
            {
                var client = Helper.CreateClient();
                var result = Media.Upload(client, "file1", Encoding.UTF8.GetBytes(data)).Result;
                if (server.Error != null) throw server.Error;
                Helper.AssertObjects(items[0], result);
            }
        }
        [TestMethod]
        public void Upload2WithDefaultClientTest()
        {
            const string data = "1234567890";
            var items = new[]
            {
                new Media
                {
                    MediaName = "file1",
                    Content = "url1",
                    ContentLength = 1024
                },
                new Media
                {
                    MediaName = "file2",
                    Content = "url2",
                    ContentLength = 2048
                }
            };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "PUT",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/media/file1", Helper.UserId),
                    EstimatedContent = data
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/media?page=0&size=5", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(items)
                }
            }))
            {
                var result = Media.Upload("file1", Encoding.UTF8.GetBytes(data)).Result;
                if (server.Error != null) throw server.Error;
                Helper.AssertObjects(items[0], result);
            }
        }
        [TestMethod]
        public void DownloadTest()
        {
            const string data = "1234567890";
            using (var server = new HttpServer(
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/media/file1", Helper.UserId),
                    ContentToSend = new StringContent(data)
                }))
            {
                var client = Helper.CreateClient();
                using (var result = Media.Download(client, "file1").Result)
                {
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual(data, Encoding.UTF8.GetString(result.Buffer));
                }
            }
        }

        [TestMethod]
        public void DownloadWithDefaultClientTest()
        {
            const string data = "1234567890";
            using (var server = new HttpServer(
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/media/file1", Helper.UserId),
                    ContentToSend = new StringContent(data)
                }))
            {
                using (var result = Media.Download("file1").Result)
                {
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual(data, Encoding.UTF8.GetString(result.Buffer));
                }
            }
        }
        [TestMethod]
        public void Download2Test()
        {
            const string data = "1234567890";
            using (var server = new HttpServer(
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/media/file1", Helper.UserId),
                    ContentToSend = new StringContent(data)
                }))
            {
                var client = Helper.CreateClient();
                using (var result = Media.Download(client, "file1", true).Result)
                {
                    if (server.Error != null) throw server.Error;
                    using (var reader = new StreamReader(result.Stream, Encoding.UTF8))
                    {
                        Assert.AreEqual(data, reader.ReadToEnd());                        
                    }
                }
            }
        }

        [TestMethod]
        public void Download2WithDefaultClientTest()
        {
            const string data = "1234567890";
            using (var server = new HttpServer(
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/media/file1", Helper.UserId),
                    ContentToSend = new StringContent(data)
                }))
            {
                using (var result = Media.Download("file1", true).Result)
                {
                    if (server.Error != null) throw server.Error;
                    using (var reader = new StreamReader(result.Stream, Encoding.UTF8))
                    {
                        Assert.AreEqual(data, reader.ReadToEnd());
                    }
                }
            }
        }

        [TestMethod]
        public void DeleteTest()
        {
            using (var server = new HttpServer(
                new RequestHandler
                {
                    EstimatedMethod = "DELETE",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/media/file1", Helper.UserId)
                }))
            {
                var client = Helper.CreateClient();
                Media.Delete(client, "file1").Wait();
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void DeleteWithDefaultClientTest()
        {
            using (var server = new HttpServer(
                new RequestHandler
                {
                    EstimatedMethod = "DELETE",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/media/file1", Helper.UserId)
                }))
            {
                Media.Delete("file1").Wait();
                if (server.Error != null) throw server.Error;
            }
        }
    }
}

