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
        internal static ParaEnums.FieldDataType CustomFieldDataTypeProvider(string dt)
        {
            ParaEnums.FieldDataType dataType = ParaEnums.FieldDataType.Unknown;
            switch (dt.ToLower())
            {
                case "boolean":

                    dataType = ParaEnums.FieldDataType.boolean;
                    break;
                case "float":

                    dataType = ParaEnums.FieldDataType.Float;
                    break;
                case "attachment":

                    dataType = ParaEnums.FieldDataType.attachment;
                    break;
                case "datetime":

                    dataType = ParaEnums.FieldDataType.DateTime;
                    break;
                case "int":
                    dataType = ParaEnums.FieldDataType.Int;

                    break;
                case "date":
                    dataType = ParaEnums.FieldDataType.Date;
                    break;


                case "readonly":
                    dataType = ParaEnums.FieldDataType.ReadOnly;
                    break;


                case "string":
                    dataType = ParaEnums.FieldDataType.String;
                    break;

                case "option":
                    dataType = ParaEnums.FieldDataType.Option;
                    break;

            }
            return dataType;
        }
    }
}