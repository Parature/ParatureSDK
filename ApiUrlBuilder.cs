using System;
using System.Collections;

namespace ParatureSDK
{
    internal static class ApiUrlBuilder
    {
        /// <summary>
        /// Build the API URL to call, Add a simple customString to be appended at the end of the call. Used for special instances, like requesting an upload URL.
        /// </summary>
        internal static string ApiObjectCustomUrl(ParaCredentials paracredentials, ParaEnums.ParatureModule module, string customstring)
        {
            if (string.IsNullOrEmpty(customstring))
            {
                customstring = "/";
            }
            else
            {
                customstring = "/" + customstring;
            }
            
            string apiCallUrl = ApiBuildBasicUrl(paracredentials) + module + customstring + "?" + ApiToken(paracredentials);
            return apiCallUrl;
        }

        internal static string ApiObjectCustomUrl(ParaCredentials paracredentials, ParaEnums.ParatureModule module, ParaEnums.ParatureEntity entity, ArrayList arguments)
        {
            var apiCallUrl = ApiBuildBasicUrl(paracredentials) + module + "/" + entity + "?" + ApiToken(paracredentials);
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
        internal static string ApiObjectUrl(ParaCredentials paracredentials, string module, Int64 objectId, bool isSchema)
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

        /// <summary>
        /// Build the API URL to call. Since a simple read (without any further options) is the same as an update, as well as a delete, this method will generate that same link for these operations. Also, indicates whether you are requesting a schema a link, or an actual operation link
        /// </summary>
        internal static string ApiObjectUrl(ParaCredentials paracredentials, ParaEnums.ParatureEntity entity, Int64 objectId, bool isSchema)
        {
            string customstring;
            string entityName = entity.ToString();
            if (isSchema == true)
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
                    if (entity != ParaEnums.ParatureEntity.ChatTranscript)
                    {
                        customstring = "/" + objectId;
                    }
                    else
                    {
                        entityName = "Chat";
                        customstring = string.Format("/{0}/transcript", objectId);
                    }
                }
            }
            var apiCallUrl = ApiBuildBasicUrl(paracredentials) + entityName + customstring + "?" + ApiToken(paracredentials);
            return apiCallUrl;
        }

        /// <summary>
        /// Build the API URL to call. Since a simple read (without any further options) is the same as an update, as well as a delete, this method will generate that same link for these operations. Also, indicates whether you are requesting a schema a link, or an actual operation link
        /// </summary>
        internal static string ApiObjectUrl(ParaCredentials paracredentials, ParaEnums.ParatureEntity entity, Int64 objectId, ArrayList arguments)
        {

            var apiCallUrl = ApiObjectUrl(paracredentials, entity, objectId, false);
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
        /// Build the API URL to call. Since a simple read (without any further options) is the same as an update, as well as a delete, this method will generate that same link for these operations. This methods accepts a list of extra arguments to pass to the API url through the query string.
        /// </summary>
        internal static string ApiObjectUrl(ParaCredentials paracredentials, ParaEnums.ParatureModule module, Int64 objectId, ArrayList arguments)
        {
            var apiCallUrl = ApiObjectUrl(paracredentials, module.ToString(), objectId, false);
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

        internal static string ApiObjectUrl(ParaCredentials paracredentials, string module, Int64 objectId, ArrayList arguments)
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
        static string ApiBuildBasicUrl(ParaCredentials paracredentials)
        {
            var basicLink = paracredentials.ServerfarmAddress + "/api/" + paracredentials.Apiversion + "/" + paracredentials.Instanceid + "/" + paracredentials.Departmentid + "/";

            return basicLink;
        }

        /// <summary>
        /// Simply returns the token query string that needs to be attached to the API URL. Will return something like: _token_=xxxx
        /// </summary>
        static string ApiToken(ParaCredentials paracredentials)
        {
            var apiTokenQs = "_token_=" + paracredentials.Token;
            return apiTokenQs;
        }
    }
}
