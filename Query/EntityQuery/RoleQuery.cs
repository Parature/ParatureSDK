namespace ParatureSDK.Query.EntityQuery
{
    public class RoleQuery : ParaQuery
    {
        protected override void BuildModuleSpecificFilter()
        {
            PageSize = 250;
        }
    }
}