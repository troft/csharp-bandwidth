using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Clients
{
    public class Account
    {
        private readonly Client _client;

        internal Account(Client client)
        {
            _client = client;
        }
        private const string AccountPath = "account";
        public Task<Data.Account> Get()
        {
            return _client.MakeGetRequest<Data.Account>(_client.ConcatUserPath(AccountPath));
        }

        public Task<AccountTransaction[]> GetTransactions(AccountTransactionQuery query = null)
        {
            query = query ?? new AccountTransactionQuery();
            return _client.MakeGetRequest<AccountTransaction[]>(_client.ConcatUserPath(string.Format("{0}/transactions", AccountPath)), query.ToDictionary());
        }
    }
}

