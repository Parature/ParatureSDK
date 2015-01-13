using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public class QueueList : PagedData.PagedData
    {
        public List<Queue> Queues = new List<Queue>();
        public QueueList()
        {
        }
        public QueueList(QueueList queueList)
            : base(queueList)
        {
            Queues = new List<Queue>(queueList.Queues);
        }

    }
}