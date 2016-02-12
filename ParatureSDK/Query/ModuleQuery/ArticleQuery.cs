using ParatureSDK.ParaObjects;
using System;

namespace ParatureSDK.Query.ModuleQuery
{
    /// <summary>
    /// Instantiate this class to build all the properties needed to get a list of Articles.
    /// The properties include the number of items per page, the page number, what custom fields to include in the list,
    /// as well as any filtering you need to do.
    /// </summary>
    public class ArticleQuery : ParaEntityQuery
    {
        internal override Type QueryTargetType
        {
            get
            {
                return typeof(Article);
            }
        }

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

            public const string DateCreated = "Date_Created";
            public const string DateUpdated = "Date_Updated";
            public const string Answer = "Answer";
            public const string ExpirationDate = "Expiration_Date";
            public const string Published = "Published";
            public const string Question = "Question";
            public const string Rating = "Rating";
            public const string TimesViewed = "Times_Viewed";
            public const string ModifiedBy = "Modified_By_id_";
            public const string CreatedBy = "Created_By_id_";
            public const string Folders = "Folders_id_";
            public const string Permissions = "Permissions_id_";
            public const string Products = "Products_id_";

            /// <summary>
            /// Returns all Articles related to the given Articles.
            /// </summary>
            public const string RelatedArticles = "Related_Articles_id_";

            /// <summary>
            /// The terms to search for in the Article's Question, Alternate Question, or Answer. Encode spaces in terms with the plus sign +. Keyword search can only be used in conjuction with the following filters: Folders_id_, Products_id_, Published=true, _searchoption_, _search_subfolders_
            /// </summary>
            public const string Keywords = "_keywords_";

            /// <summary>
            /// Specifies how to search. Default is allwords
            /// </summary>
            public const string SearchOption = "_searchoption_";

            /// <summary>
            /// Controls if articles in subfolders will be searched
            /// </summary>
            public const string SearchSubfolders = "_search_subfolders_";

            /// <summary>
            /// Status of the article: 'active', 'all', or 'trashed'. Default is 'active'
            /// </summary>
            public const string Status = "_status_";
        }
    }
}