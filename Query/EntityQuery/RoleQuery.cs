using ParatureSDK.ParaObjects;
using System;

namespace ParatureSDK.Query.EntityQuery
{
    public class RoleQuery : ParaQuery
    {
        internal override Type QueryTargetType
        {
            get
            {
                return typeof(Role);
            }
        }

        protected override void BuildModuleSpecificFilter()
        {
            PageSize = 250;
        }
    }
}