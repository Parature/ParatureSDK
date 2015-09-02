using ParatureSDK.ParaObjects;
using System;

namespace ParatureSDK.Query.EntityQuery
{
    public class CsrStatusQuery : ParaQuery
    {
        internal override Type QueryTargetType
        {
            get
            {
                return typeof(Status);
            }
        }

        protected override void BuildModuleSpecificFilter()
        {
            PageSize = 250;
        }
    }
}