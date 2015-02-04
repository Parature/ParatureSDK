namespace ParatureSDK.EntityQuery
{
    public class StatusQuery : ParaQuery
    {
        protected override void BuildModuleSpecificFilter()
        {
            PageSize = 250;
        }
    }
}