using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
