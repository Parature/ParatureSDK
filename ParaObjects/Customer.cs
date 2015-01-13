using System;
using System.Linq;
using ParatureAPI.Fields;

namespace ParatureAPI.ParaObjects
{
    /// <summary>
    /// Holds all the properties of the Customer module.
    /// </summary>
    public class Customer : ParaEntity
    {
        public Account Account = new Account();
        public Sla Sla = new Sla();
        public DateTime Date_Visited
        {
            get
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Date_Visited");
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
                var field = Fields.FirstOrDefault(f => f.Name == "Date_Visited");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Date_Visited",
                        DataType = ParaEnums.FieldDataType.DateTime
                    };
                    Fields.Add(field);
                }

                field.Value = value.ToString();
            }
        }
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
        public DateTime Date_Updated
        {
            get
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Date_Updated");
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
                var field = Fields.FirstOrDefault(f => f.Name == "Date_Updated");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Date_Updated",
                        DataType = ParaEnums.FieldDataType.DateTime
                    };
                    Fields.Add(field);
                }

                field.Value = value.ToString();
            }
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
                        DataType = ParaEnums.FieldDataType.Email
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }
        public Role Customer_Role = new Role();

        /// <summary>
        /// Only used when you use "Username" as the identifier of your account.
        /// </summary>
        public string User_Name
        {
            get
            {
                var field = Fields.FirstOrDefault(f => f.Name == "User_Name");
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
                var field = Fields.FirstOrDefault(f => f.Name == "User_Name");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "User_Name",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }
        public string First_Name
        {
            get
            {
                var field = Fields.FirstOrDefault(f => f.Name == "First_Name");
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
                var field = Fields.FirstOrDefault(f => f.Name == "First_Name");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "First_Name",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }
        public string Last_Name
        {
            get
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Last_Name");
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
                var field = Fields.FirstOrDefault(f => f.Name == "Last_Name");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Last_Name",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
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
        public bool Tou
        {
            get
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Tou");
                var val = false;
                try
                {
                    val = Convert.ToBoolean(field.Value);
                }
                catch (Exception e) { }

                return val;
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Tou");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Tou",
                        DataType = ParaEnums.FieldDataType.boolean
                    };
                    Fields.Add(field);
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
                        Name = "Last_Name",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
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
                var field = Fields.FirstOrDefault(f => f.Name == "Password_Confirm");
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
                var field = Fields.FirstOrDefault(f => f.Name == "Password_Confirm");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Password_Confirm",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }


        public CustomerStatus Status = new CustomerStatus();

        public Customer()
            : base()
        {
        }

        public Customer(Customer customer)
            : base(customer)
        {
            Id = customer.Id;
            Account = new Account(customer.Account);
            Sla = new Sla(customer.Sla);
            Date_Visited = customer.Date_Visited;
            Email = customer.Email;
            User_Name = customer.User_Name;
            First_Name = customer.First_Name;
            Tou = customer.Tou;
            Password = customer.Password;
            Password_Confirm = customer.Password_Confirm;
            Status = new CustomerStatus(customer.Status);
            Customer_Role = new Role(customer.Customer_Role);
        }



        public override string GetReadableName()
        {
            return First_Name + " " + Last_Name + "(" + Email + ")";
        }
    }
}