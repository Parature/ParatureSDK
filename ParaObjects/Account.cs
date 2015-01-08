using System;
using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    /// <summary>
    /// Holds all the properties of the Account module.
    /// </summary>
    public partial class Account : ModuleWithCustomFields
    {
        public Int64 Accountid = 0;
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

        ///// <summary>
        ///// The list of all custom fields of this object.
        ///// </summary>
        //public List<ParaObjects.CustomField> CustomFields= new List<ParaObjects.CustomField>() ;// = List<ParaObjects.CustomField>();

        public Account()
            : base()
        {
        }

        public Account(Account account)
            : base(account)
        {
            this.Accountid = account.Accountid;
            this.Account_Name = account.Account_Name;
            this.Modified_By = new Csr(account.Modified_By);
            this.Owned_By = new Csr(account.Owned_By);
            this.Sla = new Sla(account.Sla);
            this.Viewable_Account = new List<Account>(account.Viewable_Account);
            this.Default_Customer_Role = new Role(account.Default_Customer_Role);
        }

        public override string GetReadableName()
        {
            return this.Account_Name;
        }
    }
}