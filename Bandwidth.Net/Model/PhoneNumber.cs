using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public class PhoneNumber: BaseModel
    {
        private const string PhoneNumberPath = "phoneNumbers";

        private static readonly Regex PhoneNumberIdExtractor = new Regex(@"/" + PhoneNumberPath + @"/([\w\-_]+)$");

        public static async Task<PhoneNumber> Get(Client client, string id)
        {
            if (id == null) throw new ArgumentNullException("id");
            var item = await client.MakeGetRequest<PhoneNumber>(client.ConcatUserPath(PhoneNumberPath), null, id);
            item.Client = client;
            return item;
        }
#if !PCL        
        public static Task<PhoneNumber> Get(string callId)
        {
            return Get(Client.GetInstance(), callId);
        }
#endif

        public static async Task<PhoneNumber[]> List(Client client, IDictionary<string, object> query = null)
        {
            var items = await client.MakeGetRequest<PhoneNumber[]>(client.ConcatUserPath(PhoneNumberPath), query) ?? new PhoneNumber[0];
            foreach (var item in items)
            {
                item.Client = client;
            }
            return items;
        }

        public static Task<PhoneNumber[]> List(Client client, int page, int size = 25)
        {
            var query = new Dictionary<string, object> {{"page", page}, {"size", size}};
            return List(client, query);
        }

#if !PCL        
        public static Task<PhoneNumber[]> List(IDictionary<string, object> parameters = null)
        {
            return List(Client.GetInstance(), parameters);
        }

        public static Task<PhoneNumber[]> List(int page, int size = 25)
        {
            return List(Client.GetInstance(), page, size);
        }
#endif


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


#if !PCL        
        public static Task<PhoneNumber> Create(IDictionary<string, object> parameters)
        {
            return Create(Client.GetInstance(), parameters);
        }

#endif



        public Task Update(IDictionary<string, object> parameters)
        {
            return Client.MakePostRequest(Client.ConcatUserPath(string.Format("{0}/{1}", PhoneNumberPath, Id)),
                parameters, true);
        }

        public Task Delete()
        {
           return Client.MakeDeleteRequest(Client.ConcatUserPath(string.Format("{0}/{1}", PhoneNumberPath, Id)));
        }
        public string Application { get; set; }
        public string Number { get; set; }
        public string NationalNumber { get; set; }
        public string FallbackNumber { get; set; }
        public string Name { get; set; }
        public DateTime CreatedTime { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public double Price { get; set; }
        public PhoneNumberState NumberState { get; set; }
    }

    public enum PhoneNumberState
    {
        Enabled,
        Released
    }
    
}