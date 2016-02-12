using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.VisualBasic;
using ParatureSDK.Fields;
using ParatureSDK.ParaObjects;
using Attachment = System.Net.Mail.Attachment;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ParatureSDK
{
    /// <summary>
    /// The APICallFactory manages all calls made to the APIs server. No API calls should be made outside of this class.
    /// </summary>
    static internal class ApiCallFactory
    {
        public static ApiCallResponse ObjectCreateUpdate<T>(ParaCredentials paracredentials, XmlDocument fileToPost, Int64 objectid)
        {
            // Calling the next method that manages the call.
            return ObjectCreateUpdate<T>(paracredentials, fileToPost, objectid, null);
        }

        internal static async Task<ApiCallResponse> ObjectCreateUpdate(ParaCredentials paracredentials, string entityType,
            XmlDocument fileToPost, Int64 objectid)
        {
            return await ObjectCreateUpdate(paracredentials, entityType, fileToPost, objectid, null);
        }

        public static ApiCallResponse ObjectCreateUpdate<T>(ParaCredentials paracredentials, XmlDocument fileToPost,
            Int64 objectid, ArrayList arguments)
        {
            return ObjectCreateUpdate(paracredentials, typeof (T).Name, fileToPost, objectid, arguments).Result;
        }

        internal static async Task<ApiCallResponse> ObjectCreateUpdate(ParaCredentials paracredentials, string entityType, XmlDocument fileToPost, Int64 objectid, ArrayList arguments)
        {
            if (arguments == null)
            {
                arguments = new ArrayList();
            }
            switch (entityType)
            {
                case "Ticket":
                case "Account":
                case "Customer":
                case "Product":
                case "Asset":
                    if (paracredentials.EnforceRequiredFields == false)
                    {
                        arguments.Add("_enforceRequiredFields_=" + paracredentials.EnforceRequiredFields.ToString().ToLower());
                    }
                    break;
            }
            // Getting the standard API URL to call.
            var apiCallUrl = ApiUrlBuilder.ApiObjectUrl(paracredentials, entityType, objectid, arguments);

            // To set up the call method, we check if this is a create (the objectid=0 in that case)
            // or an update (when we received an objectid>0)
            var apicallhttpmethod = (objectid == 0)
                ? ParaEnums.ApiCallHttpMethod.Post
                : ParaEnums.ApiCallHttpMethod.Put;

            // Calling the next method that manages the call.
            return await ApiMakeTheCall(apiCallUrl, apicallhttpmethod, fileToPost);
        }

        public static ApiCallResponse ObjectDelete<T>(ParaCredentials paracredentials, Int64 objectid, bool purge)
        {
            return ObjectDelete(paracredentials, typeof (T).ToString(), objectid, purge);
        }

        internal static ApiCallResponse ObjectDelete(ParaCredentials paracredentials, string entityType, Int64 objectid,
            bool purge)
        {
            string apiCallUrl;

            if (purge)
            {
                var arguments = new ArrayList { "_purge_=true" };
                apiCallUrl = ApiUrlBuilder.ApiObjectUrl(paracredentials, entityType, objectid, arguments);
            }
            else
            {
                apiCallUrl = ApiUrlBuilder.ApiObjectUrl(paracredentials, entityType, objectid, false);
            }

            return ApiMakeTheCall(apiCallUrl, ParaEnums.ApiCallHttpMethod.Delete, null, null).Result;
        }

        ///  <summary>
        ///  Use this method to get the details of an object that you plan to fill.
        ///  </summary>
        ///  <param name="paracredentials">
        /// The credentials to be used for making the API call.
        /// Value Type: <see cref="ParaCredentials" />   (ParaConnect.ParaCredentials)
        /// </param>
        /// <param name="objectid">
        /// The id of the object to create or update.
        /// Value Type: <see cref="Int64" />   (System.int64)
        /// </param>
        public static ApiCallResponse ObjectGetDetail<T>(ParaCredentials paracredentials, Int64 objectid) where T: ParaEntityBaseProperties
        {
            return ObjectGetDetail<T>(paracredentials, objectid, new ArrayList());
        }

        ///  <summary>
        ///  Use this method to get the details of an object that you plan to fill.
        ///  </summary>
        ///  <param name="paracredentials">
        /// The credentials to be used for making the API call.
        /// Value Type: <see cref="ParaCredentials" />   (ParaConnect.ParaCredentials)
        /// </param>
        /// <param name="objectid">
        /// The id of the object to create or update.
        /// Value Type: <see cref="Int64" />   (System.int64)
        /// </param>
        public static ApiCallResponse ObjectGetDetail<T>(ParaCredentials paracredentials, Int64 objectid, ArrayList arguments) where T : ParaEntityBaseProperties
        {
            var entityName = typeof(T).Name;
            var apiCallUrl = ApiUrlBuilder.ApiObjectUrl(paracredentials, entityName, objectid, arguments);

            return ApiMakeTheCall(apiCallUrl, ParaEnums.ApiCallHttpMethod.Get, null, null).Result;
        }

        ///  <summary>
        ///  Use this method to get the details of an Entity that you plan to fill.
        ///  </summary>
        ///  <param name="paracredentials">
        /// The credentials to be used for making the API call.
        /// Value Type: <see cref="ParaCredentials" />   (ParaConnect.ParaCredentials)
        /// </param>
        /// <param name="objectid">
        /// The id of the object to create or update.
        /// Value Type: <see cref="Int64" />   (System.int64)
        /// </param>
        public static ApiCallResponse ChatTranscriptGetDetail(ParaCredentials paracredentials, Int64 objectid)
        {
            var apiCallUrl = ApiUrlBuilder.ApiChatTranscriptUrl(paracredentials, objectid);

            return ApiMakeTheCall(apiCallUrl, ParaEnums.ApiCallHttpMethod.Get, null, null).Result;
        }

        /// <summary>
        /// Use this method to get a list of objects that you plan to fill.
        /// </summary>
        /// <param name="paracredentials">
        ///The credentials to be used for making the API call.
        ///Value Type: <see cref="ParaCredentials" />   (ParaConnect.ParaCredentials)
        ///</param>
        /// <param name="module">
        ///The name of the module to create or update. Choose from the ParatureModule enum list.
        ///</param>
        /// <param name="arguments">
        ///The list of extra optional arguments you need to include in the call. For example, any fields filtering, any custom fields to include, etc.
        ///Value Type: <see cref="ArrayList" />
        ///</param>
        public static ApiCallResponse ObjectGetList<T>(ParaCredentials paracredentials, ArrayList arguments)
        {
            var entityType = typeof (T).Name;
            var apiCallUrl = ApiUrlBuilder.ApiObjectUrl(paracredentials, entityType, 0, arguments);
            return ApiMakeTheCall(apiCallUrl, ParaEnums.ApiCallHttpMethod.Get, null, null).Result;
        }

        public static ApiCallResponse ObjectSecondLevelGetList<TModule, TEntity>(ParaCredentials paracredentials, ArrayList arguments)
            where TModule: ParaEntity
            where TEntity: ParaEntityBaseProperties
        {
            var apiCallUrl = ApiUrlBuilder.ApiObjectCustomUrl<TModule, TEntity>(paracredentials, arguments);
            return ApiMakeTheCall(apiCallUrl, ParaEnums.ApiCallHttpMethod.Get, null, null).Result;
        }

        /// <summary>
        /// Use this method to get the Schema XML of an object.
        /// </summary>
        public static ApiCallResponse ObjectGetSchema<T>(ParaCredentials paracredentials)
        {
            var entityType = typeof (T).Name;
            var apiCallUrl = ApiUrlBuilder.ApiObjectUrl(paracredentials, entityType, 0, true);

            return ApiMakeTheCall(apiCallUrl, ParaEnums.ApiCallHttpMethod.Get, null, null).Result;
        }

        public static ApiCallResponse FileUploadGetUrl<TEntity>(ParaCredentials paracredentials) where TEntity: ParaEntity
        {
            return ApiMakeTheCall(ApiUrlBuilder.ApiObjectCustomUrl<TEntity>(paracredentials, "upload"), ParaEnums.ApiCallHttpMethod.Get).Result;
        }

        [Obsolete("To be removed in next major revision, use the FilePerformUpload(string, Byte[], string) overload instead.", false)]
        public static ApiCallResponse FilePerformUpload(string postUrl, Attachment attachment)
        {
            var filebytes = new byte[Convert.ToInt32(attachment.ContentStream.Length - 1) + 1];
            attachment.ContentStream.Read(filebytes, 0, filebytes.Length);
            return ApiMakeTheCall(postUrl, ParaEnums.ApiCallHttpMethod.Post, filebytes, attachment.ContentType.Name).Result;
        }

        [Obsolete("To be removed in next major revision, use the FilePerformUpload(string, Byte[], string) overload instead.", false)]
        public static ApiCallResponse FilePerformUpload(string postUrl, Byte[] attachment, string contentType, string fileName)
        {
            return ApiMakeTheCall(postUrl, ParaEnums.ApiCallHttpMethod.Post, attachment, fileName).Result;
        }

        public static ApiCallResponse FilePerformUpload(string postUrl, Byte[] attachment, string fileName)
        {
            return ApiMakeTheCall(postUrl, ParaEnums.ApiCallHttpMethod.Post, attachment, fileName).Result;
        }

        private static async Task<ApiCallResponse> ApiMakeTheCall(string apiCallUrl, ParaEnums.ApiCallHttpMethod httpMethod)
        {
            return await ApiMakeTheCall(apiCallUrl, httpMethod, null, null);
        }

        /// <summary>
        /// The true call is being made by this method.
        /// </summary>
        static async Task<ApiCallResponse> ApiMakeTheCall(string apiCallUrl, ParaEnums.ApiCallHttpMethod httpMethod, XmlDocument xmlPosted)
        {
            using (var handler = new HttpClientHandler())
            {
                ApiRequestSettings.GlobalPreRequest(handler);

                using (var client = new HttpClient(handler))
                {
                    client.Timeout = new TimeSpan(0, 2, 0);

                    switch (httpMethod)
                    {
                        case ParaEnums.ApiCallHttpMethod.Get:
                            using (var responseMsg = await client.GetAsync(apiCallUrl))
                            {
                                return await ApiHttpRequestProcessor(responseMsg);
                            }
                        case ParaEnums.ApiCallHttpMethod.Delete:
                            using (var responseMsg = await client.DeleteAsync(apiCallUrl))
                            {
                                return await ApiHttpRequestProcessor(responseMsg);
                            }
                        case ParaEnums.ApiCallHttpMethod.Post:
                            if (xmlPosted == null)
                            {
                                throw new ArgumentException("Invalid HTTP method, only GET or DELETE are supported without body content.", "httpMethod");
                            }
                            using (var content = new StringContent(xmlPosted.OuterXml))
                            {
                                using (var responseMsg = await client.PostAsync(apiCallUrl, content))
                                {
                                    return await ApiHttpRequestProcessor(responseMsg);
                                }
                            }
                        case ParaEnums.ApiCallHttpMethod.Put:
                            if (xmlPosted == null)
                            {
                                throw new ArgumentException("Invalid HTTP method, only GET or DELETE are supported without body content.", "httpMethod");
                            }
                            using (var content = new StringContent(xmlPosted.OuterXml))
                            {
                                using (var responseMsg = await client.PutAsync(apiCallUrl, content))
                                {
                                    return await ApiHttpRequestProcessor(responseMsg);
                                }
                            }
                        default:
                            throw new InvalidOperationException("Invalid HTTP method specified.");
                    }
                }
            }
        }

        /// <summary>
        /// The call, with passing a binary file.
        /// </summary>
        static async Task<ApiCallResponse> ApiMakeTheCall(string apiCallUrl, ParaEnums.ApiCallHttpMethod httpMethod, Byte[] attachment, string fileName)
        {
            using (var handler = new HttpClientHandler())
            {
                ApiRequestSettings.GlobalPreRequest(handler);

                using (var client = new HttpClient(handler))
                {
                    client.Timeout = new TimeSpan(0, 2, 0);

                    switch (httpMethod)
                    {
                        case ParaEnums.ApiCallHttpMethod.Get:
                            using (var responseMsg = await client.GetAsync(apiCallUrl))
                            {
                                return await ApiHttpRequestProcessor(responseMsg);
                            }
                        case ParaEnums.ApiCallHttpMethod.Delete:
                            using (var responseMsg = await client.DeleteAsync(apiCallUrl))
                            {
                                return await ApiHttpRequestProcessor(responseMsg);
                            }
                        case ParaEnums.ApiCallHttpMethod.Post:
                            if (attachment == null)
                            {
                                throw new ArgumentException("Invalid HTTP method, only GET or DELETE are supported without body content.", "httpMethod");
                            }
                            using (var content = new MultipartFormDataContent())
                            {
                                content.Add(new ByteArrayContent(attachment), fileName, fileName);
                                using (var responseMsg = await client.PostAsync(apiCallUrl, content))
                                {
                                    return await ApiHttpRequestProcessor(responseMsg);
                                }
                            }
                        case ParaEnums.ApiCallHttpMethod.Put:
                            if (attachment == null)
                            {
                                throw new ArgumentException("Invalid HTTP method, only GET or DELETE are supported without body content.", "httpMethod");
                            }
                            using (var content = new MultipartFormDataContent())
                            {
                                content.Add(new ByteArrayContent(attachment), fileName, fileName);
                                using (var responseMsg = await client.PutAsync(apiCallUrl, content))
                                {
                                    return await ApiHttpRequestProcessor(responseMsg);
                                }
                            }
                        default:
                            throw new InvalidOperationException("Invalid HTTP method specified.");
                    }
                }
            }
        }

        /// <summary>
        /// Performs the http web request for all ApiMakeCall methods.
        /// </summary>
        /// <param name="ac">
        /// Api Call response, this object is partially filled in the ApiMakeCall methods, this method will just be adding
        /// certain data to it and return it.
        /// </param>
        /// <param name="req">
        /// The http web Request object. Each ApiMakeCall method will have its own http webrequest information. This method will make
        /// the http call with the request object passed to it.
        /// </param>
        /// <returns></returns>
        static async Task<ApiCallResponse> ApiHttpRequestProcessor(HttpResponseMessage responseMsg)
        {
            var result = new ApiCallResponse()
            {
                CalledUrl = responseMsg.RequestMessage.RequestUri.ToString(),
                HttpCallMethod = responseMsg.RequestMessage.Method.Method
            };

            try
            {
                using (var content = responseMsg.RequestMessage.Content)
                {
                    result.XmlSent.Load(await content.ReadAsStreamAsync());
                }

                try
                {
                    result.HttpResponseCode = (int)responseMsg.StatusCode;
                }
                catch
                {
                    result.HttpResponseCode = -1;
                }

                string responseContent = null;

                using (var content = responseMsg.Content)
                {
                    responseContent = await content.ReadAsStringAsync();
                }

                try
                {
                    result.XmlReceived.LoadXml(responseContent);
                }
                catch (Exception)
                {
                    result.XmlReceived = null;
                }

                if (result.HttpResponseCode == 201)
                {
                    Int64.TryParse(result.XmlReceived?.DocumentElement?.Attributes["id"]?.Value, out result.Id);
                }

                result.HasException = false;
            }
            catch (WebException ex)
            {
                try
                {
                    result.HttpResponseCode = (int)((((HttpWebResponse)ex.Response).StatusCode));
                    result.ExceptionDetails = ex.Message;
                }
                catch
                { }
                result.HasException = true;

                if (String.IsNullOrEmpty(result.ExceptionDetails) == true)
                {
                    result.ExceptionDetails = ex.ToString();
                }

                var responseFromServer = await responseMsg.Content.ReadAsStringAsync();
                if (!String.IsNullOrEmpty(responseFromServer))
                {
                    result.ExceptionDetails = "Response from server: " + responseFromServer;
                }


                string exresponseFromServer = "";
                try
                {
                    var exreader = new StreamReader(ex.Response.GetResponseStream());
                    exresponseFromServer = exreader.ReadToEnd().ToString();
                    exreader.Close();

                    if (String.IsNullOrEmpty(exresponseFromServer) == false)
                    {
                        result.ExceptionDetails = result.ExceptionDetails + Environment.NewLine + "Exception response:" + exresponseFromServer;
                    }

                }
                catch (Exception exread)
                {
                    if (result.HttpResponseCode == 0)
                    {
                        result.HttpResponseCode = 503;
                    }
                }

                if (String.IsNullOrEmpty(exresponseFromServer) == false)
                {
                    try
                    {
                        result.XmlReceived.LoadXml(exresponseFromServer);

                        XmlNode mainnode = result.XmlReceived.DocumentElement;
                        if (mainnode.LocalName.ToLower() == "error")
                        {
                            if (mainnode.Attributes["code"].InnerText.ToLower() != "")
                            {
                                result.HttpResponseCode = Int32.Parse(mainnode.Attributes["code"].InnerText.ToString());
                            }
                            if (mainnode.Attributes["message"].InnerText.ToLower() != "")
                            {
                                result.ExceptionDetails = mainnode.Attributes["message"].InnerText.ToString();
                            }
                        }
                    }
                    catch (Exception exp)
                    {
                        result.XmlReceived = null;
                    }
                }
                else
                {
                    result.XmlReceived = null;
                }
            }
            finally
            {
                // xml sent and xml received cleanup
                if (result.XmlReceived != null && String.IsNullOrEmpty(result.XmlReceived.InnerXml))
                {
                    result.XmlReceived = null;
                }
                if (result.XmlSent != null && String.IsNullOrEmpty(result.XmlSent.InnerXml))
                {
                    result.XmlSent = null;
                }
            }

            return result;
        }
    }
}
