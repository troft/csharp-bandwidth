using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public class Account
    {
        private const string AccountPath = "account";
        
        /// <summary>
        /// Get User Account Info.  
        /// <a href="https://catapult.inetwork.com/docs/api-docs/account/#GET-/v1/users/{userId}/account">Documentation</a>
        /// </summary>
        /// <param name="client"></param>
        /// <returns>Account</returns>
        /// <code>
        /// var account = Account.Get(client).Result
        /// </code>
        public static Task<Account> Get(Client client)
        {
            return client.MakeGetRequest<Account>(client.ConcatUserPath(AccountPath));
        }

        /// <summary>
        /// List transactions for user acccount
        /// <a href="https://catapult.inetwork.com/docs/api-docs/account/#GET-/v1/users/{userId}/account/transactions">Documentation</a>
        /// </summary>
        /// <param name="client"></param>
        /// <param name="query"></param>
        /// <returns>AccountTransaction[]</returns>
        /// <code>
        /// var transactions = Account.GetTransactions(client, new Dictionary<string, object>{{"maxItems", 5}}).Result;
        /// </code>
        public static Task<AccountTransaction[]> GetTransactions(Client client, Dictionary<string, object> query = null)
        {
            return client.MakeGetRequest<AccountTransaction[]>(client.ConcatUserPath(string.Format("{0}/transactions", AccountPath)), query);
        }

        /// <summary>
        /// List transactions for user account
        /// <a href="https://catapult.inetwork.com/docs/api-docs/account/#GET-/v1/users/{userId}/account/transactions">Documentation</a>
        /// </summary>
        /// <param name="client"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        /// <code>
        /// var transactions = Account.GetTransactions(client, 1, 20).Result
        /// </code>
        public static Task<AccountTransaction[]> GetTransactions(Client client, int page, int size = 25)
        {
            return GetTransactions(client, new Dictionary<string, object> {{"page", page}, {"size", size}});
        }

#if !PCL
        /// <summary>
        /// Non Portable Get Account Method
        /// </summary>
        /// <returns>Account for the credentialed client</returns>
        /// <code>
        /// var account = Account.Get().Result
        /// </code>
        public static Task<Account> Get()
        {
            return Get(Client.GetInstance());
        }

        /// <summary>
        /// Non portable Get Transactions
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <code>
        /// var transactions = Account.GetTransactions(new Dictionary<string, object>{{"maxItems", 5}}).Result
        /// </code>
        public static Task<AccountTransaction[]> GetTransactions(Dictionary<string, object> query = null)
        {
            return GetTransactions(Client.GetInstance(), query);
        }

        /// <summary>
        /// Non portable Get Transactions
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        /// <code>
        /// var transactions = Account.GetTransactions(1, 20).Result
        /// </code>
        public static Task<AccountTransaction[]> GetTransactions(int page, int size = 25)
        {
            return GetTransactions(Client.GetInstance(), page, size);
        }

#endif
        public decimal Balance { get; set; }
        public AccountType AccountType { get; set; }
    }

    public enum AccountType
    {
        [EnumMember(Value = "pre-pay")]
        PrePay
    }
}
