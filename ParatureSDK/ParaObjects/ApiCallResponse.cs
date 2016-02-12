using System;
using System.Xml;

namespace ParatureSDK.ParaObjects
{
    /// <summary>
    /// This class is contains all row information regarding an API call.
    /// </summary>
    public class ApiCallResponse
    {
        /// <summary>
        /// The url that was called when making the API call.
        /// </summary>
        public string CalledUrl = "";
        /// <summary>
        /// Call method performed: eg: Post, Put, Delete, etc...
        /// </summary>
        public string HttpCallMethod = "";
        /// <summary>
        /// The Http response code that was returned. This might be 0 if the server did not respond.
        /// </summary>
        public int HttpResponseCode = 0;
        /// <summary>
        /// Whether or not there was an exception.
        /// </summary>
        public bool HasException;
        /// <summary>
        /// The details of the error message, if the call generated an exception.
        /// </summary>
        public string ExceptionDetails = "";
        /// <summary>
        /// The XML that was received back from the server. Will only contain data when a proper XML is returned. This will be null if an exception was encountered. Please check first whether there was an exception or not, then check if this XML document is not null, before trying to use it.
        /// </summary>
        public XmlDocument XmlReceived = new XmlDocument();
        /// <summary>
        /// The XML that was sent to the server when making the API Call. In case of of a list or a retrieve, there is not XML that is sent. Please check first whether this is a null object or not, before using it.
        /// </summary>
        public XmlDocument XmlSent = new XmlDocument();

        /// <summary>
        /// If you were inserting or updating a record, check this value, if it is more than 0, it means the operations
        /// was successfull. Otherwise, it means there was an issue. If you created a new object, this value will hold the new id.
        /// </summary>
        public Int64 Id = 0;

        /// <summary>
        /// Number of API retries made to get the call to go through
        /// </summary>
        public Int16 AutomatedRetries = 0;

        public ApiCallResponse()
        {
        }

        public ApiCallResponse(ApiCallResponse apiCallResponse)
        {
            CalledUrl = apiCallResponse.CalledUrl;
            HttpCallMethod = apiCallResponse.HttpCallMethod;
            HttpResponseCode = apiCallResponse.HttpResponseCode;
            HasException = apiCallResponse.HasException;
            ExceptionDetails = apiCallResponse.ExceptionDetails;
            XmlReceived = apiCallResponse.XmlReceived;
            XmlSent = apiCallResponse.XmlSent;
            Id = apiCallResponse.Id;
        }
    }
}