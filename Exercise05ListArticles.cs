using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParatureAPI;

namespace Exercises
{
    class Exercise05ListArticles
    {
        public static int getArticleCount()
        {
            var articleQuery = new ModuleQuery.ArticleQuery();
            articleQuery.RetrieveAllRecords = true;
            articleQuery.TotalOnly = true;

            var articles = ApiHandler.Article.ArticlesGetList(CredentialProvider.Creds, articleQuery);

            return articles.TotalItems;
        }

        public static ParaObjects.ArticlesList getArticles()
        {
            var articleQuery = new ModuleQuery.ArticleQuery();
            articleQuery.RetrieveAllRecords = true;

            var articles = ApiHandler.Article.ArticlesGetList(CredentialProvider.Creds, articleQuery);

            return articles;
        }

        public static ParaObjects.ArticlesList getPublishedArticles()
        {
            var articleQuery = new ModuleQuery.ArticleQuery();
            articleQuery.AddStaticFieldFilter(ModuleQuery.ArticleQuery.ArticleStaticFields.Published, Paraenums.QueryCriteria.Equal, true);
            articleQuery.RetrieveAllRecords = true;

            var articles = ApiHandler.Article.ArticlesGetList(CredentialProvider.Creds, articleQuery);

            return articles;
        }

        public static ParaObjects.ArticlesList getArticlesByFolder(long folderID)
        {
            var articleQuery = new ModuleQuery.ArticleQuery();
            articleQuery.AddStaticFieldFilter(ModuleQuery.ArticleQuery.ArticleStaticFields.Folders, Paraenums.QueryCriteria.Equal, folderID.ToString());
            articleQuery.RetrieveAllRecords = true;

            var articles = ApiHandler.Article.ArticlesGetList(CredentialProvider.Creds, articleQuery);

            return articles;
        }


    }
}
