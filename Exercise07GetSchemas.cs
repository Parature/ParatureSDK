using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParatureSDK;
using ParatureSDK.ParaObjects;

namespace Exercises
{
    class Exercise07GetSchemas
    {
        public static Customer CustomerSchema()
        {
            var customer = ParatureSDK.ApiHandler.Customer.Schema(CredentialProvider.Creds);

            return customer;
        }

        public static Account AccountSchema()
        {
            var account = ParatureSDK.ApiHandler.Account.Schema(CredentialProvider.Creds);

            return account;
        }

        public static Article ArticleSchema()
        {
            var article = ParatureSDK.ApiHandler.Article.Schema(CredentialProvider.Creds);

            return article;
        }

        public static ArticleFolder ArticleFolderSchema()
        {
            var articleFoler = ParatureSDK.ApiHandler.Article.ArticleFolder.Schema(CredentialProvider.Creds);

            return articleFoler;
        }

    }
}
