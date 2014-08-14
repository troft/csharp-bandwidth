using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Clients
{
    /// <summary>
    ///     Account API allows you to retrieve your current balance, transaction list, account type and all elements related to
    ///     your platform account.
    /// </summary>
    public class Account
    {
        private const string AccountPath = "account";
        private readonly Client _client;

        internal Account(Client client)
        {
            _client = client;
        }

        /// <summary>
        ///     Get user's current account information
        /// </summary>
        public Task<Data.Account> Get()
        {
            return _client.MakeGetRequest<Data.Account>(_client.ConcatUserPath(AccountPath));
        }

        /// <summary>
        ///     Get the transactions from the user's Account.
        /// </summary>
        public Task<AccountTransaction[]> GetTransactions(AccountTransactionQuery query = null)
        {
            query = query ?? new AccountTransactionQuery();
            return
                _client.MakeGetRequest<AccountTransaction[]>(
                    _client.ConcatUserPath(string.Format("{0}/transactions", AccountPath)), query.ToDictionary());
        }
    }
}