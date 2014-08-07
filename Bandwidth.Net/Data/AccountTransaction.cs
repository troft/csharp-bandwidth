using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace Bandwidth.Net.Data
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
        public int? Page { get; set; }
        public int? Size { get; set; }
    }

    public enum AccountTransactionType
    {
        Charge,
        Payment,
        Credit,
        [EnumMember(Value = "auto-recharge")]
        AutoRecharge
    }

    public enum ProductType
    {
        [EnumMember(Value = "sms-in")]
        SmsIn,
        [EnumMember(Value = "sms-out")]
        SmsOut,
        [EnumMember(Value = "call-in")]
        CallIn,
        [EnumMember(Value = "call-out")]
        CallOut,
        [EnumMember(Value = "local-number-per-month")]
        LocalNumberPerMonth,
        [EnumMember(Value = "toll-free-number-per-month")]
        TollFreeNumberPerMonth
    }

    public class AccountTransactionQuery : Query
    {
        public int? MaxItems { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FromDate { get; set; }
        public string Type { get; set; }

        public override IDictionary<string, string> ToDictionary()
        {
            var query = base.ToDictionary();
            if (MaxItems != null)
            {
                query.Add("maxItems", MaxItems.Value.ToString(CultureInfo.InvariantCulture));
            }
            if (ToDate != null)
            {
                query.Add("toDate", ToDate.Value.ToUniversalTime().ToString("o"));
            }
            if (FromDate != null)
            {
                query.Add("fromDate", FromDate.Value.ToUniversalTime().ToString("o"));
            }
            if (Type != null)
            {
                query.Add("type", Type);
            }
            return query;
        }
    }
}