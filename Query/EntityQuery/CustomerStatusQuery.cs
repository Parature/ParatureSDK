namespace ParatureSDK.Query.EntityQuery
{
    public class CustomerStatusQuery : ParaQuery
    {
        protected override void BuildModuleSpecificFilter()
        {
            PageSize = 250;
        }
    }
}