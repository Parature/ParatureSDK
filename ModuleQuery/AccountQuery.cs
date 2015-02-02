using System;

namespace ParatureAPI.ModuleQuery
{
    /// <summary>
    /// Instantiate this class to build all the properties needed to get a list of Accounts.
    /// The properties include the number of items per page, the page number, what custom fields to include in the list,
    /// as well as any filtering you need to do.
    /// </summary>
    public class AccountQuery : ParaEntityQuery
    {
        private Int64 _view = 0;

        /// <summary>
        /// The ID of the view
        /// </summary>
        public Int64 View
        {
            get { return _view; }
            set
            {
                _view = value;
            }
        }

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
        public static class AccountStaticFields
        {
            public readonly static string Accountname = "Account_Name";

            /// <summary>
            /// The criteria for this property can only be a SlaID
            /// </summary>
            public readonly static string AccountSlaID = "sla_id_";

            /// <summary>
            /// The criteria for this property can only be a RoleID
            /// </summary>
            public readonly static string DefaultCustomerRoleID = "Default_Customer_Role_id_";

            /// <summary>
            /// The criteria for this property can only be a CsrID
            /// </summary>
            public readonly static string OwnedByCsrID = "Owned_By_id_";

            /// <summary>
            /// The criteria for this property can only be a CsrID
            /// </summary>
            public readonly static string ModifiedByCsrID = "Modified_By_id_";


            public readonly static string Date_Created = "Date_Created";
            public readonly static string Date_Updated = "Date_Updated";

            /// <summary>
            /// In certain configurations, Accounts might be associated to products.
            /// In that case, you can filter by certain product IDs.
            /// </summary>
            public readonly static string AccountProducts = "Am_Product";

            /// <summary>
            /// In certain configurations, certain customers from an account can see tickets belonging
            /// to other accounts. You can filter by this field by entering the 
            /// account(s) id(s) (comma separated ids, in case you are entering multiple ids).
            /// </summary>
            public readonly static string ViewableAccountid = "shown_accounts";

        }
    }
}