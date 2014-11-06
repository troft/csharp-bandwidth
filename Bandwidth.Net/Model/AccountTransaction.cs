using System;
using System.Runtime.Serialization;

namespace Bandwidth.Net.Model
{
    public class AccountTransaction
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Time { get; set; }
        public AccountTransactionType Type { get; set; }
        public int Units { get; set; }
        public ProductType ProductType { get; set; }
        public string Number { get; set; }
    }

    public enum AccountTransactionType
    {
        Charge,
        Payment,
        Credit,
        [EnumMember(Value = "goodwill-credit")]
        GoodwillCredit,
        [EnumMember(Value = "auto-recharge")]
        AutoRecharge
    }

    public enum ProductType
    {
        [EnumMember(Value = "sms-in")]
        SmsIn,
        [EnumMember(Value = "sms-out")]
        SmsOut,
        [EnumMember(Value = "mms-in")]
        MmsIn,
        [EnumMember(Value = "mms-out")]
        MmsOut,
        [EnumMember(Value = "call-in")]
        CallIn,
        [EnumMember(Value = "call-out")]
        CallOut,
        [EnumMember(Value = "call-in-toll-free")]
        CallInTollFree,
        [EnumMember(Value = "call-out-toll-free")]
        CallOutTollFree,
        [EnumMember(Value = "local-number-per-month")]
        LocalNumberPerMonth,
        [EnumMember(Value = "toll-free-number-per-month")]
        TollFreeNumberPerMonth,
        [EnumMember(Value = "cnam-search")]
        CNamSearch,
        [EnumMember(Value = "sip-out")]
        SipOut
    }
}
