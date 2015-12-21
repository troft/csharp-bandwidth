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
            "User Client.GetInstance(<userId>, <apiToken>, <apiSecret>) or Client.GlobalOptions to set up them.")
        {
        }
    }
}
