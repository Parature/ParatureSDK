using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParatureSDK;
using ParatureSDK.ParaObjects;

namespace Exercises
{
    class Exercise05ListArticles
    {
        public static int getArticleCount()
        {
            var articleQuery = new ParatureSDK.ModuleQuery.ArticleQuery();
            articleQuery.RetrieveAllRecords = true;
            articleQuery.TotalOnly = true;

            var articles = ParatureSDK.ApiHandler.Article.GetList(CredentialProvider.Creds, articleQuery);

            return articles.TotalItems;
        }

        public static ParaEntityList<Article> getArticles()
        {
            var articleQuery = new ParatureSDK.ModuleQuery.ArticleQuery();
            articleQuery.RetrieveAllRecords = true;

            var articles = ParatureSDK.ApiHandler.Article.GetList(CredentialProvider.Creds, articleQuery);

            return articles;
        }

        public static ParaEntityList<Article> getPublishedArticles()
        {
            var articleQuery = new ParatureSDK.ModuleQuery.ArticleQuery();
            articleQuery.AddStaticFieldFilter(ParatureSDK.ModuleQuery.ArticleQuery.ArticleStaticFields.Published, ParaEnums.QueryCriteria.Equal, true);
            articleQuery.RetrieveAllRecords = true;

            var articles = ParatureSDK.ApiHandler.Article.GetList(CredentialProvider.Creds, articleQuery);

            return articles;
        }

        public static ParaEntityList<Article> getArticlesByFolder(long folderID)
        {
            var articleQuery = new ParatureSDK.ModuleQuery.ArticleQuery();
            articleQuery.AddStaticFieldFilter(ParatureSDK.ModuleQuery.ArticleQuery.ArticleStaticFields.Folders, ParaEnums.QueryCriteria.Equal, folderID.ToString());
            articleQuery.RetrieveAllRecords = true;

            var articles = ParatureSDK.ApiHandler.Article.GetList(CredentialProvider.Creds, articleQuery);

            return articles;
        }
    }
}
