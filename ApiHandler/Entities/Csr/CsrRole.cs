using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ParatureSDK.EntityQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler.Entities.Csr
{
    public class CsrRole
    {
        private static ParaEnums.ParatureEntity _entityType = ParaEnums.ParatureEntity.role;
        private static ParaEnums.ParatureModule _moduleType = ParaEnums.ParatureModule.Csr;

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
        public static ParaObjects.CsrRole GetDetails(Int64 id, ParaCredentials creds)
        {
            var entity = ApiUtils.FillDetails<ParaObjects.CsrRole>(id, creds, _entityType);
            return entity;
        }

        /// <summary>
        /// Returns an role object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="xml">
        /// The Role XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaObjects.CsrRole GetDetails(XmlDocument xml)
        {
            var entity = ParaEntityParser.EntityFill<ParaObjects.CsrRole>(xml);

            return entity;
        }

        /// <summary>
        /// Returns an role list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="listXml">
        /// The Role List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<ParaObjects.CsrRole> GetList(XmlDocument listXml)
        {
            var list = ParaEntityParser.FillList<ParaObjects.CsrRole>(listXml);

            list.ApiCallResponse.XmlReceived = listXml;

            return list;
        }

        /// <summary>
        /// Get the List of Roles from within your Parature license
        /// </summary>
        /// <param name="creds"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static ParaEntityList<ParaObjects.CsrRole> GetList(ParaCredentials creds, RoleQuery query, ParaEnums.ParatureModule module)
        {
            return ApiUtils.FillList<ParaObjects.CsrRole>(creds, query, _entityType, _moduleType);
        }

        public static ParaEntityList<ParaObjects.CsrRole> GetList(ParaCredentials creds, ParaEnums.ParatureModule module)
        {
            return ApiUtils.FillList<ParaObjects.CsrRole>(creds, new RoleQuery(), _entityType, _moduleType);
        }
    }
}
