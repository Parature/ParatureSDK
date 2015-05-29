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
    public class Csr : FirstLevelApiHandler<ParaObjects.Csr>
    {
        private static ParaEnums.ParatureModule _module = ParaEnums.ParatureModule.Csr;

        /// <summary>
        /// Contains all the methods needed to work with the Ticket statuses.
        /// </summary>
        public static class Status
        {
            private static ParaEnums.ParatureEntity _entityType = ParaEnums.ParatureEntity.status;
            private static ParaEnums.ParatureModule _moduleType = ParaEnums.ParatureModule.Csr;

            /// <summary>
            /// Returns a Status object with all of its properties filled.
            /// </summary>
            /// <param name="id">
            /// The Status number that you would like to get the details of.
            /// Value Type: <see cref="Int64" />   (System.Int64)
            ///</param>
            /// <param name="creds">
            /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            /// </param>
            /// <returns></returns>
            public static ParaObjects.Status GetDetails(Int64 id, ParaCredentials creds)
            {
                var status = ApiUtils.ApiGetEntity<ParaObjects.Status>(creds, _entityType, id);
                return status;
            }

            /// <summary>
            /// Returns an Status object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="xml">
            /// The Status XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.Status GetDetails(XmlDocument xml)
            {
                var status = ParaEntityParser.EntityFill<ParaObjects.Status>(xml);

                return status;
            }

            /// <summary>
            /// Returns an Status list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="listXml">
            /// The Status List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaEntityList<ParaObjects.Status> GetList(XmlDocument listXml)
            {
                var statusList = ParaEntityParser.FillList<ParaObjects.Status>(listXml);

                statusList.ApiCallResponse.XmlReceived = listXml;

                return statusList;
            }

            /// <summary>
            /// Get the List of Statuss from within your Parature license
            /// </summary>
            /// <param name="creds"></param>
            /// <param name="query"></param>
            /// <returns></returns>
            public static ParaEntityList<ParaObjects.Status> GetList(ParaCredentials creds, StatusQuery query)
            {
                return ApiUtils.ApiGetEntityList<ParaObjects.Status>(creds, query, _moduleType, _entityType);
            }

            public static ParaEntityList<ParaObjects.Status> GetList(ParaCredentials creds)
            {
                return ApiUtils.ApiGetEntityList<ParaObjects.Status>(creds, new StatusQuery(), _moduleType, _entityType);
            }

        }

        public static class Role
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
                var entity = ApiUtils.ApiGetEntity<CsrRole>(creds, _entityType, id);
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
                var entity = ParaEntityParser.EntityFill<CsrRole>(xml);

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
                var list = ParaEntityParser.FillList<CsrRole>(listXml);

                list.ApiCallResponse.XmlReceived = listXml;

                return list;
            }

            /// <summary>
            /// Get the List of Roles from within your Parature license
            /// </summary>
            /// <param name="creds"></param>
            /// <param name="query"></param>
            /// <returns></returns>
            public static ParaEntityList<ParaObjects.CsrRole> GetList(ParaCredentials creds, RoleQuery query)
            {
                return ApiUtils.ApiGetEntityList<CsrRole>(creds, query, _moduleType, _entityType);
            }

            public static ParaEntityList<ParaObjects.CsrRole> GetList(ParaCredentials creds)
            {
                return ApiUtils.ApiGetEntityList<CsrRole>(creds, new RoleQuery(), _moduleType, _entityType);
            }
        }
    }
}