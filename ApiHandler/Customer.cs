using System;
using System.Collections;
using System.Threading;
using System.Xml;
using ParatureSDK.ApiHandler.ApiMethods;
using ParatureSDK.EntityQuery;
using ParatureSDK.ModuleQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Customer module.
    /// </summary>
    public class Customer : FirstLevelApiMethods<ParaObjects.Customer, CustomerQuery>
    {
        /// <summary>
        /// Contains all the methods for retrieving Customer roles
        /// </summary>
        public class Role : SecondLevelApiMethods<ParaObjects.CustomerRole, RoleQuery, ParaObjects.Customer>
        {}

        /// <summary>
        /// Contains all the methods for retrieving Customer statuses
        /// </summary>
        public class Status : SecondLevelApiMethods<ParaObjects.Status, StatusQuery, ParaObjects.Customer>
        {}

        /// <summary>
        /// Contains all the methods for retrieving Customer views
        /// </summary>
        public class View : SecondLevelApiMethods<ParaObjects.View, ViewQuery, ParaObjects.Customer>
        {}

    }
}