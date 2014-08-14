using System;

namespace Bandwidth.Net.Data
{
    public class Application
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Uri IncomingCallUrl { get; set; }
        public int? IncomingCallUrlCallbackTimeout { get; set; }
        public Uri IncomingCallFallbackUrl { get; set; }
        public Uri IncomingSmsUrl { get; set; }
        public int? IncomingSmsUrlCallbackTimeout { get; set; }
        public Uri IncomingSmsFallbackUrl { get; set; }
        public string CallbackHttpMethod { get; set; }
        public bool? AutoAnswer { get; set; }
    }

    public class ApplicationQuery : Query
    {
    }
}