namespace ParatureAPI.EntityQuery
{
    public class DownloadFolderQuery : ParaQuery
    {
        protected override void BuildModuleSpecificFilter()
        {}

        /// <summary>
        /// Contains all the static properties you will need when filtering by static fields.
        /// </summary>
        public static class DownloadFolderStaticFields
        {
            /// <summary>
            /// Use this property to filter by the Date Modified property of the DownloadFolder.
            /// </summary>
            public readonly static string DateUpdated = "Date_Updated";
            public readonly static string IsPrivate = "Is_Private";
            public readonly static string Name = "Name";
            public readonly static string Description = "Description";
            /// <summary>
            /// The id of the parent folder.
            /// </summary>
            public readonly static string ParentFolder = "Parent_Folder_id_";

        }
    }
}