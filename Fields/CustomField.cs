using System;
using System.Collections.Generic;

namespace ParatureSDK.Fields
{
    /// <summary>
    /// A custom field class is specific to each module.
    /// </summary>
    public class CustomField : Field
    {
        /// <summary>
        /// The internal ID of the field
        /// </summary>
        public Int64 Id = 0;

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
            Id = customField.Id;
            Name = customField.Name;
            Required = customField.Required;
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
