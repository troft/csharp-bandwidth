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
    public class PhoneNumbersTests
    {
        [TestMethod]
        public void CreateTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (c, url, content) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/phoneNumbers", Helper.UserId), url);
                    var phoneNumber = Helper.ParseJsonContent<PhoneNumber>(content).Result;
                    Assert.AreEqual("Name", phoneNumber.Name);
                    Assert.AreEqual("Number", phoneNumber.Number);
                    var response = new HttpResponseMessage(HttpStatusCode.Created);
                    response.Headers.Add("Location", string.Format("/v1/users/{0}/phoneNumbers/1", Helper.UserId));
                    return Task.Run(() => response);
                };
                using (var client = Helper.CreateClient())
                {
                    var id = client.PhoneNumbers.Create(new PhoneNumber
                    {
                        Name = "Name",
                        Number = "Number"
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
                    Assert.AreEqual(string.Format("users/{0}/phoneNumbers/1", Helper.UserId), url);
                    var phoneNumber = Helper.ParseJsonContent<PhoneNumber>(content).Result;
                    Assert.AreEqual("Name", phoneNumber.Name);
                    Assert.AreEqual("Number", phoneNumber.Number);
                    var response = new HttpResponseMessage(HttpStatusCode.Created);
                    response.Headers.Add("Location", string.Format("/v1/users/{0}/phoneNumbers/1", Helper.UserId));
                    return Task.Run(() => response);
                };
                using (var client = Helper.CreateClient())
                {
                    client.PhoneNumbers.Update("1", new PhoneNumber
                    {
                        Name = "Name",
                        Number = "Number"
                    }).Wait();
                }
            }
        }

        [TestMethod]
        public void GetTest()
        {
            using (ShimsContext.Create())
            {
                var phoneNumber = new PhoneNumber
                {
                    Id = "1",
                    Name = "Name",
                    Number = "Number"
                };
                ShimHttpClient.AllInstances.GetAsyncString = (c, url) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/phoneNumbers/1", Helper.UserId), url);
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = Helper.CreateJsonContent(phoneNumber)
                    };
                    return Task.Run(() => response);
                };
                using (var client = Helper.CreateClient())
                {
                    var result = client.PhoneNumbers.Get("1").Result;
                    Helper.AssertObjects(phoneNumber, result);
                }
            }
        }

        [TestMethod]
        public void GetAllTest()
        {
            using (ShimsContext.Create())
            {
                var phoneNumbers = new[]
                {
                    new PhoneNumber
                    {
                        Id = "1",
                        Name = "Name",
                        Number = "Number"
                    },
                    new PhoneNumber
                    {
                        Id = "2",
                        Name = "Name2",
                        Number = "Number2"
                    }
                };
                ShimHttpClient.AllInstances.GetAsyncString = (c, url) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/phoneNumbers", Helper.UserId), url);
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = Helper.CreateJsonContent(phoneNumbers)
                    };
                    return Task.Run(() => response);
                };
                using (var client = Helper.CreateClient())
                {
                    var result = client.PhoneNumbers.GetAll().Result;
                    Assert.AreEqual(2, result.Length);
                    Helper.AssertObjects(phoneNumbers[0], result[0]);
                    Helper.AssertObjects(phoneNumbers[1], result[1]);
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
                    Assert.AreEqual(string.Format("users/{0}/phoneNumbers/1", Helper.UserId), url);
                    return Task.Run(() => new HttpResponseMessage(HttpStatusCode.OK));
                };
                using (var client = Helper.CreateClient())
                {
                    client.PhoneNumbers.Remove("1").Wait();
                }
            }
        }
    }
}
