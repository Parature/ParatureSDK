using System.Collections.Generic;
using ParatureAPI.ParaObjects;

namespace ParatureAPI.PagedData
{
    /// <summary>
    /// Instantiate this class to hold the result set of a list call to APIs. Whenever you need to get a list of 
    /// Knowledge base articles
    /// </summary>
    public class ArticlesList : PagedData
    {
        public List<ParaObjects.Article> Articles = new List<ParaObjects.Article>();

        public ArticlesList()
        {
        }

        public ArticlesList(ArticlesList articlesList)
            : base(articlesList)
        {
            Articles = new List<ParaObjects.Article>(articlesList.Articles);
        }
    }
}