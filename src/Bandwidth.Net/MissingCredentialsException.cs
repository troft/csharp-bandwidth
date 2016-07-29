using System;
using System.Net;

namespace Bandwidth.Net
{
    /// <summary>
    /// MissingCredentialsException
    /// </summary>
    public sealed class MissingCredentialsException: Exception
    {
        
        /// <summary>
        /// MissingCredentialsException
        /// </summary>
        public MissingCredentialsException()
            : base("Missing credentials.\n" + 
            "Use new Client(<userId>, <apiToken>, <apiSecret>) to set up them.")
        {
        }
    }
}
