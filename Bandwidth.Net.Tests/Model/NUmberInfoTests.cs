using System;
using Bandwidth.Net.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Model
{
    [TestClass]
    public class NumberInfoTests
    {
        [TestInitialize]
        public void Setup()
        {
            Helper.SetEnvironmetVariables();
        }

        [TestMethod]
        public void GetTest()
        {
            var item = new NumberInfo
            {
                Number = "111",
                Created = DateTime.Now.AddMonths(-4),
                Updated = DateTime.Now.AddMonths(-1)
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = "/v1/phoneNumbers/numberInfo/111",
                ContentToSend = Helper.CreateJsonContent(item)
            }))
            {
                var client = Helper.CreateClient();
                var result = NumberInfo.Get(client, "111").Result;
                if (server.Error != null) throw server.Error;
                Helper.AssertObjects(item, result);
            }
        }

        [TestMethod]
        public void GetWithDefaultClientTest()
        {
            var item = new NumberInfo
            {
                Number = "111",
                Created = DateTime.Now.AddMonths(-4),
                Updated = DateTime.Now.AddMonths(-1)
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = "/v1/phoneNumbers/numberInfo/111",
                ContentToSend = Helper.CreateJsonContent(item)
            }))
            {
                var result = NumberInfo.Get("111").Result;
                if (server.Error != null) throw server.Error;
                Helper.AssertObjects(item, result);
            }
        }
    }
}

