using System;
using System.Xml;
using ParatureSDK.EntityQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler.Entities.Customer
{
    public class CustomerStatus
    {
        private static ParaEnums.ParatureEntity _entityType = ParaEnums.ParatureEntity.status;
        private static ParaEnums.ParatureModule _moduleType = ParaEnums.ParatureModule.Customer;

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
        public static ParaObjects.CustomerStatus GetDetails(Int64 id, ParaCredentials creds)
        {
            var status = ApiUtils.FillDetails<ParaObjects.CustomerStatus>(id, creds, _entityType);
            return status;
        }

        /// <summary>
        /// Returns an Status object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="xml">
        /// The Status XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaObjects.CustomerStatus GetDetails(XmlDocument xml)
        {
            var status = ParaEntityParser.EntityFill<ParaObjects.CustomerStatus>(xml);

            return status;
        }

        /// <summary>
        /// Returns an Status list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="listXml">
        /// The Status List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<ParaObjects.CustomerStatus> GetList(XmlDocument listXml)
        {
            var statusList = ParaEntityParser.FillList<ParaObjects.CustomerStatus>(listXml);

            statusList.ApiCallResponse.XmlReceived = listXml;

            return statusList;
        }

        /// <summary>
        /// Get the List of Statuss from within your Parature license
        /// </summary>
        /// <param name="creds"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static ParaEntityList<ParaObjects.CustomerStatus> GetList(ParaCredentials creds, StatusQuery query, ParaEnums.ParatureModule module)
        {
            return ApiUtils.FillList<ParaObjects.CustomerStatus>(creds, query, _entityType, _moduleType);
        }

        public static ParaEntityList<ParaObjects.CustomerStatus> GetList(ParaCredentials creds, ParaEnums.ParatureModule module)
        {
            return ApiUtils.FillList<ParaObjects.CustomerStatus>(creds, new StatusQuery(), _entityType, _moduleType);
        }
    }
}