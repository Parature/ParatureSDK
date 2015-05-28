using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using ParatureSDK.Fields;
using ParatureSDK.ParaObjects.EntityReferences;

namespace ParatureSDK.ParaObjects
{
    /// <summary>
    /// Holds all the properties of the Chat module.
    /// </summary>
    public class Chat : ParaEntity
    {
        public string Chat_Number
        {
            get
            {
                return GetFieldValue<string>("Chat_Number");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Chat_Number");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Chat_Number",
                        FieldType = "text",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public string Browser_Language
        {
            get
            {
                return GetFieldValue<string>("Browser_Language");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Browser_Language");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Browser_Language",
                        FieldType = "text",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public string Browser_Type
        {
            get
            {
                return GetFieldValue<string>("Browser_Type");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Browser_Type");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Browser_Type",
                        FieldType = "text",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public string Browser_Version
        {
            get
            {
                return GetFieldValue<string>("Browser_Version");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Browser_Version");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Browser_Version",
                        FieldType = "text",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public CustomerReference Customer
        {
            get
            {
                return GetFieldValue<CustomerReference>("Customer");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Customer");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Customer",
                        FieldType = "entity",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public DateTime Date_Created
        {
            get
            {
                return GetFieldValue<DateTime>("Date_Created");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Date_Created");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Date_Created",
                        FieldType = "usdate",
                        DataType = "date"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value.ToString();
            }
        }
        public DateTime Date_Ended
        {
            get
            {
                return GetFieldValue<DateTime>("Date_Ended");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Date_Ended");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Date_Ended",
                        FieldType = "usdate",
                        DataType = "date"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value.ToString();
            }
        }
        public List<Ticket> Related_Tickets
        {
            get
            {
                return GetFieldValue<List<Ticket>>("Related_Tickets");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Related_Tickets");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Related_Tickets",
                        FieldType = "entity",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public string Email
        {
            get
            {
                return GetFieldValue<string>("Email");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Email");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Email",
                        FieldType = "email",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public CsrReference Initial_Csr
        {
            get
            {
                return GetFieldValue<CsrReference>("Initial_Csr");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Initial_Csr");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Initial_Csr",
                        FieldType = "entity",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public string Ip_Address
        {
            get
            {
                return GetFieldValue<string>("Ip_Address");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Ip_Address");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Ip_Address",
                        FieldType = "text",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public Boolean Is_Anonymous
        {
            get
            {
                return GetFieldValue<bool>("Is_Anonymous");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Is_Anonymous");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Is_Anonymous",
                        FieldType = "checkbox",
                        DataType = "boolean"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value.ToString();
            }
        }
        public string Referrer_Url
        {
            get
            {
                return GetFieldValue<string>("Referrer_Url");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Referrer_Url");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Referrer_Url",
                        FieldType = "text",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public Status Status
        {
            get
            {
                return GetFieldValue<Status>("Status");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Status");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Status",
                        FieldType = "entity",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public string Summary
        {
            get
            {
                return GetFieldValue<string>("Summary");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Summary");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Summary",
                        FieldType = "text",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public string User_Agent
        {
            get
            {
                return GetFieldValue<string>("User_Agent");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "User_Agent");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "User_Agent",
                        FieldType = "text",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public Int64 Sla_Violation
        {
            get
            {
                return GetFieldValue<Int32>("Sla_Violation");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Sla_Violation");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Sla_Violation",
                        FieldType = "int",
                        DataType = "int"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value.ToString();
            }
        }
        [XmlElement("Message")]
        public List<ChatMessage> Transcript { get; set; }
        
        public Chat()
            : base()
        {
        }

        public override string GetReadableName()
        {
            return "Chat #" + Id;
        }
    }
}