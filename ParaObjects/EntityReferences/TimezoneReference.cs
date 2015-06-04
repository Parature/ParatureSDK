using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParatureSDK.ParaObjects.EntityReferences
{
    public class TimezoneReference : EntityReference<Timezone>
    {
        public Timezone Timezone
        {
            get { return base.Entity; }
            set { base.Entity = value; }
        }
    }
}
