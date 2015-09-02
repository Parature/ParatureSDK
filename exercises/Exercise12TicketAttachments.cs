using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParatureSDK.ParaObjects;
using Action = ParatureSDK.ParaObjects.Action;
using ParatureSDK;

namespace Exercises
{
    class Exercise12TicketAttachments
    {
        static ParaService Service { get; set; }

        public Exercise12TicketAttachments()
        {
            Service = new ParaService(CredentialProvider.Creds);
        }

        /// <summary>
        /// Retrieve the attachments on a ticket
        /// </summary>
        /// <param name="ticketId">Id of the ticket to get available actions for</param>
        /// <returns>List of ticket attachments</returns>
        public static List<Attachment> GetTicketAttachments(long ticketId)
        {
            return Service.GetDetails<Ticket>(ticketId).Ticket_Attachments;
        }

        public static void AddAttachment(Ticket ticket, string fileName, string fileContents)
        {
            ticket.AddAttachment(Service, fileContents, "text/plain", fileName);
            var response = Service.Update(ticket);
        }

        /// <summary>
        /// Demonstrates how to delete individual attachments. 
        /// </summary>
        /// <param name="ticket"></param>
        public static void DeleteAllAttachments(Ticket ticket)
        {
            if (ticket.Ticket_Attachments != null)
            {
                var ticketGuids = ticket.Ticket_Attachments.Select(att => att.Guid).ToList();
                foreach (var guid in ticketGuids)
                {
                    //Must use this method to ensure the last attachment gets deleted properly
                    //You can modify the list directly to delete attachments, 
                    //  but deleting the last attachment is equivalent to deleting all attachments.
                    //  We add a check to prevent this from happening accidentally
                    ticket.DeleteAttachment(guid);
                }

                //remove the attachments on the server
                var response = Service.Update(ticket);
            }
        }

        /// <summary>
        /// Convenience method to remove all attachments for the ticket on the server
        /// </summary>
        /// <param name="ticket"></param>
        public static void DeleteAllAttachmentsBulk(Ticket ticket)
        {
            ticket.DeleteAllAttachments();

            var response = Service.Update(ticket);
        }
    }
}
