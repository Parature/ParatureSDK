using System;
using System.Collections;
using System.Xml;
using ParatureSDK.ApiHandler.ApiMethods;
using ParatureSDK.ParaObjects;
using ParatureSDK.Query.EntityQuery;
using ParatureSDK.Query.ModuleQuery;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Csr module.
    /// </summary>
    public class Csr : FirstLevelApiMethods<ParaObjects.Csr, CsrQuery>
    {
        /// <summary>
        /// Contains all the methods needed to work with the Csr statuses.
        /// </summary>
        public class Status : SecondLevelApiMethods<ParaObjects.Status, StatusQuery, ParaObjects.Csr>
        {}

        /// <summary>
        /// Contains all the methods for retrieving Csr Roles.
        /// </summary>
        public class Role : SecondLevelApiMethods<ParaObjects.CsrRole, RoleQuery, ParaObjects.Csr>
        {}
    }
}