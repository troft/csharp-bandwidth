using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Bandwidth.Net
{
    public sealed class Client
    {
        private readonly string _apiToken;
        private readonly string _apiSecret;
        private readonly string _apiEndpoint;
        private readonly string _apiVersion;
        internal readonly JsonSerializerSettings JsonSerializerSettings;
        private readonly string _userPath;

        public static Client GetInstance(string userId = null, string apiToken = null, string apiSecret = null, string apiEndpoint = null, string apiVersion = null)
        {
            return new Client(userId, apiToken, apiSecret, apiEndpoint, apiVersion);
        }


#if !PCL
        public const string BandwidthUserId = "BANDWIDTH_USER_ID";
        public const string BandwidthApiToken = "BANDWIDTH_API_TOKEN";
        public const string BandwidthApiSecret = "BANDWIDTH_API_SECRET";
        public const string BandwidthApiEndpoint = "BANDWIDTH_API_ENDPOINT";
        public const string BandwidthApiVersion = "BANDWIDTH_API_VERSION";

#endif
        public static ClientOptions GlobalOptions { get; set; }
        private Client(string userId, string apiToken, string apiSecret, string apiEndpoint, string apiVersion)
        {
            if (GlobalOptions == null)
            {
                GlobalOptions = new ClientOptions();
#if !PCL
                GlobalOptions.UserId = Environment.GetEnvironmentVariable(BandwidthUserId);
                GlobalOptions.ApiToken = Environment.GetEnvironmentVariable(BandwidthApiToken);
                GlobalOptions.ApiSecret = Environment.GetEnvironmentVariable(BandwidthApiSecret);
                GlobalOptions.ApiEndpoint = Environment.GetEnvironmentVariable(BandwidthApiEndpoint);
                GlobalOptions.ApiVersion = Environment.GetEnvironmentVariable(BandwidthApiVersion);
#endif

            }
            userId = userId ?? GlobalOptions.UserId;
            _apiToken = apiToken ?? GlobalOptions.ApiToken;
            _apiSecret = apiSecret ?? GlobalOptions.ApiSecret;
            _apiEndpoint = apiEndpoint ?? GlobalOptions.ApiEndpoint ?? "https://api.catapult.inetwork.com";
            _apiVersion = apiVersion ?? GlobalOptions.ApiVersion ?? "v1";
            _userPath = string.Format("users/{0}", userId);
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(_apiToken) || string.IsNullOrEmpty(_apiSecret))
            {
                throw new MissingCredentialsException();
            }
            JsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolverInternal()
            };
            JsonSerializerSettings.Converters.Add(new StringEnumConverter
            {
                CamelCaseText = true,
                AllowIntegerValues = false
            });
            JsonSerializerSettings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
        }

        private HttpClient CreateHttpClient()
        {
            var url = new UriBuilder(_apiEndpoint) { Path = string.Format("/{0}/", _apiVersion) };
            var client = new HttpClient { BaseAddress = url.Uri };
            var assembly = typeof(Client).GetTypeInfo().Assembly;
            // In some PCL profiles the above line is: var assembly = typeof(Client).Assembly;
            var assemblyName = new AssemblyName(assembly.FullName);
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("csharp-bandwidth", string.Format("v{0}.{1}", assemblyName.Version.Major, assemblyName.Version.Minor)));
                  client.DefaultRequestHeaders.Authorization =
                      new AuthenticationHeaderValue("Basic",
                          Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", _apiToken, _apiSecret))));
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
                    await CheckResponse(response);
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

        internal async Task<HttpResponseMessage> MakeHeadRequest(string path, IDictionary<string, object> query = null,
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
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Head,
                    RequestUri = new Uri(client.BaseAddress + urlPath)
                };
                var response = await client.SendAsync(request);
                try
                {
                    await CheckResponse(response);
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
                        ? JsonConvert.DeserializeObject<TResult>(json, JsonSerializerSettings)
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
                        JsonConvert.PopulateObject(json, targetObject, JsonSerializerSettings);
                    }
                }
            }
        }

        internal async Task<HttpResponseMessage> MakePostRequest(string path, object data, bool disposeResponse = false)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.None, JsonSerializerSettings);
            using (var client = CreateHttpClient())
            {
                var response =
                    await client.PostAsync(FixPath(path), new StringContent(json, Encoding.UTF8, "application/json"));
                try
                {
                    await CheckResponse(response);
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
                    await CheckResponse(response);
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
                        ? JsonConvert.DeserializeObject<TResult>(json, JsonSerializerSettings)
                        : default(TResult);
                }
            }
            return default(TResult);
        }

        internal async Task MakeDeleteRequest(string path, string id = null)
        {
            if (id != null)
            {
                path = path + "/" + id;
            }
            using (var client = CreateHttpClient())
            using (var response = await client.DeleteAsync(FixPath(path)))
            {
                await CheckResponse(response);
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

        private async Task CheckResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                try
                {
                    var msg = JsonConvert.DeserializeAnonymousType(json, new {Message = "", Code=""}, JsonSerializerSettings);
                    var message = msg.Message ?? msg.Code;
                    if (!string.IsNullOrEmpty(message))
                    {
                        throw new BandwidthException(message, response.StatusCode);
                    }
                }
                catch(Exception ex)
                {
                    if (ex is BandwidthException) throw;
                    Debug.WriteLine(ex.Message);
                }
                throw new BandwidthException(json, response.StatusCode);
            }
        }
    }

  internal class CamelCasePropertyNamesContractResolverInternal : CamelCasePropertyNamesContractResolver
  {
    protected override string ResolvePropertyName(string propertyName)
    {
      if (propertyName.StartsWith("X-"))
      {
        return propertyName; // don't change fields `X-ABCD`
      }
      return base.ResolvePropertyName(propertyName);
    }
  }

  public class ClientOptions
    {
        public string UserId { get; set; }
        public string ApiToken { get; set; }
        public string ApiSecret { get; set; }
        public string ApiVersion { get; set; }
        public string ApiEndpoint { get; set; }
    }
}
