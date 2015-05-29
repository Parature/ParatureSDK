using System;
using System.Collections;
using System.Threading;
using System.Xml;
using ParatureSDK.EntityQuery;
using ParatureSDK.ModuleQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;
using Action = ParatureSDK.ParaObjects.Action;

namespace ParatureSDK.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Ticket module.
    /// </summary>
    public class Ticket : FirstLevelApiHandler<ParaObjects.Ticket>
    {
        private static ParaEnums.ParatureModule _module = ParaEnums.ParatureModule.Ticket;

        /// <summary>
        /// Returns a Ticket object with all of its details.
        /// </summary>
        /// <param name="includeHistory">
        /// Indicates whether or not to return the Ticket action history. Please keep in mind that the action history for certain tickets 
        /// can be very large, and therefore might slow down the operation.
        /// </param> 
        /// <param name="ticketNumber">
        ///The Ticket number that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="pc">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>
        public static ParaObjects.Ticket GetDetails(Int64 ticketNumber, bool includeHistory, ParaCredentials pc)
        {
            var arl = new ArrayList();
            if (includeHistory)
            {
                arl.Add("_history_=true");
            }

            var ticket = GetDetails(ticketNumber, pc, arl);
            return ticket;
        }

        internal static Attachment AddAttachment(ParaCredentials pc, Byte[] attachment, string contentType, string fileName)
        {
            return ApiUtils.UploadFile(ParaEnums.ParatureModule.Ticket, pc, attachment, contentType, fileName);
        }
        
        internal static Attachment AddAttachment(ParaCredentials pc, string text, string contentType, string fileName)
        {
            return ApiUtils.UploadFile(ParaEnums.ParatureModule.Ticket, pc, text, contentType, fileName);
        }

        /// <summary>
        /// Assigns a Ticket to a specific Queue.
        /// </summary>
        /// <param name="ticketId">
        /// The Ticket you would like to run this action on.
        /// </param>
        /// <param name="action">
        /// The action object your would like to run on this ticket.
        /// </param>
        /// <param name="pc">
        /// Your credentials object.
        /// </param>
        /// <returns></returns>
        public static ApiCallResponse ActionRun(Int64 ticketId, Action action, ParaCredentials pc)
        {
            var ar = ApiUtils.ActionRun(ticketId, action, pc, ParaEnums.ParatureModule.Ticket);
            return ar;
        }

        public static class Status
        {
            private static ParaEnums.ParatureEntity _entityType = ParaEnums.ParatureEntity.status;
            private static ParaEnums.ParatureModule _moduleType = ParaEnums.ParatureModule.Ticket;

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
            private static ParaEnums.ParatureModule _moduleType = ParaEnums.ParatureModule.Ticket;

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