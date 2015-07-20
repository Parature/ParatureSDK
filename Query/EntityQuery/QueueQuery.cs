using ParatureSDK.ParaObjects;
using ParatureSDK.Query.ModuleQuery;
using System;

namespace ParatureSDK.Query.EntityQuery
{
    public class QueueQuery : ParaEntityQuery
    {
        internal override Type QueryTargetType
        {
            get
            {
                return typeof(Queue);
            }
        }

        protected override void BuildModuleSpecificFilter()
        { }
    }
}