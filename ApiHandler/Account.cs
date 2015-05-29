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
        private static ParaEnums.ParatureModule _module = ParaEnums.ParatureModule.Account;

        public class View : SecondLevelApiEntity<ParaObjects.View, ViewQuery>
        {
            private static ParaEnums.ParatureEntity _entityType = ParaEnums.ParatureEntity.view;
            private static ParaEnums.ParatureModule _moduleType = ParaEnums.ParatureModule.Account;
        }
    }
}