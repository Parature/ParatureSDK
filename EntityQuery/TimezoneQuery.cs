namespace ParatureAPI.EntityQuery
{
    public class TimezoneQuery : ParaQuery
    {
        protected override void BuildModuleSpecificFilter()
        {
            PageSize = 250;
        }
    }
}