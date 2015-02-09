using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using ParatureSDK.Fields;
using ParatureSDK.ParaObjects.EntityReferences;

namespace ParatureSDK.ParaObjects
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
                var field = StaticFields.FirstOrDefault(f => f.Name == "Account_Name");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Account_Name",
                        FieldDataType = ParaEnums.FieldDataType.String
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public CsrReference Modified_By
        {
            get
            {
                return GetFieldValue<CsrReference>("Modified_By"); 
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Modified_By");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Modified_By",
                        FieldDataType = ParaEnums.FieldDataType.EntityReference
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public CsrReference Owned_By
        {
            get
            {
                return GetFieldValue<CsrReference>("Owned_By");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Owned_By");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Owned_By",
                        FieldDataType = ParaEnums.FieldDataType.EntityReference
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
                        FieldDataType = ParaEnums.FieldDataType.Sla
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
                        FieldDataType = ParaEnums.FieldDataType.DateTime
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
                        FieldDataType = ParaEnums.FieldDataType.DateTime
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public CustomerRoleReference Default_Customer_Role
        {
            get
            {
                return GetFieldValue<CustomerRoleReference>("Default_Customer_Role");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Default_Customer_Role");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Default_Customer_Role",
                        FieldDataType = ParaEnums.FieldDataType.Role
                    };
                    StaticFields.Add(field);
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
                var field = StaticFields.FirstOrDefault(f => f.Name == "Viewable_Account");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Viewable_Account",
                        FieldDataType = ParaEnums.FieldDataType.EntityReference
                    };
                    StaticFields.Add(field);
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
            Modified_By = account.Modified_By;
            Owned_By = account.Owned_By;
            Sla = account.Sla;
            Viewable_Account = new List<Account>(account.Viewable_Account);
            Default_Customer_Role = account.Default_Customer_Role;
        }

        public override string GetReadableName()
        {
            return Account_Name;
        }
    }
}