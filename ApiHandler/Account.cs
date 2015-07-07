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
    /// Contains all the methods that allow you to interact with the Parature Account module.
    /// </summary>
    public class Account : FirstLevelApiMethods<ParaObjects.Account, AccountQuery>
    {
        /// <summary>
        /// Methods for retrieving views related to an account
        /// </summary>
        public class View : SecondLevelApiMethods<ParaObjects.View, ViewQuery, ParaObjects.Account>
        {
        }
    }
}