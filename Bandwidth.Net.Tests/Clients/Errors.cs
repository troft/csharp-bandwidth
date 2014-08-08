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
    public class ErrorsTests
    {
        [TestMethod]
        public void GetTest()
        {
            using (ShimsContext.Create())
            {
                var error = new Error
                {
                    Id = "1",
                    Message = "Error"
                };
                ShimHttpClient.AllInstances.GetAsyncString = (c, url) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/errors/1", Fake.UserId), url);
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = Fake.CreateJsonContent(error)
                    };
                    return Task.Run(() => response);
                };
                using (var client = Fake.CreateClient())
                {
                    var result = client.Errors.Get("1").Result;
                    Fake.AssertObjects(error, result);
                }
            }
        }

        [TestMethod]
        public void GetAllTest()
        {
            using (ShimsContext.Create())
            {
                var errors = new[]
                {
                    new Error
                    {
                        Id = "1",
                        Message = "Error"
                    },
                    new Error
                    {
                        Id = "2",
                        Message = "Error2"
                    }
                };
                ShimHttpClient.AllInstances.GetAsyncString = (c, url) =>
                {
                    Assert.AreEqual(string.Format("users/{0}/errors", Fake.UserId), url);
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = Fake.CreateJsonContent(errors)
                    };
                    return Task.Run(() => response);
                };
                using (var client = Fake.CreateClient())
                {
                    var result = client.Errors.GetAll().Result;
                    Assert.AreEqual(2, result.Length);
                    Fake.AssertObjects(errors[0], result[0]);
                    Fake.AssertObjects(errors[1], result[1]);
                }
            }
        }
        
    }
}
