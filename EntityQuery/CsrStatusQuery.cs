namespace ParatureSDK.EntityQuery
{
    public class CsrStatusQuery : ParaQuery
    {
        protected override void BuildModuleSpecificFilter()
        {
            PageSize = 250;
        }
    }
}