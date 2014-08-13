using System;
using System.Linq;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Demo
{
    public static class GetPhoneNumberDemo
    {
        public async static Task Run()
        {
            using (var client = new Client(Config.UserId, Config.ApiToken, Config.Secret))
            {
                var existingPhoneNumberId = (from n in await client.PhoneNumbers.GetAll() orderby n.CreatedTime descending select n.Id).First();
                var number = await client.PhoneNumbers.Get(existingPhoneNumberId);
                Console.WriteLine("Number: {0}, State: {1}", number.Number, number.NumberState);
            }
        }
    }
}
