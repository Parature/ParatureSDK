using System;

namespace ParatureSDK.ParaHelper
{
    internal class ParaEnumProvider
    {
        /// <summary>
        /// Returns the Http request method for an API call, requires an ApiCallHttpMethod enum.
        /// </summary>
        internal static String ApiHttpPostProvider(ParaEnums.ApiCallHttpMethod ApiCallHttpMethod)
        {
            string httpPostMethod = "";
            switch (ApiCallHttpMethod)
            {
                case ParaEnums.ApiCallHttpMethod.Get:
                    httpPostMethod = "GET";
                    break;

                case ParaEnums.ApiCallHttpMethod.Delete:
                    httpPostMethod = "DELETE";
                    break;

                case ParaEnums.ApiCallHttpMethod.Post:
                    httpPostMethod = "POST";
                    break;

                case ParaEnums.ApiCallHttpMethod.Update:
                    httpPostMethod = "PUT";
                    break;
            }
            return httpPostMethod;
        }
    }
}