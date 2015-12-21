using System.Collections.Generic;
using Bandwidth.Net.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Model
{
    [TestClass]
    public class EndPointTests
    {
 
        [TestMethod]
        public void DeleteTest()
        {
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "DELETE",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/domains/1/endpoints/10", Helper.UserId)
                }
            }))
            {
                var domain = new EndPoint { Id = "10", DomainId = "1", Client = Helper.CreateClient() };
                domain.Delete().Wait();
                if (server.Error != null) throw server.Error;
            }
        }

        [TestMethod]
        public void CreateAuthTokenTest()
        {
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "POST",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/domains/1/endpoints/10/tokens", Helper.UserId),
                    ContentToSend = Helper.CreateJsonContent(new EndPointTokenData{Expires = 100, Token = "123"}),
                    StatusCodeToSend = 201
                }
            }))
            {
                var domain = new EndPoint { Id = "10", DomainId = "1", Client = Helper.CreateClient() };
                var r = domain.CreateAuthToken().Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(100, r.Expires);
                Assert.AreEqual("123", r.Token);
            }
        }

        [TestMethod]
        public void DeleteAuthTokenTest()
        {
            using (var server = new HttpServer(new[]{
                new RequestHandler
                {
                    EstimatedMethod = "DELETE",
                    EstimatedPathAndQuery = string.Format("/v1/users/{0}/domains/1/endpoints/10/tokens/123", Helper.UserId)
                }
            }))
            {
                var domain = new EndPoint { Id = "10", DomainId = "1", Client = Helper.CreateClient() };
                domain.DeleteAuthToken("123").Wait();
                if (server.Error != null) throw server.Error;
            }
        }

    }
}

