using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParatureSDK;

namespace Exercises
{
    class Exercise07GetSchemas
    {
        public static ParaObjects.Customer CustomerSchema()
        {
            var customer = ParatureSDK.ApiHandler.Customer.CustomerSchema(CredentialProvider.Creds);

            return customer;
        }

        public static ParaObjects.Account AccountSchema()
        {
            var account = ParatureSDK.ApiHandler.Account.AccountSchema(CredentialProvider.Creds);

            return account;
        }

        public static ParaObjects.Article ArticleSchema()
        {
            var article = ParatureSDK.ApiHandler.Article.ArticleSchema(CredentialProvider.Creds);

            return article;
        }

        public static ParaObjects.ArticleFolder ArticleFolderSchema()
        {
            var articleFoler = ParatureSDK.ApiHandler.Article.ArticleFolder.ArticleFolderSchema(CredentialProvider.Creds);

            return articleFoler;
        }

    }
}
