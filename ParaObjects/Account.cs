using System;
using System.Collections.Generic;
using System.Linq;
using ParatureAPI.Fields;

namespace ParatureAPI.ParaObjects
{
    /// <summary>
    /// Holds all the properties of the Account module.
    /// </summary>
    public class Account : ParaEntity
    {
        public string Account_Name
        {
            get
            {
                return GetFieldValue<string>("Account_Name");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Account_Name");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Account_Name",
                        DataType = ParaEnums.FieldDataType.String
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }

        public Csr Modified_By
        {
            get
            {
                return GetFieldValue<Csr>("Modified_By"); 
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Modified_By");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Modified_By",
                        DataType = ParaEnums.FieldDataType.EntityReference
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }
        public Csr Owned_By
        {
            get
            {
                return GetFieldValue<Csr>("Owned_By");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Owned_By");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Owned_By",
                        DataType = ParaEnums.FieldDataType.EntityReference
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }
        public Sla Sla
        {
            get
            {
                return GetFieldValue<Sla>("Sla");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Sla");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Sla",
                        DataType = ParaEnums.FieldDataType.Sla
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
        public DateTime Date_Updated
        {
            get
            {
                return GetFieldValue<DateTime>("Date_Updated");
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
        public Role Default_Customer_Role
        {
            get
            {
                return GetFieldValue<Role>("Role");
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
        /// <summary>
        /// The list of all the other Viewable accounts, only available to certain configs.
        /// </summary>
        public List<Account> Viewable_Account
        {
            get
            {
                return GetFieldValue<List<Account>>("Viewable_Account");
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "Viewable_Account");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Viewable_Account",
                        DataType = ParaEnums.FieldDataType.EntityReference
                    };
                    Fields.Add(field);
                }

                field.Value = value;
            }
        }

        public Account()
        {
        }

        public Account(Account account)
            : base(account)
        {
            Id = account.Id;
            Account_Name = account.Account_Name;
            Modified_By = new Csr(account.Modified_By);
            Owned_By = new Csr(account.Owned_By);
            Sla = new Sla(account.Sla);
            Viewable_Account = new List<Account>(account.Viewable_Account);
            Default_Customer_Role = new Role(account.Default_Customer_Role);
        }

        public override string GetReadableName()
        {
            return Account_Name;
        }
    }
}