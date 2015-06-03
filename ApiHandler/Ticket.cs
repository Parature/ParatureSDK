using System;
using System.Collections;
using System.Threading;
using System.Xml;
using ParatureSDK.ApiHandler.ApiMethods;
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
    public class Ticket : FirstLevelApiMethods<ParaObjects.Ticket, TicketQuery>
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
        public static ParaObjects.Ticket GetDetails(Int64 ticketNumber, ParaCredentials pc, bool includeHistory)
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
            return ApiUtils.UploadFile(_module, pc, attachment, contentType, fileName);
        }
        
        internal static Attachment AddAttachment(ParaCredentials pc, string text, string contentType, string fileName)
        {
            return ApiUtils.UploadFile(_module, pc, text, contentType, fileName);
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
            var ar = ApiUtils.ActionRun(ticketId, action, pc, _module);
            return ar;
        }

        public class Status : SecondLevelApiMethods<ParaObjects.Status, StatusQuery, ParaObjects.Ticket>
        {}

        public class View : SecondLevelApiMethods<ParaObjects.View, ViewQuery, ParaObjects.Ticket>
        {}
    }
}