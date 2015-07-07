using System;
using System.Collections;
using System.Threading;
using System.Xml;
using ParatureSDK.ApiHandler.ApiMethods;
using ParatureSDK.ParaObjects;
using ParatureSDK.Query.EntityQuery;
using ParatureSDK.Query.ModuleQuery;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Knowledge Base module.
    /// </summary>
    public class Article : FirstLevelApiMethods<ParaObjects.Article, ArticleQuery>
    {
        /// <summary>
        /// Contains all the methods needed to work with the article module's folders.
        /// </summary>
        public class ArticleFolder : FolderApiMethods<ParaObjects.ArticleFolder, ArticleFolderQuery> {}
    }
}