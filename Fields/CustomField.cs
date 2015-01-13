using System;
using System.Collections.Generic;

namespace ParatureAPI.Fields
{
    /// <summary>
    /// A custom field class is specific to each module.
    /// </summary>
    public class CustomField : Field
    {
        /// <summary>
        /// The internal ID of the field
        /// </summary>
        public Int64 CustomFieldID = 0;

        /// <summary>
        /// This Value will be populated with the field's value. For example, if this is a textbox field, this will hold the textbox's default field.
        /// </summary>
        public string CustomFieldValue = "";

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
            CustomFieldID = customField.CustomFieldID;
            CustomFieldName = customField.CustomFieldName;
            CustomFieldRequired = customField.CustomFieldRequired;
            Editable = customField.Editable;
            DataType = customField.DataType;
            MultiValue = customField.MultiValue;
            MaxLength = customField.MaxLength;

            if (customField.CustomFieldOptionsCollection != null)
            {
                CustomFieldOptionsCollection = new List<CustomFieldOptions>();

                foreach (var cfo in customField.CustomFieldOptionsCollection)
                {
                    CustomFieldOptionsCollection.Add(new CustomFieldOptions(cfo));
                }
            }

            FlagToDelete = customField.FlagToDelete;
        }

    }
}
