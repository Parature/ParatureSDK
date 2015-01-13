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
                var field = Fields.FirstOrDefault(f => f.Name == "Account_Name");
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
        public Csr Modified_By = new Csr();
        public Csr Owned_By = new Csr();
        public Sla Sla = new Sla();
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
        public Role Default_Customer_Role = new Role();
        /// <summary>
        /// The list of all the other Viewable accounts, only available to certain configs.
        /// </summary>
        public List<Account> Viewable_Account = new List<Account>();

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