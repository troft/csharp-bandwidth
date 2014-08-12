using System;
using System.Linq;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Demo
{
    class Program
    {
        const string UserId = "FILL_USER_ID_HERE";
        const string ApiToken = "FILL_API_TOKEN_HERE";
        const string Secret = "FILL_SECRET_HERE";
        static void Main()
        {
            try
            {
                RunDemos().Wait();
                Console.WriteLine("Done");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                Environment.ExitCode = 1;
            }
        }

        static async Task RunDemos()
        {
            using (var client = new Client(UserId, ApiToken, Secret))
            {
                Console.WriteLine("Applications: {0}", string.Join(", ", from a in await client.Applications.GetAll() select a.Name));
                Console.WriteLine("Media items: {0}", string.Join(", ", from m in await client.Media.GetAll() select m.MediaName));
                Console.WriteLine("Recordings: {0}", string.Join(", ", from m in await client.Recordings.GetAll() select m.Media));
                Console.WriteLine("Errors:\n{0}", string.Join("\n", from m in await client.Errors.GetAll() select m.Message));
                var numbers = await GetAvailableNumbers(client);
                Console.WriteLine("Available numbers: {0}", string.Join(", ", numbers));
                var numberId = await BuyNumber(client, numbers[0]);
                Console.WriteLine("We have bought the number: {0} (id {1})", numbers[0], numberId);
                numbers = (from p in await client.PhoneNumbers.GetAll() select p.Number).ToArray();
                Console.WriteLine("User's phone numbers: {0}", string.Join(", ", numbers));
                
                try
                {
                    var callId = await Call(client, numbers[0], numbers[1]);
                    Console.WriteLine("We have stared a call from {0} to {1} (id {1})", numbers[0], numbers[1], callId);
                    var call = await client.Calls.Get(callId);
                    Console.WriteLine("Got call info: from {0}, to {1}, direction {2}, State {3}", call.From, call.To,
                        call.Direction, call.State);
                    await client.Calls.SetAudio(callId, new Audio
                    {
                        Gender = Gender.Female,
                        Sentence = "Thank you",
                        Locale = "en_US"
                    });
                    Console.WriteLine("Audio message has been played");
                    var messageId = await SendSms(client, numbers[0], numbers[1]);
                    Console.WriteLine("We have sent sms from {0} to {1} (id {1})", numbers[0], numbers[1], messageId);
                    var message = await client.Messages.Get(messageId);
                    Console.WriteLine("Got message info: from {0}, to {1}, text {2}, state {3}", message.From, message.To,
                        message.Text, message.State);
                }
                finally
                {
                    client.PhoneNumbers.Remove(numberId).Wait();
                    Console.WriteLine("Removed phone number (id {0})", numberId);
                }
            }
        }

        private static Task<string> Call(Client client, string from, string to)
        {
            return client.Calls.Create(new Call
            {
                From = from,
                To = to
            });
        }

        private static Task<string> SendSms(Client client, string from, string to)
        {
            return client.Messages.Send(new Message
            {
                From = from,
                To = to,
                Text = "Hello"
            });
        }

        private static Task<string> BuyNumber(Client client, string number)
        {
            return client.PhoneNumbers.Create(new PhoneNumber
            {
                Number = number
            });
        }

        private async static Task<string[]> GetAvailableNumbers(Client client)
        {
            var numbers = await client.AvailableNumbers.GetAll(new AvailableNumberQuery { State = "NC", Type = AvailableNumberType.Local });
            return (from n in numbers.Take(3) select n.Number).ToArray();
        }
    }
}
