using System;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Bandwidth.Net.Tests
{
    public class Helper
    {
        public const string UserId = "FakeUserId";
        public const string ApiKey = "FakeApiKey";
        public const string Secret = "FakeSecret";
        
        public static Client CreateClient(string baseUrl = null)
        {
            return Client.GetInstance(UserId, ApiKey, Secret, baseUrl ?? "http://localhost:3001/", "v1");
        }

        public static StringContent CreateJsonContent(object data)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            settings.Converters.Add(new StringEnumConverter{CamelCaseText = true, AllowIntegerValues = false});
            settings.TypeNameHandling = TypeNameHandling.Objects;
            return new StringContent(JsonConvert.SerializeObject(data, Formatting.Indented, settings), Encoding.UTF8, "application/json");
        }

        public async static Task<T> ParseJsonContent<T>(HttpContent content)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            settings.Converters.Add(new StringEnumConverter { CamelCaseText = true, AllowIntegerValues = false });
            var json = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        public static void AssertObjects(object estimated, object value)
        {
            var type = estimated.GetType();
            foreach(var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var est = property.GetValue(estimated);
                var val = property.GetValue(value);
                Assert.AreEqual(est, val, string.Format("Values of property {0} are mismatched", property.Name));
            }
        }

        public static string ToJsonString(object data)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            settings.Converters.Add(new StringEnumConverter{CamelCaseText = true, AllowIntegerValues = false});
            settings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
            return JsonConvert.SerializeObject(data, Formatting.None, settings);
        }

        public static void SetEnvironmetVariables(string baseUrl = null)
        {
            Client.GlobalOptions = new ClientOptions
            {
                UserId = UserId,
                ApiToken = ApiKey,
                ApiSecret = Secret,
                ApiEndpoint = baseUrl ?? "http://localhost:3001/",
                ApiVersion = "v1"
            };
        }
    }
}
