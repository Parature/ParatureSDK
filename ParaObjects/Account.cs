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
    public class Account : ParaEntity, IMutableEntity
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
                        FieldType = "text",
                        DataType = "string"
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
                        FieldType = "entity",
                        DataType = "entity"
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
                        FieldType = "entity",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        /// <summary>
        /// The list of all the other Viewable accounts, only available to certain configs.
        /// </summary>
        public List<Account> Shown_Accounts
        {
            get
            {
                return GetFieldValue<List<Account>>("Shown_Accounts");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Shown_Accounts");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Shown_Accounts",
                        FieldType = "entitymultiple",
                        DataType = "entity"
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
            Shown_Accounts = new List<Account>(account.Shown_Accounts);
            Default_Customer_Role = account.Default_Customer_Role;
        }

        public override string GetReadableName()
        {
            return Account_Name;
        }
    }
}