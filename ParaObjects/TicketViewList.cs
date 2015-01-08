using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public partial class TicketViewList : PagedData.PagedData
    {
        public List<TicketView> views = new List<TicketView>();

    }
}