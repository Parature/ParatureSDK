using System;
using System.Collections.Generic;

namespace ParatureSDK.Fields
{
    /// <summary>
    /// A custom field option is actually one of the possible values a custom field can take. So this can be an option in a dropdown, or a checkbox in a CheckBoxList field.
    /// </summary>
    public class CustomFieldOptions
    {
        public Int64 CustomFieldOptionID = 0;
        public string CustomFieldOptionName = "";

        public Int64 OptionID
        {
            get { return CustomFieldOptionID; }
            set { CustomFieldOptionID = value; }
        }

        public string OptionName
        {
            get { return CustomFieldOptionName; }
            set { CustomFieldOptionName = value; }
        }

        public bool Dependent = false;
        public bool IsSelected = false;

        /// <summary>
        /// If the custom field option has dependent fields, or dependant field options, they will be listed under the DependantCustomFields collection.
        /// </summary>
        public List<DependantCustomFields> DependantCustomFields = new List<DependantCustomFields>();

        public CustomFieldOptions()
        {
        }

        public CustomFieldOptions(CustomFieldOptions customFieldOptions)
        {
            CustomFieldOptionID = customFieldOptions.CustomFieldOptionID;
            CustomFieldOptionName = customFieldOptions.CustomFieldOptionName;
            IsSelected = customFieldOptions.IsSelected;
            Dependent = customFieldOptions.Dependent;
            DependantCustomFields = new List<DependantCustomFields>(customFieldOptions.DependantCustomFields);
        }
    }
}