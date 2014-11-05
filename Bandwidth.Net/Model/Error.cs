using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public class Error: BaseModel
    {
        private const string ErrorPath = "errors";


        public static async Task<Error> Get(Client client, string id)
        {
            if (id == null) throw new ArgumentNullException("id");
            var item = await client.MakeGetRequest<Error>(client.ConcatUserPath(ErrorPath), null, id);
            item.Client = client;
            return item;
        }
#if !PCL        
        public static Task<Error> Get(string callId)
        {
            return Get(Client.GetInstance(), callId);
        }
#endif

        public static async Task<Error[]> List(Client client, IDictionary<string, object> query = null)
        {
            var items = await client.MakeGetRequest<Error[]>(client.ConcatUserPath(ErrorPath), query) ?? new Error[0];
            foreach (var item in items)
            {
                item.Client = client;
            }
            return items;
        }

        public static Task<Error[]> List(Client client, int page, int size = 25)
        {
            var query = new Dictionary<string, object> {{"page", page}, {"size", size}};
            return List(client, query);
        }

#if !PCL        
        public static Task<Error[]> List(IDictionary<string, object> parameters = null)
        {
            return List(Client.GetInstance(), parameters);
        }

        public static Task<Error[]> List(int page, int size = 25)
        {
            return List(Client.GetInstance(), page, size);
        }
#endif

        public DateTime Time { get; set; }
        public ErrorCategory Category { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
        public ErrorDetail[] Details { get; set; }
    }

    public class ErrorDetail
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public enum ErrorCategory
    {
        Authentication,
        Authorization,
        [EnumMember(Value = "not-found")]
        NotFound,
        [EnumMember(Value = "bad-request")]
        BadRequest,
        Conflict,
        Unavailable,
        Credit,
        Limit,
        Forbidden,
        Payment
    }
}