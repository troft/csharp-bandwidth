using System;
using System.Runtime.Serialization;

namespace Bandwidth.Net.Data
{
    public class Error
    {
        public string Id { get; set; }
        public DateTime Time { get; set; }
        public ErrorCategory Category { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
        public ErrorDetail[] Details { get; set; }
        
    }

    public class ErrorDetail
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public enum ErrorCategory
    {
        Authentication,
        Authorization,
        [EnumMember(Value = "not-found")]
        NotFound,
        [EnumMember(Value = "bad-request")]
        BadRequest,
        Conflict,
        Unavailable,
        Credit,
        Limit,
        Payment
    }

    public class ErrorQuery : Query
    {
    }
}
