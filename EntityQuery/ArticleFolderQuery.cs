namespace ParatureAPI.EntityQuery
{
    public partial class ArticleFolderQuery : ParaQuery
    {
        protected override void buildModuleSpecificFilter()
        {

        }
        /// <summary>
        /// Contains all the static properties you will need when filtering by static fields.
        /// </summary>
        public static partial class ArticleFolderStaticFields
        {
            public readonly static string Name = "Name";
            public readonly static string ParentFolder = "Parent_Folder_id_";
        }
    }
}