using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public class StatusList : PagedData.PagedData
    {
        public List<Status> Statuses = new List<Status>();
    }
}