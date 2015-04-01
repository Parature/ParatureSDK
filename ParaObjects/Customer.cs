using System;
using System.Linq;
using System.Xml.Serialization;
using ParatureSDK.Fields;
using ParatureSDK.ParaObjects.EntityReferences;

namespace ParatureSDK.ParaObjects
{
    /// <summary>
    /// Holds all the properties of the Customer module.
    /// </summary>
    public class Customer : ParaEntity
    {
        public AccountReference Account
        {
            get
            {
                return GetFieldValue<AccountReference>("Account");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Account");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Account",
                        FieldType = "entity",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public SlaReference Sla
        {
            get
            {
                return GetFieldValue<SlaReference>("Sla");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Sla");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Sla",
                        FieldType = "dropdown",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public DateTime? Date_Visited
        {
            get
            {
                return GetFieldValue<DateTime?>("Date_Visited");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Date_Visited");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Date_Visited",
                        FieldType = "usdate",
                        DataType = "date"
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

                field.Value = value;
            }
        }
        public DateTime Date_Updated
        {
            get
            {
                return GetFieldValue<DateTime>("Date_Updated");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Date_Updated");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Date_Updated",
                        FieldType = "usdate",
                        DataType = "date"
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
        public CustomerRoleReference Customer_Role
        {
            get
            {
                return GetFieldValue<CustomerRoleReference>("Customer_Role");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Customer_Role");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Customer_Role",
                        FieldType = "entity",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        /// <summary>
        /// Only used when you use "Username" as the identifier of your account.
        /// </summary>
        public string User_Name
        {
            get
            {
                return GetFieldValue<string>("User_Name");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "User_Name");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "User_Name",
                        FieldType = "text",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public string First_Name
        {
            get
            {
                return GetFieldValue<string>("First_Name");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "First_Name");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "First_Name",
                        FieldType = "text",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public string Last_Name
        {
            get
            {
                return GetFieldValue<string>("Last_Name");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Last_Name");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Last_Name",
                        FieldType = "text",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public string Full_Name
        {
            get
            {
                return GetFieldValue<string>("Full_Name");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Full_Name");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Full_Name",
                        FieldType = "text",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }   
        }

        ////////////////////////////////////////// COULD NOT LOCATE IT FOR NOW //////////////
        /// <summary>
        /// User accepted terms of use.
        /// Certain configs have the terms of use feature activated. This property should be taken into account
        /// only when you are using the "customer terms of use" feature.
        /// </summary>
        public bool? Tou
        {
            get
            {
                return GetFieldValue<bool?>("Tou");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Tou");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Tou",
                        FieldType = "checkbox",
                        DataType = "boolean"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value.ToString();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Password is only used when creating a new customer. This property is not filled when retrieving the customer details. It must be empty when updating a customer object.
        /// </summary>
        public string Password
        {
            get
            {
                return GetFieldValue<string>("Password");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Password");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Last_Name",
                        FieldType = "password",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// Password confirm is only used when creating a new customer. This property is not filled when retrieving the customer details. It must be empty when updating a customer object.
        /// </summary>
        public string Password_Confirm
        {
            get
            {
                return GetFieldValue<string>("Password_Confirm");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Password_Confirm");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Password_Confirm",
                        FieldType = "password",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }


        public StatusReference Status
        {
            get
            {
                return GetFieldValue<StatusReference>("Status");
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

        public Customer()
            : base()
        {
        }

        public Customer(Customer customer)
            : base(customer)
        {
            Id = customer.Id;
            Account = customer.Account;
            Sla = customer.Sla;
            Date_Visited = customer.Date_Visited;
            Email = customer.Email;
            User_Name = customer.User_Name;
            First_Name = customer.First_Name;
            Tou = customer.Tou;
            Password = customer.Password;
            Password_Confirm = customer.Password_Confirm;
            Status = customer.Status;
            Customer_Role = customer.Customer_Role;
        }

        public override string GetReadableName()
        {
            return First_Name + " " + Last_Name + "(" + Email + ")";
        }
    }
}