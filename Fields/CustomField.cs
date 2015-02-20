using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ParatureSDK.Fields
{
    /// <summary>
    /// A custom field class is specific to each module.
    /// </summary>
    [XmlRoot("Custom_Field")]
    public class CustomField : Field
    {
        /// <summary>
        /// The internal ID of the field
        /// </summary>
        [XmlAttribute(AttributeName = "id")]
        public Int64 Id = 0;

        [XmlAttribute("multi-value")]
        public bool MultiValue;
        [XmlIgnore]
        public bool FlagToDelete = false;

        /// <summary>
        /// If this is a custom field that holds multiple options, this collection of CustomFieldOptions will be populated.
        /// </summary>
        [XmlElement("Option")]
        public List<CustomFieldOptions> CustomFieldOptionsCollection = new List<CustomFieldOptions>();

        /// <summary>
        /// Value of the Custom field. Can be integer, string, text, or date/time values
        /// </summary>
        [XmlText()]
        public string Value
        {
            get { return (string)base.Value; }
            set { base.Value = value; }
        }

        public CustomField()
        {
        }

        public CustomField(CustomField customField)
        {
            Id = customField.Id;
            Name = customField.Name;
            Required = customField.Required;
            Editable = customField.Editable;
            FieldDataType = customField.FieldDataType;
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
