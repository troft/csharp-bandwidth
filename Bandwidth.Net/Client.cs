using System;
using System.Collections.Generic;
using System.IO;
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
    public sealed class Client
    {
        private readonly string _apiToken;
        private readonly string _secret;
        private readonly string _apiEndpoint;
        private readonly string _apiVersion;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private readonly string _userPath;

        public static Client GetInstance(string userId, string apiToken, string secret, string apiEndpoint = "https://api.catapult.inetwork.com", string apiVersion = "v1")
        {
            return new Client(userId, apiToken, secret, apiEndpoint, apiVersion);
        }

        
#if !PCL
        public const string BandwidthUserId = "BANDWIDTH_USER_ID";
        public const string BandwidthApiToken = "BANDWIDTH_API_TOKEN";
        public const string BandwidthApiSecret = "BANDWIDTH_API_SECRET";
        public const string BandwidthApiEndpoint = "BANDWIDTH_API_ENDPOINT";
        public const string BandwidthApiVersion = "BANDWIDTH_API_VERSION";

        public static Client GetInstance()
        {
            return GetInstance(Environment.GetEnvironmentVariable(BandwidthUserId),
                Environment.GetEnvironmentVariable(BandwidthApiToken),
                Environment.GetEnvironmentVariable(BandwidthApiSecret),
                Environment.GetEnvironmentVariable(BandwidthApiEndpoint),
                Environment.GetEnvironmentVariable(BandwidthApiVersion));
        }

        
#endif
        private Client(string userId, string apiToken, string secret, string apiEndpoint, string apiVersion)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (apiToken == null) throw new ArgumentNullException("apiToken");
            if (secret == null) throw new ArgumentNullException("secret");
            if (apiEndpoint == null) throw new ArgumentNullException("apiEndpoint");
            if (apiVersion == null) throw new ArgumentNullException("apiVersion");
            _apiToken = apiToken;
            _secret = secret;
            _apiEndpoint = apiEndpoint;
            _apiVersion = apiVersion;
            _userPath = string.Format("users/{0}", userId);
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            _jsonSerializerSettings.Converters.Add(new StringEnumConverter
            {
                CamelCaseText = true,
                AllowIntegerValues = false
            });
            _jsonSerializerSettings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
        }

        private HttpClient CreateHttpClient()
        {
            var url = new UriBuilder(_apiEndpoint) { Path = string.Format("/{0}/", _apiVersion) };
            var client = new HttpClient { BaseAddress = url.Uri };
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", _apiToken, _secret))));
            return client;
        }

        #region Base Http methods

        internal async Task<HttpResponseMessage> MakeGetRequest(string path, IDictionary<string, object> query = null,
            string id = null, bool disposeResponse = false)
        {
            var urlPath = FixPath(path);
            if (id != null)
            {
                urlPath = urlPath + "/" + id;
            }
            if (query != null && query.Count > 0)
            {
                urlPath = string.Format("{0}?{1}", urlPath,
                    string.Join("&",
                        from p in query select string.Format("{0}={1}", p.Key, Uri.EscapeDataString(Convert.ToString(p.Value)))));
            }
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync(urlPath);
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch
                {
                    response.Dispose();
                    throw;
                }
                if (!disposeResponse) return response;
                response.Dispose();
                return null;
            }
        }


        internal async Task<TResult> MakeGetRequest<TResult>(string path, IDictionary<string, object> query = null,
            string id = null)
        {
            using (var response = await MakeGetRequest(path, query, id))
            {
                if (response.Content.Headers.ContentType != null &&
                    response.Content.Headers.ContentType.MediaType == "application/json")
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return json.Length > 0
                        ? JsonConvert.DeserializeObject<TResult>(json, _jsonSerializerSettings)
                        : default(TResult);
                }
            }
            return default(TResult);
        }

        internal async Task MakeGetRequestToObject(object targetObject, string path, IDictionary<string, object> query = null,
            string id = null)
        {
            using (var response = await MakeGetRequest(path, query, id))
            {
                if (response.Content.Headers.ContentType != null &&
                    response.Content.Headers.ContentType.MediaType == "application/json")
                {
                    var json = await response.Content.ReadAsStringAsync();
                    if (json.Length > 0)
                    {
                        JsonConvert.PopulateObject(json, targetObject, _jsonSerializerSettings);
                    }
                }
            }
        }

        internal async Task<HttpResponseMessage> MakePostRequest(string path, object data, bool disposeResponse = false)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.None, _jsonSerializerSettings);
            using (var client = CreateHttpClient())
            {
                var response =
                    await client.PostAsync(FixPath(path), new StringContent(json, Encoding.UTF8, "application/json"));
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch
                {
                    response.Dispose();
                    throw;
                }
                if (!disposeResponse) return response;
                response.Dispose();
                return null;
            }
        }

        internal async Task<HttpResponseMessage> PutData(string path, Stream stream, string mediaType,
            bool disposeResponse = false)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            return await PutFileContent(path, mediaType, disposeResponse, new StreamContent(stream));
        }

        internal async Task<HttpResponseMessage> PutData(string path, byte[] buffer, string mediaType,
            bool disposeResponse = false)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            return await PutFileContent(path, mediaType, disposeResponse, new ByteArrayContent(buffer));
        }

        private async Task<HttpResponseMessage> PutFileContent(string path, string mediaType, bool disposeResponse,
            HttpContent content)
        {
            if (mediaType != null)
            {
                content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            }
            using (var client = CreateHttpClient())
            {
                var response = await client.PutAsync(FixPath(path), content);
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch
                {
                    response.Dispose();
                    throw;
                }
                if (!disposeResponse) return response;
                response.Dispose();
                return null;
            }
        }


        internal async Task<TResult> MakePostRequest<TResult>(string path, object data)
        {
            using (var response = await MakePostRequest(path, data))
            {
                if (response.Content.Headers.ContentType.MediaType == "application/json")
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return json.Length > 0
                        ? JsonConvert.DeserializeObject<TResult>(json, _jsonSerializerSettings)
                        : default(TResult);
                }
            }
            return default(TResult);
        }

        internal async Task MakeDeleteRequest(string path)
        {
            using (var client = CreateHttpClient())
            using (var response = await client.DeleteAsync(FixPath(path)))
            {
                response.EnsureSuccessStatusCode();
            }
        }

        #endregion


        internal string ConcatUserPath(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException("path");
            if (path[0] == '/')
            {
                return _userPath + path;
            }
            return string.Format("{0}/{1}", _userPath, path);
        }

        private static string FixPath(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException("path");
            return (path[0] == '/') ? path.Substring(1) : path;
        }
    }
}