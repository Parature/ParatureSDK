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
        [XmlIgnore]
        public object Value
        {
            get
            {
                if (StringValue != null && StringValue.Length > 0)
                {
                    var txtVal = StringValue[0];
                    switch (DataType)
                    {
                        case "boolean":
                            return Convert.ToBoolean(txtVal);
                            break;
                        case "int":
                            return Convert.ToInt64(txtVal);
                            break;
                        case "date":
                            return DateTime.Parse(txtVal);
                            break;
                        case "string":
                        default:
                            return txtVal;
                            break;
                    }
                }

                return null;
            }
            set { StringValue = new[] { value.ToString() }; }
        }

        /// <summary>
        /// This is the stringified text contents of the custom field
        /// It is primarily used for the XmlSerialization and deserialization. 
        /// Use the Value property instead
        /// </summary>
        [XmlText()]
        public string[] StringValue;

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
