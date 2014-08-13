using System;
using System.Linq;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Demo
{
    public static class BuyPhoneNumberDemo
    {
        public async static Task Run()
        {
            using (var client = new Client(Config.UserId, Config.ApiToken, Config.Secret))
            {
                var availableNumber = (from n in await client.AvailableNumbers.GetAll(new AvailableNumberQuery{State = "NC", Type = AvailableNumberType.Local}) select n.Number).First();
                var numberId = await client.PhoneNumbers.Create(new PhoneNumber {Number = availableNumber});
                Console.WriteLine("Number {0} has been bought (id {1})", availableNumber, numberId);
            }
        }
    }
}
