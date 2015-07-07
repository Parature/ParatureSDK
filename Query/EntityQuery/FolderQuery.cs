namespace ParatureSDK.Query.EntityQuery
{
    public class FolderQuery: ParaQuery
    {
        protected override void BuildModuleSpecificFilter()
        {
        }

        /// <summary>
        /// Contains all the static properties you will need when filtering by static fields.
        /// </summary>
        public static class FolderStaticFields
        {
            /// <summary>
            /// Use this property to filter by the Date Modified property.
            /// </summary>
            public readonly static string DateUpdated = "Date_Updated";
            public readonly static string IsPrivate = "Is_Private";
            public readonly static string Description = "Description";
            public readonly static string Name = "Name";
            /// <summary>
            /// The id of the parent folder.
            /// </summary>
            public readonly static string ParentFolder = "Parent_Folder_id_";
        }
    }
}
