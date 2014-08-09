using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Fakes;
using System.Threading.Tasks;
using Bandwidth.Net.Data;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Clients
{
    [TestClass]
    public class ApplicationsTests
    {
        [TestMethod]
        public void CreateTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (c, url, content) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/applications", Helper.UserId), url);
                    var application = Helper.ParseJsonContent<Application>(content).Result;
                    Assert.AreEqual("Name", application.Name);
                    Assert.AreEqual("http://localhost/", application.IncomingCallUrl.ToString());
                    var response = new HttpResponseMessage(HttpStatusCode.Created);
                    response.Headers.Add("Location", string.Format("/v1/users/{0}/applications/1", Helper.UserId));
                    return Task.Run(() => response);
                };
                using (var client = Helper.CreateClient())
                {
                    var id = client.Applications.Create(new Application
                    {
                        Name = "Name",
                        IncomingCallUrl = new Uri("http://localhost/")
                    }).Result;
                    Assert.AreEqual("1", id);
                }
            }
        }

        [TestMethod]
        public void UpdateTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (c, url, content) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/applications/1", Helper.UserId), url);
                    var application = Helper.ParseJsonContent<Application>(content).Result;
                    Assert.AreEqual("Name", application.Name);
                    Assert.AreEqual("http://localhost/", application.IncomingCallUrl.ToString());
                    var response = new HttpResponseMessage(HttpStatusCode.Created);
                    response.Headers.Add("Location", string.Format("/v1/users/{0}/applications/1", Helper.UserId));
                    return Task.Run(() => response);
                };
                using (var client = Helper.CreateClient())
                {
                    client.Applications.Update("1", new Application
                    {
                        Name = "Name",
                        IncomingCallUrl = new Uri("http://localhost/")
                    }).Wait();
                }
            }
        }

        [TestMethod]
        public void GetTest()
        {
            using (ShimsContext.Create())
            {
                var application = new Application
                {
                    Id = "1",
                    Name = "Name"
                };
                ShimHttpClient.AllInstances.GetAsyncString = (c, url) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/applications/1", Helper.UserId), url);
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = Helper.CreateJsonContent(application)
                    };
                    return Task.Run(() => response);
                };
                using (var client = Helper.CreateClient())
                {
                    var result = client.Applications.Get("1").Result;
                    Helper.AssertObjects(application, result);
                }
            }
        }

        [TestMethod]
        public void GetAllTest()
        {
            using (ShimsContext.Create())
            {
                var applications = new[]
                {
                    new Application
                    {
                        Id = "1",
                        Name = "Application1"
                    },
                    new Application
                    {
                        Id = "1",
                        Name = "Application2"
                    }
                };
                ShimHttpClient.AllInstances.GetAsyncString = (c, url) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/applications", Helper.UserId), url);
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = Helper.CreateJsonContent(applications)
                    };
                    return Task.Run(() => response);
                };
                using (var client = Helper.CreateClient())
                {
                    var result = client.Applications.GetAll().Result;
                    Assert.AreEqual(2, result.Length);
                    Helper.AssertObjects(applications[0], result[0]);
                    Helper.AssertObjects(applications[1], result[1]);
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
                    Assert.AreEqual(string.Format("users/{0}/applications/1", Helper.UserId), url);
                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.OK));
                };
                using (var client = Helper.CreateClient())
                {
                    client.Applications.Remove("1").Wait();
                }
            }
        }
    }
}
