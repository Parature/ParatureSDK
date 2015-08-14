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
        Exercise07GetSchemas()
        {
            ParaService.Credentials = CredentialProvider.Creds;
        }

        public static Customer CustomerSchema()
        {
            return ParaService.Create<Customer>();
        }

        public static Account AccountSchema()
        {
            return ParaService.Create<Account>();
        }

        public static Article ArticleSchema()
        {
            return ParaService.Create<Article>();
        }

        /// <summary>
        /// Get an empty ArticleFolder
        /// </summary>
        /// <returns></returns>
        public static ArticleFolder ArticleFolderSchema()
        {
            //There is an API call to retrieve an article folder schema, 
            //but there aren't any custom fields so we are not providing a 
            //method fo schema retrieval
            return ParaService.Create<ArticleFolder>();
        }

    }
}
