using System;
using System.Collections.Generic;

namespace ParatureAPI.Fields
{
    /// <summary>
    /// A custom field class is specific to each module.
    /// </summary>
    public class CustomField
    {
        /// <summary>
        /// The internal ID of the field
        /// </summary>
        public Int64 CustomFieldID = 0;

        /// <summary>
        /// The public name of the field
        /// </summary>
        public string CustomFieldName = "";

        public bool CustomFieldRequired;

        public bool dependent = false;

        public Int32 MaxLength=0;

        /// <summary>
        /// This Value will be populated with the field's value. For example, if this is a textbox field, this will hold the textbox's default field.
        /// </summary>
        public string CustomFieldValue = "";

        /// <summary>
        /// this indicates whether the custom field is editable or read only. If it is a read only, inluding it in an update will not result in that field value being updated.
        /// </summary>
        public bool Editable;

        public ParaEnums.CustomFieldDataType DataType;

        public bool MultiValue;

        public bool FlagToDelete = false;

        /// <summary>
        /// If this is a custom field that holds multiple options, this collection of CustomFieldOptions will be populated.
        /// </summary>
        public List<CustomFieldOptions> CustomFieldOptionsCollection = new List<CustomFieldOptions>();

        public CustomField()
        {
        }

        public CustomField(CustomField customField)
        {
            this.CustomFieldID = customField.CustomFieldID;
            this.CustomFieldName = customField.CustomFieldName;
            this.CustomFieldRequired = customField.CustomFieldRequired;
            this.Editable = customField.Editable;
            this.DataType = customField.DataType;
            this.MultiValue = customField.MultiValue;
            this.MaxLength = customField.MaxLength;
            //this.CustomFieldOptionsCollection = new List<CustomFieldOptions>(customField.CustomFieldOptionsCollection);

            if (customField != null && customField.CustomFieldOptionsCollection != null)
            {
                this.CustomFieldOptionsCollection = new List<CustomFieldOptions>();

                foreach (CustomFieldOptions cfo in customField.CustomFieldOptionsCollection)
                {
                    this.CustomFieldOptionsCollection.Add(new CustomFieldOptions(cfo));
                }
            }

            this.FlagToDelete = customField.FlagToDelete;
        }

    }
}
