using System;
using System.Collections.Generic;
using System.Linq;
using ParatureAPI.Fields;
using ParatureAPI.ParaHelper;

namespace ParatureAPI.ParaObjects
{
    public abstract class ModuleWithCustomFields : ObjectBaseProperties
    {
        public ModuleWithCustomFields()
        {

        }

        public ModuleWithCustomFields(ModuleWithCustomFields moduleWithCustomFields)
            : base(moduleWithCustomFields)
        {
            if (moduleWithCustomFields != null && moduleWithCustomFields.CustomFields != null)
            {
                CustomFields = moduleWithCustomFields.CustomFields;
            }
        }

        public List<Field> Fields = new List<Field>();

        /// <summary>
        /// Convenience method to retrieve only static fields
        /// </summary>
        public IEnumerable<StaticField> StaticFields
        {
            get { return Fields.Where(f => f is StaticField) as List<StaticField>; }
            set
            {
                Fields.RemoveAll(f => f is StaticField);
                Fields.AddRange(value);
            }
        }

        /// <summary>
        /// Convenience method to retrieve only custom fields
        /// </summary>
        public IEnumerable<CustomField> CustomFields
        {
            get
            {
                return Fields.OfType<CustomField>();
            }
            set
            {
                Fields.RemoveAll(f => f is CustomField);
                Fields.AddRange(value);
            }
        }

        /// <summary>
        /// This method accepts a custom field id and will reset all of its values: if this custom field has any options, they will be 
        /// all unselected. If the custom field value is not set to "", this method will set it. Basically, the field will be empty.            /// 
        /// </summary>           
        /// <returns>Returns True if the custom field was modified, False if there was no need to modify the custom field.</returns>
        public bool CustomFieldReset(Int64 CustomFieldid)
        {
            return DirtyStateManager(HelperMethods.CustomFieldReset(CustomFieldid, CustomFields));
        }

        /// <summary>
        /// Sets the value of a custom field in the fields collection of this object. If there is no custom field with the 
        /// id that you pass to this method, a new custom field will be created and its value will be set to the one 
        /// passed to this method. Set the Ignore case to indicate whether the comparison should take into account the case or not.
        /// </summary>
        /// <returns>
        /// returns True if the custom field was modified (or created), False if there was no need to modify the custom field.
        ///</returns>
        public bool CustomFieldSetValue(Int64 CustomFieldid, string CustomFieldValue, bool ignoreCase)
        {
            return DirtyStateManager(CustomFieldSetValue(CustomFieldid, CustomFieldValue, CustomFields, ignoreCase));
        }

        /// <summary>
        /// Sets the value of a custom field in the fields collection of this object. If there is no custom field with the 
        /// id that you pass to this method, a new custom field will be created and its value will be set to the one 
        /// passed to this method.
        /// </summary>
        /// <returns>
        /// returns True if the custom field was modified (or created), False if there was no need to modify the custom field.
        ///</returns>
        public bool CustomFieldSetValue(Int64 CustomFieldid, string CustomFieldValue)
        {
            return CustomFieldSetValue(CustomFieldid, CustomFieldValue, true);
        }

        /// <summary>
        /// Sets the value of a custom field in the fields collection of this object. If there is no custom field with the 
        /// id that you pass to this method, a new custom field will be created and its value will be set to the one 
        /// passed to this method.
        /// </summary>
        /// <returns>
        /// returns True if the custom field was modified (or created), False if there was no need to modify the custom field.
        ///</returns>
        public bool CustomFieldSetValue(Int64 CustomFieldid, bool CustomFieldValue)
        {
            return DirtyStateManager(CustomFieldSetValue(CustomFieldid, CustomFieldValue.ToString().ToLower(), CustomFields, true));
        }


        /// <summary>
        /// Sets the value of a custom field in the fields collection of this object. If there is no custom field with the 
        /// id that you pass to this method, a new custom field will be created and its value will be set to the one 
        /// passed to this method.
        /// </summary>
        /// <returns>
        /// returns True if the custom field was modified (or created), False if there was no need to modify the custom field.
        ///</returns>
        public bool CustomFieldSetValue(Int64 CustomFieldid, DateTime CustomFieldValue)
        {
            string date = CustomFieldValue.ToString("yyyy-MM-ddTHH:mm:ss");
            return DirtyStateManager(CustomFieldSetValue(CustomFieldid, date, CustomFields, true));
        }

