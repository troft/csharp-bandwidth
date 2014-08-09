using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bandwidth.Net.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Clients
{
    [TestClass]
    public class AvailableNumbersTests
    {
        [TestMethod]
        public void GetAllTest()
        {
            var numbers = new[]{
                new AvailableNumber
                {
                    Number = "111"
                },
                new AvailableNumber
                {
                    Number = "222"
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = "/v1/availableNumbers/local?size=2",
                ContentToSend = Helper.CreateJsonContent(numbers)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var result = client.AvailableNumbers.GetAll(new AvailableNumberQuery{ Size = 2 }).Result;
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual(2, result.Length);
                    Helper.AssertObjects(numbers[0], result[0]);
                    Helper.AssertObjects(numbers[1], result[1]);
                }
            }
        }
    }
}
