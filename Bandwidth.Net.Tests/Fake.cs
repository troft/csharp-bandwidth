using System;
using System.Collections.Generic;
using System.Linq;
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
    public class Fake
    {
        public const string UserId = "FakeUserId";
        public const string ApiKey = "FakeApiKey";
        public const string Secret = "FakeSecret";
        public const string Host = "FakeHost";

        public static Net.Client CreateClient()
        {
            return new Net.Client(UserId, ApiKey, Secret, Host);
        }

        public static StringContent CreateJsonContent(object data)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            settings.Converters.Add(new StringEnumConverter{CamelCaseText = true, AllowIntegerValues = false});
            return new StringContent(JsonConvert.SerializeObject(data, Formatting.Indented, settings), Encoding.UTF8, "application/json");
        }

        public static void AssertObjects(object estimated, object value)
        {
            var type = estimated.GetType();
            foreach(var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var est = property.GetValue(estimated);
                var val = property.GetValue(value);
                Assert.AreEqual(est, val);
            }
        }
    }
}
