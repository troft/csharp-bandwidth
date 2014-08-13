using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bandwidth.Net.Demo
{
    public static class GetRecordingsDemo
    {
        public async static Task Run()
        {
            using (var client = new Client(Config.UserId, Config.ApiToken, Config.Secret))
            {
                Console.WriteLine("Recordings: {0}", string.Join(", ", from r in await client.Recordings.GetAll() select r.Media));
            }
        }
    }
}
