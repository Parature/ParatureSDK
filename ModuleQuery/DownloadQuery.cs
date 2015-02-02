namespace ParatureAPI.ModuleQuery
{
    /// <summary>
    /// Instantiate this class to build all the properties needed to get a list of Downloads.
    /// The properties include the number of items per page, the page number, what custom fields to include in the list,
    /// as well as any filtering you need to do.
    /// </summary>
    public partial class DownloadQuery : ParaQuery
    {
        protected override void buildModuleSpecificFilter()
        {

        }
        /// <summary>
        /// Contains all the static properties you will need when filtering by static fields.
        /// </summary>
        public static partial class DownloadStaticFields
        {
            /// <summary>
            /// Use this property to filter by the date created property of a Download.
            /// </summary>
            public readonly static string DateCreated = "Date_Created";

            /// <summary>
            /// Use this property to filter by the Date Modified property of the Download.
            /// </summary>
            public readonly static string DateUpdated = "Date_Updated";
            /// <summary>
            /// Use this property to filter by the description of the download.
            /// </summary>
            public readonly static string Description = "Description";
            /// <summary>
            /// Use this property to filter by the Eula ID of the Download, in case your configs have this feature activated.
            /// </summary>
            public readonly static string Eula = "Eula_id_";
            /// <summary>
            /// Use this property to filter by the external link of the download.
            /// </summary>
            public readonly static string ExternalLink = "External_Link";
            /// <summary>
            /// Use this property to filter by the id of the folder the download belongs to.
            /// </summary>
            public readonly static string Folder = "Folder_id_";
            /// <summary>
            /// Use this property to filter by the guid of the download file.
            /// </summary>
            public readonly static string Guid = "Guid";
            /// <summary>
            /// Use this property to filter by the Name of the download.
            /// </summary>
            public readonly static string name = "Name";
            /// <summary>
            /// Use this property to filter by one or many SLAs that are specified for this download.
            /// </summary>
            public readonly static string Permissions = "Permissions_id_";
            /// <summary>
            /// Use this property to filter by the Product id of the download.
            /// </summary>
            public readonly static string Products = "Products_id_";
            /// <summary>
            /// Use this property to filter by the published state of the download.
            /// </summary>
            public readonly static string Published = "Published";

            /// <summary>
            /// Use this property to filter by the Title of the download.
            /// </summary>
            public readonly static string Title = "Title";

            /// <summary>
            /// Use this property to filter by the Visibility state of the download.
            /// </summary>
            public readonly static string Visible = "Visible";
        }
    }
}