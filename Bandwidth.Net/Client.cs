using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Bandwidth.Net
{
    public sealed partial class Client: IDisposable
    {
        private readonly HttpClient _client;
        private readonly string _userPath;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        private const string MessagesPath = "messages";
        private const string PhoneNumbersPath = "phoneNumbers";
        private const string AvailableNumbersPath = "availableNumbers";
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
        internal async Task<HttpResponseMessage> MakeGetRequest(string path, IDictionary<string, string> query = null, string id = null)
        {
            var urlPath = path;
            if(id != null)
            {
                urlPath = urlPath + "/" + id;
            }
            if (query != null && query.Count > 0)
            {
                urlPath = string.Format("{0}?{1}", urlPath, string.Join("&", from p in query select string.Format("{0}={1}", p.Key, Uri.EscapeDataString(p.Value))));
            }
            var response = await _client.GetAsync(urlPath);
            response.EnsureSuccessStatusCode();
            return response;
        }

        internal async Task<TResult> MakeGetRequest<TResult>(string path, IDictionary<string, string> query = null,
            string id = null)
        {
            var response = await MakeGetRequest(path, query, id);
            if (response.Content.Headers.ContentType.MediaType == "application/json")
            {
                var json = await response.Content.ReadAsStringAsync();
                return json.Length > 0 ? JsonConvert.DeserializeObject<TResult>(json, _jsonSerializerSettings) : default(TResult);
            }
            return default(TResult);
        }

        internal async Task<HttpResponseMessage> MakePostRequest(string path, object data)
        {

            var json = JsonConvert.SerializeObject(data, Formatting.None, _jsonSerializerSettings);
            var response = await _client.PostAsync(path, new StringContent(json, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            return response;
        }

        internal async Task<TResult> MakePostRequest<TResult>(string path, object data)
        {
            var response = await MakePostRequest(path, data);
            if (response.Content.Headers.ContentType.MediaType == "application/json")
            {
                var json = await response.Content.ReadAsStringAsync();
                return json.Length > 0 ? JsonConvert.DeserializeObject<TResult>(json, _jsonSerializerSettings) : default(TResult);
            }
            return default(TResult);
        }

        internal async Task<HttpResponseMessage> MakeDeleteRequest(string path)
        {
            var response = await _client.DeleteAsync(path);
            response.EnsureSuccessStatusCode();
            return response;
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

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
