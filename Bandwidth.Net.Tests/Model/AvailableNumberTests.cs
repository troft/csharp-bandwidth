using Bandwidth.Net.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Model
{
    [TestClass]
    public class AvailableNumberTests
    {
        [TestInitialize]
        public void Setup()
        {
            Helper.SetEnvironmetVariables();
        }

        [TestMethod]
        public void TestSearchLocal()
        {
            var availableNumbers = new[]
                {
                    new AvailableNumber
                        {
                            Number = "1",
                            City = "City1",
                            Lata = "Lata1",
                            NationalNumber = "NationalNumber1",
                            PatternMatch = "PatternMatch1",
                            Price = 0.01m,
                            RateCenter = "RateCenter1",
                            State = "State1",

                        },
                    new AvailableNumber
                        {
                            Number = "2",
                            City = "City2",
                            Lata = "Lata2",
                            NationalNumber = "NationalNumber2",
                            PatternMatch = "PatternMatch2",
                            Price = 0.01m,
                            RateCenter = "RateCenter2",
                            State = "State2",

                        }
                };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = "/v1/availableNumbers/local",
                ContentToSend = Helper.CreateJsonContent(availableNumbers)
            }))
            {
                var client = Helper.CreateClient();
                var result = AvailableNumber.SearchLocal(client).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(availableNumbers[0], result[0]);
                Helper.AssertObjects(availableNumbers[1], result[1]);
            }
        }

        [TestMethod]
        public void TestSearchLocalWithDefaultClient()
        {
            var availableNumbers = new[]
                {
                    new AvailableNumber
                        {
                            Number = "1",
                            City = "City1",
                            Lata = "Lata1",
                            NationalNumber = "NationalNumber1",
                            PatternMatch = "PatternMatch1",
                            Price = 0.01m,
                            RateCenter = "RateCenter1",
                            State = "State1",

                        },
                    new AvailableNumber
                        {
                            Number = "2",
                            City = "City2",
                            Lata = "Lata2",
                            NationalNumber = "NationalNumber2",
                            PatternMatch = "PatternMatch2",
                            Price = 0.01m,
                            RateCenter = "RateCenter2",
                            State = "State2",

                        }
                };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = "/v1/availableNumbers/local",
                ContentToSend = Helper.CreateJsonContent(availableNumbers)
            }))
            {
                var result = AvailableNumber.SearchLocal().Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(availableNumbers[0], result[0]);
                Helper.AssertObjects(availableNumbers[1], result[1]);
            }
        }

        [TestMethod]
        public void TestSearchTollFree()
        {
            var availableNumbers = new[]
                {
                    new AvailableNumber
                        {
                            Number = "1",
                            NationalNumber = "NationalNumber1",
                            PatternMatch = "PatternMatch1",
                            Price = 0.01m,
                        },
                    new AvailableNumber
                        {
                            Number = "2",
                            NationalNumber = "NationalNumber2",
                            PatternMatch = "PatternMatch2",
                            Price = 0.01m,
                        }
                };

            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = "/v1/availableNumbers/tollFree",
                ContentToSend = Helper.CreateJsonContent(availableNumbers)
            }))
            {
                var client = Helper.CreateClient();
                var result = AvailableNumber.SearchTollFree(client).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(availableNumbers[0], result[0]);
                Helper.AssertObjects(availableNumbers[1], result[1]);
            }
        }

        [TestMethod]
        public void TestSearchTollFreeWithDefaultClient()
        {
            var availableNumbers = new[]
                {
                    new AvailableNumber
                        {
                            Number = "1",
                            NationalNumber = "NationalNumber1",
                            PatternMatch = "PatternMatch1",
                            Price = 0.01m,
                        },
                    new AvailableNumber
                        {
                            Number = "2",
                            NationalNumber = "NationalNumber2",
                            PatternMatch = "PatternMatch2",
                            Price = 0.01m,
                        }
                };

            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = "/v1/availableNumbers/tollFree",
                ContentToSend = Helper.CreateJsonContent(availableNumbers)
            }))
            {
                var result = AvailableNumber.SearchTollFree().Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(availableNumbers[0], result[0]);
                Helper.AssertObjects(availableNumbers[1], result[1]);
            }
        }
    }
}
