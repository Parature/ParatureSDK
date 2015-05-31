using System;
using System.Threading;
using System.Xml;
using ParatureSDK.EntityQuery;
using ParatureSDK.ModuleQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Account module.
    /// </summary>
    public class Account : FirstLevelApiHandler<ParaObjects.Account, AccountQuery>
    {
        public class View : SecondLevelApiEntity<ParaObjects.View, ViewQuery, ParaObjects.Account>
        {
        }
    }
}