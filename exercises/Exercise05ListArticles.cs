using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParatureSDK;
using ParatureSDK.ParaObjects;
using ParatureSDK.Query.ModuleQuery;

namespace Exercises
{
    class Exercise05ListArticles
    {
        static ParaService Service { get; set; }

        public Exercise05ListArticles()
        {
            Service = new ParaService(CredentialProvider.Creds);
        }

        public static int getArticleCount()
        {
            var articleQuery = new ArticleQuery();
            articleQuery.RetrieveAllRecords = true;
            articleQuery.TotalOnly = true;

            return Service.GetList<Article>(articleQuery).TotalItems;
        }

        public static ParaEntityList<Article> getArticles()
        {
            var articleQuery = new ArticleQuery();
            articleQuery.RetrieveAllRecords = true;

            return Service.GetList<Article>(articleQuery);
        }

        public static ParaEntityList<Article> getPublishedArticles()
        {
            var articleQuery = new ArticleQuery();
            articleQuery.AddStaticFieldFilter(ArticleQuery.ArticleStaticFields.Published, ParaEnums.QueryCriteria.Equal, true);
            articleQuery.RetrieveAllRecords = true;

            return Service.GetList<Article>(articleQuery);
        }

        public static ParaEntityList<Article> getArticlesByFolder(long folderID)
        {
            var articleQuery = new ArticleQuery();
            articleQuery.AddStaticFieldFilter(ArticleQuery.ArticleStaticFields.Folders, ParaEnums.QueryCriteria.Equal, folderID.ToString());
            articleQuery.RetrieveAllRecords = true;

            return Service.GetList<Article>(articleQuery);
        }
    }
}
