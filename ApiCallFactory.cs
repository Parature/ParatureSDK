using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
namespace ParaConnect
{
    /// <summary>
    /// The APICallFactory manages all calls made to the APIs server. No API calls should be made outside of this class.
    /// </summary>    
    static internal class ApiCallFactory
    {
        //No longer need with the new Throttler class
        //private static System.Timers.Timer callTimer = null;
        //private static bool wait = false;
        //private static System.Timers.Timer ThrottlerCleaner = null;
        //private static Hashtable throttlers = new Hashtable();

        private static Dictionary<Int64, DateTime> Throttlers = new Dictionary<long, DateTime>();

        /// <summary>
        /// This method will create/update an Object in Parature.
        /// </summary>
        /// <param name="paracredentials">
        ///The credentials to be used for making the API call. 
        ///Value Type: <see cref="ParaCredentials" />   (ParaConnect.ParaCredentials)
        ///</param>
        /// <param name="module">
        ///The name of the module to create or update. Choose from the ParatureModule enum list. 
        ///Value Type: <see cref="Paraenums.ParatureModule" />   (ParatureModule)
        ///</param>
        /// <param name="Objectid">
        ///Provides the ID of the object being inserted or updated. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="FileToPost">
        ///When creating or updating an object, you will need to pass the properly formatted XML document to be sent to the server.
        ///Value Type: <see cref="String" />   (System.String)
        ///</param>
        public static ParaObjects.ApiCallResponse ObjectCreateUpdate(ParaCredentials paracredentials, Paraenums.ParatureModule module, System.Xml.XmlDocument FileToPost, Int64 Objectid)
        {
            // Calling the next method that manages the call.
            return ObjectCreateUpdate(paracredentials, module, FileToPost, Objectid, null);
        }

        /// <summary>
        /// This method will create/update an Object in Parature.
        /// </summary>
        /// <param name="paracredentials">
        ///The credentials to be used for making the API call. 
        ///Value Type: <see cref="ParaCredentials" />   (ParaConnect.ParaCredentials)
        ///</param>
        /// <param name="module">
        ///The name of the module to create or update. Choose from the ParatureModule enum list. 
        ///Value Type: <see cref="Paraenums.ParatureModule" />   (ParatureModule)
        ///</param>
        /// <param name="FileToPost">
        ///When creating or updating an object, you will need to pass the properly formatted XML document to be sent to the server.
        ///Value Type: <see cref="String" />   (System.String)
        /// </param>
        /// <param name="Objectid">
        ///Provides the ID of the object being inserted or updated. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        ///<param name="Extras"   
        /// Extra string to attach to the URL
        ///</param>
        public static ParaObjects.ApiCallResponse ObjectCreateUpdate(ParaCredentials paracredentials, Paraenums.ParatureModule module, System.Xml.XmlDocument FileToPost, Int64 Objectid, ArrayList arguments)
        {
            if (arguments == null)
            {
                arguments = new ArrayList();
            }
            switch (module)
            {
                case Paraenums.ParatureModule.Ticket:
                case Paraenums.ParatureModule.Account:
                case Paraenums.ParatureModule.Customer:
                case Paraenums.ParatureModule.Product:
                case Paraenums.ParatureModule.Asset:
                    if (paracredentials.EnforceRequiredFields == false)
                    {
                        arguments.Add("_enforceRequiredFields_=" + paracredentials.EnforceRequiredFields.ToString().ToLower());
                    }
                    break;
            }
            string ApiCallUrl;
            // Getting the standard API URL to call.
            ApiCallUrl = ApiUrlBuilder.ApiObjectReadUpdateDeleteUrl(paracredentials, module, Objectid, arguments);

            Paraenums.ApiCallHttpMethod apicallhttpmethod;
            // To set up the call method, we check if this is a create (the objectid=0 in that case)
            // or an update (when we received an objectid>0)
            if (Objectid == 0)
            {
                apicallhttpmethod = Paraenums.ApiCallHttpMethod.Post;
            }
            else
            {
                apicallhttpmethod = Paraenums.ApiCallHttpMethod.Update;
            }

            // Calling the next method that manages the call.
            return ApiMakeCall(ApiCallUrl, apicallhttpmethod, FileToPost, paracredentials.Accountid, paracredentials);
        }

        /// <summary>
        /// This method will create/update an Object in Parature.
        /// </summary>
        /// <param name="paracredentials">
        ///The credentials to be used for making the API call. 
        ///Value Type: <see cref="ParaCredentials" />   (ParaConnect.ParaCredentials)
        ///</param>
        /// <param name="module">
        ///The name of the module to create or update. Choose from the ParatureModule enum list. 
        ///Value Type: <see cref="ParatureModule" />   (ParatureModule)
        ///</param>
        /// <param name="Objectid">
        ///Provides the ID of the object being inserted or updated. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="FileToPost">
        ///When creating or updating an object, you will need to pass the properly formatted XML document to be sent to the server.
        ///Value Type: <see cref="String" />   (System.String)
        ///</param>
        public static ParaObjects.ApiCallResponse EntityCreateUpdate(ParaCredentials paracredentials, Paraenums.ParatureEntity entity, System.Xml.XmlDocument FileToPost, Int64 Objectid)
        {
            string ApiCallUrl;
            // Getting the standard API URL to call.
            ApiCallUrl = ApiUrlBuilder.ApiObjectReadUpdateDeleteUrl(paracredentials, entity, Objectid, false);

            Paraenums.ApiCallHttpMethod apicallhttpmethod;
            // To set up the call method, we check if this is a create (the objectid=0 in that case)
            // or an update (when we received an objectid>0)
            if (Objectid == 0)
            {
                apicallhttpmethod = Paraenums.ApiCallHttpMethod.Post;
            }
            else
            {
                apicallhttpmethod = Paraenums.ApiCallHttpMethod.Update;
            }

            // Calling the next method that manages the call.
            return ApiMakeCall(ApiCallUrl, apicallhttpmethod, FileToPost, paracredentials.Accountid, paracredentials);
        }

