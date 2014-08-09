using System;
using System.Collections.Generic;
using Bandwidth.Net.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Clients
{
    [TestClass]
    public class ApplicationsTests
    {
        [TestMethod]
        public void CreateTest()
        {
            var application = new Application
            {
                Name = "Application",
                IncomingCallUrl = new Uri("http://localhost/")
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "POST",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/applications", Helper.UserId),
                EstimatedContent = Helper.ToJsonString(application),
                HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/applications/1", Helper.UserId)} }
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var id = client.Applications.Create(application).Result;
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual("1", id);
                }
            }
        }

        [TestMethod]
        public void UpdateTest()
        {
            var application = new Application
            {
                Name = "Application",
                IncomingCallUrl = new Uri("http://localhost/")
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "POST",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/applications/1", Helper.UserId),
                EstimatedContent = Helper.ToJsonString(application),
                HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/applications/1", Helper.UserId) } }
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    client.Applications.Update("1", application).Wait();
                    if (server.Error != null) throw server.Error;
                }
            }
        }

        [TestMethod]
        public void GetTest()
        {
            var application = new Application
            {
                Id = "1",
                Name = "Application",
                IncomingCallUrl = new Uri("http://localhost/")
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/applications/1", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(application)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var result = client.Applications.Get("1").Result;
                    if (server.Error != null) throw server.Error;
                    Helper.AssertObjects(application, result);
                }
            }
        }

        [TestMethod]
        public void GetAllTest()
        {
            var applications = new[]{
                new Application
                {
                    Id = "1",
                    Name = "Application",
                    IncomingCallUrl = new Uri("http://localhost/")
                },
                new Application
                {
                    Id = "2",
                    Name = "Application2",
                    IncomingCallUrl = new Uri("http://localhost/2")
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/applications", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(applications)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var result = client.Applications.GetAll().Result;
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual(2, result.Length);
                    Helper.AssertObjects(applications[0], result[0]);
                    Helper.AssertObjects(applications[1], result[1]);
                }
            }
        }
        [TestMethod]
        public void RemoveTest()
        {
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "DELETE",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/applications/1", Helper.UserId)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    client.Applications.Remove("1").Wait();
                    if (server.Error != null) throw server.Error;
                }
            }
        }
    }
}
