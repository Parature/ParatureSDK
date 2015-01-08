using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public partial class ArticleFoldersList : PagedData.PagedData
    {
        public List<ArticleFolder> ArticleFolders = new List<ArticleFolder>();

        public ArticleFoldersList()
        {
        }

        public ArticleFoldersList(ArticleFoldersList articleFoldersList)
            : base(articleFoldersList)
        {
            this.ArticleFolders = new List<ArticleFolder>(articleFoldersList.ArticleFolders);
        }
    }
}