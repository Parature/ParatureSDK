using System;

namespace ParatureSDK.ParaObjects
{
    public class Queue
    {
        // Specific properties for this module

        public int QueueId { get; set; }

        public string Name { get; set; }

        public Queue()
        {
            Name = "";
            QueueId = 0;
        }

        public Queue(Queue queue)
        {
            QueueId = queue.QueueId;
            Name = queue.Name;
        }

    }
}