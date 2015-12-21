using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bandwidth.Net.Model;
namespace Samples
{
    //This is a demo of making of call
    public class MakeCall
    {
        public static async Task Run()
        {
            //Please fill these constants
            const string fromNumber = "+1"; //your number on catapult
            const string toNumber = "+1"; //any number which can receive incoming call
            var call = await Call.Create(new Dictionary<string, object> {{"from", fromNumber}, {"to", toNumber}});
            Console.WriteLine("Call id is {0}", call.Id);
        }
    }
}