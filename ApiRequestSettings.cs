using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ParatureSDK
{
    public static class ApiRequestSettings
    {
        public static Action<HttpWebRequest> GlobalPreRequest = (request) => { };
    }
}
