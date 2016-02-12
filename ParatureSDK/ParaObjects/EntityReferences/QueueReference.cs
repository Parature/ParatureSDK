using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParatureSDK.ParaObjects.EntityReferences
{
    public class QueueReference : EntityReference<Queue>
    {
        public Queue Queue
        {
            get { return base.Entity; }
            set { base.Entity = value; }
        }
    }
}
