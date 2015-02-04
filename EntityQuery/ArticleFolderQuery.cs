namespace ParatureSDK.EntityQuery
{
    public class ArticleFolderQuery : ParaQuery
    {
        protected override void BuildModuleSpecificFilter()
        {}

        /// <summary>
        /// Contains all the static properties you will need when filtering by static fields.
        /// </summary>
        public static class ArticleFolderStaticFields
        {
            public readonly static string Name = "Name";
            public readonly static string ParentFolder = "Parent_Folder_id_";
        }
    }
}