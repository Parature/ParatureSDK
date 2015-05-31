using System;
using System.Threading;
using System.Xml;
using ParatureSDK.ApiHandler.ApiMethods;
using ParatureSDK.ModuleQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;
using Action = ParatureSDK.ParaObjects.Action;

namespace ParatureSDK.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Asset module.
    /// </summary>
    public class Asset : FirstLevelApiGetMethods<ParaObjects.Asset, AssetQuery>
    {
        private static ParaEnums.ParatureModule _module = ParaEnums.ParatureModule.Asset;

        /// <summary>
        /// Runs an Asset action.
        /// </summary>
        /// <param name="assetId">
        /// The Asset you would like to run this action on.
        /// </param>
        /// <param name="action">
        /// The action object your would like to run on this ticket.
        /// </param>
        /// <param name="creds">
        /// Your credentials object.
        /// </param>
        /// <returns></returns>
        public static ApiCallResponse ActionRun(Int64 assetId, Action action, ParaCredentials creds)
        {
            var ar = ApiUtils.ActionRun(assetId, action, creds, _module);
            return ar;
        }
    }
}