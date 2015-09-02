using ParatureSDK.ParaObjects;
using ParatureSDK.Query.ModuleQuery;
using System;

namespace ParatureSDK.Query.EntityQuery
{
    public class TimezoneQuery : ParaEntityQuery
    {
        internal override Type QueryTargetType
        {
            get
            {
                return typeof(Timezone);
            }
        }

        protected override void BuildModuleSpecificFilter()
        {
            PageSize = 250;
        }
    }
}