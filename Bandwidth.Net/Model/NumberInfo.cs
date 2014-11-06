using System;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public class NumberInfo
    {
        private const string NumberInfoPath = "phoneNumbers/numberInfo";
        
        public static Task<NumberInfo> Get(Client client, string number)
        {
            if (number == null) throw new ArgumentNullException("number");
            return client.MakeGetRequest<NumberInfo>(NumberInfoPath, null, Uri.EscapeDataString(number));
        }
#if !PCL        
        public static Task<NumberInfo> Get(string number)
        {
            return Get(Client.GetInstance(), number);
        }
#endif
        public string Name { get; set; }
        public string Number { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
    
}