using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ParatureSDK.Fields
{
    /// <summary>
    /// A custom field option is actually one of the possible values a custom field can take. So this can be an option in a dropdown, or a checkbox in a CheckBoxList field.
    /// </summary>
    public class CustomFieldOptions
    {
        [XmlAttribute("id")]
        public Int64 Id = 0;
        public string Value;
        [XmlAttribute("viewOrder")]
        public Int64 ViewOrder;
        public bool Dependent = false;
        [XmlAttribute("selected")]
        public bool Selected = false;

        /// <summary>
        /// If the custom field option has dependent fields, or dependant field options, they will be listed under the DependantCustomFields collection.
        /// </summary>
        public List<DependantCustomFields> DependantCustomFields = new List<DependantCustomFields>();

        public CustomFieldOptions()
        {
        }

        public CustomFieldOptions(CustomFieldOptions customFieldOptions)
        {
            Id = customFieldOptions.Id;
            Value = customFieldOptions.Value;
            Selected = customFieldOptions.Selected;
            Dependent = customFieldOptions.Dependent;
            DependantCustomFields = new List<DependantCustomFields>(customFieldOptions.DependantCustomFields);
        }
    }
}