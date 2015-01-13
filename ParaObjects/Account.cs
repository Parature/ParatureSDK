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
        public Int64 AccountId
        {
            get
            {
                var field = Fields.FirstOrDefault(f => f.Name == "id");
                var val = 0;
                try
                {
                    return Convert.ToInt64(field.Value);
                }
                catch (Exception e) { }

                return val;
            }
            set
            {
                var field = Fields.FirstOrDefault(f => f.Name == "id");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "id",
                        DataType = ParaEnums.FieldDataType.Int
                    };
                    Fields.Add(field);
                }

                field.Value = value.ToString();
            }
        }
        public string Account_Name = "";
        public Csr Modified_By = new Csr();
        public Csr Owned_By = new Csr();
        public Sla Sla = new Sla();
        public DateTime Date_Created;
        public DateTime Date_Updated;
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
            AccountId = account.AccountId;
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