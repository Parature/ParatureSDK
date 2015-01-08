using System;

namespace ParatureAPI.ParaHelper
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


        /// <summary>
        /// Return the customFieldDataType enum, requires a string. Usually the string comes from the API response.
        /// </summary>
        internal static ParaEnums.CustomFieldDataType CustomFieldDataTypeProvider(string dt)
        {
            ParaEnums.CustomFieldDataType dataType = ParaEnums.CustomFieldDataType.Unknown;
            switch (dt.ToLower())
            {
                case "boolean":

                    dataType = ParaEnums.CustomFieldDataType.boolean;
                    break;
                case "float":

                    dataType = ParaEnums.CustomFieldDataType.Float;
                    break;
                case "attachment":

                    dataType = ParaEnums.CustomFieldDataType.attachment;
                    break;
                case "datetime":

                    dataType = ParaEnums.CustomFieldDataType.DateTime;
                    break;
                case "int":
                    dataType = ParaEnums.CustomFieldDataType.Int;

                    break;
                case "date":
                    dataType = ParaEnums.CustomFieldDataType.Date;
                    break;


                case "readonly":
                    dataType = ParaEnums.CustomFieldDataType.ReadOnly;
                    break;


                case "string":
                    dataType = ParaEnums.CustomFieldDataType.String;
                    break;

                case "option":
                    dataType = ParaEnums.CustomFieldDataType.Option;
                    break;

            }
            return dataType;
        }
    }
}