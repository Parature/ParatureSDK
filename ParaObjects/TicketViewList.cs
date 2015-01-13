using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public class TicketViewList : PagedData.PagedData
    {
        public List<TicketView> views = new List<TicketView>();

    }
}