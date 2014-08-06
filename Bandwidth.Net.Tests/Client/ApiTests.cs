using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Fakes;
using System.Text;
using System.Threading.Tasks;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Bandwidth.Net.Tests.Client
{
    [TestClass]
    public class ApiTests
    {
        [TestMethod]
        public void AuthHeaderTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (c, url, content) =>
                {
                    Assert.AreEqual("Basic", c.DefaultRequestHeaders.Authorization.Scheme);
                    Assert.AreEqual(string.Format("{0}:{1}", Fake.ApiKey, Fake.Secret), Encoding.UTF8.GetString(Convert.FromBase64String(c.DefaultRequestHeaders.Authorization.Parameter)));
                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.Created));
                };
                using (var client = Fake.CreateClient())
                {
                    var response = client.MakeCall(new Call
                    {
                        From = "From",
                        To = "To"
                    }).Result;
                    Assert.IsTrue(response.IsSuccessStatusCode);
                }
            }
        }
        
        [TestMethod]
        public void MakeCallTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (c, url, content) =>
                {
                    Assert.AreEqual(string.Format("/users/{0}/calls", Fake.UserId), url);
                    dynamic res = JObject.Parse(content.ReadAsStringAsync().Result);
                    Assert.AreEqual("From", res.from.ToString());
                    Assert.AreEqual("To", res.to.ToString());
                    var response = new HttpResponseMessage(HttpStatusCode.Created);
                    response.Headers.Add("Location", string.Format("/v1/users/{0}/calls/1", Fake.UserId));
                    return Task.Run(() => response);
                };
                using (var client = Fake.CreateClient())
                {
                    var response = client.MakeCall(new Call
                    {
                        From = "From",
                        To = "To"
                    }).Result;
                    Assert.IsTrue(response.IsSuccessStatusCode);
                    Assert.AreEqual(string.Format("/v1/users/{0}/calls/1", Fake.UserId), response.Headers.Location.ToString());
                }
            }
        }

        [TestMethod]
        public void ChangeCallTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (c, url, content) =>
                {
                    Assert.AreEqual(string.Format("/users/{0}/calls/1", Fake.UserId), url);
                    dynamic res = JObject.Parse(content.ReadAsStringAsync().Result);
                    Assert.AreEqual("transferring", res.state.ToString());
                    Assert.AreEqual("Number", res.transferTo.ToString());
                    Assert.AreEqual("http://localhost", res.callbackUrl.ToString());
                    var response = new HttpResponseMessage(HttpStatusCode.Created);
                    response.Headers.Add("Location", string.Format("/v1/users/{0}/calls/1", Fake.UserId));
                    return Task.Run(() => response);
                };
                using (var client = Fake.CreateClient())
                {
                    var response = client.ChangeCall("1", new CallData
                    {
                        State = CallState.Transferring,
                        TransferTo = "Number",
                        CallbackUrl = "http://localhost"
                    }).Result;
                    Assert.IsTrue(response.IsSuccessStatusCode);
                    Assert.AreEqual(string.Format("/v1/users/{0}/calls/1", Fake.UserId), response.Headers.Location.ToString());
                }
            }
        }
    }
}
