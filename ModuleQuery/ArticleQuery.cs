namespace ParatureSDK.ModuleQuery
{
    /// <summary>
    /// Instantiate this class to build all the properties needed to get a list of Articles.
    /// The properties include the number of items per page, the page number, what custom fields to include in the list,
    /// as well as any filtering you need to do.
    /// </summary>
    public class ArticleQuery : ParaEntityQuery
    {
        protected override void BuildModuleSpecificFilter()
        {

        }
        /// <summary>
        /// Contains all the static properties you will need when filtering by static fields.
        /// </summary>
        public static class ArticleStaticFields
        {
            /// <summary>
            /// Use this property to filter by the date created property of an Article.
            /// </summary>

            public readonly static string DateCreated = "Date_Created";
            public readonly static string DateUpdated = "Date_Updated";
            public readonly static string Answer = "Answer";
            public readonly static string ExpirationDate = "Expiration_Date";
            public readonly static string Published = "Published";
            public readonly static string Question = "Question";
            public readonly static string Rating = "Rating";
            public readonly static string TimesViewed = "Times_Viewed";
            public readonly static string ModifiedBy = "Modified_By_id_";
            public readonly static string CreatedBy = "Created_By_id_";
            public readonly static string Folders = "Folders_id_";
            public readonly static string Permissions = "Permissions_id_";
            public readonly static string Products = "Products_id_";
        }
    }
}