using System;
using System.Collections;
using System.Xml;
using ParatureSDK.EntityQuery;
using ParatureSDK.ModuleQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Csr module.
    /// </summary>
    public class Csr : FirstLevelApiHandler<ParaObjects.Csr, CsrQuery>
    {
        private static ParaEnums.ParatureModule _module = ParaEnums.ParatureModule.Csr;

        /// <summary>
        /// Contains all the methods needed to work with the Ticket statuses.
        /// </summary>
        public class Status : SecondLevelApiEntity<ParaObjects.Status, StatusQuery>
        {
            private static ParaEnums.ParatureEntity _entityType = ParaEnums.ParatureEntity.status;
            private static ParaEnums.ParatureModule _moduleType = ParaEnums.ParatureModule.Csr;
        }

        public class Role : SecondLevelApiEntity<ParaObjects.CsrRole, RoleQuery>
        {
            private static ParaEnums.ParatureEntity _entityType = ParaEnums.ParatureEntity.role;
            private static ParaEnums.ParatureModule _moduleType = ParaEnums.ParatureModule.Csr;
        }
    }
}