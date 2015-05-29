using System;
using System.Collections;
using System.Threading;
using System.Xml;
using ParatureSDK.EntityQuery;
using ParatureSDK.ModuleQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Customer module.
    /// </summary>
    public class Customer : FirstLevelApiHandler<ParaObjects.Customer>
    {
        private static ParaEnums.ParatureModule _module = ParaEnums.ParatureModule.Customer;

        /// <summary>
        /// Creates a Parature Customer. Requires an Object and a credentials object. Need to specific whether to include a customer notification for the insert, as well as if the password should be included in the Notificaiton. 
        /// Will return the Newly Created Customerid. Returns 0 if the Customer creation fails.
        /// </summary>
        public static ApiCallResponse Insert(ParaObjects.Customer customer, ParaCredentials ParaCredentials, bool NotifyCustomer, bool IncludePasswordInNotification)
        {
            // Extra arguments for the customer module
            var arguments = new ArrayList
            {
                "_notify_=" + NotifyCustomer.ToString().ToLower()
            };

            // Only if the user selects to notify the customer that we include the password.
            if (NotifyCustomer)
            {
                // This is currently a bug. Needs to be uncommented when fixed.
                arguments.Add("_includePassword_=" + IncludePasswordInNotification.ToString().ToLower());
            }

            var doc = XmlGenerator.GenerateXml(customer);
            var ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, ParaEnums.ParatureModule.Customer, doc, 0, arguments);
            customer.Id = ar.Id;
            return ar;
        }

        /// <summary>
        /// Updates a Parature Customer. Requires an Object and a credentials object. Need to specific whether to include a customer notification for the insert, as well as if the password should be included in the Notificaiton. Will return the updated Customerid. Returns 0 if the Customer update operation fails.
        /// </summary>
        public static ApiCallResponse Update(ParaObjects.Customer Customer, ParaCredentials ParaCredentials, bool NotifyCustomer, bool IncludePasswordInNotification)
        {
            // Extra arguments for the customer module
            var arguments = new ArrayList();
            arguments.Add("_notify_=" + NotifyCustomer.ToString().ToLower());
            // Only if the user selects to notify the customer that we include the password.
            if (NotifyCustomer == true)
            {
                //arguments.Add("_include_password_=" + IncludePasswordInNotification.ToString().ToLower());
            }
            var ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, ParaEnums.ParatureModule.Customer, XmlGenerator.GenerateXml(Customer), Customer.Id, arguments);
            return ar;
        }

        public static class Role
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
                var entity = ApiUtils.ApiGetEntity<ParaObjects.CustomerRole>(creds, _entityType, id);
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
            public static ParaEntityList<ParaObjects.CustomerRole> GetList(ParaCredentials creds, RoleQuery query)
            {
                return ApiUtils.ApiGetEntityList<ParaObjects.CustomerRole>(creds, query, _moduleType, _entityType);
            }

            public static ParaEntityList<ParaObjects.CustomerRole> GetList(ParaCredentials creds)
            {
                return ApiUtils.ApiGetEntityList<ParaObjects.CustomerRole>(creds, new RoleQuery(), _moduleType, _entityType);
            }
        }

        public static class Status
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

        public static class View
        {
            private static ParaEnums.ParatureEntity _entityType = ParaEnums.ParatureEntity.view;
            private static ParaEnums.ParatureModule _moduleType = ParaEnums.ParatureModule.Customer;

            /// <summary>
            /// Returns a view object with all of its properties filled.
            /// </summary>
            /// <param name="id">
            /// The view number that you would like to get the details of.
            /// Value Type: <see cref="long" />   (System.Int64)
            ///</param>
            /// <param name="creds">
            /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            /// </param>
            /// <returns></returns>
            public static ParaObjects.View GetDetails(Int64 id, ParaCredentials creds)
            {
                var entity = ApiUtils.ApiGetEntity<ParaObjects.View>(creds, _entityType, id);
                return entity;
            }

            /// <summary>
            /// Returns an view object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="xml">
            /// The view XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.View GetDetails(XmlDocument xml)
            {
                var entity = ParaEntityParser.EntityFill<ParaObjects.View>(xml);

                return entity;
            }

            /// <summary>
            /// Returns an view list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="listXml">
            /// The view List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaEntityList<ParaObjects.View> GetList(XmlDocument listXml)
            {
                var list = ParaEntityParser.FillList<ParaObjects.View>(listXml);

                list.ApiCallResponse.XmlReceived = listXml;

                return list;
            }

            /// <summary>
            /// Get the List of views from within your Parature license
            /// </summary>
            /// <param name="creds"></param>
            /// <param name="query"></param>
            /// <returns></returns>
            public static ParaEntityList<ParaObjects.View> GetList(ParaCredentials creds, ViewQuery query)
            {
                return ApiUtils.ApiGetEntityList<ParaObjects.View>(creds, query, _moduleType, _entityType);
            }

            public static ParaEntityList<ParaObjects.View> GetList(ParaCredentials creds)
            {
                return ApiUtils.ApiGetEntityList<ParaObjects.View>(creds, new ViewQuery(), _moduleType, _entityType);
            }
        }

    }
}