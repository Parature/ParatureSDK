using ParatureSDK.ParaObjects;
using System;

namespace ParatureSDK.Query.ModuleQuery
{
    /// <summary>
    /// Instantiate this class to build all the properties needed to get a list of Assets.
    /// The properties include the number of items per page, the page number, what custom fields to include in the list,
    /// as well as any filtering you need to do.
    /// </summary>
    public class AssetQuery : ParaEntityQuery
    {
        internal override Type QueryTargetType
        {
            get
            {
                return typeof(Asset);
            }
        }

        protected override void BuildModuleSpecificFilter()
        { }
    }
}