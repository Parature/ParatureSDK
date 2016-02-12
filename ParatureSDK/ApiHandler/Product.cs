using System;
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
    /// Contains all the methods that allow you to interact with the Parature Product module.
    /// </summary>
    public class Product : FirstLevelApiMethods<ParaObjects.Product, ProductQuery>
    {
        /// <summary>
        /// Contains all the methods needed to work with the download module's folders.
        /// </summary>
        public class ProductFolder : FolderApiMethods<ParaObjects.ProductFolder, ProductFolderQuery>
        {}
    }
}