namespace ParatureSDK.Query.ModuleQuery
{
    /// <summary>
    /// Instantiate this class to build all the properties needed to get a list of Tickets.
    /// The properties include the number of items per page, the page number, what custom fields to include in the list,
    /// as well as any filtering you need to do.
    /// </summary>
    public class TicketQuery : ParaEntityQuery
    {
        // Status Type to filter tickets by.
        private ParaEnums.TicketStatusType _statusType = ParaEnums.TicketStatusType.All;

        /// <summary>
        /// If there is any need to limit the ticket returned to a certain status type. Default is 
        /// to return all.
        /// </summary>
        public ParaEnums.TicketStatusType Status_Type
        {
            get { return _statusType; }
            set
            {
                _statusType = value;
            }
        }

        /// <summary>
        /// Whether to show only tickets assigned to the CSR associated with the token you are using or not.
        /// </summary>
        public bool MyTickets { get; set; }

        public TicketQuery()
        {
            View = 0;
            MyTickets = false;
        }

        /// <summary>
        /// The ID of the view
        /// </summary>
        public long View { get; set; }

        protected override void BuildModuleSpecificFilter()
        {

            // need to filter if requesting by status type
            if (Status_Type != ParaEnums.TicketStatusType.All)
            {
                _QueryFilters.Add("_status_type_=" + Status_Type.ToString().ToLower());
            }

            if (MyTickets == true)
            {
                _QueryFilters.Add("_my_tickets_=true");
            }

            // Checking if this is a view request.
            if (View > 0)
            {
                _QueryFilters.Add("_view_=" + View.ToString());
            }
        }

        /// <summary>
        /// Contains all the static properties you will need when filtering by static fields.
        /// </summary>
        public static class TicketStaticFields
        {
            /// <summary>
            /// Use this property to filter by the date created property of a ticket.
            /// </summary>
            public readonly static string Date_Created = "Date_Created";

            /// <summary>
            /// Allows for filtering through the Account of the Customer that currently owns the ticket.
            /// </summary>
            public readonly static string Ticket_Account = "Ticket_Account_id_";

            /// <summary>
            /// Provide filtering with the department id of the tickets you would like to list.
            /// </summary>
            public readonly static string Ticket_Department = "Department_id_";

            /// <summary>
            /// Use this property to filter by the date created property of a ticket.
            /// </summary>
            public readonly static string Date_Updated = "Date_Updated";

            /// <summary>
            /// Filter by the "Assigned To" Csr ID. 
            /// </summary>
            public readonly static string Assigned_To = "Assigned_To_id_";

            /// <summary>
            /// Filter by the "additional_contact_account_id_". 
            /// </summary>
            public readonly static string additional_contact_account_id = "additional_contact_account_id_";

            /// <summary>
            /// Filter by the "additional_contact_id_". 
            /// </summary>
            public readonly static string additional_contact_id = "additional_contact_id_";

            /// <summary>
            /// Filter by the "Additional Contact's Account" ID. 
            /// </summary>
            public readonly static string Additional_Contact_Account = "Additional_Contact_Account_id_";

            /// <summary>
            /// 
            /// </summary>
            public readonly static string Cc_Csr = "Cc_Csr";

            /// <summary>
            /// 
            /// </summary>
            public readonly static string CC_Customer = "Cc_Customer";

            /// <summary>
            /// 
            /// </summary>
            public readonly static string Email_Notification = "Email_Notification";

            /// <summary>
            /// 
            /// </summary>
            public readonly static string Hide_From_Customer = "Hide_From_Customer";

            /// <summary>
            /// 
            /// </summary>
            public readonly static string Customer = "Ticket_Customer_id_";

            /// <summary>
            /// 
            /// </summary>
            public readonly static string Additional_Contact = "Additional_Contact_id_";

            /// <summary>
            /// 
            /// </summary>
            public readonly static string Ticket_Queue = "Ticket_Queue_id_";

            /// <summary>
            /// Filter by the created by CSR id.
            /// </summary>
            public readonly static string Entered_By = "Entered_By_id_";

            /// <summary>
            /// Tickets that have a specific ticket parentid.
            /// </summary>
            public readonly static string Ticket_Parent = "Ticket_Parent_id_";

            /// <summary>
            /// Tickets that have a specific ticket status.
            /// </summary>
            public readonly static string Status = "ticket_status_id_";

            /// <summary>
            /// Tickets owned by Customers who belong to the specific Account
            /// </summary>
            public readonly static string Account = "Ticket_Account_id_";

            /// <summary>
            /// Tickets associated to a specific Asset
            /// </summary>
            public readonly static string Asset = "Ticket_Product_id_";
        }
    }
}