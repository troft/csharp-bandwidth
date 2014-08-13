using System;
using System.Linq;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Demo
{
    public static class PlayAudioDemo
    {
        public async static Task Run()
        {
            using (var client = new Client(Config.UserId, Config.ApiToken, Config.Secret))
            {
                var existingPhoneNumber = (from n in await client.PhoneNumbers.GetAll() select n.Number).First();
                var callId = await client.Calls.Create(new Call
                {
                    From = existingPhoneNumber,
                    To = Config.RealPhoneNumber
                });
                System.Threading.Thread.Sleep(5000); // waiting for accepting incoming call
                var call = await client.Calls.Get(callId);
                if (call.State != null && call.State.Value == CallState.Active)
                {
                    //We can play audion only for active call
                    await client.Calls.SetAudio(callId, new Audio
                    {
                        Gender = Gender.Female,
                        Sentence = "Thank you",
                        Locale = "en_US"
                    });
                }
                
            }
        }
    }
}
