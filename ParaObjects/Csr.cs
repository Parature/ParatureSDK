using System;
using System.Collections.Generic;
using System.Linq;
using ParatureSDK.Fields;

namespace ParatureSDK.ParaObjects
{
    public class Csr : ParaEntity
    {
        public string Full_Name
        {
            get
            {
                return GetFieldValue<string>("Full_Name");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Full_Name");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Full_Name",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
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
                var field = Fields.FirstOrDefault(f => f.Name == "Email");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Email",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }
        public string Fax
        {
            get
            {
                return GetFieldValue<string>("Fax");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Fax");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Fax",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }
        public string Phone_1
        {
            get
            {
                return GetFieldValue<string>("Phone_1");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Phone_1");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Phone_1",
                        DataType = ParaEnums.FieldDataType.InternationalPhone
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }
        public string Phone_2
        {
            get
            {
                return GetFieldValue<string>("Phone_2");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Phone_2");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Phone_2",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }
        public string Screen_Name
        {
            get
            {
                return GetFieldValue<string>("Screen_Name");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Screen_Name");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Screen_Name",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }
        /// <summary>
        /// The following strings are the valid options for Date_Format:
        /// mm/dd/yyyy | mm/dd/yy | dd/mm/yyyy | dd/mm/yy | month dd, yyyy | month dd, yy
        /// </summary>
        public string Date_Format
        {
            get
            {
                return GetFieldValue<string>("Date_Format");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Date_Format");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Date_Format",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }

        public string Password
        {
            get
            {
                return GetFieldValue<string>("Password");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Password");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Password",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }
        public Timezone Timezone
        {
            get
            {
                return GetFieldValue<Timezone>("Timezone");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Timezone");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Timezone",
                        DataType = ParaEnums.FieldDataType.Timezone
                    };
                    Fields.Add(field);
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
                var field = Fields.FirstOrDefault(f => f.Name == "Status");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Status",
                        DataType = ParaEnums.FieldDataType.Status
                    };
                    Fields.Add(field);
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
        public List<Role> Role
        {
            get
            {
                return GetFieldValue<List<Role>>("Role");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Role");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Role",
                        DataType = ParaEnums.FieldDataType.Role
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }

        public Csr()
        {
        }

        public Csr(Csr csr)
        {
            Id = csr.Id;
            Full_Name = csr.Full_Name;
            Email = csr.Email;
            Fax = csr.Fax;
            Phone_1 = csr.Phone_1;
            Phone_2 = csr.Phone_2;
            Screen_Name = csr.Screen_Name;
            Date_Format = csr.Date_Format;
            Status = new Status(csr.Status);
            Timezone = new Timezone(csr.Timezone);
            Date_Created = csr.Date_Created;
            Role = new List<Role>(csr.Role);
            Password = csr.Password;
        }

        public override string GetReadableName()
        {
            return Full_Name;
        }
    }
}