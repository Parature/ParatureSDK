using System;
using System.Collections;

namespace ParatureSDK
{
    internal static class ApiUrlBuilder
    {
        /// <summary>
        /// Build the API URL to call, Add a simple customString to be appended at the end of the call. Used for special instances, like requesting an upload URL.
        /// </summary>
        internal static string ApiObjectReadUpdateDeleteCustomUrl(ParaCredentials paracredentials, ParaEnums.ParatureModule module, string customstring)
        {
            if (string.IsNullOrEmpty(customstring))
            {
                customstring = "/";
            }
            else
            {
                customstring = "/" + customstring;
            }
            
            string ApiCallUrl = ApiBuildBasicUrl(paracredentials) + module.ToString() + customstring + "?" + ApiToken(paracredentials);
            return ApiCallUrl;
        }


        internal static string ApiObjectReadUpdateDeleteCustomUrl(ParaCredentials paracredentials, ParaEnums.ParatureModule module, ParaEnums.ParatureEntity entity, ArrayList Arguments)
        {
            string ApiCallUrl = ApiBuildBasicUrl(paracredentials) + module.ToString() + "/" + entity.ToString() + "?" + ApiToken(paracredentials);
            if (Arguments.Count > 0)
            {
                for (int i = 0; i <= Arguments.Count - 1; i++)
                {
                    ApiCallUrl = ApiCallUrl + "&" + Arguments[i].ToString();
                }
            }
            return ApiCallUrl;
        }

        internal static string ApiObjectReadUpdateDeleteCustomUrl(ParaCredentials paracredentails, ParaEnums.ParatureEntity entity, string customstring)
        {
            if (string.IsNullOrEmpty(customstring))
            {
                customstring = "/";
            }
            else
            {
                customstring = "/" + customstring;
            }
            string ApiCallUrl = ApiBuildBasicUrl(paracredentails) + customstring + "?" + ApiToken(paracredentails);
            return ApiCallUrl;
        }

        /// <summary>
        /// Build the API URL to call, Add a simple customString to be appended at the end of the call. This is not a module specific call.
        /// </summary>
        internal static string ApiObjectReadUpdateDeleteCustomUrl(ParaCredentials paracredentials, string customstring)
        {
            if (string.IsNullOrEmpty(customstring))
            {
                customstring = "/";
            }
            else
            {
                customstring = "/" + customstring;
            }

            string ApiCallUrl = ApiBuildBasicUrl(paracredentials) + customstring + "?" + ApiToken(paracredentials);
            return ApiCallUrl;
        }


        /// <summary>
        /// Build the API URL to call. Since a simple read (without any further options) is the same as an update, as well as a delete, this method will generate that same link for these operations. Also, indicates whether you are requesting a schema a link, or an actual operation link
        /// </summary>
        internal static string ApiObjectReadUpdateDeleteUrl(ParaCredentials paracredentials, ParaEnums.ParatureModule module, Int64 objectId, bool IsSchema)
        {

            //string objectnum;
            string customstring;
            if (IsSchema)
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
                    customstring = "/" + objectId.ToString();
                }
            }
            var apiCallUrl = ApiBuildBasicUrl(paracredentials) + module.ToString() + customstring + "?" + ApiToken(paracredentials);
            return apiCallUrl;
        }


        /// <summary>
        /// Build the API URL to call. Since a simple read (without any further options) is the same as an update, as well as a delete, this method will generate that same link for these operations. Also, indicates whether you are requesting a schema a link, or an actual operation link
        /// </summary>
        internal static string ApiObjectReadUpdateDeleteUrl(ParaCredentials paracredentials, ParaEnums.ParatureEntity entity, Int64 objectId, bool IsSchema)
        {

            //string objectnum;
            string customstring;
            string entityName = entity.ToString();
            if (IsSchema == true)
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
                        customstring = "/" + objectId.ToString();
                    }
                    else
                    {
                        entityName = "Chat";
                        customstring = string.Format("/{0}/transcript", objectId.ToString());
                    }
                }
            }
            string ApiCallUrl = ApiBuildBasicUrl(paracredentials) + entityName + customstring + "?" + ApiToken(paracredentials);
            return ApiCallUrl;
        }

        /// <summary>
        /// Build the API URL to call. Since a simple read (without any further options) is the same as an update, as well as a delete, this method will generate that same link for these operations. Also, indicates whether you are requesting a schema a link, or an actual operation link
        /// </summary>
        internal static string ApiObjectReadUpdateDeleteUrl(ParaCredentials paracredentials, ParaEnums.ParatureEntity entity, Int64 objectId, ArrayList Arguments)
        {

            string ApiCallUrl = ApiObjectReadUpdateDeleteUrl(paracredentials, entity, objectId, false);
            if (Arguments.Count > 0)
            {
                for (int i = 0; i <= Arguments.Count - 1; i++)
                {
                    ApiCallUrl = ApiCallUrl + "&" + Arguments[i].ToString();
                }
            }


            return ApiCallUrl;
        }

        /// <summary>
        /// Build the API URL to call. Since a simple read (without any further options) is the same as an update, as well as a delete, this method will generate that same link for these operations. This methods accepts a list of extra arguments to pass to the API url through the query string.
        /// </summary>
        internal static string ApiObjectReadUpdateDeleteUrl(ParaCredentials paracredentials, ParaEnums.ParatureModule module, Int64 objectId, ArrayList Arguments)
        {
            string ApiCallUrl = ApiObjectReadUpdateDeleteUrl(paracredentials, module, objectId, false);
            if (Arguments != null)
            {
                if (Arguments.Count > 0)
                {
                    for (int i = 0; i <= Arguments.Count - 1; i++)
                    {
                        ApiCallUrl = ApiCallUrl + "&" + Arguments[i].ToString();
                    }
                }
            }
            return ApiCallUrl;
        }

        /// <summary>
        /// Build the basic link to the APIs. This link is independant from what you will be doing with the APIs.
        /// </summary>
        static string ApiBuildBasicUrl(ParaCredentials paracredentials)
        {
            string basicLink = paracredentials.ServerfarmAddress + "/api/" + paracredentials.Apiversion + "/" + paracredentials.Instanceid + "/" + paracredentials.Departmentid + "/";

            return basicLink;

        }

        /// <summary>
        /// Simply returns the token query string that needs to be attached to the API URL. Will return something like: _token_=xxxx
        /// </summary>
        static string ApiToken(ParaCredentials paracredentials)
        {
            string ApiTokenQS = "_token_=" + paracredentials.Token.ToString();
            return ApiTokenQS;

        }

    }
}
