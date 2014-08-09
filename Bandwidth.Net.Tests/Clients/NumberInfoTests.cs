using Bandwidth.Net.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Clients
{
    [TestClass]
    public class NumberInfoTests
    {
        [TestMethod]
        public void GetTest()
        {
            var info = new NumberInfo
            {
                Name = "PhoneNumber",
                Number = "Number"
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = "/v1/phoneNumbers/numberInfo/Number",
                ContentToSend = Helper.CreateJsonContent(info)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var result = client.NumberInfo.Get("Number").Result;
                    if (server.Error != null) throw server.Error;
                    Helper.AssertObjects(info, result);
                }
            }
        }
    }
}
