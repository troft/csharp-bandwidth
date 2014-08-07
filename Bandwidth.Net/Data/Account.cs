using System.Runtime.Serialization;

namespace Bandwidth.Net.Data
{
    public class Account
    {
        public decimal Balance { get; set; }
        public AccountType AccountType { get; set; }
    }

    public enum AccountType
    {
        [EnumMember(Value = "pre-pay")]
        PrePay
    }
}
