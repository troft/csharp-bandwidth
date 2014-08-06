using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Bandwidth.Net
{
    public sealed class Client: IDisposable
    {
        private readonly HttpClient _client;
        private readonly string _userPath;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        private const string MessagesPath = "messages";
        private const string PhoneNumbersPath = "phoneNumbers";
        private const string AvailableNumbersPath = "availableNumbers";
        private const string CallsPath = "calls";
        private const string RecordingsPath = "recordings";
        private const string ApplicationsPath = "applications";
        private const string PortInAvailablePath = "portInAvailable";
        private const string PortInPath = "portIns";
        private const string CallAudioPath = "audio";

        public Client(string userId, string apiToken, string secret, string host = "api.catapult.inetwork.com")
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (apiToken == null) throw new ArgumentNullException("apiToken");
            if (secret == null) throw new ArgumentNullException("secret");
            _userPath = string.Format("/users/{0}", userId);
            _client = new HttpClient {BaseAddress = new UriBuilder("https", host, 443, "/v1").Uri};
            _client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", apiToken, secret))));
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            _jsonSerializerSettings.Converters.Add(new StringEnumConverter{CamelCaseText = true, AllowIntegerValues = false});
        }
        #region Base Http methods
        internal Task<HttpResponseMessage> MakeGetRequest(string path, IDictionary<string, string> query, string id = null)
        {
            var urlPath = path;
            if(id != null)
            {
                urlPath = urlPath + "/" + id;
            }
            if (query.Count > 0)
            {
                urlPath = string.Format("{0}?{1}", urlPath, string.Join("&", from p in query select string.Format("{0}={1}", p.Key, Uri.EscapeDataString(p.Value))));
            }
            return _client.GetAsync(urlPath).ContinueWith((task) =>
            {
                task.Result.EnsureSuccessStatusCode();
                return task.Result;
            });
        }

        internal Task<HttpResponseMessage> MakePostRequest(string path, object data)
        {

            var json = JsonConvert.SerializeObject(data, Formatting.None, _jsonSerializerSettings);
            return _client.PostAsync(path, new StringContent(json, Encoding.UTF8, "application/json")).ContinueWith((task) =>
            {
                task.Result.EnsureSuccessStatusCode();
                return task.Result;
            });
        }

        internal Task<HttpResponseMessage> MakeDeleteRequest(string path)
        {
            return _client.DeleteAsync(path).ContinueWith((task) =>
            {
                task.Result.EnsureSuccessStatusCode();
                return task.Result;
            });
        }
        #endregion


        private string ConcatUserPath(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException("path");
            if (path[0] == '/')
            {
                return _userPath + path;
            }
            return string.Format("{0}/{1}", _userPath, path);
        }

        public Task<HttpResponseMessage> MakeCall(Call call)
        {
            return MakePostRequest(ConcatUserPath(CallsPath), call);
        }

        public Task<HttpResponseMessage> ChangeCall(string callId, CallData data)
        {
            return MakePostRequest(ConcatUserPath(string.Format("{0}/{1}", CallsPath, callId)), data);
        }




        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
