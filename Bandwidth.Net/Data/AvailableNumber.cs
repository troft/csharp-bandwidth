using System.Collections.Generic;
using System.Globalization;

namespace Bandwidth.Net.Data
{
    public class AvailableNumber
    {
        public string Number { get; set; }
        public string NationalNumber { get; set; }
        public string PatternMatch { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Lata { get; set; }
        public string RateCenter { get; set; }
        public double Price { get; set; }
    }
    public class AvailableNumberQuery: Query
    {
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string AreaCode { get; set; }
        public string LocalNumber { get; set; }
        public bool? InLocalCallingArea { get; set; }
        public int? Quantity { get; set; }
        public string Pattern { get; set; }
        public AvailableNumberType Type { get; set; }

        public override IDictionary<string, string> ToDictionary()
        {
            var query = base.ToDictionary();
            if (City != null)
            {
                query.Add("city", City);
            }
            if (State != null)
            {
                query.Add("state", State);
            }
            if (Zip != null)
            {
                query.Add("zip", Zip);
            }
            if (AreaCode != null)
            {
                query.Add("areaCode", AreaCode);
            }
            if (LocalNumber != null)
            {
                query.Add("localNumber", LocalNumber);
            }
            if (InLocalCallingArea != null)
            {
                query.Add("inLocalCallingArea", InLocalCallingArea.ToString());
            }
            if (Quantity!= null)
            {
                query.Add("quantity", Quantity.Value.ToString(CultureInfo.InvariantCulture));
            }
            if (Pattern != null)
            {
                query.Add("pattern", Pattern);
            }
            return query;
        }
    }

    public enum AvailableNumberType
    {
        Local,
        TollFree
    }

}
