using System;
using System.Collections.Generic;
using System.Linq;
using ParatureAPI.Fields;

namespace ParatureAPI.ParaObjects
{
    /// <summary>
    /// Holds all the properties of the Chat module.
    /// </summary>
    public class Chat : ParaEntity
    {
        public Int64 Chat_Number
        {
            get
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Chat_Number");
                var val = default(Int64);
                try
                {
                    val = Convert.ToInt64(field.Value);
                }
                catch (Exception e) { }

                return val;
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Chat_Number");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Chat_Number",
                        DataType = ParaEnums.FieldDataType.Int
                    };
                    Fields.Add(field);
                }

                field.Value = value.ToString();
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
                var field = Fields.FirstOrDefault(f => f.Name == "Browser_Language");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Browser_Language",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
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
                var field = Fields.FirstOrDefault(f => f.Name == "Browser_Type");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Browser_Type",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
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
                var field = Fields.FirstOrDefault(f => f.Name == "Browser_Version");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Browser_Version",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }
        public Customer Customer = new Customer();
        public DateTime Date_Created
        {
            get
            {
                return GetFieldValue<DateTime>("Date_Created");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Date_Created");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Date_Created",
                        DataType = ParaEnums.FieldDataType.DateTime
                    };
                    Fields.Add(field);
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
                var field = Fields.FirstOrDefault(f => f.Name == "Date_Ended");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Date_Ended",
                        DataType = ParaEnums.FieldDataType.DateTime
                    };
                    Fields.Add(field);
                }

                field.Value = value.ToString();
            }
        }
        public List<Ticket> Related_Tickets = new List<Ticket>();
        public string Email
        {
            get
            {
                return GetFieldValue<string>("Email");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Email");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Email",
                        DataType = ParaEnums.FieldDataType.Email
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }
        public Csr Initial_Csr = new Csr();
        public Role Customer_Role = new Role();
        public string Ip_Address
        {
            get
            {
                return GetFieldValue<string>("Ip_Address");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Ip_Address");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Ip_Address",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
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
                var field = Fields.FirstOrDefault(f => f.Name == "Is_Anonymous");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Is_Anonymous",
                        DataType = ParaEnums.FieldDataType.Boolean
                    };
                    Fields.Add(field);
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
                var field = Fields.FirstOrDefault(f => f.Name == "Referrer_Url");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Referrer_Url",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }
        public Status Status = new Status();
        public string Summary
        {
            get
            {
                return GetFieldValue<string>("Summary");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Summary");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Summary",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
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
                var field = Fields.FirstOrDefault(f => f.Name == "User_Agent");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "User_Agent",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }
        public Int32 Sla_Violation
        {
            get
            {
                return GetFieldValue<Int32>("Sla_Violation");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Sla_Violation");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Sla_Violation",
                        DataType = ParaEnums.FieldDataType.Int
                    };
                    Fields.Add(field);
                }

                field.Value = value.ToString();
            }
        }
        public List<ChatTranscript> ChatTranscripts = new List<ChatTranscript>();

        public Chat()
            : base()
        {
        }

        public override string GetReadableName()
        {
            return "Chat #" + uniqueIdentifier.ToString();
        }
    }
}