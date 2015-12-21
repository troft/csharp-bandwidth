using System;
using System.Net;

namespace Bandwidth.Net
{
    public sealed class BandwidthException: Exception
    {
        public HttpStatusCode Code { get; private set; }

        public BandwidthException(string message, HttpStatusCode code): base(message)
        {
            Code = code;
        }
    }
}
