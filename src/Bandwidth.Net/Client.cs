using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace Bandwidth.Net
{
    public class Client: IClient
    {
        private readonly string _userId;
        private readonly string _apiToken;
        private readonly string _apiSecret;
        private static readonly ProductInfoHeaderValue _userAgent = BuildUserAgent();
        
        public Client(string userId, string apiToken, string apiSecret)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(apiToken) || string.IsNullOrEmpty(apiSecret))
            {
                throw new MissingCredentialsException();
            }
            _userId = userId;
            _apiToken = apiToken;
            _apiSecret = apiSecret;
        }

        private HttpClient CreateHttpClient()
        {
            var url = new UriBuilder("https://api.catapult.inetwork.com") { Path = "/v1/" };
            var client = new HttpClient { BaseAddress = url.Uri };
            var assembly = typeof(Client).GetTypeInfo().Assembly;
            var assemblyName = new AssemblyName(assembly.FullName);
            client.DefaultRequestHeaders.UserAgent.Add(_userAgent);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_apiToken}:{_apiSecret}")));
            return client;
        }

        private static ProductInfoHeaderValue BuildUserAgent()
        {
            var assembly = typeof(Client).GetTypeInfo().Assembly;
            var assemblyName = new AssemblyName(assembly.FullName);
            return new ProductInfoHeaderValue("csharp-bandwidth", $"v{assemblyName.Version.Major}.{assemblyName.Version.Minor}");
        }

    }

    public interface IClient 
    {
    }
}
