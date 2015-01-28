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

    }
}

