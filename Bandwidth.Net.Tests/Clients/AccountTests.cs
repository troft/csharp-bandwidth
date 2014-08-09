using System;
using Bandwidth.Net.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests.Clients
{
    [TestClass]
    public class AccountTests
    {
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
                using (var client = Helper.CreateClient())
                {
                    var result = client.Account.Get().Result;
                    if (server.Error != null) throw server.Error;
                    Helper.AssertObjects(account, result);
                }
            }
        }

        [TestMethod]
        public void GetTransactionsTest()
        {
            var transactions = new[]
            {
                new AccountTransaction
                {
                    Id = "1",
                    Type = AccountTransactionType.AutoRecharge,
                    Amount = 10,
                    Units = 1,
                    Time = DateTime.Now.AddMinutes(-10)
                },
                new AccountTransaction
                {
                    Id = "2",
                    Type = AccountTransactionType.Payment,
                    Amount = 16,
                    Units = 2,
                    Time = DateTime.Now.AddMinutes(-15)
                }
            };
            using (var server = new HttpServer(new RequestHandler
            {
                EstimatedMethod = "GET",
                EstimatedPathAndQuery = string.Format("/v1/users/{0}/account/transactions", Helper.UserId),
                ContentToSend = Helper.CreateJsonContent(transactions)
            }))
            {
                using (var client = Helper.CreateClient())
                {
                    var result = client.Account.GetTransactions().Result;
                    if (server.Error != null) throw server.Error;
                    Assert.AreEqual(2, result.Length);
                    Helper.AssertObjects(transactions[0], result[0]);
                    Helper.AssertObjects(transactions[1], result[1]);
                }
            }
        }
    }
}
