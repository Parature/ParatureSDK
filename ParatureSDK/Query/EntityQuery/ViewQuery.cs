using ParatureSDK.ParaObjects;
using System;

namespace ParatureSDK.Query.EntityQuery
{
    public class ViewQuery : ParaQuery
    {
        internal override Type QueryTargetType
        {
            get
            {
                return typeof(View);
            }
        }

        protected override void BuildModuleSpecificFilter()
        { }
    }
}