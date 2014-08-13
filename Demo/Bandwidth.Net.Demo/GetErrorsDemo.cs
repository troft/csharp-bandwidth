using System;
using System.Linq;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Demo
{
    public static class GetErrorsDemo
    {
        public async static Task Run()
        {
            using (var client = new Client(Config.UserId, Config.ApiToken, Config.Secret))
            {
                Console.WriteLine("Errors:\n{0}", string.Join("\n", from n in await client.Errors.GetAll() select n.Message));
            }
        }
    }
}
