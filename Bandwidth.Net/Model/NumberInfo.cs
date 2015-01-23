using System;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    /// <summary>
    /// This resource provides a CNAM number info. CNAM is an acronym which stands for Caller ID Name.
    /// </summary>
    public class NumberInfo
    {
        private const string NumberInfoPath = "phoneNumbers/numberInfo";
        
        /// <summary>
        /// Get the CNAM info of a number
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="number">Phone number</param>
        /// <returns>Number information</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/cnam/#GET-/v1/phoneNumbers/numberInfo/{number}"/>
        public static Task<NumberInfo> Get(Client client, string number)
        {
            if (number == null) throw new ArgumentNullException("number");
            return client.MakeGetRequest<NumberInfo>(NumberInfoPath, null, Uri.EscapeDataString(number));
        }
#if !PCL        
        /// <summary>
        /// Get the CNAM info of a number
        /// </summary>
        /// <param name="number">Phone number</param>
        /// <returns>Number information</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/cnam/#GET-/v1/phoneNumbers/numberInfo/{number}"/>
        public static Task<NumberInfo> Get(string number)
        {
            return Get(Client.GetInstance(), number);
        }
#endif
        /// <summary>
        /// The Caller ID name information
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The full phone number, specified in E.164 format
        /// </summary>
        public string Number { get; set; }
        
        /// <summary>
        /// The time this Caller ID information was first queried
        /// </summary>
        public DateTime Created { get; set; }
        
        /// <summary>
        /// The time this Caller ID information was last queried
        /// </summary>
        public DateTime Updated { get; set; }
    }
    
}