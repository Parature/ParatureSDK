using System;
using System.Xml;
using ParatureSDK.EntityQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler.Entities
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
        public static ParaObjects.Status CustomerStatusGetDetails(Int64 customerStatusId, ParaCredentials paraCredentials)
        {
            ParaObjects.Status CustomerStatus;
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
        public static ParaObjects.Status CustomerStatusGetDetails(XmlDocument customerStatusXml)
        {
            var customerStatus = ParaEntityParser.EntityFill<ParaObjects.CustomerStatus>(customerStatusXml);

            customerStatus.ApiCallResponse.XmlReceived = customerStatusXml;
            customerStatus.ApiCallResponse.Id = customerStatus.Id;

            return customerStatus;
        }

        /// <summary>
        /// Returns an CustomerStatus list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="customerStatusListXml">
        /// The CustomerStatus List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<ParaObjects.CustomerStatus> CustomerStatusGetList(XmlDocument customerStatusListXml)
        {
            var customerStatussList = ParaEntityParser.FillList<ParaObjects.CustomerStatus>(customerStatusListXml);

            customerStatussList.ApiCallResponse.XmlReceived = customerStatusListXml;

            return customerStatussList;
        }

        /// <summary>
        /// Fills a Sla list object.
        /// </summary>
        private static ParaEntityList<ParaObjects.CustomerStatus> CustomerStatusFillList(ParaCredentials paraCredentials, CustomerStatusQuery query)
        {

            var customerStatusList = new ParaEntityList<ParaObjects.CustomerStatus>();
            var ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectSecondLevelGetList(paraCredentials, ParaEnums.ParatureModule.Customer, ParaEnums.ParatureEntity.status, query.BuildQueryArguments());
            if (ar.HasException == false)
            {
                customerStatusList = ParaEntityParser.FillList<ParaObjects.CustomerStatus>(ar.XmlReceived);
            }
            customerStatusList.ApiCallResponse = ar;

            return customerStatusList;
        }

        private static ParaObjects.Status CustomerStatusFillDetails(Int64 customerStatusId, ParaCredentials paraCredentials)
        {
            var customerStatus = new ParaObjects.Status();
            var ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetDetail(paraCredentials, ParaEnums.ParatureEntity.CustomerStatus, customerStatusId);
            if (ar.HasException == false)
            {
                customerStatus = ParaEntityParser.EntityFill<ParaObjects.CustomerStatus>(ar.XmlReceived);
            }
            else
            {

                customerStatus.Id = 0;
            }

            return customerStatus;
        }
    }
}