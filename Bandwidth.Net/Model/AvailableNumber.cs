using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public class AvailableNumber
    {
        private const string AvailableNumbersTollFreePath = "availableNumbers/tollFree";
        private const string AvailableNumbersLocalPath = "availableNumbers/local";

        public static async Task<AvailableNumber[]> SearchTollFree(Client client, Dictionary<string, object> query = null)
        {
            var availableNumbers = await client.MakeGetRequest<AvailableNumber[]>(AvailableNumbersTollFreePath, query);
            return availableNumbers;
        }

        public static async Task<AvailableNumber[]> SearchLocal(Client client, Dictionary<string, object> query = null)
        {
            var availableNumbers = await client.MakeGetRequest<AvailableNumber[]>(AvailableNumbersLocalPath, query);
            return availableNumbers;
        }

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
