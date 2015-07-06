using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParatureSDK.ParaObjects;
using ApiHandler = ParatureSDK.ApiHandler;
using Action = ParatureSDK.ParaObjects.Action;

namespace Exercises
{
    class Exercise11TicketActions
    {
        /// <summary>
        /// The actions that can be performed on a ticket depend on the current State.
        /// Retrieve the ticket to get a list of Actions available. This ensures no invalid transition is requested.
        /// </summary>
        /// <param name="ticketId">Id of the ticket to get available actions for</param>
        /// <returns></returns>
        public static List<Action> GetAvailableTicketActions(long ticketId)
        {
            var ticketResponse = ApiHandler.Ticket.GetDetails(ticketId, CredentialProvider.Creds, false);
            
            return ticketResponse.Actions;
        }

        /// <summary>
        /// Run an action on a ticket.
        /// If you need to add an attachment, run the AttachmentToAction method first!
        /// </summary>
        /// <param name="ticketId">Id of the ticket</param>
        /// <param name="action">Action object, which should be one of the Actions retrieved in GetAvailableTicketActions.</param>
        /// <returns></returns>
        public static ApiCallResponse RunAction(long ticketId, Action action)
        {
            var actionResponse = ApiHandler.Ticket.ActionRun(ticketId, action, CredentialProvider.Creds);
            return actionResponse;
        }

        /// <summary>
        /// Before running an action, attachments need to be uploaded to the server and a GUID retrieved
        /// This requires an API call first, and must occur before the action is run
        /// The GUID returned is temporary, and will expire after a while
        /// </summary>
        /// <param name="action"></param>
        public static void AddAttachmentToAction(Action action)
        {
            action.AddAttachment(CredentialProvider.Creds, "Contents of the file", "text/plain", "attachment1.txt");
        }
    }
}
