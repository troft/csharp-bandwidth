using System;
using Bandwidth.Net;
using Bandwidth.Net.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Model
{
    [TestClass]
    public class AccountTests
    {
        [TestInitialize]
        public void Setup()
        {
            Helper.SetEnvironmetVariables();
        }

        [TestMethod]
        public void GetTest()
        {
            var account = new Account
            {
                AccountType = AccountType.PrePay,
                Balance = 100
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/account", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(account)
            }))
            {
                var client = Helper.CreateClient();
                var result = Account.Get(client).Result;
                if (server.Error != null) throw server.Error;
                Helper.AssertObjects(account, result);
            }
        }

        [TestMethod]
        public void GetWithMissingCredentialsTest()
        {
            var account = new Account
            {
                AccountType = AccountType.PrePay,
                Balance = 100
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/account", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(account)
            }))
            {
                try
                {
                    var client = Client.GetInstance("", "", "");
                    Account.Get(client).Wait();
                    throw new Exception("An exception is estimated");
                }
                catch (MissingCredentialsException)
                {
                    return;
                }
            }
        }

        [TestMethod]
        public void GetWithDefaultClientTest()
        {
            var account = new Account
            {
                AccountType = AccountType.PrePay,
                Balance = 100
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/account", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(account)
            }))
            {
                var result = Account.Get().Result;
                if (server.Error != null) throw server.Error;
                Helper.AssertObjects(account, result);
            }
        }

        [TestMethod]
        public void GetTransactionsTest()
        {
            var transactions = new[]{
                new AccountTransaction
                {
                    Id = "1",
                    Amount = 10,
                    Type  = AccountTransactionType.Charge,
                    Time = DateTime.Now.AddMinutes(-20)
                },
                new AccountTransaction
                {
                    Id = "2",
                    Amount = 20,
                    Type  = AccountTransactionType.Payment,
                    Time = DateTime.Now.AddMinutes(-10)
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/account/transactions", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(transactions)
            }))
            {
                var client = Helper.CreateClient();
                var result = Account.GetTransactions(client).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(transactions[0], result[0]);
                Helper.AssertObjects(transactions[1], result[1]);
            }
        }

        [TestMethod]
        public void GetTransactionsWithDefaultClientTest()
        {
            var transactions = new[]{
                new AccountTransaction
                {
                    Id = "1",
                    Amount = 10,
                    Type  = AccountTransactionType.Charge,
                    Time = DateTime.Now.AddMinutes(-20)
                },
                new AccountTransaction
                {
                    Id = "2",
                    Amount = 20,
                    Type  = AccountTransactionType.Payment,
                    Time = DateTime.Now.AddMinutes(-10)
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/account/transactions", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(transactions)
            }))
            {
                var result = Account.GetTransactions().Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(transactions[0], result[0]);
                Helper.AssertObjects(transactions[1], result[1]);
            }
        }

        [TestMethod]
        public void GetTransactions2Test()
        {
            var transactions = new[]{
                new AccountTransaction
                {
                    Id = "1",
                    Amount = 10,
                    Type  = AccountTransactionType.Charge,
                    Time = DateTime.Now.AddMinutes(-20)
                },
                new AccountTransaction
                {
                    Id = "2",
                    Amount = 20,
                    Type  = AccountTransactionType.Payment,
                    Time = DateTime.Now.AddMinutes(-10)
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/account/transactions?page=1&size=25", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(transactions)
            }))
            {
                var client = Helper.CreateClient();
                var result = Account.GetTransactions(client, 1).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(transactions[0], result[0]);
                Helper.AssertObjects(transactions[1], result[1]);
            }
        }

        [TestMethod]
        public void GetTransactions2WithDefaultClientTest()
        {
            var transactions = new[]{
                new AccountTransaction
                {
                    Id = "1",
                    Amount = 10,
                    Type  = AccountTransactionType.Charge,
                    Time = DateTime.Now.AddMinutes(-20)
                },
                new AccountTransaction
                {
                    Id = "2",
                    Amount = 20,
                    Type  = AccountTransactionType.Payment,
                    Time = DateTime.Now.AddMinutes(-10)
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/account/transactions?page=2&size=10", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(transactions)
            }))
            {
                var result = Account.GetTransactions(2, 10).Result;
                if (server.Error != null) throw server.Error;
                Assert.AreEqual(2, result.Length);
                Helper.AssertObjects(transactions[0], result[0]);
                Helper.AssertObjects(transactions[1], result[1]);
            }
        }

    }
}
