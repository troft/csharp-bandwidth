using System;
using System.Linq;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Demo
{
    public static class SendSmsDemo
    {
        public async static Task Run()
        {
            using (var client = new Client(Config.UserId, Config.ApiToken, Config.Secret))
            {
                var existingPhoneNumber = (from n in await client.PhoneNumbers.GetAll() select n.Number).First();
                var smsId = await client.Messages.Send(new Message
                {
                    From = existingPhoneNumber,
                    To = "+1-202-555-0148", //fake number
                    Text = "Hello"
                });
                Console.WriteLine("Sent sms from number {0} (message id {1})", existingPhoneNumber, smsId);
            }
        }
    }
}
