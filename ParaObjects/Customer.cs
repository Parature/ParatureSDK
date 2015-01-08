using System;

namespace ParatureAPI.ParaObjects
{
    /// <summary>
    /// Holds all the properties of the Customer module.
    /// </summary>
    public partial class Customer : ModuleWithCustomFields
    {
        public Int64 customerid = 0;
        public Account Account = new Account();
        public Sla Sla = new Sla();
        public DateTime Date_Visited;
        public DateTime Date_Created;
        public DateTime Date_Updated;
        public string Email = "";
        public Role Customer_Role = new Role();

        /// <summary>
        /// Only used when you use "Username" as the identifier of your account.
        /// </summary>
        public string User_Name = "";
        public string First_Name = "";
        public string Last_Name = "";


        ////////////////////////////////////////// COULD NOT LOCATE IT FOR NOW //////////////
        /// <summary>
        /// User accepted terms of use.
        /// Certain configs have the terms of use feature activated. This property should be taken into account
        /// only when you are using the "customer terms of use" feature.
        /// </summary>
        public bool Tou;
        /////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Password is only used when creating a new customer. This property is not filled when retrieving the customer details. It must be empty when updating a customer object.
        /// </summary>
        public string Password = "";

        /// <summary>
        /// Password confirm is only used when creating a new customer. This property is not filled when retrieving the customer details. It must be empty when updating a customer object.
        /// </summary>
        public string Password_Confirm = "";


        public CustomerStatus Status = new CustomerStatus();

        ///// <summary>
        ///// The list of all of the customer custom fields of this object.
        ///// </summary>
        //public List<ParaObjects.CustomField> CustomFields= new List<ParaObjects.CustomField>();

        public Customer()
            : base()
        {
        }

        public Customer(Customer customer)
            : base(customer)
        {
            this.customerid = customer.customerid;
            this.Account = new Account(customer.Account);
            this.Sla = new Sla(customer.Sla);
            this.Date_Visited = customer.Date_Visited;
            this.Email = customer.Email;
            this.User_Name = customer.User_Name;
            this.First_Name = customer.First_Name;
            this.Tou = customer.Tou;
            this.Password = customer.Password;
            this.Password_Confirm = customer.Password_Confirm;
            this.Status = new CustomerStatus(customer.Status);
            this.Customer_Role = new Role(customer.Customer_Role);
        }



        public override string GetReadableName()
        {
            return this.First_Name + " " + this.Last_Name + "(" + this.Email + ")";
        }
    }
}