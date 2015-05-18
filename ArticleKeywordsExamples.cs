using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParatureSDK;
using ParatureSDK.ApiHandler;

namespace Exercises
{
    public static class ArticleKeywordsExamples
    {
        /// <summary>
        /// Add keywords to an article that we're assuming does not already have articles
        /// </summary>
        /// <param name="articleID"></param>
        /// <param name="keywords"></param>
        /// <param name="creds"></param>
        /// <returns></returns>
        public static bool AddKeywords(long articleId, List<string> keywords, ParaCredentials creds)
        {
            //Get the article
            var article = ParatureSDK.ApiHandler.Article.GetDetails(articleId, creds);

            //Set the keywords
            article.Keywords = String.Join(",", keywords);

            //Perform the update
            var response = ParatureSDK.ApiHandler.Article.Update(article, creds);

            //Verify response
            return !response.HasException;
        }

        /// <summary>
        /// Get the keywords from an article
        /// </summary>
        /// <param name="articleID"></param>
        /// <param name="creds"></param>
        /// <returns></returns>
        public static List<string> GetKeywords(long articleId, ParaCredentials creds)
        {
            //Get the article
            var article = ParatureSDK.ApiHandler.Article.GetDetails(articleId, creds);

            return article.Keywords.Split(',').ToList();
        }
    }
}
