using System;
using System.Xml;
using ParatureAPI.EntityQuery;
using ParatureAPI.ParaObjects;
using ParatureAPI.XmlToObjectParser;

namespace ParatureAPI.ApiHandler.Entities
{
    public class CustomerStatus
    {
        ///  <summary>
        ///  Returns an Customer object with all of its properties filled.
        ///  </summary>
        /// <param name="customerStatusId"></param>
        /// <param name="paraCredentials">
        ///  The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        ///  </param>               
        public static ParaObjects.CustomerStatus CustomerStatusGetDetails(Int64 customerStatusId, ParaCredentials paraCredentials)
        {
            ParaObjects.CustomerStatus CustomerStatus = new ParaObjects.CustomerStatus();
            CustomerStatus = CustomerStatusFillDetails(customerStatusId, paraCredentials);
            return CustomerStatus;
        }

        /// <summary>
        /// Get the list of Customers from within your Parature license.
        /// </summary>
        public static ParaEntityList<ParaObjects.CustomerStatus> CustomerStatusGetList(ParaCredentials paraCredentials)
        {
            return CustomerStatusFillList(paraCredentials, new CustomerStatusQuery());
        }

        /// <summary>
        /// Get the list of Customers from within your Parature license.
        /// </summary>
        public static ParaEntityList<ParaObjects.CustomerStatus> CustomerStatusGetList(ParaCredentials paraCredentials, CustomerStatusQuery query)
        {
            return CustomerStatusFillList(paraCredentials, query);
        }

        /// <summary>
        /// Returns an CustomerStatus object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="customerStatusXml">
        /// The CustomerStatus XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaObjects.CustomerStatus CustomerStatusGetDetails(XmlDocument customerStatusXml)
        {
            ParaObjects.CustomerStatus CustomerStatus = new ParaObjects.CustomerStatus();
            CustomerStatus = CustomerStatusParser.CustomerStatusFill(customerStatusXml);

            CustomerStatus.ApiCallResponse.xmlReceived = customerStatusXml;
            CustomerStatus.ApiCallResponse.Objectid = CustomerStatus.StatusID;

            return CustomerStatus;
        }

        /// <summary>
        /// Returns an CustomerStatus list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="customerStatusListXml">
        /// The CustomerStatus List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<ParaObjects.CustomerStatus> CustomerStatusGetList(XmlDocument customerStatusListXml)
        {
            var CustomerStatussList = new ParaEntityList<ParaObjects.CustomerStatus>();
            CustomerStatussList = CustomerStatusParser.CustomerStatusFillList(customerStatusListXml);

            CustomerStatussList.ApiCallResponse.xmlReceived = customerStatusListXml;

            return CustomerStatussList;
        }

        /// <summary>
        /// Fills a Sla list object.
        /// </summary>
        private static ParaEntityList<ParaObjects.CustomerStatus> CustomerStatusFillList(ParaCredentials paraCredentials, CustomerStatusQuery query)
        {

            var CustomerStatusList = new ParaEntityList<ParaObjects.CustomerStatus>();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectSecondLevelGetList(paraCredentials, ParaEnums.ParatureModule.Customer, ParaEnums.ParatureEntity.status, query.BuildQueryArguments());
            if (ar.HasException == false)
            {
                CustomerStatusList = CustomerStatusParser.CustomerStatusFillList(ar.xmlReceived);
            }
            CustomerStatusList.ApiCallResponse = ar;





            return CustomerStatusList;
        }

        private static ParaObjects.CustomerStatus CustomerStatusFillDetails(Int64 customerStatusId, ParaCredentials paraCredentials)
        {
            ParaObjects.CustomerStatus CustomerStatus = new ParaObjects.CustomerStatus();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetDetail(paraCredentials, ParaEnums.ParatureEntity.CustomerStatus, customerStatusId);
            if (ar.HasException == false)
            {
                CustomerStatus = CustomerStatusParser.CustomerStatusFill(ar.xmlReceived);
            }
            else
            {

                CustomerStatus.StatusID = 0;
            }

            return CustomerStatus;
        }
    }
}