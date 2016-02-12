using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParatureSDK.ParaObjects.EntityReferences
{
    public class SlaReference : EntityReference<Sla>
    {
        public Sla Sla
        {
            get { return base.Entity; }
            set { base.Entity = value; }
        }
    }
}
