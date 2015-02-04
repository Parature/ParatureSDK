using System;

namespace ParatureSDK.ParaObjects
{
    public class Queue
    {
        // Specific properties for this module

        private Int32 _QueueID = 0;

        public Int32 QueueID
        {
            get { return _QueueID; }
            set { _QueueID = value; }
        }
        private string _Name = "";

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public Queue()
        {
        }

        public Queue(Queue queue)
        {
            QueueID = queue.QueueID;
            Name = queue.Name;
        }

    }
}