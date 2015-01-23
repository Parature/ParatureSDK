using System.Collections.Generic;
using ParatureAPI.ParaObjects;

namespace ParatureAPI.PagedData
{
    /// <summary>
    /// Instantiate this class to hold the result set of a list call to APIs. Whenever you need to get a list of 
    /// Knowledge base articles
    /// </summary>
    public class ArticlesList : ParaEntityList<ParaObjects.Article>
    {
        public List<ParaObjects.Article> Data = new List<ParaObjects.Article>();
    }
}