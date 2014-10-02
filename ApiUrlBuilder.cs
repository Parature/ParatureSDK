using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ParatureAPI
{
    internal static class ApiUrlBuilder
    {
        /// <summary>
        /// Build the API URL to call, Add a simple customString to be appended at the end of the call. Used for special instances, like requesting an upload URL.
        /// </summary>
        internal static string ApiObjectReadUpdateDeleteCustomUrl(ParaCredentials paracredentials, Paraenums.ParatureModule module, string customstring)
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


        internal static string ApiObjectReadUpdateDeleteCustomUrl(ParaCredentials paracredentials, Paraenums.ParatureModule module, Paraenums.ParatureEntity entity, ArrayList Arguments)
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

        internal static string ApiObjectReadUpdateDeleteCustomUrl(ParaCredentials paracredentails, Paraenums.ParatureEntity entity, string customstring)
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
        internal static string ApiObjectReadUpdateDeleteUrl(ParaCredentials paracredentials, Paraenums.ParatureModule module, Int64 objectid, bool IsSchema)
        {

            //string objectnum;
            string customstring;
            if (IsSchema == true)
            {
                customstring = "/schema";

            }
            else
            {
                if (objectid == 0)
                {
                    customstring = "";
                }
                else
                {
                    customstring = "/" + objectid.ToString();
                }
            }
            string ApiCallUrl = ApiBuildBasicUrl(paracredentials) + module.ToString() + customstring + "?" + ApiToken(paracredentials);
            return ApiCallUrl;
        }


        /// <summary>
        /// Build the API URL to call. Since a simple read (without any further options) is the same as an update, as well as a delete, this method will generate that same link for these operations. Also, indicates whether you are requesting a schema a link, or an actual operation link
        /// </summary>
        internal static string ApiObjectReadUpdateDeleteUrl(ParaCredentials paracredentials, Paraenums.ParatureEntity entity, Int64 objectid, bool IsSchema)
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
                if (objectid == 0)
                {
                    customstring = "";
                }
                else
                {
                    if (entity != Paraenums.ParatureEntity.ChatTranscript)
                    {
                        customstring = "/" + objectid.ToString();
                    }
                    else
                    {
                        entityName = "Chat";
                        customstring = string.Format("/{0}/transcript", objectid.ToString());
                    }
                }
            }
            string ApiCallUrl = ApiBuildBasicUrl(paracredentials) + entityName + customstring + "?" + ApiToken(paracredentials);
            return ApiCallUrl;
        }

        /// <summary>
        /// Build the API URL to call. Since a simple read (without any further options) is the same as an update, as well as a delete, this method will generate that same link for these operations. Also, indicates whether you are requesting a schema a link, or an actual operation link
        /// </summary>
        internal static string ApiObjectReadUpdateDeleteUrl(ParaCredentials paracredentials, Paraenums.ParatureEntity entity, Int64 objectid, ArrayList Arguments)
        {

            string ApiCallUrl = ApiObjectReadUpdateDeleteUrl(paracredentials, entity, objectid, false);
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
        internal static string ApiObjectReadUpdateDeleteUrl(ParaCredentials paracredentials, Paraenums.ParatureModule module, Int64 objectid, ArrayList Arguments)
        {
            string ApiCallUrl = ApiObjectReadUpdateDeleteUrl(paracredentials, module, objectid, false);
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
        /// Build the basic link to Delete an object from the APIs.
        /// </summary>
        static string ApiDeleteObjectUrl(ParaCredentials paracredentials, Paraenums.ParatureModule module, Int64 objectid)
        {
            return ApiObjectReadUpdateDeleteUrl(paracredentials, module, objectid, false);
        }

        /// <summary>
        /// Build the basic link to read an object from the APIs.
        /// </summary>
        static string ApiReadObjectUrl(ParaCredentials paracredentials, Paraenums.ParatureModule module, Int64 objectid)
        {
            return ApiObjectReadUpdateDeleteUrl(paracredentials, module, objectid, false);
        }

        /// <summary>
        /// Build the basic link to update an object using the APIs.
        /// </summary>
        static string ApiUpdateObjectUrl(ParaCredentials paracredentials, Paraenums.ParatureModule module, Int64 objectid)
        {
            return ApiObjectReadUpdateDeleteUrl(paracredentials, module, objectid, false);
        }

        /// <summary>
        /// Build the basic link to the APIs. This link is independant from what you will be doing with the APIs.
        /// </summary>
        static string ApiBuildBasicUrl(ParaCredentials paracredentials)
        {
            string basicLink = paracredentials.ServerfarmAddress.ToString() + "api/" + paracredentials.Apiversion + "/" + paracredentials.Accountid + "/" + paracredentials.Departmentid + "/";

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
