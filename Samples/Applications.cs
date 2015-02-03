using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bandwidth.Net.Model;
namespace Samples
{
    //This is a demo of creating and removing and application
    public  class Applications
    {
        public static async Task Run()
        {
            var app = await Application.Create(new Dictionary<string, object>
            {
                {"name", "Demo Application from sample"},
                {"incomingCallUrl", "http://localhost"}
            });
            Console.WriteLine("Created application name is {0}", app.Name);
            await app.Delete();
            Console.WriteLine("The application has been removed");
        }
    }
}