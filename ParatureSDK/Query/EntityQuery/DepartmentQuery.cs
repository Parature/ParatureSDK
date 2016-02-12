using System;
using ParatureSDK.Query.ModuleQuery;
using ParatureSDK.ParaObjects;

namespace ParatureSDK.Query.EntityQuery
{
    public class DepartmentQuery : ParaEntityQuery
    {
        internal override Type QueryTargetType
        {
            get
            {
                return typeof(Department);
            }
        }

        protected override void BuildModuleSpecificFilter()
        {}
    }
}