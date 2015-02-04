using System;

namespace ParatureSDK.Fields
{
    /// <summary>
    /// Holds the list of dependant field IDs, as well as the field options contained within it.
    /// </summary>
    public class DependantCustomFields
    {
        public Int64 DependantFieldID = 0;
        public Int64[] DependantFieldOptions;
        public string DependantFieldPath;

        public DependantCustomFields()
        {
        }

        public DependantCustomFields(DependantCustomFields dependantCustomFields)
        {
            this.DependantFieldID = dependantCustomFields.DependantFieldID;
            this.DependantFieldOptions = dependantCustomFields.DependantFieldOptions;
            this.DependantFieldPath = dependantCustomFields.DependantFieldPath;
        }

    }
}