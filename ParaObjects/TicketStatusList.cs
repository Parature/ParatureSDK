using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public class TicketStatusList : PagedData.PagedData
    {
        public List<TicketStatus> TicketStatuses = new List<TicketStatus>();
    }
}