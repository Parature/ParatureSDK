using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<FieldOption> Options = new List<FieldOption>();

        public bool SetSelectedOption(Int64 optId, bool resetOtherOptions)
        {
            var modified = false;
            var optionFound = false;
            foreach (var option in this.Options)
            {
                if (option.Id == optId)
                {
                    if (option.Selected == false)
                    {
                        modified = true;
                        option.Selected = true;
                    }

                    optionFound = true;
                    if (resetOtherOptions == false)
                    {
                        return modified;
                    }
                }
                else
                {
                    if ((resetOtherOptions || MultiValue)
                        && option.Selected)
                    {
                        modified = true;
                        option.Selected = false;
                    }
                }
            }
            if (optionFound == false)
            {
                var newOption = new FieldOption
                {
                    Id = optId,
                    Selected = true
                };
                this.Options.Add(newOption);
                modified = true;
            }

            return modified;
        }

        public IEnumerable<FieldOption> GetSelectedOptions()
        {
            return Options.Where(opt => opt.Selected);
        }

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
            MultiValue = customField.MultiValue;
            MaxLength = customField.MaxLength;

            if (customField.Options != null)
            {
                Options = new List<FieldOption>();

                foreach (var cfo in customField.Options)
                {
                    Options.Add(new FieldOption(cfo));
                }
            }

            FlagToDelete = customField.FlagToDelete;
        }

    }
}
