using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bandwidth.Net.Model;
namespace Samples
{
    //This is a demo of sending of sms
    public class SendSms
    {
        public static async Task Run()
        {
            //Please fill these constants
            const string fromNumber = "+1"; //your number on catapult
            const string toNumber = "+1"; //any number which can receive a message
            var message = await Message.Create(new Dictionary<string, object> {{"from", fromNumber}, {"to", toNumber}, {"text", "Hello there"}});
            Console.WriteLine("Message id is {0}", message.Id);
        }
    }
}