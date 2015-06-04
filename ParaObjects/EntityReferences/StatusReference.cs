using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParatureSDK.ParaObjects.EntityReferences
{
    public class StatusReference : EntityReference<Status>
    {
        public Status Status
        {
            get { return base.Entity; }
            set { base.Entity = value; }
        }
    }
}
