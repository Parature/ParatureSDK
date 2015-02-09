using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using ParatureSDK.Fields;
using ParatureSDK.ParaHelper;

namespace ParatureSDK.ParaObjects
{
    /// <summary>
    /// Base class for all Parature Entities (Modules) retrievable via the API
    /// </summary>
    public abstract class ParaEntity : ParaEntityBaseProperties
    {
        public ParaEntity()
        { }

        public ParaEntity(ParaEntity paraEntity)
            : base(paraEntity)
        {
            if (paraEntity == null)
            {
                Fields = new List<Field>();
                return;
            }

            Id = paraEntity.Id;
            Fields = paraEntity.Fields;
        }

        /// <summary>
        /// Retrieve a Field based off of its field name (tag element). 
        /// This is only for static fields - use an integer to search custom fields.
        /// </summary>
        /// <param name="fieldName">Name of the field. Use the tag, not the display name ("Date_Created" rather than "Date Created" for example)</param>
        /// <returns>StaticField if found, otherwise null</returns>
        public StaticField this[string fieldName]
        {
            get { return StaticFields.FirstOrDefault(f => f.Name == fieldName); }
            set
            {
                Fields.RemoveAll(f => f.Name == fieldName && f is StaticField);
                Fields.Add(value);
            }
        }

        /// <summary>
        /// Retrieve a Field based off of its custom field Id. 
        /// This is only for custom fields - use a string to search custom fields.
        /// </summary>
        /// <param name="fieldId">Custom Field Id</param>
        /// <returns>CustomField if found, otherwise null</returns>
        public CustomField this[int fieldId]
        {
            get { return CustomFields.FirstOrDefault(f => f.Id == fieldId); }
            set
            {
                var matchingCustomField = CustomFields.FirstOrDefault(f => f.Id == fieldId);
                Fields.Remove(matchingCustomField);
                Fields.Add(value);
            }
        }

        /// <summary>
        /// Retrieve the value of a specific field, typecast to the provided type.
        /// </summary>
        /// <param name="fieldName">Static Field Name</param>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidCastException">Thrown if the type provided is incorrect</exception>
        public T GetFieldValue<T>(string fieldName)
        {
            var field = this[fieldName];
            if (field != null)
            {
                return (T)field.Value;
            }

            return default(T);
        }

        /// <summary>
        /// Retrieve the value of a specific field, typecast to the provided type.
        /// </summary> 
        /// <param name="fieldId">Custom Field Id</param>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <exception cref="InvalidCastException">Thrown if the type provided is incorrect</exception>
        public T GetFieldValue<T>(int fieldId)
        {
            var field = this[fieldId];
            if (field != null)
            {
                return (T)field.Value;
            }

            return default(T);
        }

        /// <summary>
        /// List of all fields for this entity
        /// </summary>
        [XmlIgnore]
        public List<Field> Fields = new List<Field>();

        /// <summary>
        /// Convenience method to retrieve only static fields
        /// </summary>
        [XmlIgnore]
        public IEnumerable<StaticField> StaticFields
        {
            get { return Fields.OfType<StaticField>(); }
            set
            {
                Fields.RemoveAll(f => f is StaticField);
                Fields.AddRange(value);
            }
        }

        /// <summary>
        /// Convenience method to retrieve only custom fields
        /// </summary>
        [XmlIgnore]
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
        public bool CustomFieldReset(Int64 customFieldId)
        {
            return DirtyStateManager(HelperMethods.CustomFieldReset(customFieldId, CustomFields));
        }

        /// <summary>
        /// Sets the value of a custom field in the fields collection of this object. If there is no custom field with the 
        /// id that you pass to this method, a new custom field will be created and its value will be set to the one 
        /// passed to this method. Set the Ignore case to indicate whether the comparison should take into account the case or not.
        /// </summary>
        /// <returns>
        /// returns True if the custom field was modified (or created), False if there was no need to modify the custom field.
        ///</returns>
        public bool CustomFieldSetValue(Int64 customFieldId, string customFieldValue, bool ignoreCase)
        {
            return DirtyStateManager(CustomFieldSetValue(customFieldId, customFieldValue, CustomFields, ignoreCase));
        }

        /// <summary>
        /// Sets the value of a custom field in the fields collection of this object. If there is no custom field with the 
        /// id that you pass to this method, a new custom field will be created and its value will be set to the one 
        /// passed to this method.
        /// </summary>
        /// <returns>
        /// returns True if the custom field was modified (or created), False if there was no need to modify the custom field.
        ///</returns>
        public bool CustomFieldSetValue(Int64 customFieldId, string customFieldValue)
        {
            return CustomFieldSetValue(customFieldId, customFieldValue, true);
        }

