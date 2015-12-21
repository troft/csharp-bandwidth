using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    /// <summary>
    /// The Phone Numbers resource lets you get phone numbers for use with your programs and manage numbers you already have.
    /// </summary>
    public class PhoneNumber: BaseModel
    {
        private const string PhoneNumberPath = "phoneNumbers";

        private static readonly Regex PhoneNumberIdExtractor = new Regex(@"/" + PhoneNumberPath + @"/([\w\-_]+)$");

        /// <summary>
        /// Gets information about one of your numbers using the number's ID.
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="id">Id of number</param>
        /// <returns>Number information</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/phonenumbers/#GET-/v1/users/{userId}/phoneNumbers/{numberString}"/>
        public static async Task<PhoneNumber> Get(Client client, string id)
        {
            if (id == null) throw new ArgumentNullException("id");
            var item = await client.MakeGetRequest<PhoneNumber>(client.ConcatUserPath(PhoneNumberPath), null, id);
            item.Client = client;
            return item;
        }

        /// <summary>
        /// Gets information about one of your numbers using the number's ID.
        /// </summary>
        /// <param name="id">Id of number</param>
        /// <returns>Number information</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/phonenumbers/#GET-/v1/users/{userId}/phoneNumbers/{numberString}"/>
        public static Task<PhoneNumber> Get(string id)
        {
            return Get(Client.GetInstance(), id);
        }

        /// <summary>
        /// Gets a list of your numbers. 
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="query">Query parameters</param>
        /// <returns>List of phone numbers</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/phonenumbers/#GET-/v1/users/{userId}/phoneNumbers"/>
        public static async Task<PhoneNumber[]> List(Client client, IDictionary<string, object> query = null)
        {
            var items = await client.MakeGetRequest<PhoneNumber[]>(client.ConcatUserPath(PhoneNumberPath), query) ?? new PhoneNumber[0];
            foreach (var item in items)
            {
                item.Client = client;
            }
            return items;
        }

        /// <summary>
        /// Gets a list of your numbers. 
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="page">Page number</param>
        /// <param name="size">Page size</param>
        /// <returns>List of phone numbers</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/phonenumbers/#GET-/v1/users/{userId}/phoneNumbers"/>
        public static Task<PhoneNumber[]> List(Client client, int page, int size = 25)
        {
            var query = new Dictionary<string, object> {{"page", page}, {"size", size}};
            return List(client, query);
        }

        /// <summary>
        /// Gets a list of your numbers. 
        /// </summary>
        /// <param name="query">Query parameters</param>
        /// <returns>List of phone numbers</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/phonenumbers/#GET-/v1/users/{userId}/phoneNumbers"/>
        public static Task<PhoneNumber[]> List(IDictionary<string, object> query = null)
        {
            return List(Client.GetInstance(), query);
        }

        /// <summary>
        /// Gets a list of your numbers. 
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="size">Page size</param>
        /// <returns>List of phone numbers</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/phonenumbers/#GET-/v1/users/{userId}/phoneNumbers"/>
        public static Task<PhoneNumber[]> List(int page, int size = 25)
        {
            return List(Client.GetInstance(), page, size);
        }

        /// <summary>
        /// Allocates a number so you can use it to make and receive calls and send and receive messages.
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="parameters">Parameters to create new number</param>
        /// <returns>Created number</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/phonenumbers/#POST-/v1/users/{userId}/phoneNumbers"/>
        public static async Task<PhoneNumber> Create(Client client, IDictionary<string, object> parameters)
        {
            using (var response = await client.MakePostRequest(client.ConcatUserPath(PhoneNumberPath), parameters))
            {
                var match = (response.Headers.Location != null)
                    ? PhoneNumberIdExtractor.Match(response.Headers.Location.OriginalString)
                    : null;
                if (match == null)
                {
                    throw new Exception("Missing id in response");
                }
                return await Get(client, match.Groups[1].Value);
            }
        }


        /// <summary>
        /// Allocates a number so you can use it to make and receive calls and send and receive messages.
        /// </summary>
        /// <param name="parameters">Parameters to create new number</param>
        /// <returns>Created number</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/phonenumbers/#POST-/v1/users/{userId}/phoneNumbers"/>
        public static Task<PhoneNumber> Create(IDictionary<string, object> parameters)
        {
            return Create(Client.GetInstance(), parameters);
        }



        /// <summary>
        /// Makes changes to a number you have.
        /// </summary>
        /// <param name="parameters">Changed parameters</param>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/phonenumbers/#POST-/v1/users/{userId}/phoneNumbers/{numberId}"/>
        public Task Update(IDictionary<string, object> parameters)
        {
            return Client.MakePostRequest(Client.ConcatUserPath(string.Format("{0}/{1}", PhoneNumberPath, Id)),
                parameters, true);
        }

        /// <summary>
        /// Removes a number from your account so you can no longer make or receive calls, or send or receive messages with it
        /// </summary>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/phonenumbers/#DELETE-/v1/users/{userId}/phoneNumbers/{numberId}"/>
        public Task Delete()
        {
           return Client.MakeDeleteRequest(Client.ConcatUserPath(string.Format("{0}/{1}", PhoneNumberPath, Id)));
        }
        
        /// <summary>
        /// The URI of the application associated with the number
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// The telephone number in E.164 format
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// The telephone number in a friendly national format
        /// </summary>
        public string NationalNumber { get; set; }

        /// <summary>
        /// Number to transfer an incoming call when the callback/fallback events can't be delivered
        /// </summary>
        public string FallbackNumber { get; set; }

        /// <summary>
        /// The telephone number in E.164 format
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Date when the number was created. 
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }
        
        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; }
        
        /// <summary>
        /// Price
        /// </summary>
        public double Price { get; set; }
        
        /// <summary>
        /// Number state
        /// </summary>
        public PhoneNumberState NumberState { get; set; }
    }

    /// <summary>
    /// Number states
    /// </summary>
    public enum PhoneNumberState
    {
        Enabled,
        Released
    }
    
}