        /// <summary>
        /// Use this method to delete the object passed to it.
        /// </summary>
        /// <param name="paracredentials">
        ///The credentials to be used for making the API call. 
        ///Value Type: <see cref="ParaCredentials" />   (ParaConnect.ParaCredentials)
        ///</param>
        /// <param name="module">
        ///The name of the module to create or update. Choose from the ParatureModule enum list. 
        ///Value Type: <see cref="Paraenums.ParatureModule" />   (Paraenums.ParatureModule)
        ///</param>
        /// <param name="objectid">
        ///The id of the object to create or update. 
        ///Value Type: <see cref="Int64" />   (System.int64)
        ///</param>
        /// <param name="purge">
        ///Indicates whether this a Purge (permanent deletion), or just a deletion (move to trash bin). Indicate TRUE for a purge, FALSE for a delete
        ///Value Type: <see cref="bool" />   (System.bool)
        ///</param>
        public static ParaObjects.ApiCallResponse ObjectDelete(ParaCredentials paracredentials, Paraenums.ParatureModule module, Int64 objectid, bool purge)
        {
            string ApiCallUrl;

            if (purge == true)
            {
                ArrayList arguments = new ArrayList();
                arguments.Add("_purge_=true");
                ApiCallUrl = ApiUrlBuilder.ApiObjectReadUpdateDeleteUrl(paracredentials, module, objectid, arguments);
            }
            else
            {
                ApiCallUrl = ApiUrlBuilder.ApiObjectReadUpdateDeleteUrl(paracredentials, module, objectid, false);
            }


            return ApiMakeCall(ApiCallUrl, Paraenums.ApiCallHttpMethod.Delete, paracredentials.Accountid, paracredentials);
        }

        /// <summary>
        /// Use this method to delete the entity passed to it.
        /// </summary>
        /// <param name="paracredentials">
        ///The credentials to be used for making the API call. 
        ///Value Type: <see cref="ParaCredentials" />   (ParaConnect.ParaCredentials)
        ///</param>
        /// <param name="entity">
        ///The name of the entity to delete. Choose from the ParatureEntity enum list. 
        ///Value Type: <see cref="Paraenums.ParatureEntity" />   (Paraenums.ParatureEntity)
        /// </param>
        /// <param name="entityid">
        ///The id of the entity to delete. 
        ///Value Type: <see cref="Int64" />   (System.int64)
        /// </param>
        public static ParaObjects.ApiCallResponse EntityDelete(ParaCredentials paracredentials, Paraenums.ParatureEntity entity, Int64 entityid)
        {
            string ApiCallUrl;

            //if (purge == true)
            //{
            ArrayList arguments = new ArrayList();
            arguments.Add("_purge_=true");
            ApiCallUrl = ApiUrlBuilder.ApiObjectReadUpdateDeleteUrl(paracredentials, entity, entityid, arguments);
            //}
            //else
            //{
            //    ApiCallUrl = ApiUrlBuilder.ApiObjectReadUpdateDeleteUrl(paracredentials, entity, entityid, false);
            //}


            return ApiMakeCall(ApiCallUrl, Paraenums.ApiCallHttpMethod.Delete, paracredentials.Accountid, paracredentials);
        }

        /// <summary>
        /// Use this method to get the details of an object that you plan to fill.
        /// </summary>
        /// <param name="paracredentials">
        ///The credentials to be used for making the API call. 
        ///Value Type: <see cref="ParaCredentials" />   (ParaConnect.ParaCredentials)
        ///</param>
        /// <param name="module">
        ///The name of the module to create or update. Choose from the ParatureModule enum list. 
        ///Value Type: <see cref="Paraenums.ParatureModule" />   (Paraenums.ParatureModule)
        ///</param>
        /// <param name="objectid">
        ///The id of the object to create or update. 
        ///Value Type: <see cref="Int64" />   (System.int64)
        ///</param>
        public static ParaObjects.ApiCallResponse ObjectGetDetail(ParaCredentials paracredentials, Paraenums.ParatureModule module, Int64 objectid)
        {

            string ApiCallUrl;

            ApiCallUrl = ApiUrlBuilder.ApiObjectReadUpdateDeleteUrl(paracredentials, module, objectid, false);

            return ApiMakeCall(ApiCallUrl, Paraenums.ApiCallHttpMethod.Get, paracredentials.Accountid, paracredentials);
        }

        /// <summary>
        /// Use this method to get the details of an Entity that you plan to fill.
        /// </summary>
        /// <param name="paracredentials">
        ///The credentials to be used for making the API call. 
        ///Value Type: <see cref="ParaCredentials" />   (ParaConnect.ParaCredentials)
        ///</param>
        /// <param name="module">
        ///The name of the entity to create or update. Choose from the ParatureEntity enum list. 
        ///Value Type: <see cref="Paraenums.ParatureEntity" />   (Paraenums.ParatureEntity)
        ///</param>
        /// <param name="objectid">
        ///The id of the object to create or update. 
        ///Value Type: <see cref="Int64" />   (System.int64)
        ///</param>
        public static ParaObjects.ApiCallResponse ObjectGetDetail(ParaCredentials paracredentials, Paraenums.ParatureEntity entity, Int64 objectid)
        {

            string ApiCallUrl;

            ApiCallUrl = ApiUrlBuilder.ApiObjectReadUpdateDeleteUrl(paracredentials, entity, objectid, false);

            return ApiMakeCall(ApiCallUrl, Paraenums.ApiCallHttpMethod.Get, paracredentials.Accountid, paracredentials);
        }

