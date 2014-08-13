using System;using System.Linq;
using System.Threading.Tasks;

namespace Bandwidth.Net.Demo
{
    public static class GetCallsDemo
    {
        public async static Task Run()
        {
            using (var client = new Client(Config.UserId, Config.ApiToken, Config.Secret))
            {
                Console.WriteLine("Calls:\n", string.Join("\n", from c in await client.Calls.GetAll() select string.Format("{0} -> {1} ({2})", c.From, c.To, c.State)));
            }
        }
    }
}
