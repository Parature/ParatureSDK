using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
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

        [XmlElement("Resource_Set")]
        public string ResourceSet
        {
            get
            {
                return GetFieldValue<string>("Resource_Set");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Resource_Set");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Resource_Set",
                        FieldType = "text",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }


        [XmlElement("Style_From_Portal_Alias_Id")]
        public int StyleId
        {
            get
            {
                return GetFieldValue<int>("Style_From_Portal_Alias_Id");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Style_From_Portal_Alias_Id ");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Style_From_Portal_Alias_Id",
                        FieldType = "int",
                        DataType = "int"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        [XmlElement("Product_Id")]
        public int ProductId
        {
            get
            {
                return GetFieldValue<int>("Product_Id");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Product_Id ");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Product_Id",
                        FieldType = "int",
                        DataType = "int"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }


        [XmlElement("Deactivated")]
        public DateTime DeactivatedDate
        {
            get
            {
                return GetFieldValue<DateTime>("Deactivated");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Deactivated");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Deactivated",
                        FieldType = "usdate",
                        DataType = "date"
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
