using System;
using System.Collections.Generic;
using System.Linq;
using ParatureAPI.Fields;

namespace ParatureAPI.ParaObjects
{
    public class Csr : ParaEntity
    {
        private string _Full_Name = "";

        public string Full_Name
        {
            get { return _Full_Name; }
            set { _Full_Name = value; }
        }
        public string Email
        {
            get
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Email");
                var val = string.Empty;
                try
                {
                    val = field.Value;
                }
                catch (Exception e) { }

                return val;
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
                var field = Fields.FirstOrDefault(f => f.Name == "Fax");
                var val = string.Empty;
                try
                {
                    val = field.Value;
                }
                catch (Exception e) { }

                return val;
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
                var field = Fields.FirstOrDefault(f => f.Name == "Phone_1");
                var val = string.Empty;
                try
                {
                    val = field.Value;
                }
                catch (Exception e) { }

                return val;
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
                var field = Fields.FirstOrDefault(f => f.Name == "Phone_2");
                var val = string.Empty;
                try
                {
                    val = field.Value;
                }
                catch (Exception e) { }

                return val;
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
                var field = Fields.FirstOrDefault(f => f.Name == "Screen_Name");
                var val = string.Empty;
                try
                {
                    val = field.Value;
                }
                catch (Exception e) { }

                return val;
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
                var field = Fields.FirstOrDefault(f => f.Name == "Date_Format");
                var val = string.Empty;
                try
                {
                    val = field.Value;
                }
                catch (Exception e) { }

                return val;
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
                var field = Fields.FirstOrDefault(f => f.Name == "Password");
                var val = string.Empty;
                try
                {
                    val = field.Value;
                }
                catch (Exception e) { }

                return val;
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
        public Timezone Timezone = new Timezone();
        public Status Status = new Status();
        public DateTime Date_Created
        {
            get
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Date_Created");
                var val = DateTime.MinValue;
                try
                {
                    return DateTime.Parse(field.Value);
                }
                catch (Exception e) { }

                return val;
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
        public List<Role> Role = new List<Role>();

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