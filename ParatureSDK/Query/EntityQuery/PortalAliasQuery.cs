using System;
using ParatureSDK.Query.ModuleQuery;
using ParatureSDK.ParaObjects;

namespace ParatureSDK.Query.EntityQuery
{
    public class PortalAliasQuery : ParaEntityQuery
    {
        internal override Type QueryTargetType
        {
            get
            {
                return typeof(PortalAlias);
            }
        }

        protected override void BuildModuleSpecificFilter()
        {

        }

    }
}