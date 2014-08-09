using Bandwidth.Net.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Clients
{
    [TestClass]
    public class ErrorsTests
    {
        [TestMethod]
        public void GetTest()
        {
            var error = new Error
            {
                Id = "1",
                Message = "Message"
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/errors/1", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(error)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var result = client.Errors.Get("1").Result;
                    if (server.Error != null) throw server.Error;
                    Helper.AssertObjects(error, result);
                }
            }
        }

        [TestMethod]
        public void GetAllTest()
        {
            var errors = new[]{
                new Error
                {
                    Id = "1",
                    Message = "Message"
                },
                new Error
                {
                    Id = "2",
                    Message = "Message2"
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/errors", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(errors)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var result = client.Errors.GetAll().Result;
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual(2, result.Length);
                    Helper.AssertObjects(errors[0], result[0]);
                    Helper.AssertObjects(errors[1], result[1]);
                }
            }
        }
        
    }
}
