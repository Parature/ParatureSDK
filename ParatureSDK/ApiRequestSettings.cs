using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace ParatureSDK
{
    /// <summary>
    /// Global settings for API requests. Runs across EACH API request made.
    /// </summary>
    public static class ApiRequestSettings
    {
        private static ParaCredentials _creds;

        /// <summary>
        /// Delegate to modify the HttpWebRequest object on each request.
        /// Useful to specify a proxy, timeouts, certificates, or any other part of the object.
        /// Use at your own risk.
        /// </summary>
        public static Action<HttpClientHandler> GlobalPreRequest = (handler) => { };
    }
}