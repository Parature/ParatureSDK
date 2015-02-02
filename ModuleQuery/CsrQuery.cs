namespace ParatureAPI.ModuleQuery
{
    public partial class CsrQuery : ParaQuery
    {
        protected override void buildModuleSpecificFilter()
        {

        }

        public static partial class CsrStaticFields
        {
            /// <summary>
            /// Use this property to filter by the date created property of an Article.
            /// </summary>

            public readonly static string DateCreated = "Date_Created";
            public readonly static string DateUpdated = "Date_Format";
            public readonly static string Email = "Email";
            public readonly static string Fax = "Fax";
            public readonly static string Full_Name = "Full_Name";
            public readonly static string Phone1 = "Phone1";
            public readonly static string Phone2 = "Phone2";
            public readonly static string Screen_Name = "Screen_Name";
            public readonly static string ModifiedBy = "Modified_By_id_";
            public readonly static string Role = "CsrRole_id_";
            public readonly static string Status = "Status_id_";
            public readonly static string Timezone = "Timezone_id_";
        }


    }
}