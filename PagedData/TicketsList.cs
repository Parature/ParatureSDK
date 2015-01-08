using System.Collections.Generic;
using ParatureAPI.ParaObjects;

namespace ParatureAPI.PagedData
{
    /// <summary>
    /// Instantiate this class to hold the result set of a list call to APIs. Whenever you need to get a list of 
    /// Tickets
    /// </summary>
    public class TicketsList : PagedData
    {
        /// <summary>
        /// The collection of Tickets objects returned.
        /// </summary>
        public List<Ticket> Tickets = new List<Ticket>();

        public TicketsList()
        {
        }

        public TicketsList(TicketsList ticketsList)
            : base(ticketsList)
        {
            Tickets = new List<Ticket>(ticketsList.Tickets);
        }

    }
}