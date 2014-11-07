using System.Collections.Generic;
using Bandwidth.Net.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Model
{
    [TestClass]
    public class ApplicationTests
    {
        [TestInitialize]
        public void SetUp()
        {
            Helper.SetEnvironmetVariables();
        }

        [TestMethod]
        public void GetTest()
        {
            var application = new Application
            {
                Id = "1",
                Name = "Test Application",
                AutoAnswer = true,
                CallbackHttpMethod = "POST",
                IncomingCallFallbackUrl = "http://testCallFallbackUrl.com",
                IncomingCallUrl = "http://testCallUrl.com",
                IncomingCallUrlCallbackTimeout = 200,
                IncomingSmsFallbackUrl = "http://testSmsFallbackUrl.com",
                IncomingSmsUrl = "http://testSmsUrl.com",
                IncomingSmsUrlCallbackTimeout = 200
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/applications/1", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(application)
            }))
            {
                var client = Helper.CreateClient();
                var result = Application.Get(client, "1").Result;
                if (server.Error != null) throw server.Error;
                Helper.AssertObjects(application, result);
            }
        }

        [TestMethod]
        public void CreateApplicationTest()
        {
            var application = new Dictionary<string, object>
            {
                {"name",  "Test Application"},
                {"autoAnswer", true},
                {"callbackHttpMethod", "POST"},
                {"incomingCallUrl", "http://testCallUrl.com"},
                {"incomingCallUrlCallbackTimeout", 200},
                {"incomingSmsUrl", "http://testSmsUrl.com"},
                {"incomingSmsUrlCallbackTimeout", 200},
            };

            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/applications", Helper.UserId),
                    EstimatedContent = Helper.ToJsonString(application),
                    HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/applications/1", Helper.UserId) } }
                },
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/applications/1", Helper.UserId),
                    
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}, 
                        {"autoAnswer", true},
                        {"callbackHttpMethod", "POST"},
                        {"incomingCallUrl", "http://testCallUrl.com"},
                        {"incomingCallUrlCallbackTimeout", 200},
                        {"incomingSmsUrl", "http://testSmsUrl.com"},
                        {"incomingSmsUrlCallbackTimeout", 200},
                    })
                }
            }))
            {
                var client = Helper.CreateClient();
                var app = Application.Create(client, application).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual("1", app.Id);
                Assert.AreEqual(true, app.AutoAnswer);
                Assert.AreEqual("POST", app.CallbackHttpMethod);
                Assert.AreEqual("http://testCallUrl.com", app.IncomingCallUrl);
                Assert.AreEqual(200, app.IncomingCallUrlCallbackTimeout);
                Assert.AreEqual("http://testSmsUrl.com", app.IncomingSmsUrl);
                Assert.AreEqual(200, app.IncomingSmsUrlCallbackTimeout);
            }
        }

        [TestMethod]
        public void UpdateApplicationTest()
        {
            var data = new Dictionary<string, object>
            {
                {"incomingSmsUrl", "http://testSmsUrl1.com"}
            };
            using (var server = new HttpServer(new[]
            {
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/applications/1", Helper.UserId),
                    
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}, 
                        {"autoAnswer", true},
                        {"callbackHttpMethod", "POST"},
                        {"incomingCallUrl", "http://testCallUrl.com"},
                        {"incomingCallUrlCallbackTimeout", 200},
                        {"incomingSmsUrl", "http://testSmsUrl.com"},
                        {"incomingSmsUrlCallbackTimeout", 200},
                    })
                },
                new RequestHandler
                    {
                        EstimatedMethod = "POST",
                        EstimatedPathAndQuery = string.Format("/v1/users/{0}/applications/1", Helper.UserId),
                        EstimatedContent = Helper.ToJsonString(data)
                    },
            }))
            {
                var client = Helper.CreateClient();
                var application = Application.Get(client, "1").Result;
                application.Update(data).Wait();
                if (server.Error != null) throw server.Error;
            }
        }
        [TestMethod]
        public void DeleteApplicationTest()
        {
            using (var server = new HttpServer(new[]
            {
                new RequestHandler
                {
                    EstimatedMethod = "GET",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/applications/1", Helper.UserId),
                    
                    ContentToSend = Helper.CreateJsonContent(new Dictionary<string, object>{{"id", "1"}, 
                        {"autoAnswer", true},
                        {"callbackHttpMethod", "POST"},
                        {"incomingCallUrl", "http://testCallUrl.com"},
                        {"incomingCallUrlCallbackTimeout", 200},
                        {"incomingSmsUrl", "http://testSmsUrl.com"},
                        {"incomingSmsUrlCallbackTimeout", 200},
                    })
                },
                new RequestHandler
                    {
                            EstimatedMethod = "DELETE",
                            EstimatedPathAndQuery = string.Format("/v1/users/{0}/applications/1", Helper.UserId),
                    },
            }))
            {
                var client = Helper.CreateClient();
                var application = Application.Get(client, "1").Result;
                application.Delete().Wait();
                if (server.Error != null) throw server.Error;
            }
        }

    }
}
