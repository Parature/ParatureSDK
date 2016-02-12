using System;
using System.Collections;
using ParatureSDK.ParaObjects;

namespace ParatureSDK
{
    internal static class ApiUrlBuilder
    {
        /// <summary>
        /// Build the API URL to call, Add a simple customString to be appended at the end of the call. Used for special instances, like requesting an upload URL.
        /// </summary>
        internal static string ApiObjectCustomUrl<TEntity>(ParaCredentials paracredentials, string customstring)
        {
            if (string.IsNullOrEmpty(customstring))
            {
                customstring = "/";
            }
            else
            {
                customstring = "/" + customstring;
            }

            string apiCallUrl = ApiBuildBasicUrl(paracredentials) + typeof(TEntity).Name + customstring + "?" +
                                ApiToken(paracredentials);
            return apiCallUrl;
        }

        internal static string ApiObjectCustomUrl<TModule, TEntity>(ParaCredentials paracredentials, ArrayList arguments) 
            where TModule: ParaEntity
            where TEntity: ParaEntityBaseProperties
        {
            var apiCallUrl = ApiBuildBasicUrl(paracredentials) + typeof(TModule).Name + "/" + typeof(TEntity).Name.ToLower() + "?" + ApiToken(paracredentials);
            if (arguments.Count > 0)
            {
                for (int i = 0; i <= arguments.Count - 1; i++)
                {
                    apiCallUrl = apiCallUrl + "&" + arguments[i];
                }
            }
            return apiCallUrl;
        }

        /// <summary>
        /// Build the API URL to call. Since a simple read (without any further options) is the same as an update, as well as a delete, this method will generate that same link for these operations. Also, indicates whether you are requesting a schema a link, or an actual operation link
        /// </summary>
        internal static string ApiObjectUrl(ParaCredentials paracredentials, string module, Int64 objectId,
            bool isSchema)
        {
            string customstring;
            if (isSchema)
            {
                customstring = "/schema";
            }
            else
            {
                if (objectId == 0)
                {
                    customstring = "";
                }
                else
                {
                    customstring = "/" + objectId;
                }
            }
            var apiCallUrl = ApiBuildBasicUrl(paracredentials) + module + customstring + "?" + ApiToken(paracredentials);
            return apiCallUrl;
        }

        internal static string ApiChatTranscriptUrl(ParaCredentials creds, Int64 chatId)
        {
            var entityName = "Chat";
            var customstring = string.Format("/{0}/transcript", chatId);

            var apiCallUrl = ApiBuildBasicUrl(creds) + entityName + customstring + "?" +
                             ApiToken(creds);
            return apiCallUrl;
        }

        internal static string ApiObjectUrl(ParaCredentials paracredentials, string module, Int64 objectId,
            ArrayList arguments)
        {
            var apiCallUrl = ApiObjectUrl(paracredentials, module, objectId, false);
            if (arguments != null)
            {
                if (arguments.Count > 0)
                {
                    for (var i = 0; i <= arguments.Count - 1; i++)
                    {
                        apiCallUrl = apiCallUrl + "&" + arguments[i];
                    }
                }
            }
            return apiCallUrl;
        }

        /// <summary>
        /// Build the basic link to the APIs. This link is independant from what you will be doing with the APIs.
        /// </summary>
        private static string ApiBuildBasicUrl(ParaCredentials paracredentials)
        {
            var basicLink = paracredentials.ServerfarmAddress + "/api/" + paracredentials.Apiversion + "/" +
                            paracredentials.Instanceid + "/" + paracredentials.Departmentid + "/";

            return basicLink;
        }

        /// <summary>
        /// Simply returns the token query string that needs to be attached to the API URL. Will return something like: _token_=xxxx
        /// </summary>
        private static string ApiToken(ParaCredentials paracredentials)
        {
            var apiTokenQs = "_token_=" + paracredentials.Token;
            return apiTokenQs;
        }
    }
}