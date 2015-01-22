using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public class ArticleFoldersList : PagedData.PagedData
    {
        public List<ArticleFolder> ArticleFolders = new List<ArticleFolder>();
    }
}