using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParatureSDK;
using ParatureSDK.Query.ModuleQuery;
using ParatureSDK.ParaObjects;

namespace Exercises
{
    static class TicketExamples
    {
        static ParaService Service { get; set; }

        static TicketExamples()
        {
            Service = new ParaService(CredentialProvider.Creds);
        }

        /// <summary>
        /// Retrieve all tickets from a department of Parature minus archived and deleted tickets.
        /// </summary>
        /// <param name="creds"></param>
        /// <returns></returns>
        public static List<Ticket> GetAllTickets(ParaCredentials creds)
        {
            var tq = new TicketQuery();
            tq.RetrieveAllRecords = true;
            var tickets = Service.GetList<Ticket>(tq);

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
            var tickets = Service.GetList<Ticket>(tq);

            if (tickets.ApiCallResponse.HasException)
            {
                throw new Exception(tickets.ApiCallResponse.ExceptionDetails);
            }

            return tickets.ToList();
        }
    }
}
