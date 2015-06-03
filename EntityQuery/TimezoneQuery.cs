using ParatureSDK.ModuleQuery;

namespace ParatureSDK.EntityQuery
{
    public class TimezoneQuery : ParaEntityQuery
    {
        protected override void BuildModuleSpecificFilter()
        {
            PageSize = 250;
        }
    }
}