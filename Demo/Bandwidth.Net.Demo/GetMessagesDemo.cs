using System;
using System.Linq;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Demo
{
    public static class GetMessagesDemo
    {
        public async static Task Run()
        {
            using (var client = new Client(Config.UserId, Config.ApiToken, Config.Secret))
            {
                Console.WriteLine("Messages:\n{0}", string.Join("\n", from m in await client.Messages.GetAll() select string.Format("{0}: {1}", m.From, m.Text)));
            }
        }
    }
}
