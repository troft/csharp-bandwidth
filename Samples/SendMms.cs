using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bandwidth.Net.Model;
namespace Samples
{
    //This is a demo of sending of mms
    public class SendMms
    {
        public static async Task Run()
        {
            //Please fill these constants
            const string fromNumber = "+1"; //your number on catapult
            const string toNumber = "+1"; //any number which can receive a message

            //Upload file if need
            var file = (await Media.List()).FirstOrDefault(f => f.MediaName == "net_test.png");
            if (file == null)
            {
                await Media.Upload("net_test.png",
                    Assembly.GetExecutingAssembly().GetManifestResourceStream("Samples.test.png"), "image/png");
                file = (await Media.List()).FirstOrDefault(f => f.MediaName == "net_test.png");
            }

            //Send mms
            try
            {
                var message = await Message.Create(new Dictionary<string, object>
                {
                    {"from", fromNumber},
                    {"to", toNumber},
                    {"text", "Hello there"},
                    {"media", new[] {file.Content}}
                });
                Console.WriteLine("Message id is {0}", message.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}