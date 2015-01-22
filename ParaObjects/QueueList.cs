using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public class QueueList : PagedData.PagedData
    {
        public List<Queue> Queues = new List<Queue>();
    }
}