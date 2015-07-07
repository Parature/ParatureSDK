namespace ParatureSDK.Query.ModuleQuery
{
    /// <summary>
    /// Instantiate this class to build all the properties needed to get a list of Customers.
    /// The properties include the number of items per page, the page number, what custom fields to include in the list,
    /// as well as any filtering you need to do.
    /// </summary>
    public class CustomerQuery : ParaEntityQuery
    {
        public CustomerQuery()
        {
            View = 0;
        }

        /// <summary>
        /// The ID of the view
        /// </summary>
        public long View { get; set; }

        protected override void BuildModuleSpecificFilter()
        {
            // Checking if this is a view request.
            if (View > 0)
            {
                _QueryFilters.Add("_view_=" + View.ToString());
            }
        }

        /// <summary>
        /// Contains all the static properties you will need when filtering by static fields.
        /// </summary>
        public static class CustomerStaticFields
        {
            /// <summary>
            /// Use this property to filter by the date created property of a customer.
            /// </summary>
            public readonly static string DateCreated = "Date_Created";

            /// <summary>
            /// Use this property to filter by the date updated property of a customer.
            /// </summary>
            public readonly static string DateUpdated = "Date_Updated";

            /// <summary>
            /// Use this property to filter by the date created property of a customer.Visited
            /// </summary>
            public readonly static string DateVisited = "Date_Visited";

            /// <summary>
            /// To search by customer User Name.
            /// </summary>
            public readonly static string User_Name = "User_Name";

            /// <summary>
            /// To search by customer Email.
            /// </summary>
            public readonly static string CustomerEmail = "Email";

            /// <summary>
            /// Search by customer First Name
            /// </summary>
            public readonly static string FirstName = "First_Name";

            /// <summary>
            /// Search by customer Last Name
            /// </summary>
            public readonly static string LastName = "Last_Name";

            /// <summary>
            /// Filter by the accountid of the customers being returned. 
            /// </summary>
            public readonly static string AccountID = "Account_id_";

            /// <summary>
            /// To filter by Customers' SLAs
            /// </summary>
            public readonly static string CustomerSla = "Sla_id_";

            /// <summary>
            /// To filter by Customers' Roles
            /// </summary>
            public readonly static string CustomerRoleID = "Customer_Role_id_";

            /// <summary>
            /// To filter by the status field. 
            /// </summary>
            public readonly static string Status = "Status_id_";

            /// <summary>
            /// In certain configurations, customers might be associated to products.
            /// In that case, you can filter by certain product IDs.
            /// </summary>
            public readonly static string CustomerProducts = "Cust_Product_id_";

            /// <summary>
            /// In certain configurations, customers might have to accept the terms of use.
            /// If you have that configuration activated, you will be able to filter by this field.
            /// </summary>
            public readonly static string AcceptUseOfTerm = "";

        }
    }
}