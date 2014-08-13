using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bandwidth.Net.Demo
{
    public static class GetApplicationsDemo
    {
        public async static Task Run()
        {
            using (var client = new Client(Config.UserId, Config.ApiToken, Config.Secret))
            {
                Console.WriteLine("Applications: {0}", string.Join(", ", from a in await client.Applications.GetAll() select a.Name));
            }
        }
    }
}
