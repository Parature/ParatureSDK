using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace ParatureSDK.Fields
{
    /// <summary>
    /// A field option is actually one of the possible values a custom field can take. So this can be an option in a dropdown, or a checkbox in a CheckBoxList field.
    /// </summary>
    public class FieldOption
    {
        [XmlAttribute("id")]
        public Int64 Id = 0;
        public string Value;
        [XmlAttribute("viewOrder")]
        public Int64 ViewOrder;
        public bool? Dependent = false;
        [XmlAttribute("selected")]
        public bool Selected = false;
        /// <summary>
        /// With regards to Field Dependencies:
        /// Pseudo-URI for the field that gets displayed by enabling this field. 
        /// </summary>
        [XmlElement("Enables")]
        public List<string> Enables = new List<string>();

        public FieldOption()
        {
        }

        public FieldOption(FieldOption customFieldOptions)
        {
            Id = customFieldOptions.Id;
            Value = customFieldOptions.Value;
            Selected = customFieldOptions.Selected;
            Dependent = customFieldOptions.Dependent;
            Enables = customFieldOptions.Enables;
        }
    }
}