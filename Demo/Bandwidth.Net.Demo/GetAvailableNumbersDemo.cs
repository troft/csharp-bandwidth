using System;
using System.Linq;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Demo
{
    public static class GetAvailableNumbersDemo
    {
        public async static Task Run()
        {
            using (var client = new Client(Config.UserId, Config.ApiToken, Config.Secret))
            {
                Console.WriteLine("Available numbers: {0}", string.Join(", ", from n in await client.AvailableNumbers.GetAll(new AvailableNumberQuery{State = "NC", Type = AvailableNumberType.Local}) select n.Number));
            }
        }
    }
}
