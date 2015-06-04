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
        public class Role : SecondLevelApiMethods<ParaObjects.CustomerRole, RoleQuery, ParaObjects.Customer>
        {}

        public class Status : SecondLevelApiMethods<ParaObjects.Status, StatusQuery, ParaObjects.Customer>
        {}

        public class View : SecondLevelApiMethods<ParaObjects.View, ViewQuery, ParaObjects.Customer>
        {}

    }
}