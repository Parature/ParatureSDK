using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParatureSDK;
using ParatureSDK.ParaObjects;

namespace Exercises
{
    public static class ArticleKeywordsExamples
    {
        public static ParaService Service { get; set; }

        static ArticleKeywordsExamples()
        {
            Service = new ParaService(CredentialProvider.Creds);
        }

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
            var article = Service.GetDetails<Article>(articleId);

            //Set the keywords
            article.Keywords = String.Join(",", keywords);

            //Perform the update
            var response = Service.Update(article);

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
            var article = Service.GetDetails<Article>(articleId);

            return article.Keywords.Split(',').ToList();
        }

        //Example Setting Keywords
        public static void AddingKeywordsExample(ParaCredentials creds)
        {
            var keywords = new List<string>();
            keywords.Add("one");
            keywords.Add("two");

            AddKeywords(10, keywords, creds);
        }
    }
}
