namespace ParatureAPI.ParaObjects
{
    public partial class ArticleFolder : Folder
    {

        public ArticleFolder Parent_Folder;

        public bool FullyLoaded = false;

        public ArticleFolder()
        {
        }

        public ArticleFolder(ArticleFolder articleFolder)
        {
            this.Description = articleFolder.Description;
            this.FolderID = articleFolder.FolderID;
            this.FullyLoaded = articleFolder.FullyLoaded;
            this.Is_Private = articleFolder.Is_Private;
            this.Name = articleFolder.Name;
        }
    }
}