        /// <summary>
        /// Sets the value of a custom field in the fields collection of this object. If there is no custom field with the 
        /// id that you pass to this method, a new custom field will be created and its value will be set to the one 
        /// passed to this method.
        /// </summary>
        /// <returns>
        /// returns True if the custom field was modified (or created), False if there was no need to modify the custom field.
        ///</returns>
        public bool CustomFieldSetValue(Int64 customFieldId, bool customFieldValue)
        {
            return DirtyStateManager(CustomFieldSetValue(customFieldId, customFieldValue.ToString().ToLower(), CustomFields, true));
        }

        /// <summary>
        /// Sets the value of a custom field in the fields collection of this object. If there is no custom field with the 
        /// id that you pass to this method, a new custom field will be created and its value will be set to the one 
        /// passed to this method.
        /// </summary>
        /// <returns>
        /// returns True if the custom field was modified (or created), False if there was no need to modify the custom field.
        ///</returns>
        public bool CustomFieldSetValue(Int64 customFieldId, DateTime customFieldValue)
        {
            string date = customFieldValue.ToString("yyyy-MM-ddTHH:mm:ss");
            return DirtyStateManager(CustomFieldSetValue(customFieldId, date, CustomFields, true));
        }

        /// <summary>
        /// Will reset the value of the custom field with id you pass to this method, and then will make 
        /// sure to send the custom field back with an empty value so that it deletes the value stored in Parature 
        /// for this custom field.
        /// </summary>
        public void CustomFieldFlagToDelete(Int64 customFieldId)
        {
            if (customFieldId > 0)
            {
                List<CustomField> fields = new List<CustomField>();
                foreach (CustomField cf in CustomFields)
                {
                    if (cf.Id == customFieldId)
                    {
                        fields.Add(cf);
                        DirtyStateManager(HelperMethods.CustomFieldReset(customFieldId, fields));
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
        public bool CustomFieldSetSelectedFieldOption(Int64 customFieldId, Int64 customFieldOptionId)
        {
            return DirtyStateManager(CustomFieldSetFieldOption(customFieldId, customFieldOptionId, CustomFields, true));
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
        public bool CustomFieldSetSelectedFieldOption(Int64 customFieldId, string customFieldOptionName, bool ignoreCase)
        {
            return DirtyStateManager(CustomFieldSetFieldOption(customFieldId, customFieldOptionName, CustomFields, true, ignoreCase));
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
        public bool CustomFieldAddSelectedFieldOption(Int64 customFieldId, Int64 customFieldOptionId)
        {
            return DirtyStateManager(CustomFieldSetFieldOption(customFieldId, customFieldOptionId, CustomFields, false));
        }

        /// <summary>
        /// Allows you to add an option to the selected field options already in this custom field.
        /// </summary>
        public bool CustomFieldAddSelectedFieldOption(Int64 customFieldId, string customFieldOptionName, bool ignoreCase)
        {
            return DirtyStateManager(CustomFieldSetFieldOption(customFieldId, customFieldOptionName, CustomFields, false, ignoreCase));
        }

        /// <summary>
        /// Returns the selected custom field option object for a custom field. Will return the first encountered selected 
        /// field options object.
        /// </summary>           
        public CustomFieldOptions CustomFieldGetSelectedOption(Int64 customFieldId)
        {
            foreach (CustomField cf in CustomFields)
            {
                if (cf.Id == customFieldId)
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
        public CustomFieldOptions CustomFieldGetSelectedOption(CustomField customField)
        {

            foreach (CustomFieldOptions cfo in customField.CustomFieldOptionsCollection)
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
        public CustomFieldOptions CustomFieldGetSelectedOption(string customFieldName)
        {
            foreach (CustomField cf in CustomFields)
            {
                if (cf.Name == customFieldName)
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
        public List<CustomFieldOptions> CustomFieldGetSelectedOptions(Int64 customFieldid)
        {
            List<CustomFieldOptions> SelectedOptions = new List<CustomFieldOptions>();
            foreach (CustomField cf in CustomFields)
            {
                if (cf.Id == customFieldid)
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
        /// <param name="customFieldId"></param>
        /// <returns></returns>
        public List<CustomFieldOptions> CustomFieldGetOptions(Int64 customFieldId)
        {
            List<CustomFieldOptions> options = new List<CustomFieldOptions>();
                
            foreach (CustomField cf in CustomFields)
            {
                if (cf.Id == customFieldId)
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
        public List<CustomFieldOptions> CustomFieldGetSelectedOptions(CustomField customField)
        {
            List<CustomFieldOptions> SelectedOptions = new List<CustomFieldOptions>();
            foreach (CustomFieldOptions cfo in customField.CustomFieldOptionsCollection)
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
        public List<CustomFieldOptions> CustomFieldGetSelectedOptions(string customFieldName)
        {
            List<CustomFieldOptions> SelectedOptions = new List<CustomFieldOptions>();
            foreach (CustomField cf in CustomFields)
            {
                if (cf.Name == customFieldName)
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
        /// <param name="customFieldId"></param>
        /// <returns></returns>
        public string CustomFieldGetDisplayName(Int64 customFieldId)
        {
            foreach (CustomField cf in CustomFields)
            {
                if (cf.Id == customFieldId)
                {
                    return cf.Name;
                }
            }

            return String.Empty;
        }

        /// <summary>
        ///  Return the id for the custom field name you specified.  Will return -1 if the custom field is not found 
        ///  or duplicates are found.  Search will be case-insensitive.
        /// </summary>
        /// <param name="customFieldName"></param>
        /// <returns></returns>
        public Int64 CustomFieldGetId(string customFieldName)
        {
            Int64 customFieldId = -1;

            if (CustomFields != null)
            {
                foreach(CustomField cf in CustomFields)
                {
                    if (cf.Name.ToLower() == customFieldName.ToLower())
                    {
                        if (customFieldId != -1)
                        {
                            // field name was found more than one time, return -1
                            return -1;
                        }
                        else
                        {
                            customFieldId = cf.Id;
                        }
                    }
                }
            }

            return customFieldId;
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
                    if (cfn.Id == customFieldId)
                    {
                        cf = cfn;
                        found = true;
                        break;
                    }
                }

                // Not found in the list of CFs, need to add this field.
                if (found == false)
                {
                    cf.Id = customFieldId;
                    modified = true;
                }

                if (String.Compare(cf.GetFieldValue<string>(), customFieldValue, ignoreCase) != 0)
                {
                    modified = true;
                    if (String.IsNullOrEmpty(customFieldValue))
                    {
                        cf.FlagToDelete = true;
                    }
                    cf.Value = customFieldValue;
                }

                if (found == false)
                {
                    Fields.Add(cf);
                }

            }

            return modified;
        }

        internal bool CustomFieldSetFieldOption(Int64 customFieldid, string customFieldOptionName, IEnumerable<CustomField> customFields, bool resetOtherOptions, bool ignoreCase)
        {


            if (customFieldid > 0 && String.IsNullOrEmpty(customFieldOptionName) == false)
            {
                foreach (CustomField cfn in customFields)
                {
                    if (cfn.Id == customFieldid)
                    {
                        foreach (CustomFieldOptions option in cfn.CustomFieldOptionsCollection)
                        {
                            if (String.Compare(option.CustomFieldOptionName, customFieldOptionName, ignoreCase) == 0)
                            {
                                return CustomFieldSetFieldOption(customFieldid, option.CustomFieldOptionID, customFields, resetOtherOptions);
                            }
                        }

                        break;
                    }

                }
            }
            return false;
        }

        internal bool CustomFieldSetFieldOption(Int64 customFieldid, Int64 customFieldOptionId, IEnumerable<CustomField> customFields, bool resetOtherOptions)
        {
            bool modified = false;

            if (customFieldid > 0 && customFieldOptionId > 0)
            {
                bool found = false;
                foreach (var cfn in customFields)
                {
                    if (cfn.Id == customFieldid)
                    {
                        found = true;
                        bool optionFound = false;
                        foreach (CustomFieldOptions option in cfn.CustomFieldOptionsCollection)
                        {
                            if (option.CustomFieldOptionID == customFieldOptionId)
                            {
                                if (option.IsSelected == false)
                                {
                                    modified = true;
                                    option.IsSelected = true;
                                }

                                optionFound = true;
                                if (resetOtherOptions == false)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                if (resetOtherOptions == true)
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
                            NewOption.CustomFieldOptionID = customFieldOptionId;
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
                    cf.Id = customFieldid;
                    CustomFieldOptions NewOption = new CustomFieldOptions();
                    NewOption.CustomFieldOptionID = customFieldOptionId;
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