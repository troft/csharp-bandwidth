using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public class AvailableNumber
    {
        private const string AvailableNumbersTollFreePath = "availableNumbers/tollFree";
        private const string AvailableNumbersLocalPath = "availableNumbers/local";

        public static Task<AvailableNumber[]> SearchTollFree(Client client, Dictionary<string, object> query = null)
        {
            return client.MakeGetRequest<AvailableNumber[]>(AvailableNumbersTollFreePath, query);
        }

        public static Task<AvailableNumber[]> SearchLocal(Client client, Dictionary<string, object> query = null)
        {
            return client.MakeGetRequest<AvailableNumber[]>(AvailableNumbersLocalPath, query);
        }

#if !PCL
        public static Task<AvailableNumber[]> SearchTollFree(Dictionary<string, object> query = null)
        {
            return SearchTollFree(Client.GetInstance(), query);
        }
        public static Task<AvailableNumber[]> SearchLocal(Dictionary<string, object> query = null)
        {
            return SearchLocal(Client.GetInstance(), query);
        }

#endif

        public string Number { get; set; }
        public string NationalNumber { get; set; }
        public string PatternMatch { get; set; }
        public string City { get; set; }
        public string Lata { get; set; }
        public string RateCenter { get; set; }
        public string State { get; set; }
        public decimal Price { get; set; }
    }
}
