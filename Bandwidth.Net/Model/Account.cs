using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    /// <summary>
    /// Account API allows you to retrieve your current balance, transaction list, account type and all elements related to your platform account.
    /// </summary>
    /// <seealso href="https://catapult.inetwork.com/docs/api-docs/account/"/>
    public class Account
    {
        private const string AccountPath = "account";
        
        /// <summary>
        /// Get User Account Info.  
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <returns>Account data</returns>
        /// <example>
        /// <code>
        /// var account = await Account.Get(client);
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/account/#GET-/v1/users/{userId}/account"/>
        public static Task<Account> Get(Client client)
        {
            return client.MakeGetRequest<Account>(client.ConcatUserPath(AccountPath));
        }

        /// <summary>
        /// List transactions for user acccount
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="query">Query dictionary with optional keys maxItems, toDate, fromDate, type, page, size</param>
        /// <returns>Array of AccountTransaction</returns>
        /// <example>
        /// <code>
        /// var transactions = await Account.GetTransactions(client, new Dictionary&lt;string, object&gt;{{"maxItems", 5}});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/account/#GET-/v1/users/{userId}/account/transactions"/>
        public static Task<AccountTransaction[]> GetTransactions(Client client, Dictionary<string, object> query = null)
        {
            return client.MakeGetRequest<AccountTransaction[]>(client.ConcatUserPath(string.Format("{0}/transactions", AccountPath)), query);
        }

        /// <summary>
        /// List transactions for user account
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="page">Page number</param>
        /// <param name="size">Optional size of each page (default 25)</param>
        /// <returns>Array of AccountTransaction</returns>
        /// <example>
        /// <code>
        /// var transactions = await Account.GetTransactions(client, 1, 20);
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/account/#GET-/v1/users/{userId}/account/transactions"/>
        public static Task<AccountTransaction[]> GetTransactions(Client client, int page, int size = 25)
        {
            return GetTransactions(client, new Dictionary<string, object> {{"page", page}, {"size", size}});
        }

        /// <summary>
        /// Get User Account Info.  
        /// </summary>
        /// <returns>Account data</returns>
        /// <example>
        /// <code>
        /// var account = await Account.Get();
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/account/#GET-/v1/users/{userId}/account"/>
        public static Task<Account> Get()
        {
            return Get(Client.GetInstance());
        }

        /// <summary>
        /// List transactions for user acccount
        /// </summary>
        /// <param name="query">Query dictionary with optional keys maxItems, toDate, fromDate, type, page, size</param>
        /// <returns>Array of AccountTransaction</returns>
        /// <example>
        /// <code>
        /// var transactions = await Account.GetTransactions(client, new Dictionary&lt;string, object&gt;{{"maxItems", 5}});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/account/#GET-/v1/users/{userId}/account/transactions"/>
        public static Task<AccountTransaction[]> GetTransactions(Dictionary<string, object> query = null)
        {
            return GetTransactions(Client.GetInstance(), query);
        }

        /// <summary>
        /// List transactions for user account
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="size">Optional size of each page (default 25)</param>
        /// <returns>Array of AccountTransaction</returns>
        /// <example>
        /// <code>
        /// var transactions = await Account.GetTransactions(client, 1, 20);
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/account/#GET-/v1/users/{userId}/account/transactions"/>
        public static Task<AccountTransaction[]> GetTransactions(int page, int size = 25)
        {
            return GetTransactions(Client.GetInstance(), page, size);
        }

        /// <summary>
        /// Balance value
        /// </summary>
        public decimal Balance { get; set; }
        
        /// <summary>
        /// Account type
        /// </summary>
        public AccountType AccountType { get; set; }
    }

    /// <summary>
    /// Account types
    /// </summary>
    public enum AccountType
    {
        /// <summary>
        ///  The type of account where you increase your available balance with credit card payments
        /// </summary>
        [EnumMember(Value = "pre-pay")]
        PrePay
    }
}
