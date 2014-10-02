using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
namespace ParatureAPI
{
    internal class ParaHelper
    {
        internal partial class ParaEnumProvider
        {

            /// <summary>
            /// Resolves the server farm name to the actual server farm URL.
            /// </summary>
            internal static String ServerFarmEnumProvider(Paraenums.ServerFarm sf)
            {


                FieldInfo fieldInfo = sf.GetType().GetField(sf.ToString());

                object[] attribArray = fieldInfo.GetCustomAttributes(typeof(Paraenums.EnumDescriptionAttribute), false);
                if (attribArray.Length == 0)
                    return String.Empty;
                else
                {
                    foreach (object attribValue in attribArray)
                    {
                        string objType = attribValue.GetType().ToString().ToLower();
                        if (attribValue.GetType() == typeof(Paraenums.EnumDescriptionAttribute))
                        {
                            Paraenums.EnumDescriptionAttribute attrib = (Paraenums.EnumDescriptionAttribute)attribValue;
                            return attrib.Description;
                        }
                    }
                    return String.Empty;
                }


                ////string serverfarmaddress = "";
                ////switch (sf)
                ////{
                ////    case ParaConnect.Paraenums.ServerFarm.Demo:
                ////        serverfarmaddress = "https://demo.parature.com/";
                ////        break;
                ////    case ParaConnect.Paraenums.ServerFarm.SCO:
                ////        serverfarmaddress = "https://www.supportcenteronline.com/";
                ////        break;
                ////    case ParaConnect.Paraenums.ServerFarm.SandboxSCO:
                ////        serverfarmaddress = "https://sco-sandbox.parature.com/";
                ////        break;
                ////    case ParaConnect.Paraenums.ServerFarm.Eas:
                ////        serverfarmaddress = "https://eas.parature.net/";
                ////        break;
                ////    case ParaConnect.Paraenums.ServerFarm.D2:
                ////        serverfarmaddress = "https://d2.parature.com/";
                ////        break;
                ////    case ParaConnect.Paraenums.ServerFarm.D3:
                ////        serverfarmaddress = "https://d3.parature.com/";
                ////        break;
                ////    case ParaConnect.Paraenums.ServerFarm.Premium:
                ////        serverfarmaddress = "https://premium.parature.com/";
                ////        break;
                ////    case ParaConnect.Paraenums.ServerFarm.S2:
                ////        serverfarmaddress = "https://s2.parature.com/";
                ////        break;
                ////    case ParaConnect.Paraenums.ServerFarm.S3:
                ////        serverfarmaddress = "https://s3.parature.com/";
                ////        break;
                ////    case ParaConnect.Paraenums.ServerFarm.SandboxS3:
                ////        serverfarmaddress = "https://s3-sandbox.parature.com/";
                ////        break;
                ////}
                ////return serverfarmaddress;
            }

            /// <summary>
            /// Returns the Http request method for an API call, requires an ApiCallHttpMethod enum.
            /// </summary>
            internal static String ApiHttpPostProvider(Paraenums.ApiCallHttpMethod ApiCallHttpMethod)
            {
                string httpPostMethod = "";
                switch (ApiCallHttpMethod)
                {
                    case ParaConnect.Paraenums.ApiCallHttpMethod.Get:
                        httpPostMethod = "GET";
                        break;

                    case ParaConnect.Paraenums.ApiCallHttpMethod.Delete:
                        httpPostMethod = "DELETE";
                        break;

                    case ParaConnect.Paraenums.ApiCallHttpMethod.Post:
                        httpPostMethod = "POST";
                        break;

                    case ParaConnect.Paraenums.ApiCallHttpMethod.Update:
                        httpPostMethod = "PUT";
                        break;
                }
                return httpPostMethod;
            }


