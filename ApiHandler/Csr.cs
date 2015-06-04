using System;
using System.Collections;
using System.Xml;
using ParatureSDK.ApiHandler.ApiMethods;
using ParatureSDK.EntityQuery;
using ParatureSDK.ModuleQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Csr module.
    /// </summary>
    public class Csr : FirstLevelApiMethods<ParaObjects.Csr, CsrQuery>
    {
        /// <summary>
        /// Contains all the methods needed to work with the Ticket statuses.
        /// </summary>
        public class Status : SecondLevelApiMethods<ParaObjects.Status, StatusQuery, ParaObjects.Csr>
        {}

        public class Role : SecondLevelApiMethods<ParaObjects.CsrRole, RoleQuery, ParaObjects.Csr>
        {}
    }
}