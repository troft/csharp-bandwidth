using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public class Account
    {
        private const string AccountPath = "account";
        
        public static Task<Account> Get(Client client)
        {
            return client.MakeGetRequest<Account>(client.ConcatUserPath(AccountPath));
        }

        public static Task<AccountTransaction[]> GetTransactions(Client client, Dictionary<string, object> query = null)
        {
            return client.MakeGetRequest<AccountTransaction[]>(client.ConcatUserPath(string.Format("{0}/transactions", AccountPath)), query);
        }

        public static Task<AccountTransaction[]> GetTransactions(Client client, int page, int size = 25)
        {
            return GetTransactions(client, new Dictionary<string, object> {{"page", page}, {"size", size}});
        }

#if !PCL
        public static Task<Account> Get()
        {
            return Get(Client.GetInstance());
        }

        public static Task<AccountTransaction[]> GetTransactions(Dictionary<string, object> query = null)
        {
            return GetTransactions(Client.GetInstance(), query);
        }

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
