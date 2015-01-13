using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public class ArticleFoldersList : PagedData.PagedData
    {
        public List<ArticleFolder> ArticleFolders = new List<ArticleFolder>();

        public ArticleFoldersList()
        {
        }

        public ArticleFoldersList(ArticleFoldersList articleFoldersList)
            : base(articleFoldersList)
        {
            ArticleFolders = new List<ArticleFolder>(articleFoldersList.ArticleFolders);
        }
    }
}