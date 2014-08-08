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
    public class AvailableNumbersTests
    {
        [TestMethod]
        public void GetAllTest()
        {
            using (ShimsContext.Create())
            {
                var recordings = new[]
                {
                    new AvailableNumber
                    {
                        Number = "111"
                    },
                    new AvailableNumber
                    {
                        Number = "222"
                    }
                };
                ShimHttpClient.AllInstances.GetAsyncString = (c, url) =>
                {
                    Assert.AreEqual("availableNumbers?quantity=2" , url);
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = Fake.CreateJsonContent(recordings)
                    };
                    return Task.Run(() => response);
                };
                using (var client = Fake.CreateClient())
                {
                    var result = client.AvailableNumbers.GetAll(new AvailableNumberQuery{Quantity = 2}).Result;
                    Assert.AreEqual(2, result.Length);
                    Fake.AssertObjects(recordings[0], result[0]);
                    Fake.AssertObjects(recordings[1], result[1]);
                }
            }
        }
    }
}
