using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public class Recording: BaseModel
    {
        private const string RecordingPath = "recordings";
        
        public static async Task<Recording> Get(Client client, string id)
        {
            if (id == null) throw new ArgumentNullException("id");
            var item = await client.MakeGetRequest<Recording>(client.ConcatUserPath(RecordingPath), null, id);
            item.Client = client;
            return item;
        }
#if !PCL
        public static Task<Recording> Get(string id)
        {
            return Get(Client.GetInstance(), id);
        }
#endif

        public static async Task<Recording[]> List(Client client, IDictionary<string, object> query = null)
        {
            var items = await client.MakeGetRequest<Recording[]>(client.ConcatUserPath(RecordingPath), query) ?? new Recording[0];
            foreach (var item in items)
            {
                item.Client = client;
            }
            return items;
        }

        public static Task<Recording[]> List(Client client, int page, int size = 25)
        {
            var query = new Dictionary<string, object> { { "page", page }, { "size", size } };
            return List(client, query);
        }

#if !PCL
        public static Task<Recording[]> List(IDictionary<string, object> parameters = null)
        {
            return List(Client.GetInstance(), parameters);
        }

        public static Task<Recording[]> List(int page, int size = 25)
        {
            return List(Client.GetInstance(), page, size);
        }
#endif
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Call { get; set; }

        public string Media { get; set; }

        public RecordingState State { get; set; }
    }

    public enum RecordingState
    {
        Recording,
        Complete,
        Saving,
        Error
    }

}
