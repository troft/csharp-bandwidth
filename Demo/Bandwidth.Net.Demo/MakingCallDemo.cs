using System;
using System.Linq;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Demo
{
    public static class MakingCallDemo
    {
        public async static Task Run()
        {
            using (var client = new Client(Config.UserId, Config.ApiToken, Config.Secret))
            {
                var existingPhoneNumber = (from n in await client.PhoneNumbers.GetAll() select n.Number).First();
                var callId = await client.Calls.Create(new Call
                {
                    From = existingPhoneNumber,
                    To = "+1-202-555-0148" //fake number
                });
                Console.WriteLine("Calling from {0} (call id {1})", existingPhoneNumber, callId);
                //Hang Up 
                await client.Calls.Update(callId, new Call{State = CallState.Completed});
            }
        }
    }
}
