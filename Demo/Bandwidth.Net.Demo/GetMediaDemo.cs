using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bandwidth.Net.Demo
{
    public static class GetMediaDemo
    {
        public async static Task Run()
        {
            using (var client = new Client(Config.UserId, Config.ApiToken, Config.Secret))
            {
                Console.WriteLine("Media: {0}", string.Join(", ", from m in await client.Media.GetAll() select m.MediaName));
            }
        }
    }
}