        /// <summary>
        /// Will reset the value of the custom field with id you pass to this method, and then will make 
        /// sure to send the custom field back with an empty value so that it deletes the value stored in Parature 
        /// for this custom field.
        /// </summary>
        public void CustomFieldFlagToDelete(Int64 CustomFieldid)
        {
            if (CustomFieldid > 0)
            {
                List<CustomField> fields = new List<CustomField>();
                foreach (CustomField cf in CustomFields)
                {
                    if (cf.CustomFieldID == CustomFieldid)
                    {
                        fields.Add(cf);
                        DirtyStateManager(HelperMethods.CustomFieldReset(CustomFieldid, fields));
                        cf.FlagToDelete = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Look for a custom field with the id passed and then set the selected option to the CustomFieldOptionid passed.
        /// If any other custom field option is selected, it will be unselected. If the option you need to select is not part 
        /// of the custom field options, it will add it. Finally, if a custom field with the id you are passing does not exist, it will create one 
        /// with the proper custom field option you need.
        /// </summary>          
        /// <returns>
        /// returns True if the custom field was modified (or created), False if there was no need to modify the custom field.
        /// </returns>
        public bool CustomFieldSetSelectedFieldOption(Int64 CustomFieldid, Int64 CustomFieldOptionid)
        {
            return DirtyStateManager(CustomFieldSetFieldOption(CustomFieldid, CustomFieldOptionid, CustomFields, true));
        }

        /// <summary>
        /// Look for a custom field with the id passed and then set the selected option to the CustomFieldOption name passed.
        /// If any other custom field option is selected, it will be unselected. If the option you need to select is not part 
        /// of the custom field options, it will add it. Finally, if a custom field with the id you are passing does not exist, it will create one 
        /// with the proper custom field option you need.
        /// </summary>          
        /// <returns>
        /// returns True if the custom field was modified (or created), False if there was no need to modify the custom field.
        /// </returns>
        public bool CustomFieldSetSelectedFieldOption(Int64 CustomFieldid, string CustomFieldOptionname, bool ignoreCase)
        {
            return DirtyStateManager(CustomFieldSetFieldOption(CustomFieldid, CustomFieldOptionname, CustomFields, true, ignoreCase));
        }

        /// <summary>
        /// Look for a custom field with the id passed and then set the selected option to the CustomFieldOptionid passed.
        /// If any other custom field option is selected, it will stay selected. If the option you need to select is not part 
        /// of the custom field options, it will add it. Finally, if a custom field with the id you are passing does not exist, it will create one 
        /// with the proper custom field option you need.
        /// </summary>          
        /// <returns>
        /// returns True if the custom field was modified (or created), False if there was no need to modify the custom field.
        /// </returns>
        public bool CustomFieldAddSelectedFieldOption(Int64 CustomFieldid, Int64 CustomFieldOptionid)
        {
            return DirtyStateManager(CustomFieldSetFieldOption(CustomFieldid, CustomFieldOptionid, CustomFields, false));
        }

        /// <summary>
        /// Allows you to add an option to the selected field options already in this custom field.
        /// </summary>
        public bool CustomFieldAddSelectedFieldOption(Int64 CustomFieldid, string CustomFieldOptionname, bool ignoreCase)
        {
            return DirtyStateManager(CustomFieldSetFieldOption(CustomFieldid, CustomFieldOptionname, CustomFields, false, ignoreCase));
        }

        /// <summary>
        /// Returns the selected custom field option object for a custom field. Will return the first encountered selected 
        /// field options object.
        /// </summary>           
        public CustomFieldOptions CustomFieldGetSelectedOption(Int64 CustomFieldid)
        {
            foreach (CustomField cf in CustomFields)
            {
                if (cf.CustomFieldID == CustomFieldid)
                {
                    foreach (CustomFieldOptions cfo in cf.CustomFieldOptionsCollection)
                    {
                        if (cfo.IsSelected == true)
                        {
                            return cfo;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the selected custom field option object for a custom field. Will return the first encountered selected 
        /// field options object.
        /// </summary>  
        public CustomFieldOptions CustomFieldGetSelectedOption(CustomField CustomField)
        {

            foreach (CustomFieldOptions cfo in CustomField.CustomFieldOptionsCollection)
            {
                if (cfo.IsSelected == true)
                {
                    return cfo;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the selected custom field option object for a custom field. Will return the first encountered selected 
        /// field options object.
        /// </summary>  
        public CustomFieldOptions CustomFieldGetSelectedOption(string CustomFieldName)
        {
            foreach (CustomField cf in CustomFields)
            {
                if (cf.CustomFieldName == CustomFieldName)
                {
                    foreach (CustomFieldOptions cfo in cf.CustomFieldOptionsCollection)
                    {
                        if (cfo.IsSelected == true)
                        {
                            return cfo;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the list of all selected custom field options objects for a custom field.             
        /// </summary>  
        public List<CustomFieldOptions> CustomFieldGetSelectedOptions(Int64 CustomFieldid)
        {
            List<CustomFieldOptions> SelectedOptions = new List<CustomFieldOptions>();
            foreach (CustomField cf in CustomFields)
            {
                if (cf.CustomFieldID == CustomFieldid)
                {
                    foreach (CustomFieldOptions cfo in cf.CustomFieldOptionsCollection)
                    {
                        if (cfo.IsSelected == true)
                        {
                            SelectedOptions.Add(cfo);
                        }
                    }
                }
            }
            return SelectedOptions;
        }

        /// <summary>
        /// Returns the list of all custom field options for a custom field.
        /// </summary>
        /// <param name="CustomFieldId"></param>
        /// <returns></returns>
        public List<CustomFieldOptions> CustomFieldGetOptions(Int64 CustomFieldId)
        {
            List<CustomFieldOptions> options = new List<CustomFieldOptions>();
                
            foreach (CustomField cf in CustomFields)
            {
                if (cf.CustomFieldID == CustomFieldId)
                {
                    foreach (CustomFieldOptions cfo in cf.CustomFieldOptionsCollection)
                    {
                        options.Add(cfo);
                    }
                }
            }

            return options;

        }

        /// <summary>
        /// Returns the list of all selected custom field options objects for a custom field.             
        /// </summary>  
        public List<CustomFieldOptions> CustomFieldGetSelectedOptions(CustomField CustomField)
        {
            List<CustomFieldOptions> SelectedOptions = new List<CustomFieldOptions>();
            foreach (CustomFieldOptions cfo in CustomField.CustomFieldOptionsCollection)
            {
                if (cfo.IsSelected == true)
                {
                    SelectedOptions.Add(cfo);
                }
            }

            return SelectedOptions;
        }

        /// <summary>
        /// Returns the list of all selected custom field options objects for a custom field.             
        /// </summary>  
        public List<CustomFieldOptions> CustomFieldGetSelectedOptions(string CustomFieldName)
        {
            List<CustomFieldOptions> SelectedOptions = new List<CustomFieldOptions>();
            foreach (CustomField cf in CustomFields)
            {
                if (cf.CustomFieldName == CustomFieldName)
                {
                    foreach (CustomFieldOptions cfo in cf.CustomFieldOptionsCollection)
                    {
                        if (cfo.IsSelected == true)
                        {
                            SelectedOptions.Add(cfo);
                        }
                    }
                }
            }
            return SelectedOptions;
        }

        /// <summary>
        /// Returns the display name for the custom field id you specified.  Will return empty string if
        /// the custom field is not found.
        /// </summary>
        /// <param name="CustomFieldId"></param>
        /// <returns></returns>
        public string CustomFieldGetDisplayName(Int64 CustomFieldId)
        {
            foreach (CustomField cf in CustomFields)
            {
                if (cf.CustomFieldID == CustomFieldId)
                {
                    return cf.CustomFieldName;
                }
            }

            return String.Empty;
        }

        /// <summary>
        ///  Return the id for the custom field name you specified.  Will return -1 if the custom field is not found 
        ///  or duplicates are found.  Search will be case-insensitive.
        /// </summary>
        /// <param name="CustomFieldName"></param>
        /// <returns></returns>
        public Int64 CustomFieldGetId(string CustomFieldName)
        {
            Int64 customFieldId = -1;

            if (this.CustomFields != null)
            {
                foreach(CustomField cf in this.CustomFields)
                {
                    if (cf.CustomFieldName.ToLower() == CustomFieldName.ToLower())
                    {
                        if (customFieldId != -1)
                        {
                            // field name was found more than one time, return -1
                            return -1;
                        }
                        else
                        {
                            customFieldId = cf.CustomFieldID;
                        }
                    }
                }
            }

            return customFieldId;
        }


        /// <summary>
        /// Returns the value for the custom field id you specified. Multiple values will be separated by "||"
        /// Will return empty string if the custom field was not found.
        /// </summary>  
        public string CustomFieldGetValue(Int64 CustomFieldid)
        {
            foreach (CustomField cf in CustomFields)
            {
                if (cf.CustomFieldID == CustomFieldid)
                {
                    if (cf.DataType == ParaEnums.CustomFieldDataType.Option)
                    {
                        // Custom field is an option field, iterate through the options

                        string returnValue = "";

                        foreach (CustomFieldOptions cfo in cf.CustomFieldOptionsCollection)
                        {
                            if (cfo.IsSelected == true)
                            {
                                if (!String.IsNullOrEmpty(returnValue))
                                {
                                    returnValue += "||";
                                }

                                returnValue += cfo.CustomFieldOptionName;
                            }
                        }

                        return returnValue;
                    }
                    else
                    {
                        return cf.CustomFieldValue;
                    }
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// Returns the value for the custom field name you specified. Will return null if the custom field 
        /// was not found.
        /// </summary> 
        public string CustomFieldGetValue(string CustomFieldName)
        {
            foreach (CustomField cf in CustomFields)
            {
                if (cf.CustomFieldName == CustomFieldName)
                {
                    return cf.CustomFieldValue;
                }
            }
            return "";
        }

        public abstract string GetReadableName();

        ///  <summary>
        ///  Adds a custom field to the account object, with the value you specify.
        ///  </summary>
        ///  <param name="account">
        ///  The account object you need to add the custom field to.
        ///  </param>
        ///  <param name="customFieldId">
        ///  The id of the custom field to add.
        ///  </param>
        ///  <param name="customFieldValue">
        ///  The value to set for the cust field.
        /// </param>
        /// <param name="customFields"></param>
        /// <param name="ignoreCase"></param>
        internal bool CustomFieldSetValue(Int64 customFieldId, string customFieldValue, IEnumerable<CustomField> customFields, bool ignoreCase)
        {
            var modified = false;
            var found = false;
            var cf = new CustomField();
            if (customFieldId > 0)
            {
                foreach (var cfn in customFields)
                {
                    if (cfn.CustomFieldID == customFieldId)
                    {
                        cf = cfn;
                        found = true;
                        break;
                    }
                }

                // Not found in the list of CFs, need to add this field.
                if (found == false)
                {
                    cf.CustomFieldID = customFieldId;
                    modified = true;
                }

                if (String.Compare(cf.CustomFieldValue.ToString(), customFieldValue.ToString(), ignoreCase) != 0)
                {
                    modified = true;
                    if (String.IsNullOrEmpty(customFieldValue))
                    {
                        cf.FlagToDelete = true;
                    }
                    cf.CustomFieldValue = customFieldValue;
                }

                if (found == false)
                {
                    Fields.Add(cf);
                }

            }

            return modified;
        }

        internal bool CustomFieldSetFieldOption(Int64 CustomFieldid, string CustomFieldOptionName, IEnumerable<CustomField> customFields, bool ResetOtherOptions, bool ignoreCase)
        {


            if (CustomFieldid > 0 && String.IsNullOrEmpty(CustomFieldOptionName) == false)
            {
                foreach (CustomField cfn in customFields)
                {
                    if (cfn.CustomFieldID == CustomFieldid)
                    {
                        foreach (CustomFieldOptions option in cfn.CustomFieldOptionsCollection)
                        {
                            if (String.Compare(option.CustomFieldOptionName, CustomFieldOptionName, ignoreCase) == 0)
                            {
                                return CustomFieldSetFieldOption(CustomFieldid, option.CustomFieldOptionID, customFields, ResetOtherOptions);
                            }
                        }

                        break;
                    }

                }
            }
            return false;
        }

        internal bool CustomFieldSetFieldOption(Int64 CustomFieldid, Int64 CustomFieldOptionid, IEnumerable<CustomField> customFields, bool ResetOtherOptions)
        {
            bool modified = false;

            if (CustomFieldid > 0 && CustomFieldOptionid > 0)
            {
                bool found = false;
                foreach (var cfn in customFields)
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
                    modified = true;
                    Fields.Add(cf);
                }
            }
            return modified;
        }
    }
}