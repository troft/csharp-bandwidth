using System.Collections.Generic;
using Bandwidth.Net.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Model
{
    [TestClass]
    public class DomainTests
    {
        [TestInitialize]
        public void Setup()
        {
            Helper.SetEnvironmetVariables();
        }

        [TestMethod]
        public void ListTest()
        {
            var domains = new[]{
                new Domain
                {
                    Id = "1",
                    Name = "Domain1"
                },
                new Domain
                {
                    Id = "2",
                    Name = "Domain2"
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/domains", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(domains)
            }))
            {
                var client = Helper.CreateClient();
                var result = Domain.List(client).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(domains[0], result[0]);
                Helper.AssertObjects(domains[1], result[1]);
            }
        }

        [TestMethod]
        public void ListWithDefaultClientTest()
        {
            var domains = new[]{
                new Domain
                {
                    Id = "1",
                    Name = "Domain1"
                },
                new Domain
                {
                    Id = "2",
                    Name = "Domain2"
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/domains", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(domains)
            }))
            {
                var result = Domain.List().Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(domains[0], result[0]);
                Helper.AssertObjects(domains[1], result[1]);
            }
        }


        [TestMethod]
        public void CreateTest()
        {
            var domain = new Dictionary<string, object>
            {
                {"name", "domain1"},
                {"description", "test domain"}
            };
            
            using (var server = new HttpServer(new []{
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/domains", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(domain),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/domains/1", Helper.UserId) } }
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/domains/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                }
            }))
            {
                var client = Helper.CreateClient();
                var d = Domain.Create(client, domain).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual("1", d.Id);
            }
        }

        [TestMethod]
        public void CreateCallWithDefaultClientTest()
        {
            var domain = new Dictionary<string, object>
            {
                {"name", "domain1"},
                {"description", "test domain"}
            };

            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/domains", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(domain),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/domains/1", Helper.UserId) } }
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/domains/1", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}})
                }
            }))
            {
                var d = Domain.Create(domain).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual("1", d.Id);
            }
        }


        [TestMethod]
        public void DeleteTest()
        {
            using (var server = new HttpServer(new []{
                new RequestHandler
                {
                    EstimatedMethod = "DELETE",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/domains/1", Helper.UserId)
                }
            }))
            {
                var domain = new Domain {Id = "1", Client = Helper.CreateClient()};
                domain.Delete().Wait();
                if (server.Error != null) throw server.Error;
            }
        }


        [TestMethod]
        public void CreateEndPointTest()
        {
            var data = new Dictionary<string, object>
            {
                {"name", "point1"},
                {"applicationId", "id"}
            };
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/domains/1/endpoints", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(data),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/domains/1/endpoints/10", Helper.UserId) } }
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/domains/1/endpoints/10", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "10"}})
                }
            }))
            {
                var domain =  new Domain{Id = "1", Client = Helper.CreateClient()};
                domain.CreateEndPoint(data).Wait();
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void GetEndPointTest()
        {
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/domains/1/endpoints/10", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "10"}})
                }
            }))
            {
                var domain = new Domain{Id = "1", Client = Helper.CreateClient()};
                var endPoint = domain.GetEndPoint("10").Result;
                Assert.AreEqual("10", endPoint.Id);
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void GetEndPointsTest()
        {
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/domains/1/endpoints", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new[]
                    {
                        new Dictionary<string, object>{{"id", "10"}},
                        new Dictionary<string, object>{{"id", "11"}}
                    })
                }
            }))
            {
                var domain = new Domain { Id = "1", Client = Helper.CreateClient() };
                var endPoints = domain.GetEndPoints().Result;
                Assert.AreEqual(2, endPoints.Length);
                Assert.AreEqual("10", endPoints[0].Id);
                Assert.AreEqual("11", endPoints[1].Id);
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void DeleteEndPointTest()
        {
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "DELETE",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/domains/1/endpoints/10", Helper.UserId)
                }
            }))
            {
                var domain = new Domain { Id = "1", Client = Helper.CreateClient() };
                domain.DeleteEndPoint("10").Wait();
                if (server.Error != null) throw server.Error;
            }
        }

    }
}

