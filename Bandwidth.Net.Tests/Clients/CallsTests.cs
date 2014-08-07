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
    public class CallsTests
    {
        
        
        [TestMethod]
        public void CreateTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (c, url, content) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/calls", Fake.UserId), url);
                    var call = Fake.ParseJsonContent<Call>(content).Result;
                    Assert.AreEqual("From", call.From);
                    Assert.AreEqual("To", call.To);
                    var response = new HttpResponseMessage(HttpStatusCode.Created);
                    response.Headers.Add("Location", string.Format("/v1/users/{0}/calls/1", Fake.UserId));
                    return Task.Run(() => response);
                };
                using (var client = Fake.CreateClient())
                {
                    var id = client.Calls.Create(new Call
                    {
                        From = "From",
                        To = "To"
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
                    Assert.AreEqual(string.Format("users/{0}/calls/1", Fake.UserId), url);
                    var call = Fake.ParseJsonContent<Call>(content).Result;
                    Assert.AreEqual(CallState.Transferring, call.State);
                    Assert.AreEqual("Number", call.TransferTo);
                    Assert.AreEqual("http://localhost/", call.CallbackUrl.ToString());
                    var response = new HttpResponseMessage(HttpStatusCode.Created);
                    response.Headers.Add("Location", string.Format("/v1/users/{0}/calls/1", Fake.UserId));
                    return Task.Run(() => response);
                };
                using (var client = Fake.CreateClient())
                {
                    client.Calls.Update("1", new Call
                    {
                        State = CallState.Transferring,
                        TransferTo = "Number",
                        CallbackUrl = new Uri("http://localhost/")
                    }).Wait();
                }
            }
        }
    }
}
