using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParatureSDK.Fields;

namespace ParatureSDK.ParaObjects
{
    public class PortalAlias : ParaEntity
    {
        public string Name
        {
            get
            {
                return GetFieldValue<string>("Name");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Name");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Name",
                        FieldType = "text",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public override string GetReadableName()
        {
            return Name;
        }
    }
}
