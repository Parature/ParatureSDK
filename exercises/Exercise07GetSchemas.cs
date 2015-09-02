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
        static ParaService Service { get; set; }

        Exercise07GetSchemas()
        {
            Service = new ParaService(CredentialProvider.Creds);
        }

        public static Customer CustomerSchema()
        {
            return Service.Create<Customer>();
        }

        public static Account AccountSchema()
        {
            return Service.Create<Account>();
        }

        public static Article ArticleSchema()
        {
            return Service.Create<Article>();
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
            return Service.Create<ArticleFolder>();
        }

    }
}
