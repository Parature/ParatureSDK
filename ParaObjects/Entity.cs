using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParatureAPI.Fields;

namespace ParatureAPI.ParaObjects
{
    class Entity : ModuleWithCustomFields
    {
        public List<Field> Fields;

        /// <summary>
        /// Convenience method to retrieve only static fields
        /// </summary>
        public List<StaticField> StaticFields
        {
            get { return Fields.Where(f => f is StaticField) as List<StaticField>; }
            set
            {
                Fields.RemoveAll(f => f is StaticField);
                Fields.AddRange(value);
            }
        }

        /// <summary>
        /// Convenience method to retrieve only custom fields
        /// </summary>
        public List<CustomField> CustomFields
        {
            get { return Fields.Where(f => f is CustomField) as List<CustomField>; }
            set
            {
                Fields.RemoveAll(f => f is CustomField);
                Fields.AddRange(value);
            }
        }

        public override string GetReadableName()
        {
            throw new NotImplementedException();
        }
    }
}
