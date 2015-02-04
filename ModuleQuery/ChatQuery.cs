namespace ParatureSDK.ModuleQuery
{
    /// <summary>
    /// Instantiate this class to build all the properties needed to get a list of Chats.
    /// The properties include the number of items per page, the page number, what custom fields to include in the list,
    /// as well as any filtering you need to do.
    /// </summary>
    public class ChatQuery : ParaEntityQuery
    {
        protected override void BuildModuleSpecificFilter()
        { }

        /// <summary>
        /// Contains all the static properties you will need when filtering by static fields.
        /// </summary>
        public static class ChatStaticFields
        {
            /// <summary>
            /// Use this property to filter by the date created property of an Article.
            /// </summary>

            public readonly static string Browser_Language = "Browser_Language";
            public readonly static string Browser_Type = "Browser_Type";
            public readonly static string Browser_Version = "Browser_Version";
            public readonly static string Chat_Number = "Chat_Number";
            public readonly static string Customer = "Customer_id_";
            public readonly static string  Date_Created = "Date_Created";
            public readonly static string  Date_Ended= "Date_Ended";
            public readonly static string  Ip_Address= "Ip_Address";
            public readonly static string  Is_Anonymous = "Is_Anonymous";
            public readonly static string Referrer_Url  = "Referrer_Url ";
            public readonly static string Related_Tickets = "Related_Tickets_id_";
            public readonly static string Status = "Status_id_";
            public readonly static string Summary  = "Summary";
            public readonly static string  User_Agent= "User_Agent";
            public readonly static string Email = "Email";
        }
    }
}