using ParatureSDK.ParaObjects;
using ParatureSDK.Query.ModuleQuery;
using System;

namespace ParatureSDK.Query.EntityQuery
{
    public class SlaQuery : ParaEntityQuery
    {
        internal override Type QueryTargetType
        {
            get
            {
                return typeof(Sla);
            }
        }

        protected override void BuildModuleSpecificFilter()
        {

        }

    }
}