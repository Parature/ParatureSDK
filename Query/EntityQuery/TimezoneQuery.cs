using ParatureSDK.Query.ModuleQuery;

namespace ParatureSDK.Query.EntityQuery
{
    public class TimezoneQuery : ParaEntityQuery
    {
        protected override void BuildModuleSpecificFilter()
        {
            PageSize = 250;
        }
    }
}