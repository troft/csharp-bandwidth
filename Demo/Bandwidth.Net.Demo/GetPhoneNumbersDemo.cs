using System;
using System.Linq;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Demo
{
    public static class GetPhoneNumbersDemo
    {
        public async static Task Run()
        {
            using (var client = new Client(Config.UserId, Config.ApiToken, Config.Secret))
            {
                Console.WriteLine("Numbers: {0}", string.Join(", ", from p in await client.PhoneNumbers.GetAll() select p.Number));
            }
        }
    }
}
