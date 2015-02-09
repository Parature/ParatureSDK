namespace ParatureSDK.ParaObjects
{
    public class ArticleFolder : Folder
    {

        public ArticleFolder Parent_Folder;

        public bool FullyLoaded = false;

        public ArticleFolder()
        {
        }

        public ArticleFolder(ArticleFolder articleFolder)
        {
            Description = articleFolder.Description;
            Id = articleFolder.Id;
            FullyLoaded = articleFolder.FullyLoaded;
            Is_Private = articleFolder.Is_Private;
            Name = articleFolder.Name;
        }
    }
}