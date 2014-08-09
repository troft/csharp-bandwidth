using System.Collections.Generic;
using Bandwidth.Net.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Clients
{
    [TestClass]
    public class PhoneNumbersTests
    {
        [TestMethod]
        public void CreateTest()
        {
            var phoneNumber = new PhoneNumber
            {
                Name = "PhoneNumber",
                Number = "Number"
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "POST",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/phoneNumbers", Helper.UserId),
                EstimatedContent = Helper.ToJsonString(phoneNumber),
                HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/phoneNumbers/1", Helper.UserId) } }
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var id = client.PhoneNumbers.Create(phoneNumber).Result;
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual("1", id);
                }
            }
        }

        [TestMethod]
        public void UpdateTest()
        {
            var phoneNumber = new PhoneNumber
            {
                Name = "PhoneNumber",
                Number = "Number"
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "POST",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/phoneNumbers/1", Helper.UserId),
                EstimatedContent = Helper.ToJsonString(phoneNumber),
                HeadersToSend = new Dictionary<string, string> { { "Location", string.Format("/v1/users/{0}/phoneNumbers/1", Helper.UserId) } }
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    client.PhoneNumbers.Update("1", phoneNumber).Wait();
                    if (server.Error != null) throw server.Error;
                }
            }
        }

        [TestMethod]
        public void GetTest()
        {
            var phoneNumber = new PhoneNumber
            {
                Id = "1",
                Name = "PhoneNumber",
                Number = "Number"
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/phoneNumbers/1", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(phoneNumber)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var result = client.PhoneNumbers.Get("1").Result;
                    if (server.Error != null) throw server.Error;
                    Helper.AssertObjects(phoneNumber, result);
                }
            }
        }

        [TestMethod]
        public void GetAllTest()
        {
            var phoneNumbers = new[]{
                new PhoneNumber
                {
                    Id = "1",
                    Name = "PhoneNumber",
                    Number = "Number"
                },
                new PhoneNumber
                {
                    Id = "2",
                    Name = "PhoneNumber2",
                    Number = "Number2"
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/phoneNumbers", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(phoneNumbers)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var result = client.PhoneNumbers.GetAll().Result;
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual(2, result.Length);
                    Helper.AssertObjects(phoneNumbers[0], result[0]);
                    Helper.AssertObjects(phoneNumbers[1], result[1]);
                }
            }
        }
        [TestMethod]
        public void RemoveTest()
        {
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "DELETE",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/phoneNumbers/1", Helper.UserId)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    client.PhoneNumbers.Remove("1").Wait();
                    if (server.Error != null) throw server.Error;
                }
            }
        }
    }
}
