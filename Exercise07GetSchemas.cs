using System;
using System.Collections.Generic;
using System.Text;
using ParatureAPI;

namespace Exercises
{
    class Exercise07GetSchemas
    {
        public static ParaObjects.Customer CustomerSchema()
        {
            var customer = ApiHandler.Customer.CustomerSchema(CredentialProvider.Creds);

            return customer;
        }

        public static ParaObjects.Account AccountSchema()
        {
            var account = ApiHandler.Account.AccountSchema(CredentialProvider.Creds);

            return account;
        }

        public static ParaObjects.Article ArticleSchema()
        {
            var article = ApiHandler.Article.ArticleSchema(CredentialProvider.Creds);

            return article;
        }

        public static ParaObjects.ArticleFolder ArticleFolderSchema()
        {
            var articleFoler = ApiHandler.Article.ArticleFolder.ArticleFolderSchema(CredentialProvider.Creds);

            return articleFoler;
        }

    }
}
