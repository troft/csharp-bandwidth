using System;
using System.Runtime.Serialization;

namespace Bandwidth.Net.Model
{
    /// <summary>
    /// Transaction of user's account
    /// </summary>
    public class AccountTransaction
    {
        /// <summary>
        /// The unique identifier for the transaction
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// The transaction amount in dollars
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The time the transaction was processed
        /// </summary>
        public DateTime Time { get; set; }
        
        /// <summary>
        /// The type of transaction
        /// </summary>
        public AccountTransactionType Type { get; set; }
        
        /// <summary>
        /// The number of product units the transaction charged or credited
        /// </summary>
        public int Units { get; set; }

        /// <summary>
        /// The product the transaction was related to (not all transactions are related to a product)
        /// </summary>
        public ProductType ProductType { get; set; }

        /// <summary>
        /// The phone number the transaction was related to (not all transactions are related to a phone number)
        /// </summary>
        public string Number { get; set; }
    }

    /// <summary>
    /// Transaction types
    /// </summary>
    public enum AccountTransactionType
    {
        /// <summary>
        /// A charge for the use of a service or resource (for example, phone calls, SMS messages, phone numbers)
        /// </summary>
        Charge,

        /// <summary>
        /// A payment you made to increase your account balance
        /// </summary>
        Payment,
        
        /// <summary>
        /// An increase to your account balance that you did not pay for (for example, an initial account credit or promotion)
        /// </summary>
        Credit,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember(Value = "goodwill-credit")]
        GoodwillCredit,

        /// <summary>
        /// An automated payment made to keep your account balance above the minimum balance you configured
        /// </summary>
        [EnumMember(Value = "auto-recharge")]
        AutoRecharge
    }

    /// <summary>
    /// Product types
    /// </summary>
    public enum ProductType
    {
        /// <summary>
        /// An SMS message that came in to one of your numbers
        /// </summary>
        [EnumMember(Value = "sms-in")]
        SmsIn,
        
        /// <summary>
        /// An SMS message that was sent from one of your numbers
        /// </summary>
        [EnumMember(Value = "sms-out")]
        SmsOut,

        /// <summary>
        /// An MMS message that came in to one of your numbers
        /// </summary>
        [EnumMember(Value = "mms-in")]
        MmsIn,

        /// <summary>
        /// An MMS message that came in to one of your numbers
        /// </summary>
        [EnumMember(Value = "mms-out")]
        MmsOut,

        /// <summary>
        /// A phone call that came in to one of your numbers
        /// </summary>
        [EnumMember(Value = "call-in")]
        CallIn,

        /// <summary>
        /// A phone call that was made from one of your numbers
        /// </summary>
        [EnumMember(Value = "call-out")]
        CallOut,

        /// <summary>
        /// A phone call that came in to one of your toll-free numbers
        /// </summary>
        [EnumMember(Value = "call-in-toll-free")]
        CallInTollFree,
        
        /// <summary>
        /// A phone call that was made from one of your toll-free numbers
        /// </summary>
        [EnumMember(Value = "call-out-toll-free")]
        CallOutTollFree,
        
        /// <summary>
        /// The monthly charge for a local phone number you have
        /// </summary>
        [EnumMember(Value = "local-number-per-month")]
        LocalNumberPerMonth,
        
        /// <summary>
        /// The monthly charge for a toll-free phone number you have
        /// </summary>
        [EnumMember(Value = "toll-free-number-per-month")]
        TollFreeNumberPerMonth,
        
        /// <summary>
        /// Search of CNAME
        /// </summary>
        [EnumMember(Value = "cnam-search")]
        CNamSearch,
        
        /// <summary>
        /// A call from SIP
        /// </summary>
        [EnumMember(Value = "sip-out")]
        SipOut
    }
}
