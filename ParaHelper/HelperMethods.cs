using System;
using System.Collections.Generic;
using ParatureAPI.Fields;

namespace ParatureAPI.ParaHelper
{
    internal class HelperMethods
    {
        internal static bool CustomFieldReset(Int64 CustomFieldid, List<CustomField> fields)
        {
            bool modified = false;
            if (CustomFieldid > 0 && fields != null)
            {
                foreach (CustomField cf in fields)
                {
                    if (cf.CustomFieldID == CustomFieldid)
                    {
                        if (cf.CustomFieldOptionsCollection.Count > 0)
                        {
                            foreach (CustomFieldOptions cfo in cf.CustomFieldOptionsCollection)
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
        internal static bool CustomFieldSetValue(Int64 CustomFieldid, string CustomFieldValue, List<CustomField> Fields, bool ignoreCase)
        {
            bool modified = false;
            bool found = false;
            CustomField cf = new CustomField();
            if (CustomFieldid > 0)
            {
                foreach (CustomField cfn in Fields)
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

                if (String.Compare(cf.CustomFieldValue.ToString(), CustomFieldValue.ToString(), ignoreCase) != 0)
                {
                    modified = true;
                    if (String.IsNullOrEmpty(CustomFieldValue))
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


        internal static bool CustomFieldSetFieldOption(Int64 CustomFieldid, string CustomFieldOptionName, List<CustomField> Fields, bool ResetOtherOptions, bool ignoreCase)
        {


            if (CustomFieldid > 0 && String.IsNullOrEmpty(CustomFieldOptionName) == false)
            {
                foreach (CustomField cfn in Fields)
                {
                    if (cfn.CustomFieldID == CustomFieldid)
                    {
                        foreach (CustomFieldOptions option in cfn.CustomFieldOptionsCollection)
                        {
                            if (String.Compare(option.CustomFieldOptionName, CustomFieldOptionName, ignoreCase) == 0)
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


        internal static bool CustomFieldSetFieldOption(Int64 CustomFieldid, Int64 CustomFieldOptionid, List<CustomField> Fields, bool ResetOtherOptions)
        {
            bool modified = false;

            if (CustomFieldid > 0 && CustomFieldOptionid > 0)
            {
                bool found = false;
                foreach (CustomField cfn in Fields)
                {
                    if (cfn.CustomFieldID == CustomFieldid)
                    {
                        found = true;
                        bool optionFound = false;
                        foreach (CustomFieldOptions option in cfn.CustomFieldOptionsCollection)
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
                            CustomFieldOptions NewOption = new CustomFieldOptions();
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
                    CustomField cf = new CustomField();
                    cf.CustomFieldID = CustomFieldid;
                    CustomFieldOptions NewOption = new CustomFieldOptions();
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