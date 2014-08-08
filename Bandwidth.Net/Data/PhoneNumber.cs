using System;

namespace Bandwidth.Net.Data
{
    public class PhoneNumber
    {
        public string Id { get; set; }
        public Uri Application { get; set; }
        public string Number { get; set; }
        public string NationalNumber { get; set; }
        public string FallbackNumber { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public double? Price { get; set; }
        public PhoneNumberState? NumberState { get; set; }
    }

    public class PhoneNumberQuery: Query
    {
    }

    public enum PhoneNumberState
    {
        Enabled,
        Released
    }
}
