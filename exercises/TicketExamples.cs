using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParatureSDK;
using ParatureSDK.Query.ModuleQuery;

namespace Exercises
{
    class TicketExamples
    {
        /// <summary>
        /// Retrieve all tickets from a department of Parature minus archived and deleted tickets.
        /// </summary>
        /// <param name="creds"></param>
        /// <returns></returns>
        public static List<ParatureSDK.ParaObjects.Ticket> GetAllTickets(ParaCredentials creds)
        {
            var tq = new TicketQuery();
            tq.RetrieveAllRecords = true;
            var tickets = ParatureSDK.ApiHandler.Ticket.GetList(creds, tq);

            if (tickets.ApiCallResponse.HasException)
            {
                throw new Exception(tickets.ApiCallResponse.ExceptionDetails);
            }

            return tickets.ToList();
        }

        /// <summary>
        /// Retrieve all tickets that belong to an organization minus archived and deleted tickets
        /// </summary>
        /// <param name="creds"></param>
        /// <param name="accountId">Account ID of the organization. Not to be confused with Instance ID. Use the Account APIs to determine the account ID</param>
        /// <returns></returns>
        public static List<ParatureSDK.ParaObjects.Ticket> GetAllTicketsForAccount(ParaCredentials creds, long accountId)
        {
            var tq = new TicketQuery();
            //Add an account filter: https://support.parature.com/public/doc/api.html#ticket-list
            tq.AddStaticFieldFilter(TicketQuery.TicketStaticFields.Account, ParaEnums.QueryCriteria.Equal, accountId.ToString());
            tq.RetrieveAllRecords = true;
            var tickets = ParatureSDK.ApiHandler.Ticket.GetList(creds, tq);

            if (tickets.ApiCallResponse.HasException)
            {
                throw new Exception(tickets.ApiCallResponse.ExceptionDetails);
            }

            return tickets.ToList();
        }
    }
}
