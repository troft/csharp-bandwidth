using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bandwidth.Net.Model;

namespace Samples
{
    //This is a demo of searching and allocating a phone number
    public class BuyPhoneNumber
    {
        public static async Task Run()
        {
            var numbers =
                await
                    AvailableNumber.SearchLocal(new Dictionary<string, object>
                    {
                        {"city", "Cary"},
                        {"state", "NC"},
                        {"quantity", 3}
                    });
            Console.WriteLine("Found numbers: {0}", string.Join(", ", numbers.Select(n=>n.Number)));
            var number = await PhoneNumber.Create(new Dictionary<string, object> {{"number", numbers[0].Number}});
            Console.WriteLine("Now you are owner of number {0} (id {1})", number.Number, number.Id);
        }
    }
}