            /// <summary>
            /// Return the customFieldDataType enum, requires a string. Usually the string comes from the API response.
            /// </summary>
            internal static Paraenums.CustomFieldDataType CustomFieldDataTypeProvider(string dt)
            {
                Paraenums.CustomFieldDataType dataType = Paraenums.CustomFieldDataType.Unknown;
                switch (dt.ToLower())
                {
                    case "boolean":

                        dataType = Paraenums.CustomFieldDataType.boolean;
                        break;
                    case "float":

                        dataType = Paraenums.CustomFieldDataType.Float;
                        break;
                    case "attachment":

                        dataType = Paraenums.CustomFieldDataType.attachment;
                        break;
                    case "datetime":

                        dataType = Paraenums.CustomFieldDataType.DateTime;
                        break;
                    case "int":
                        dataType = Paraenums.CustomFieldDataType.Int;

                        break;
                    case "date":
                        dataType = Paraenums.CustomFieldDataType.Date;
                        break;


                    case "readonly":
                        dataType = Paraenums.CustomFieldDataType.ReadOnly;
                        break;


                    case "string":
                        dataType = Paraenums.CustomFieldDataType.String;
                        break;

                    case "option":
                        dataType = Paraenums.CustomFieldDataType.Option;
                        break;

                }
                return dataType;
            }
        }

        internal partial class ParaThrottler
        {


            /// <summary>
            /// Returns the path of the application
            /// </summary>
            static string GetAppPath()
            {
                // Returns the application path...
                System.Reflection.Module[] modules = System.Reflection.Assembly.GetExecutingAssembly().GetModules();

                string aPath = System.IO.Path.GetDirectoryName(modules[0].FullyQualifiedName);

                if ((aPath != "") && (aPath[aPath.Length - 1] != '\\'))
                {
                    aPath += '\\';
                }
                return aPath;
            }
            static String LoadLastAccessed()
            {
                if (File.Exists(StampFilePath()) == false)
                {
                    SaveLastAccessed(ToUtcTime(DateTime.UtcNow));
                }
                System.IO.StreamReader objStreamReader = new System.IO.StreamReader(StampFilePath());
                //Now, read the entire file into a string
                String contents = objStreamReader.ReadToEnd();
                objStreamReader.Close();
                System.IO.File.Delete(StampFilePath());
                return contents;
                //return DateTime.Parse(contents);
            }
            static void SaveLastAccessed(string stamp)
            {
                if (File.Exists(StampFilePath()) == true)
                {
                    File.Delete(StampFilePath());
                }
                System.IO.StreamWriter objStreamWriter = new System.IO.StreamWriter(StampFilePath());
                objStreamWriter.WriteLine(stamp);
                objStreamWriter.Close();
            }
            private static string StampFilePath()
            {
                return GetAppPath() + "config\\thrott.txt";
            }
            /// <summary>
            /// To Zulu (UTC) time format.
            /// </summary>
            /// <param name="date"> The datetime.</param>
            /// <returns> Returns the datetime in zulu string format.</returns>
            private static string ToUtcTime(DateTime date)
            {
                //return date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
                return date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            }

        }

        internal partial class HelperMethods
        {
            internal static bool CustomFieldReset(Int64 CustomFieldid, List<ParaObjects.CustomField> fields)
            {
                bool modified = false;
                if (CustomFieldid > 0 && fields != null)
                {
                    foreach (ParaObjects.CustomField cf in fields)
                    {
                        if (cf.CustomFieldID == CustomFieldid)
                        {
                            if (cf.CustomFieldOptionsCollection.Count > 0)
                            {
                                foreach (ParaObjects.CustomFieldOptions cfo in cf.CustomFieldOptionsCollection)
                                {
                                    if (cfo.IsSelected == true)
                                    {
                                        cfo.IsSelected = false;
                                        modified = true;
                                    }
                                }
                            }
                            if (cf.CustomFieldValue != "")
                            {
                                cf.CustomFieldValue = "";
                                modified = true;
                            }

                            break;
                        }
                    }
                }

                return modified;
            }

