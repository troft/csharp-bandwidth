using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    /// <summary>
    /// The Available Numbers resource lets you search for numbers that are available for use with your application.
    /// </summary>
    /// <seealso href="https://catapult.inetwork.com/docs/api-docs/available-numbers/"/>
    public class AvailableNumber
    {
        private const string AvailableNumbersTollFreePath = "availableNumbers/tollFree";
        private const string AvailableNumbersLocalPath = "availableNumbers/local";

        /// <summary>
        /// Search for available toll free numbers
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="query">Dictionary with optional keys: quantity, pattern</param>
        /// <returns>Array of available numbers</returns> 
        /// <example>
        /// <code>
        /// var numbers = await AvailableNumber.SearchTollFree(client, new Dictionary&lt;string, object&gt;{{"quantity", 20}});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/available-numbers/#GET-/v1/availableNumbers/tollFree"/>
        public static Task<AvailableNumber[]> SearchTollFree(Client client, Dictionary<string, object> query = null)
        {
            return client.MakeGetRequest<AvailableNumber[]>(AvailableNumbersTollFreePath, query);
        }

        /// <summary>
        /// Search for available local numbers
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="query">Dictionary with optional keys: city, state, zip, areaCode, localNumber, inLocalCallingArea, quantity, pattern</param>
        /// <returns>Array of available numbers</returns> 
        /// <example>
        /// <code>
        /// var numbers = await AvailableNumber.SearchLocal(client, new Dictionary&lt;string, object&gt;{{"state", "NC"}, {"zip", "27306"}});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/available-numbers/#local-get"/>
        public static Task<AvailableNumber[]> SearchLocal(Client client, Dictionary<string, object> query = null)
        {
            return client.MakeGetRequest<AvailableNumber[]>(AvailableNumbersLocalPath, query);
        }

        /// <summary>
        /// Search for available toll free numbers
        /// </summary>
        /// <param name="query">Dictionary with optional keys: quantity, pattern</param>
        /// <returns>Array of available numbers</returns> 
        /// <example>
        /// <code>
        /// var numbers = await AvailableNumber.SearchTollFree(client, new Dictionary&lt;string, object&gt;{{"quantity", 20}});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/available-numbers/#GET-/v1/availableNumbers/tollFree"/>
        public static Task<AvailableNumber[]> SearchTollFree(Dictionary<string, object> query = null)
        {
            return SearchTollFree(Client.GetInstance(), query);
        }

        /// <summary>
        /// Search for available local numbers
        /// </summary>
        /// <param name="query">Dictionary with optional keys: city, state, zip, areaCode, localNumber, inLocalCallingArea, quantity, pattern</param>
        /// <returns>Array of available numbers</returns> 
        /// <example>
        /// <code>
        /// var numbers = await AvailableNumber.SearchLocal(new Dictionary&lt;string, object&gt;{{"state", "NC"}, {"zip", "27306"}});
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/available-numbers/#local-get"/>
        public static Task<AvailableNumber[]> SearchLocal(Dictionary<string, object> query = null)
        {
            return SearchLocal(Client.GetInstance(), query);
        }

        /// <summary>
        /// The telephone number in E.164 format
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// The telephone number in a friendly national format
        /// </summary>
        public string NationalNumber { get; set; }

        /// <summary>
        /// The telephone number in a friendly national format with some numbers replaced by letters if a pattern was used to search the number
        /// </summary>
        public string PatternMatch { get; set; }

        /// <summary>
        /// The city of the phone number
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Local access and transport area (LATA), represents an area within which a regional operating company is permitted to offer exchange telecommunications and exchange access services.
        /// </summary>
        public string Lata { get; set; }

        /// <summary>
        /// The rate center is a term used to identify a telephone local exchange service area.
        /// </summary>
        public string RateCenter { get; set; }

        /// <summary>
        /// The state of the phone number
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// The monthly price for this number
        /// </summary>
        public decimal Price { get; set; }
    }
}
