using System.Threading;
using System.Threading.Tasks;

namespace Bandwidth.Net.Api
{
  internal interface IAccountApi
  {
    Task<Account> Get(CancellationToken? cancellationToken = default(CancellationToken?));
    LazyEnumerable<AccountTransaction> GetTransactions(AccountTransactionQuery query = null, CancellationToken? cancellationToken = default(CancellationToken?));
  }
}