using System;

namespace ParatureSDK.ParaObjects
{
    public class Queue : ParaEntity
    {
        // Specific properties for this module
        public string Name { get; set; }

        public Queue()
        {
            Name = "";
        }

        public Queue(Queue queue)
        {
            Id = queue.Id;
            Name = queue.Name;
        }

        public override string GetReadableName()
        {
            return Name;
        }
    }
}