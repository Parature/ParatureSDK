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

        public static ApiCallResponse ObjectCreateUpdate<T>(ParaCredentials paracredentials, XmlDocument fileToPost, Int64 objectid, ArrayList arguments)
        {
            var entityType = typeof (T).ToString();
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
                : ParaEnums.ApiCallHttpMethod.Update;

            // Calling the next method that manages the call.
            return ApiMakeTheCall(apiCallUrl, apicallhttpmethod, fileToPost);
        }

        public static ApiCallResponse ObjectDelete<T>(ParaCredentials paracredentials, Int64 objectid, bool purge)
        {
            var entityType = typeof (T).ToString();
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


            return ApiMakeTheCall(apiCallUrl, ParaEnums.ApiCallHttpMethod.Delete);
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

            return ApiMakeTheCall(apiCallUrl, ParaEnums.ApiCallHttpMethod.Get);
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

            return ApiMakeTheCall(apiCallUrl, ParaEnums.ApiCallHttpMethod.Get);
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
            var entityType = typeof (T).ToString();
            var apiCallUrl = ApiUrlBuilder.ApiObjectUrl(paracredentials, entityType, 0, arguments);
            return ApiMakeTheCall(apiCallUrl, ParaEnums.ApiCallHttpMethod.Get);
        }

        public static ApiCallResponse ObjectSecondLevelGetList<TModule, TEntity>(ParaCredentials paracredentials, ArrayList arguments)
            where TModule: ParaEntity
            where TEntity: ParaEntityBaseProperties
        {
            var apiCallUrl = ApiUrlBuilder.ApiObjectCustomUrl<TModule, TEntity>(paracredentials, arguments);
            return ApiMakeTheCall(apiCallUrl, ParaEnums.ApiCallHttpMethod.Get);
        }

        /// <summary>
        /// Use this method to get the Schema XML of an object.
        /// </summary>
        public static ApiCallResponse ObjectGetSchema<T>(ParaCredentials paracredentials)
        {
            var entityType = typeof (T).ToString();
            var apiCallUrl = ApiUrlBuilder.ApiObjectUrl(paracredentials, entityType, 0, true);

            return ApiMakeTheCall(apiCallUrl, ParaEnums.ApiCallHttpMethod.Get);
        }

        /// <summary>
        /// Use this method to determine if any custom fields have custom validation
        /// </summary>
        public static T ObjectCheckCustomFieldTypes<T>(ParaCredentials paracredentials, ParaEntity baseObject) where T: ParaEntity, new()
        {
            var entityType = typeof (T).ToString();
            paracredentials.EnforceRequiredFields = false;
            var apiCallUrl = ApiUrlBuilder.ApiObjectUrl(paracredentials, entityType, 0, true);

            if (baseObject.CustomFields != null)
            {
                foreach (var cf in baseObject.CustomFields
                    .Where(cf => cf.FieldType == "string"))
                {
                    cf.Value = "a";
                }
            }

            var doc = new XmlDocument();
            switch (entityType)
            {
                case "Account":
                    doc = XmlGenerator.GenerateXml((Account)baseObject);
                    break;
                case "Customer":
                    doc = XmlGenerator.GenerateXml((Customer)baseObject);
                    break;
                case "Product":
                    doc = XmlGenerator.GenerateXml((Product)baseObject);
                    break;
                case "Asset":
                    doc = XmlGenerator.GenerateXml((Asset)baseObject);
                    break;
                case "Ticket":
                    doc = XmlGenerator.GenerateXml((Ticket)baseObject);
                    break;
                default:
                    break;
            }

            var ar = ApiMakeTheCall(apiCallUrl, ParaEnums.ApiCallHttpMethod.Post, doc);

            if (ar.HasException)
            {
                string errors = ar.ExceptionDetails;

                string[] errorLines = errors.Split(';');

                foreach (string line in errorLines)
                {

                    //added below line, since the validation message is changed for the API call
                    var tempLine = line.Contains("Short Name") 
                        ? line.Remove(line.IndexOf(", Short Name"), line.IndexOf("]") - line.IndexOf(", Short Name")) 
                        : line;

                    tempLine = tempLine.ToLower().Trim();

                    string id;

                    if (tempLine.StartsWith("invalid field validation message"))
                    {

                        if (line.ToLower().Contains("us phone number"))
                        {
                            var m = Regex.Match(line, @"Invalid Field Validation Message : (.+?) is not a valid US phone number");
                            id = m.Groups[1].Value.Trim();

                            foreach (CustomField cf in baseObject.CustomFields)
                            {
                                if (cf.Id == Int64.Parse(id))
                                {
                                    cf.FieldType = "usdate";
                                }
                            }
                        }
                        else if (tempLine.Contains("email address"))
                        {
                            Match m = Regex.Match(line, @"Invalid Field Validation Message : The Email Address '(.+?)' is not valid.");
                            id = m.Groups[1].Value.Trim();

                            foreach (CustomField cf in baseObject.CustomFields)
                            {
                                if (cf.Id == Int64.Parse(id))
                                {
                                    cf.FieldType = "email";
                                }
                            }
                        }
                        else if (tempLine.Contains("international phone number"))
                        {
                            Match m = Regex.Match(line, @"Invalid Field Validation Message : (.+?) is not a valid international phone number");
                            id = m.Groups[1].Value.Trim();

                            foreach (CustomField cf in baseObject.CustomFields)
                            {
                                if (cf.Id == Int64.Parse(id))
                                {
                                    cf.FieldType = "internationalphone";
                                }
                            }
                        }
                        else if (tempLine.Contains("url"))
                        {
                            Match m = Regex.Match(line, @"Invalid Field Validation Message : (.+?) is an invalid URL.");
                            id = m.Groups[1].Value.Trim();
                            if (Information.IsNumeric(id.Trim()))
                            {
                                foreach (CustomField cf in baseObject.CustomFields)
                                {
                                    if (cf.Id == Int64.Parse(id.Trim()))
                                    {
                                        cf.FieldType = "url";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                // The call was successfull, deleting the item
                apiCallUrl = ApiUrlBuilder.ApiObjectUrl(paracredentials, entityType, ar.Id, false);
                ar = ApiMakeTheCall(apiCallUrl, ParaEnums.ApiCallHttpMethod.Delete);

                // purging the item
                ArrayList arguments = new ArrayList();
                arguments.Add("_purge_=true");
                apiCallUrl = ApiUrlBuilder.ApiObjectUrl(paracredentials, entityType, ar.Id, arguments);
                ar = ApiMakeTheCall(apiCallUrl, ParaEnums.ApiCallHttpMethod.Delete);
            }

            return baseObject as T;
        }

        public static ApiCallResponse FileUploadGetUrl<TEntity>(ParaCredentials paracredentials) where TEntity: ParaEntity
        {
            var resp = ApiMakeTheCall(ApiUrlBuilder.ApiObjectCustomUrl<TEntity>(paracredentials, "upload"), ParaEnums.ApiCallHttpMethod.Get);
            return resp;
        }

        public static ApiCallResponse FilePerformUpload(string postUrl, Attachment attachment)
        {
            return ApiMakeTheCall(postUrl, ParaEnums.ApiCallHttpMethod.Post, attachment);
        }

        public static ApiCallResponse FilePerformUpload(string postUrl, Byte[] attachment, string contentType, string fileName)
        {
            return ApiMakeTheCall(postUrl, ParaEnums.ApiCallHttpMethod.Post, attachment, contentType, fileName);
        }

        private static ApiCallResponse ApiMakeTheCall(string apiCallUrl, ParaEnums.ApiCallHttpMethod httpMethod)
        {
            var ac = new ApiCallResponse();
            var uriAddress = new Uri(apiCallUrl);
            var req = WebRequest.Create(uriAddress) as HttpWebRequest;

            req.Method = ApiHttpPostProvider(httpMethod);
            ac.HttpCallMethod = req.Method;
            req.KeepAlive = false;

            //2 minutes request timeout
            req.Timeout = 120 * 1000;

            ac.XmlSent = null;

            ac.HasException = false;
            ac.CalledUrl = apiCallUrl;
            return ApiHttpRequestProcessor(ac, req);
        }

        /// <summary>
        /// The true call is being made by this method.
        /// </summary>
        static ApiCallResponse ApiMakeTheCall(string callurl, ParaEnums.ApiCallHttpMethod httpMethod, XmlDocument xmlPosted)
        {
            var ac = new ApiCallResponse();
            var uriAddress = new Uri(callurl);
            var req = WebRequest.Create(uriAddress) as HttpWebRequest;

            req.Method = ApiHttpPostProvider(httpMethod);
            ac.HttpCallMethod = req.Method;
            req.KeepAlive = false;
            
            //2 minutes request timeout
            req.Timeout = 120 * 1000;

            if (xmlPosted != null)
            {
                req.ContentType = "application/x-www-form-urlencoded";

                // Create a byte array of the data we want to send
                var bytedata = Encoding.UTF8.GetBytes(xmlPosted.OuterXml);

                // Set the content length in the request headers   
                req.ContentLength = bytedata.Length;

                //Provide a way for the user to configure the connection -> proxy, timeout, etc
                ApiRequestSettings.GlobalPreRequest(req);

                // Write data   
                using (Stream postStream = req.GetRequestStream())
                {
                    postStream.Write(bytedata, 0, bytedata.Length);
                }
                ac.XmlSent = xmlPosted;
            }
            else
            {
                ac.XmlSent = null;
            }

            ac.HasException = false;
            ac.CalledUrl = callurl;
            return ApiHttpRequestProcessor(ac, req);

        }

        /// <summary>
        /// The call, with passing a binary file.
        /// </summary>
        static ApiCallResponse ApiMakeTheCall(string callurl, ParaEnums.ApiCallHttpMethod httpMethod, Attachment att)
        {

            const string boundary = "--ParaBoundary";
            const string lineBreak = "\r\n";
            var contentDisposition = String.Format("Content-Disposition: {0}; name=\"{1}\"; filename=\"{1}\"", att.ContentType.MediaType, att.ContentType.Name);
            var ac = new ApiCallResponse();
            var uriAddress = new Uri(callurl);

            var req = WebRequest.Create(uriAddress) as HttpWebRequest;
            req.Method = ApiHttpPostProvider(httpMethod);
            req.KeepAlive = false;
            ac.HttpCallMethod = req.Method;

            req.AllowWriteStreamBuffering = true;
            req.ReadWriteTimeout = 10 * 60 * 1000;
            req.Timeout = -1;

            //Provide a way for the user to configure the connection -> proxy, timeout, etc
            ApiRequestSettings.GlobalPreRequest(req);

            req.ContentType = att.ContentType.MediaType + "; boundary:" + boundary; ;

            var filebytes = new byte[Convert.ToInt32(att.ContentStream.Length - 1) + 1];
            att.ContentStream.Read(filebytes, 0, filebytes.Length);
            var sb = new StringBuilder();
            sb.AppendLine(boundary);
            sb.AppendLine(contentDisposition);
            sb.AppendLine("Content-Type: " + att.ContentType.MediaType);
            sb.AppendLine("");

            string header = sb.ToString();
            string endboundary = lineBreak + boundary + "--";

            byte[] footerBytes = Encoding.ASCII.GetBytes(endboundary);
            byte[] headBytes = Encoding.ASCII.GetBytes(header);

            req.ContentLength = headBytes.Length + filebytes.Length + footerBytes.Length;
            var reqStreamTest = req.GetRequestStream();
            // String to Byte Array
            var totalRequest = new byte[headBytes.Length + filebytes.Length + footerBytes.Length];
            headBytes.CopyTo(totalRequest, 0);
            filebytes.CopyTo(totalRequest, headBytes.Length);
            footerBytes.CopyTo(totalRequest, headBytes.Length + filebytes.Length);
            reqStreamTest.Write(totalRequest, 0, totalRequest.Length);

            reqStreamTest.Close();

            ac.HasException = false;
            ac.CalledUrl = callurl;

            return ApiHttpRequestProcessor(ac, req);

        }

        /// <summary>
        /// The call, with passing a binary file.
        /// </summary>
        static ApiCallResponse ApiMakeTheCall(string callurl, ParaEnums.ApiCallHttpMethod httpMethod, Byte[] attachment, string contentType, string fileName)
        {

            const string boundary = "--ParaBoundary";
            const string lineBreak = "\r\n";
            string contentDisposition = String.Format("Content-Disposition: {0}; name=\"{1}\"; filename=\"{1}\"", contentType, fileName);
            var ac = new ApiCallResponse();
            var uriAddress = new Uri(callurl);

            var req = WebRequest.Create(uriAddress) as HttpWebRequest;
            req.Method = ApiHttpPostProvider(httpMethod);
            req.KeepAlive = false;
            ac.HttpCallMethod = req.Method;

            req.AllowWriteStreamBuffering = true;
            req.ReadWriteTimeout = 10 * 60 * 1000;
            req.Timeout = -1;

            //Provide a way for the user to configure the connection -> proxy, timeout, etc
            ApiRequestSettings.GlobalPreRequest(req);

            req.ContentType = contentType + "; boundary:" + boundary; ;

            var sb = new StringBuilder();
            sb.AppendLine(boundary);
            sb.AppendLine(contentDisposition);
            sb.AppendLine("Content-Type: " + contentType);
            sb.AppendLine("");

            var header = sb.ToString();
            const string endBoundary = lineBreak + boundary + "--";

            var footerBytes = Encoding.ASCII.GetBytes(endBoundary);
            var headBytes = Encoding.ASCII.GetBytes(header);

            req.ContentLength = headBytes.Length + attachment.Length + footerBytes.Length;
            var reqStreamTest = req.GetRequestStream();
            // String to Byte Array
            var totalRequest = new byte[headBytes.Length + attachment.Length + footerBytes.Length];
            headBytes.CopyTo(totalRequest, 0);
            attachment.CopyTo(totalRequest, headBytes.Length);
            footerBytes.CopyTo(totalRequest, headBytes.Length + attachment.Length);
            reqStreamTest.Write(totalRequest, 0, totalRequest.Length);

            reqStreamTest.Close();

            ac.HasException = false;
            ac.CalledUrl = callurl;

            return ApiHttpRequestProcessor(ac, req);
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
        static ApiCallResponse ApiHttpRequestProcessor(ApiCallResponse ac, HttpWebRequest req)
        {
            var responseFromServer = "";

            try
            {
                using (var httpWResp = req.GetResponse() as HttpWebResponse)
                {
                    try
                    {
                        ac.HttpResponseCode = (int)httpWResp.StatusCode;
                    }
                    catch (Exception exRespCode)
                    {
                        ac.HttpResponseCode = -1;
                    }

                    var reader = new StreamReader(httpWResp.GetResponseStream());

                    responseFromServer = reader.ReadToEnd();

                    reader.Close();

                    try
                    {
                        ac.XmlReceived.LoadXml(responseFromServer);
                    }
                    catch (Exception ex)
                    {
                        ac.XmlReceived = null;
                    }

                    try
                    {
                        ac.HttpResponseCode = (int)httpWResp.StatusCode;
                        if (ac.HttpResponseCode == 201)
                        {
                            try
                            {
                                ac.Id = Int64.Parse(ac.XmlReceived.DocumentElement.Attributes["id"].Value);
                            }
                            catch (Exception exx)
                            {
                                ac.Id = 0;
                            }
                        }
                    }
                    catch (Exception exx)
                    {
                        ac.HttpResponseCode = -1;
                    }

                    ac.HasException = false;
                    ac.ExceptionDetails = "";
                }

            }
            catch (WebException ex)
            {
                try
                {
                    ac.HttpResponseCode = (int)((((HttpWebResponse)ex.Response).StatusCode));
                    ac.ExceptionDetails = ex.Message;
                }
                catch
                {}
                ac.HasException = true;

                if (String.IsNullOrEmpty(ac.ExceptionDetails) == true)
                {
                    ac.ExceptionDetails = ex.ToString();
                }

                if (String.IsNullOrEmpty(responseFromServer) == false)
                {
                    ac.ExceptionDetails = "Response from server: " + responseFromServer;
                }


                string exresponseFromServer = "";
                try
                {
                    var exreader = new StreamReader(ex.Response.GetResponseStream());
                    exresponseFromServer = exreader.ReadToEnd().ToString();
                    exreader.Close();

                    if (String.IsNullOrEmpty(exresponseFromServer) == false)
                    {
                        ac.ExceptionDetails = ac.ExceptionDetails + Environment.NewLine + "Exception response:" + exresponseFromServer;
                    }

                }
                catch (Exception exread)
                {
                    if (ac.HttpResponseCode == 0)
                    {
                        ac.HttpResponseCode = 503;
                    }
                }

                if (String.IsNullOrEmpty(exresponseFromServer) == false)
                {
                    try
                    {
                        ac.XmlReceived.LoadXml(exresponseFromServer);

                        XmlNode mainnode = ac.XmlReceived.DocumentElement;
                        if (mainnode.LocalName.ToLower() == "error")
                        {
                            if (mainnode.Attributes["code"].InnerText.ToLower() != "")
                            {
                                ac.HttpResponseCode = Int32.Parse(mainnode.Attributes["code"].InnerText.ToString());
                            }
                            if (mainnode.Attributes["message"].InnerText.ToLower() != "")
                            {
                                ac.ExceptionDetails = mainnode.Attributes["message"].InnerText.ToString();
                            }
                        }
                    }
                    catch (Exception exp)
                    {
                        ac.XmlReceived = null;
                    }
                }
                else
                {
                    ac.XmlReceived = null;
                }
            }
            finally
            {
                // xml sent and xml received cleanup
                // TEMP FIX
                if (ac.XmlReceived != null && String.IsNullOrEmpty(ac.XmlReceived.InnerXml))
                {
                    ac.XmlReceived = null;
                }
                if (ac.XmlSent != null && String.IsNullOrEmpty(ac.XmlSent.InnerXml))
                {
                    ac.XmlSent = null;
                }
            }

            return ac;
        }

        /// <summary>
        /// Returns the Http request method for an API call, requires an ApiCallHttpMethod enum.
        /// </summary>
        internal static String ApiHttpPostProvider(ParaEnums.ApiCallHttpMethod apiCallHttpMethod)
        {
            string httpPostMethod = "";
            switch (apiCallHttpMethod)
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
