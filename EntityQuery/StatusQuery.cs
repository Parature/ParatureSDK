namespace ParatureAPI.EntityQuery
{
    public class StatusQuery : ParaQuery
    {
        protected override void BuildModuleSpecificFilter()
        {
            PageSize = 250;
        }
    }
}