            /// <summary>
            /// Adds a custom field to the account object, with the value you specify.
            /// </summary>
            /// <param name="account">
            /// The account object you need to add the custom field to.
            /// </param>
            /// <param name="CustomFieldid">
            /// The id of the custom field to add.
            /// </param>
            /// <param name="CustomFieldValue">
            /// The value to set for the cust field.
            ///</param>
            internal static bool CustomFieldSetValue(Int64 CustomFieldid, string CustomFieldValue, List<ParaObjects.CustomField> Fields, bool ignoreCase)
            {
                bool modified = false;
                bool found = false;
                ParaObjects.CustomField cf = new ParaObjects.CustomField();
                if (CustomFieldid > 0)
                {
                    foreach (ParaObjects.CustomField cfn in Fields)
                    {
                        if (cfn.CustomFieldID == CustomFieldid)
                        {
                            cf = cfn;
                            found = true;
                            break;
                        }
                    }

                    // Not found in the list of CFs, need to add this field.
                    if (found == false)
                    {
                        cf.CustomFieldID = CustomFieldid;
                        modified = true;
                    }

                    if (string.Compare(cf.CustomFieldValue.ToString(), CustomFieldValue.ToString(), ignoreCase) != 0)
                    {
                        modified = true;
                        if (string.IsNullOrEmpty(CustomFieldValue))
                        {
                            cf.FlagToDelete = true;
                        }
                        cf.CustomFieldValue = CustomFieldValue;
                    }

                    if (found == false)
                    {
                        Fields.Add(cf);
                    }

                }

                return modified;
            }


            internal static bool CustomFieldSetFieldOption(Int64 CustomFieldid, string CustomFieldOptionName, List<ParaObjects.CustomField> Fields, bool ResetOtherOptions, bool ignoreCase)
            {


                if (CustomFieldid > 0 && string.IsNullOrEmpty(CustomFieldOptionName) == false)
                {
                    foreach (ParaObjects.CustomField cfn in Fields)
                    {
                        if (cfn.CustomFieldID == CustomFieldid)
                        {
                            foreach (ParaObjects.CustomFieldOptions option in cfn.CustomFieldOptionsCollection)
                            {
                                if (string.Compare(option.CustomFieldOptionName, CustomFieldOptionName, ignoreCase) == 0)
                                {
                                    return CustomFieldSetFieldOption(CustomFieldid, option.CustomFieldOptionID, Fields, ResetOtherOptions);
                                }
                            }

                            break;
                        }

                    }
                }
                return false;
            }


            internal static bool CustomFieldSetFieldOption(Int64 CustomFieldid, Int64 CustomFieldOptionid, List<ParaObjects.CustomField> Fields, bool ResetOtherOptions)
            {
                bool modified = false;

                if (CustomFieldid > 0 && CustomFieldOptionid > 0)
                {
                    bool found = false;
                    foreach (ParaObjects.CustomField cfn in Fields)
                    {
                        if (cfn.CustomFieldID == CustomFieldid)
                        {
                            found = true;
                            bool optionFound = false;
                            foreach (ParaObjects.CustomFieldOptions option in cfn.CustomFieldOptionsCollection)
                            {
                                if (option.CustomFieldOptionID == CustomFieldOptionid)
                                {
                                    if (option.IsSelected == false)
                                    {
                                        modified = true;
                                        option.IsSelected = true;
                                    }

                                    optionFound = true;
                                    if (ResetOtherOptions == false)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    if (ResetOtherOptions == true)
                                    {
                                        if (option.IsSelected == true)
                                        {
                                            modified = true;
                                            option.IsSelected = false;
                                        }
                                    }
                                }
                            }
                            if (optionFound == false)
                            {
                                ParaObjects.CustomFieldOptions NewOption = new ParaObjects.CustomFieldOptions();
                                NewOption.CustomFieldOptionID = CustomFieldOptionid;
                                NewOption.IsSelected = true;
                                cfn.CustomFieldOptionsCollection.Add(NewOption);
                                modified = true;
                                found = true;
                            }
                            break;
                        }
                    }
                    if (found == false)
                    {
                        ParaObjects.CustomField cf = new ParaObjects.CustomField();
                        cf.CustomFieldID = CustomFieldid;
                        ParaObjects.CustomFieldOptions NewOption = new ParaObjects.CustomFieldOptions();
                        NewOption.CustomFieldOptionID = CustomFieldOptionid;
                        NewOption.IsSelected = true;
                        cf.CustomFieldOptionsCollection.Add(NewOption);
                        //cf.CustomFieldValue = CustomFieldValue;
                        modified = true;
                        Fields.Add(cf);
                    }
                }
                return modified;
            }
            
            internal static string SafeHtmlDecode(string input)
            {
                return input.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&");
            }
        }
    }
}