        /// <summary>
        /// Use this method to get the details of an object that you plan to fill.
        /// </summary>
        /// <param name="paracredentials">
        ///The credentials to be used for making the API call. 
        ///Value Type: <see cref="ParaCredentials" />   (ParaConnect.ParaCredentials)
        ///</param>
        /// <param name="module">
        ///The name of the module to create or update. Choose from the ParatureModule enum list. 
        ///Value Type: <see cref="Paraenums.ParatureModule" />   (ParatureModule)
        ///</param>
        /// <param name="objectid">
        ///The id of the object to create or update. 
        ///Value Type: <see cref="Int64" />   (System.int64)
        ///</param>
        /// <param name="Arguments">
        ///The list of extra optional arguments you need to include in the call. For example, for a ticket, we might want to get the action history.
        ///Value Type: <see cref="ArrayList" />   (System.String[])
        ///</param>
        public static ParaObjects.ApiCallResponse ObjectGetDetail(ParaCredentials paracredentials, Paraenums.ParatureModule module, Int64 objectid, ArrayList Arguments)
        {
            string ApiCallUrl;

            ApiCallUrl = ApiUrlBuilder.ApiObjectReadUpdateDeleteUrl(paracredentials, module, objectid, Arguments);

            return ApiMakeCall(ApiCallUrl, Paraenums.ApiCallHttpMethod.Get, paracredentials.Accountid, paracredentials);
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
        /// <param name="Arguments">
        ///The list of extra optional arguments you need to include in the call. For example, any fields filtering, any custom fields to include, etc.
        ///Value Type: <see cref="ArrayList" />   
        ///</param>
        public static ParaObjects.ApiCallResponse ObjectGetList(ParaCredentials paracredentials, Paraenums.ParatureModule module, ArrayList Arguments)
        {
            string ApiCallUrl;
            ApiCallUrl = ApiUrlBuilder.ApiObjectReadUpdateDeleteUrl(paracredentials, module, 0, Arguments);
            return ApiMakeCall(ApiCallUrl, Paraenums.ApiCallHttpMethod.Get, paracredentials.Accountid, paracredentials);
        }

        /// <summary>
        /// Use this method to get a list of objects that you plan to fill.
        /// </summary>
        /// <param name="paracredentials">
        ///The credentials to be used for making the API call. 
        ///Value Type: <see cref="ParaCredentials" />   (ParaConnect.ParaCredentials)
        ///</param>
        /// <param name="entity">
        ///The name of the entity to list. Choose from the ParatureEntity enum list. 
        ///</param>      
        public static ParaObjects.ApiCallResponse ObjectGetList(ParaCredentials paracredentials, Paraenums.ParatureEntity entity, ArrayList Arguments)
        {
            string ApiCallUrl;
            ApiCallUrl = ApiUrlBuilder.ApiObjectReadUpdateDeleteUrl(paracredentials, entity, 0, Arguments);
            return ApiMakeCall(ApiCallUrl, Paraenums.ApiCallHttpMethod.Get, paracredentials.Accountid, paracredentials);
        }


        public static ParaObjects.ApiCallResponse ObjectSecondLevelGetList(ParaCredentials paracredentials, Paraenums.ParatureModule module, Paraenums.ParatureEntity entity, ArrayList Arguments)
        {
            string ApiCallUrl;
            ApiCallUrl = ApiUrlBuilder.ApiObjectReadUpdateDeleteCustomUrl(paracredentials, module, entity, Arguments);
            return ApiMakeCall(ApiCallUrl, Paraenums.ApiCallHttpMethod.Get, paracredentials.Accountid, paracredentials);
        }

        /// <summary>
        /// Use this method to get the Schema XML of an object.
        /// </summary>
        public static ParaObjects.ApiCallResponse ObjectGetSchema(ParaCredentials paracredentials, Paraenums.ParatureModule module)
        {
            string ApiCallUrl;

            ApiCallUrl = ApiUrlBuilder.ApiObjectReadUpdateDeleteUrl(paracredentials, module, 0, true);

            return ApiMakeCall(ApiCallUrl, Paraenums.ApiCallHttpMethod.Get, paracredentials.Accountid, paracredentials);
        }

        /// <summary>
        /// Use this method to determine if any custom fields have custom validation
        /// </summary>
        public static ParaObjects.ModuleWithCustomFields ObjectCheckCustomFieldTypes(ParaCredentials paracredentials, Paraenums.ParatureModule module, ParaObjects.ModuleWithCustomFields baseObject)
        {
            string ApiCallUrl;
            paracredentials.EnforceRequiredFields = false;
            ApiCallUrl = ApiUrlBuilder.ApiObjectReadUpdateDeleteUrl(paracredentials, module, 0, true);

            if (baseObject.CustomFields != null)
            {
                foreach (ParaObjects.CustomField cf in baseObject.CustomFields)
                {
                    if (cf.DataType == Paraenums.CustomFieldDataType.String)
                    {
                        cf.CustomFieldValue = "a";
                    }
                }
            }

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            switch (module)
            {
                case Paraenums.ParatureModule.Account:
                    doc = xmlgenerator.AccountGenerateXML((ParaObjects.Account)baseObject);
                    break;
                case Paraenums.ParatureModule.Customer:
                    doc = xmlgenerator.customerGenerateXML((ParaObjects.Customer)baseObject);
                    break;
                case Paraenums.ParatureModule.Product:
                    doc = xmlgenerator.ProductGenerateXML((ParaObjects.Product)baseObject);
                    break;
                case Paraenums.ParatureModule.Asset:
                    doc = xmlgenerator.AssetGenerateXML((ParaObjects.Asset)baseObject);
                    break;
                case Paraenums.ParatureModule.Ticket:
                    doc = xmlgenerator.TicketGenerateXML((ParaObjects.Ticket)baseObject);
                    break;
                default:
                    break;
            }

            ParaObjects.ApiCallResponse ar = ApiMakeCall(ApiCallUrl, Paraenums.ApiCallHttpMethod.Post, doc, paracredentials.Accountid, paracredentials);

            if (ar.HasException)
            {
                string errors = ar.ExceptionDetails;

                string[] errorLines = errors.Split(';');

                foreach (string line in errorLines)
                {
                    string tempLine = line.ToLower().Trim();

                    string id = null;

                    if (tempLine.StartsWith("invalid field validation message"))
                    {

                        if (line.ToLower().Contains("us phone number"))
                        {
                            Match m = Regex.Match(line, @"Invalid Field Validation Message : (.+?) is not a valid US phone number");
                            id = m.Groups[1].Value.Trim();

                            foreach (ParaObjects.CustomField cf in baseObject.CustomFields)
                            {
                                if (cf.CustomFieldID == long.Parse(id))
                                {
                                    cf.DataType = Paraenums.CustomFieldDataType.UsPhone;
                                }
                            }
                        }
                        else if (tempLine.Contains("email address"))
                        {
                            Match m = Regex.Match(line, @"Invalid Field Validation Message : The Email Address '(.+?)' is not valid.");
                            id = m.Groups[1].Value.Trim();

                            foreach (ParaObjects.CustomField cf in baseObject.CustomFields)
                            {
                                if (cf.CustomFieldID == long.Parse(id))
                                {
                                    cf.DataType = Paraenums.CustomFieldDataType.Email;
                                }
                            }
                        }
                        else if (tempLine.Contains("international phone number"))
                        {
                            Match m = Regex.Match(line, @"Invalid Field Validation Message : (.+?) is not a valid international phone number");
                            id = m.Groups[1].Value.Trim();

                            foreach (ParaObjects.CustomField cf in baseObject.CustomFields)
                            {
                                if (cf.CustomFieldID == long.Parse(id))
                                {
                                    cf.DataType = Paraenums.CustomFieldDataType.InternationalPhone;
                                }
                            }
                        }
                        else if (tempLine.Contains("url"))
                        {
                            Match m = Regex.Match(line, @"Invalid Field Validation Message : (.+?) is an invalid URL.");
                            id = m.Groups[1].Value.Trim();
                            if (Information.IsNumeric(id.Trim()))
                            {
                            foreach (ParaObjects.CustomField cf in baseObject.CustomFields)
                            {
                                if (cf.CustomFieldID == long.Parse(id.Trim()))
                                {
                                    cf.DataType = Paraenums.CustomFieldDataType.Url;
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
                ApiCallUrl = ApiUrlBuilder.ApiObjectReadUpdateDeleteUrl(paracredentials, module, ar.Objectid, false);
                ar = ApiMakeCall(ApiCallUrl, Paraenums.ApiCallHttpMethod.Delete, paracredentials.Accountid, paracredentials);

                // purging the item
                ArrayList arguments = new ArrayList();
                arguments.Add("_purge_=true");
                ApiCallUrl = ApiUrlBuilder.ApiObjectReadUpdateDeleteUrl(paracredentials, module, ar.Objectid, arguments);
                ar = ApiMakeCall(ApiCallUrl, Paraenums.ApiCallHttpMethod.Delete, paracredentials.Accountid, paracredentials);
            }

            return baseObject;
        }

        /// <summary>
        /// Use this method to get the Schema XML of an object.
        /// </summary>
        /// <param name="paracredentials"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static ParaObjects.ApiCallResponse EntityGetSchema(ParaCredentials paracredentials, Paraenums.ParatureEntity entity)
        {
            string ApiCallUrl;

            ApiCallUrl = ApiUrlBuilder.ApiObjectReadUpdateDeleteUrl(paracredentials, entity, 0, true);

            return ApiMakeCall(ApiCallUrl, Paraenums.ApiCallHttpMethod.Get, paracredentials.Accountid, paracredentials);
        }

        /// <summary>
        /// This method makes the call to the API Server and return a class holding the response. It is used when you are also posting an XML To the server (in the case of a create, or an update).
        /// </summary>
        static ParaObjects.ApiCallResponse ApiMakeCall(string callurl, Paraenums.ApiCallHttpMethod ApiCallHttpMethod, System.Xml.XmlDocument XmlPosted, int accountID, ParaCredentials paracredentials)
        {
            ParaObjects.ApiCallResponse resp = MakeThrottledCall(callurl, ApiCallHttpMethod, XmlPosted, accountID, paracredentials);

            #region handing Random API server issues
            // Handling our servers having issues
            if (paracredentials.AutoretryMode != Paraenums.AutoRetryMode.None && ((resp.httpResponseCode == 500 && resp.ExceptionDetails.ToLower().Contains("invalid action given the current status of the ticket") == false) || resp.httpResponseCode == 401 || resp.ExceptionDetails.Contains("An unexpected error occurred")))
            {
                StringBuilder callLogger = new StringBuilder();
                Int32 attemptNumber = 1;

                while (attemptNumber < 5 && (resp.httpResponseCode == 500 || resp.httpResponseCode == 401 || resp.ExceptionDetails.Contains("An unexpected error occurred")))
                {
                    buildAutoRetryAPIErrorLogMessage(ref callLogger, attemptNumber.ToString(), callurl,
                        ApiCallHttpMethod.ToString(), resp.httpResponseCode.ToString(), resp.ExceptionDetails,
                        resp.xmlSent, resp.xmlReceived);

                    attemptNumber++;

                    HandleRandomAPIErrorsSleepTime(attemptNumber, paracredentials);

                    // try the call again
                    resp = MakeThrottledCall(callurl, ApiCallHttpMethod, XmlPosted, accountID, paracredentials);

                }
                if (attemptNumber > 1 && paracredentials.logRetries)
                {
                    buildAutoRetryAPIErrorLogMessage(ref callLogger, attemptNumber.ToString(), callurl,
                        ApiCallHttpMethod.ToString(), resp.httpResponseCode.ToString(), resp.ExceptionDetails,
                        resp.xmlSent, resp.xmlReceived);

                    ParaLogger.LogManager.LogExceptionToFile(callLogger.ToString(), "RandomAPIErrorsLog" + DateTime.Now.ToString("yyyy-MM-dd"));
                }
                callLogger = null;
            }

            #endregion

            return resp;
        }

        /// <summary>
        /// This method makes the call to the API Server and return a class holding the response.
        /// </summary>
        /// 
        static ParaObjects.ApiCallResponse ApiMakeCall(string callurl, Paraenums.ApiCallHttpMethod ApiCallHttpMethod, int accountID, ParaCredentials paracredentials)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc = null;
            ParaObjects.ApiCallResponse resp = MakeThrottledCall(callurl, ApiCallHttpMethod, doc, accountID, paracredentials);
            Int16 attemptNumber = 1;

            #region handling Random API server issues
            // Handling our servers having issues
            if (paracredentials.AutoretryMode != Paraenums.AutoRetryMode.None && (resp.httpResponseCode == 500 || resp.httpResponseCode == 401 || resp.ExceptionDetails.Contains("An unexpected error occurred")))
            {
                StringBuilder callLogger = new StringBuilder();
               

                while (attemptNumber < 5 && (resp.httpResponseCode == 500 || resp.httpResponseCode == 401 || resp.ExceptionDetails.Contains("An unexpected error occurred")))
                {
                    buildAutoRetryAPIErrorLogMessage(ref callLogger, attemptNumber.ToString(),
                        callurl, ApiCallHttpMethod.ToString(), resp.httpResponseCode.ToString(),
                        resp.ExceptionDetails, null, resp.xmlReceived);

                    attemptNumber++;

                    HandleRandomAPIErrorsSleepTime(attemptNumber, paracredentials);
                    // try the call again
                    resp = MakeThrottledCall(callurl, ApiCallHttpMethod, doc, accountID, paracredentials);
                }
                if (attemptNumber > 1 && paracredentials.logRetries)
                {
                    buildAutoRetryAPIErrorLogMessage(ref callLogger, attemptNumber.ToString(),
                        callurl, ApiCallHttpMethod.ToString(), resp.httpResponseCode.ToString(),
                        resp.ExceptionDetails, null, resp.xmlReceived);

                    ParaLogger.LogManager.LogExceptionToFile(callLogger.ToString(), "RandomAPIErrorsLog" + DateTime.Now.ToString("yyyy-MM-dd"));
                }
                callLogger = null;
            }

            #endregion

            resp.AutomatedRetries = attemptNumber;

            return resp;
        }

        private static void buildAutoRetryAPIErrorLogMessage(ref StringBuilder callLogger, string attemptNumber, string URL, string Method, string responseCode, string message, System.Xml.XmlDocument sent, System.Xml.XmlDocument received)
        {
            callLogger.AppendLine("Call [" + attemptNumber + "]");
            callLogger.AppendLine("Call URL [" + URL + "]");
            callLogger.AppendLine("Call Method [" + Method + "]");
            callLogger.AppendLine("Call Time [" + DateTime.Now.ToString("MM/dd/yyyy-HH:mm:ss' GMT'z") + "]");
            callLogger.AppendLine("HTTP Response [" + responseCode + "]");
            callLogger.AppendLine("Exception Message [" + message + "]");

            callLogger.AppendLine("XML Sent [" + ((sent == null) ? "" : sent.InnerXml) + "]");

            callLogger.AppendLine("XML Received [" + ((received == null) ? "" : received.InnerXml) + "]");
        }

        private static void HandleRandomAPIErrorsSleepTime(Int32 attemptNumber, ParaCredentials paracredentials)
        {
            Int32 sleepTime = 0;

            if (attemptNumber == 2)
            {
                if (paracredentials.AutoretryMode == Paraenums.AutoRetryMode.ConsoleApp)
                {
                    sleepTime = 5000;
                }
                else
                {
                    sleepTime = 1000;
                }
            }
            else if (attemptNumber == 3)
            {
                if (paracredentials.AutoretryMode == Paraenums.AutoRetryMode.ConsoleApp)
                {
                    sleepTime = 10000;
                }
                else
                {
                    sleepTime = 2000;
                }
            }
            else if (attemptNumber == 4)
            {
                if (paracredentials.AutoretryMode == Paraenums.AutoRetryMode.ConsoleApp)
                {
                    sleepTime = 60000;
                }
                else
                {
                    sleepTime = 5000;
                }
            }

            // The call had issues with our APIs, sleeping a little bit.               
            System.Threading.Thread.Sleep(sleepTime);
        }

        public static ParaObjects.ApiCallResponse FileUploadGetUrl(ParaCredentials paracredentials, Paraenums.ParatureModule module)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc = null;
            ParaObjects.ApiCallResponse resp = MakeThrottledCall(ApiUrlBuilder.ApiObjectReadUpdateDeleteCustomUrl(paracredentials, module, "upload"), Paraenums.ApiCallHttpMethod.Get, doc, paracredentials.Accountid, paracredentials);

            #region handing Random API server issues
            // Handling our servers having issues
            if (paracredentials.AutoretryMode != Paraenums.AutoRetryMode.None && (resp.httpResponseCode == 500 || resp.httpResponseCode == 401 || resp.ExceptionDetails.Contains("An unexpected error occurred")))
            {
                StringBuilder callLogger = new StringBuilder();
                Int32 attemptNumber = 1;

                while (attemptNumber < 5 && (resp.httpResponseCode == 500 || resp.httpResponseCode == 401 || resp.ExceptionDetails.Contains("An unexpected error occurred")))
                {
                    buildAutoRetryAPIErrorLogMessage(ref callLogger, attemptNumber.ToString(), resp.CalledUrl,
                        resp.httpCallMethod, resp.httpResponseCode.ToString(), resp.ExceptionDetails,
                        resp.xmlSent, resp.xmlReceived);

                    attemptNumber++;

                    HandleRandomAPIErrorsSleepTime(attemptNumber, paracredentials);

                    // try the call again
                    resp = MakeThrottledCall(ApiUrlBuilder.ApiObjectReadUpdateDeleteCustomUrl(paracredentials, module, "upload"), Paraenums.ApiCallHttpMethod.Get, doc, paracredentials.Accountid, paracredentials);
                }
                if (attemptNumber > 1 && paracredentials.logRetries)
                {
                    buildAutoRetryAPIErrorLogMessage(ref callLogger, attemptNumber.ToString(), resp.CalledUrl,
                        resp.httpCallMethod, resp.httpResponseCode.ToString(), resp.ExceptionDetails,
                        resp.xmlSent, resp.xmlReceived);

                    ParaLogger.LogManager.LogExceptionToFile(callLogger.ToString(), "RandomAPIErrorsLog" + DateTime.Now.ToString("yyyy-MM-dd"));
                }
                callLogger = null;
            }

            #endregion

            return resp;
        }

        public static ParaObjects.ApiCallResponse FilePerformUpload(string PostUrl, System.Net.Mail.Attachment Attachment, int accountID, ParaCredentials paraCredentials)
        {
            return MakeThrottledCall(PostUrl, Paraenums.ApiCallHttpMethod.Post, Attachment, accountID, paraCredentials);
        }

        public static ParaObjects.ApiCallResponse FilePerformUpload(string PostUrl, Byte[] Attachment, string contentType, string FileName, int accountID, ParaCredentials paraCredentials)
        {
            return MakeThrottledCall(PostUrl, Paraenums.ApiCallHttpMethod.Post, Attachment, contentType, FileName, accountID, paraCredentials);
        }

        /// <summary>
        /// Manages throttling when making a call to the server.
        /// </summary>
        static ParaObjects.ApiCallResponse MakeThrottledCall(string callurl, Paraenums.ApiCallHttpMethod ApiCallHttpMethod, System.Xml.XmlDocument XmlPosted, int accountID, ParaCredentials pc)
        {
            ParaObjects.ApiCallResponse resp = new ParaObjects.ApiCallResponse();
            resp = ApiMakeTheCall(callurl, ApiCallHttpMethod, XmlPosted, accountID, pc);
            int sleepTime = 121000;
            //ParaLogger.LogManager.LogEvent("first call before the loop, http code is: "  + resp.httpResponseCode, ParaLogger.ParaLoggerEnums.EventType.Event, ParaLogger.ParaLoggerEnums.LogMode.TextFile);
            while ((resp.httpResponseCode == 503 || resp.httpResponseCode == 0) && sleepTime < 240000)
            {
                // The call has been rejected by the API throttling service.                
                System.Threading.Thread.Sleep(sleepTime);
                //calls.Clear();
                resp = ApiMakeTheCall(callurl, ApiCallHttpMethod, XmlPosted, accountID, pc);
                sleepTime += 60000;
            }

            sleepTime = 2000;
            while (resp.httpResponseCode == 401 && sleepTime < 6001)
            {
                // The call has been rejected by the API throttling service.
                System.Threading.Thread.Sleep(2000);
                resp = ApiMakeTheCall(callurl, ApiCallHttpMethod, XmlPosted, accountID, pc);
                sleepTime += 2000;
            }

            return resp;
        }

        /// <summary>
        /// Manages throttling when making a call to the server.
        /// </summary>
        static ParaObjects.ApiCallResponse MakeThrottledCall(string callurl, Paraenums.ApiCallHttpMethod ApiCallHttpMethod, System.Net.Mail.Attachment att, int accountID, ParaCredentials paraCredentials)
        {
            ParaObjects.ApiCallResponse resp = new ParaObjects.ApiCallResponse();
            resp = ApiMakeTheCall(callurl, ApiCallHttpMethod, att, accountID, paraCredentials);
            int sleepTime = 121000;

            while ((resp.httpResponseCode == 503 || resp.httpResponseCode == 0) && sleepTime < 240000)
            {

                // The call has been rejected by the API throttling service.
                System.Threading.Thread.Sleep(sleepTime);
                //calls.Clear();
                resp = ApiMakeTheCall(callurl, ApiCallHttpMethod, att, accountID, paraCredentials);
                sleepTime += 60000;
            }

            sleepTime = 2000;
            while (resp.httpResponseCode == 401 && sleepTime<6001)
            {
                // The call has been rejected by the API throttling service.
                System.Threading.Thread.Sleep(2000);
                resp = ApiMakeTheCall(callurl, ApiCallHttpMethod, att, accountID, paraCredentials);
                sleepTime += 2000;
            }

            return resp;
        }

        /// <summary>
        /// Manages throttling when making a call to the server.
        /// </summary>
        static ParaObjects.ApiCallResponse MakeThrottledCall(string callurl, Paraenums.ApiCallHttpMethod ApiCallHttpMethod, Byte[] Attachment, string ContentType, string FileName, int accountID, ParaCredentials paraCredentials)
        {
            ParaObjects.ApiCallResponse resp = new ParaObjects.ApiCallResponse();
            resp = ApiMakeTheCall(callurl, ApiCallHttpMethod, Attachment, ContentType, FileName, accountID, paraCredentials);
            int sleepTime = 121000;

            while ((resp.httpResponseCode == 503 || resp.httpResponseCode == 0) && sleepTime < 240000)
            {
                // The call has been rejected by the API throttling service.
                System.Threading.Thread.Sleep(sleepTime);
                //calls.Clear();
                resp = ApiMakeTheCall(callurl, ApiCallHttpMethod, Attachment, ContentType, FileName, accountID, paraCredentials);
                sleepTime += 60000;
            }

            sleepTime = 2000;
            while (resp.httpResponseCode == 401 && sleepTime < 10001)
            {
                // The call has been rejected by the API throttling service.
                System.Threading.Thread.Sleep(2000);
                resp = ApiMakeTheCall(callurl, ApiCallHttpMethod, Attachment, ContentType, FileName, accountID, paraCredentials);
                sleepTime += 2000;
            }

            return resp;

        }

        /// <summary>
        /// The true call is being made by this method.
        /// </summary>
        static ParaObjects.ApiCallResponse ApiMakeTheCall(string callurl, Paraenums.ApiCallHttpMethod ApiCallHttpMethod, System.Xml.XmlDocument XmlPosted, int accountID, ParaCredentials paraCredentials)
        {
            ParaObjects.ApiCallResponse ac = new ParaObjects.ApiCallResponse();
            Uri uriAddress = new Uri(callurl);
            HttpWebRequest req = WebRequest.Create(uriAddress) as HttpWebRequest;
            //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();

            req.Method = ParaHelper.ParaEnumProvider.ApiHttpPostProvider(ApiCallHttpMethod);
            ac.httpCallMethod = req.Method;
            req.KeepAlive = false;
            
            //2 minutes request timeout
            req.Timeout = 120 * 1000;

            if (XmlPosted != null)
            {
                req.ContentType = "application/x-www-form-urlencoded";

                // Create a byte array of the data we want to send
                byte[] bytedata = UTF8Encoding.UTF8.GetBytes(XmlPosted.OuterXml);

                // Set the content length in the request headers   
                req.ContentLength = bytedata.Length;

                // Write data   
                using (Stream postStream = req.GetRequestStream())
                {
                    postStream.Write(bytedata, 0, bytedata.Length);
                }
                ac.xmlSent = XmlPosted;
            }
            else
            {
                ac.xmlSent = null;
            }

            ac.HasException = false;
            ac.CalledUrl = callurl;
            return ApiHttpRequestProcessor(ac, req, accountID, paraCredentials);

        }

        /// <summary>
        /// The call, with passing a binary file.
        /// </summary>
        static ParaObjects.ApiCallResponse ApiMakeTheCall(string callurl, Paraenums.ApiCallHttpMethod ApiCallHttpMethod, System.Net.Mail.Attachment att, int accountID, ParaCredentials paraCredentials)
        {

            string Boundary = "--ParaBoundary";
            string LineBreak = "\r\n";
            string ContentDisposition = string.Format("Content-Disposition: {0}; name=\"{1}\"; filename=\"{1}\"", att.ContentType.MediaType, att.ContentType.Name);
            ParaObjects.ApiCallResponse ac = new ParaObjects.ApiCallResponse();
            Uri uriAddress = new Uri(callurl);

            HttpWebRequest req = WebRequest.Create(uriAddress) as HttpWebRequest;
            req.Method = ParaHelper.ParaEnumProvider.ApiHttpPostProvider(ApiCallHttpMethod);
            req.KeepAlive = false;
            ac.httpCallMethod = req.Method;

            req.AllowWriteStreamBuffering = true;
            req.ReadWriteTimeout = 10 * 60 * 1000;
            req.Timeout = -1;

            req.ContentType = att.ContentType.MediaType + "; boundary:" + Boundary; ;

            byte[] Filebytes = new byte[Convert.ToInt32(att.ContentStream.Length - 1) + 1];
            att.ContentStream.Read(Filebytes, 0, Filebytes.Length);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Boundary);
            sb.AppendLine(ContentDisposition);
            sb.AppendLine("Content-Type: " + att.ContentType.MediaType);
            sb.AppendLine("");
            // sb.AppendLine(Boundary + "--");
            string header = sb.ToString();
            string endboundary = LineBreak + Boundary + "--";
            //int CurrOffset=0;
            byte[] FooterBytes = System.Text.Encoding.ASCII.GetBytes(endboundary);
            byte[] HeadBytes = System.Text.Encoding.ASCII.GetBytes(header);
            //req.ContentLength = header.Length +  Filebytes.Length + endboundary.Length;
            req.ContentLength = HeadBytes.Length + Filebytes.Length + FooterBytes.Length;
            Stream reqStreamTest = req.GetRequestStream();
            // String to Byte Array
            byte[] TotalRequest = new byte[HeadBytes.Length + Filebytes.Length + FooterBytes.Length];
            HeadBytes.CopyTo(TotalRequest, 0);
            Filebytes.CopyTo(TotalRequest, HeadBytes.Length);
            FooterBytes.CopyTo(TotalRequest, HeadBytes.Length + Filebytes.Length);
            reqStreamTest.Write(TotalRequest, 0, TotalRequest.Length);

            reqStreamTest.Close();

            ac.HasException = false;
            ac.CalledUrl = callurl;


            return ApiHttpRequestProcessor(ac, req, accountID, paraCredentials);

        }

        /// <summary>
        /// The call, with passing a binary file.
        /// </summary>
        static ParaObjects.ApiCallResponse ApiMakeTheCall(string callurl, Paraenums.ApiCallHttpMethod ApiCallHttpMethod, Byte[] Attachment, string ContentType, string FileName, int accountID, ParaCredentials paraCredentials)
        {

            string Boundary = "--ParaBoundary";
            string LineBreak = "\r\n";
            string ContentDisposition = string.Format("Content-Disposition: {0}; name=\"{1}\"; filename=\"{1}\"", ContentType, FileName);
            ParaObjects.ApiCallResponse ac = new ParaObjects.ApiCallResponse();
            Uri uriAddress = new Uri(callurl);

            HttpWebRequest req = WebRequest.Create(uriAddress) as HttpWebRequest;
            req.Method = ParaHelper.ParaEnumProvider.ApiHttpPostProvider(ApiCallHttpMethod);
            req.KeepAlive = false;
            ac.httpCallMethod = req.Method;

            req.AllowWriteStreamBuffering = true;
            req.ReadWriteTimeout = 10 * 60 * 1000;
            req.Timeout = -1;

            req.ContentType = ContentType + "; boundary:" + Boundary; ;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Boundary);
            sb.AppendLine(ContentDisposition);
            sb.AppendLine("Content-Type: " + ContentType);
            sb.AppendLine("");

            string header = sb.ToString();
            string endboundary = LineBreak + Boundary + "--";

            byte[] FooterBytes = System.Text.Encoding.ASCII.GetBytes(endboundary);
            byte[] HeadBytes = System.Text.Encoding.ASCII.GetBytes(header);

            req.ContentLength = HeadBytes.Length + Attachment.Length + FooterBytes.Length;
            Stream reqStreamTest = req.GetRequestStream();
            // String to Byte Array
            byte[] TotalRequest = new byte[HeadBytes.Length + Attachment.Length + FooterBytes.Length];
            HeadBytes.CopyTo(TotalRequest, 0);
            Attachment.CopyTo(TotalRequest, HeadBytes.Length);
            FooterBytes.CopyTo(TotalRequest, HeadBytes.Length + Attachment.Length);
            reqStreamTest.Write(TotalRequest, 0, TotalRequest.Length);

            reqStreamTest.Close();

            ac.HasException = false;
            ac.CalledUrl = callurl;

            return ApiHttpRequestProcessor(ac, req, accountID, paraCredentials);
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
        static ParaObjects.ApiCallResponse ApiHttpRequestProcessor(ParaObjects.ApiCallResponse ac, HttpWebRequest req, int accountID, ParaCredentials pc)
        {
            string responseFromServer = "";


            //if (pc.retriesWaitTime <= 500)
            //{
                //Calling the auto throttling method.
                ThrottlingManagerPause(accountID, pc.retriesWaitTime);
            //}

            try
            {
                using (HttpWebResponse HttpWResp = req.GetResponse() as HttpWebResponse)
                {
                    try
                    {
                        ac.httpResponseCode = (int)HttpWResp.StatusCode;
                    }
                    catch (Exception ExRespCode)
                    {
                        ac.httpResponseCode = -1;
                    }

                    StreamReader reader = new StreamReader(HttpWResp.GetResponseStream());

                    responseFromServer = reader.ReadToEnd().ToString();

                    reader.Close();

                    if (responseFromServer != null)
                    {
                        try
                        {
                            ac.xmlReceived.LoadXml(responseFromServer);
                        }

                        catch (Exception ex)
                        {
                            ac.xmlReceived = null;
                        }

                    }
                    else
                    {
                        ac.xmlReceived = null;
                    }
                    try
                    {
                        ac.httpResponseCode = (int)HttpWResp.StatusCode;
                        if (ac.httpResponseCode == 201)
                        {
                            try
                            {
                                ac.Objectid = Int64.Parse(ac.xmlReceived.DocumentElement.Attributes["id"].Value);
                            }
                            catch (Exception exx)
                            {
                                ac.Objectid = 0;
                            }
                        }
                    }
                    catch (Exception exx)
                    {
                        ac.httpResponseCode = -1;
                    }

                    ac.HasException = false;
                    ac.ExceptionDetails = "";
                }

            }
            catch (WebException ex)
            {
                try
                {
                    ac.httpResponseCode = (int)((((HttpWebResponse)ex.Response).StatusCode));
                    ac.ExceptionDetails = ex.Message.ToString();
                }
                catch
                {

                }
                ac.HasException = true;

                if (string.IsNullOrEmpty(ac.ExceptionDetails) == true)
                {
                    ac.ExceptionDetails = ex.ToString();
                }

                if (string.IsNullOrEmpty(responseFromServer) == false)
                {
                    ac.ExceptionDetails = "Response from server: " + responseFromServer;
                }


                string exresponseFromServer = "";
                //StreamReader exreader= new StreamReader();
                try
                {

                    StreamReader exreader = new StreamReader(((WebException)ex).Response.GetResponseStream());
                    exresponseFromServer = exreader.ReadToEnd().ToString();
                    exreader.Close();

                    if (string.IsNullOrEmpty(exresponseFromServer) == false)
                    {
                        ac.ExceptionDetails = ac.ExceptionDetails + Environment.NewLine + "Exception response:" + exresponseFromServer;
                    }

                }
                catch (Exception exread)
                {
                    if (ac.httpResponseCode == 0)
                    {
                        ac.httpResponseCode = 503;
                    }
                }

                if (string.IsNullOrEmpty(exresponseFromServer) == false)
                {
                    try
                    {
                        ac.xmlReceived.LoadXml(exresponseFromServer);

                        System.Xml.XmlNode mainnode = ac.xmlReceived.DocumentElement;
                        if (mainnode.LocalName.ToLower() == "error")
                        {
                            if (mainnode.Attributes["code"].InnerText.ToLower() != "")
                            {
                                ac.httpResponseCode = int.Parse(mainnode.Attributes["code"].InnerText.ToString());
                            }
                            if (mainnode.Attributes["message"].InnerText.ToLower() != "")
                            {
                                ac.ExceptionDetails = mainnode.Attributes["message"].InnerText.ToString();
                            }
                        }
                    }
                    catch (Exception exp)
                    {
                        ac.xmlReceived = null;
                    }
                }
                else
                {
                    ac.xmlReceived = null;
                }
            }
            finally
            {
                try
                {
                    if (req != null)
                    {
                        req = null;
                    }
                }
                catch (Exception exReq)
                {

                }
                // xml sent and xml received cleanup
                // TEMP FIX
                if (ac.xmlReceived != null && string.IsNullOrEmpty(ac.xmlReceived.InnerXml))
                {
                    ac.xmlReceived = null;
                }
                if (ac.xmlSent != null && string.IsNullOrEmpty(ac.xmlSent.InnerXml))
                {
                    ac.xmlSent = null;
                }
            }
            
            // Set the last call time.
            ThrottlingManagerSet(accountID);

            return ac;
        }

        /// <summary>
        /// Manage the sleep time needed between calls to avoid throttling.
        /// </summary>
        static void ThrottlingManagerPause(Int64 AccountID, Int32 WaitTimeMilliseconds)
        {
            if (Throttlers.ContainsKey(AccountID))
            {
                // We have already made a call previously for this account.

                // Getting the cutoff time
                DateTime cutoff = DateTime.UtcNow;

                // Getting the time the last call was made.
                DateTime lastCall = Throttlers[AccountID];

                // Getting the time difference between the last call and the current time
                TimeSpan sp = cutoff.Subtract(lastCall);
                if (sp.TotalMilliseconds < WaitTimeMilliseconds)
                {
                    // deciding the sleep time needed, and then sleeping
                    Int32 SleepTime =  WaitTimeMilliseconds -(int) Math.Floor(sp.TotalMilliseconds)  ;
                    System.Threading.Thread.Sleep( SleepTime);
                }
            }
        }

        /// <summary>
        /// Set the datetime of the last call made.
        /// </summary>
        static void ThrottlingManagerSet(Int64 AccountID)
        {
            if (Throttlers.ContainsKey(AccountID))
            {
                Throttlers[AccountID] = DateTime.UtcNow;
            }
            else
            {
                try
                {
                    Throttlers.Add(AccountID, DateTime.UtcNow);
                }
                catch (Exception ex)
                {
                    // Another thread probably added the AccountID to the dictionary                    
                    Throttlers[AccountID] = DateTime.UtcNow; 
                }
            }
        }        
    }



    public class TrustAllCertificatePolicy : System.Net.ICertificatePolicy
    {
        public TrustAllCertificatePolicy() { }

        #region ICertificatePolicy Members

        public bool CheckValidationResult(ServicePoint srvPoint, System.Security.Cryptography.X509Certificates.X509Certificate certificate, WebRequest request, int certificateProblem)
        {
            return true;
        }

        #endregion
    }
}
