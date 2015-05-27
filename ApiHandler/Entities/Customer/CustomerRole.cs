using System;
using System.Xml;
using ParatureSDK.EntityQuery;
using ParatureSDK.ModuleQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler.Entities.Customer
{
    public class CustomerRole
    {
        private static ParaEnums.ParatureEntity _entityType = ParaEnums.ParatureEntity.role;
        private static ParaEnums.ParatureModule _moduleType = ParaEnums.ParatureModule.Customer;

        /// <summary>
        /// Returns a Role object with all of its properties filled.
        /// </summary>
        /// <param name="id">
        /// The Role number that you would like to get the details of.
        /// Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="creds">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>
        /// <returns></returns>
        public static ParaObjects.CustomerRole GetDetails(Int64 id, ParaCredentials creds)
        {
            var entity = ApiUtils.FillDetails<ParaObjects.CustomerRole>(id, creds, _entityType);
            return entity;
        }

        /// <summary>
        /// Returns an role object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="xml">
        /// The Role XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaObjects.CustomerRole GetDetails(XmlDocument xml)
        {
            var entity = ParaEntityParser.EntityFill<ParaObjects.CustomerRole>(xml);

            return entity;
        }

        /// <summary>
        /// Returns an role list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="listXml">
        /// The Role List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<ParaObjects.CustomerRole> GetList(XmlDocument listXml)
        {
            var list = ParaEntityParser.FillList<ParaObjects.CustomerRole>(listXml);

            list.ApiCallResponse.XmlReceived = listXml;

            return list;
        }

        /// <summary>
        /// Get the List of Roles from within your Parature license
        /// </summary>
        /// <param name="creds"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static ParaEntityList<ParaObjects.CustomerRole> GetList(ParaCredentials creds, RoleQuery query, ParaEnums.ParatureModule module)
        {
            return ApiUtils.FillList<ParaObjects.CustomerRole>(creds, query, _entityType, _moduleType);
        }

        public static ParaEntityList<ParaObjects.CustomerRole> GetList(ParaCredentials creds, ParaEnums.ParatureModule module)
        {
            return ApiUtils.FillList<ParaObjects.CustomerRole>(creds, new RoleQuery(), _entityType, _moduleType);
        }
    }
}