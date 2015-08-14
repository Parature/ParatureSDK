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
            ParaService.Credentials = CredentialProvider.Creds;
            return ParaService.Create<Customer>();
        }

        public static Account AccountSchema()
        {
            ParaService.Credentials = CredentialProvider.Creds;
            return ParaService.Create<Account>();
        }

        public static Article ArticleSchema()
        {
            ParaService.Credentials = CredentialProvider.Creds;
            return ParaService.Create<Article>();
        }

        /// <summary>
        /// Get an empty ArticleFolder
        /// </summary>
        /// <returns></returns>
        public static ArticleFolder ArticleFolderSchema()
        {
            //There is an API call to retrieve an article folder schema, but there aren't any custom fields so we are not providing a method fo schema retrieval
            ParaService.Credentials = CredentialProvider.Creds;
            return ParaService.Create<ArticleFolder>();
        }

    }
}
