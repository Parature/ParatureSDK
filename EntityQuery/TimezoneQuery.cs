namespace ParatureAPI.EntityQuery
{
    public partial class TimezoneQuery : ParaQuery
    {
        protected override void buildModuleSpecificFilter()
        {
            PageSize = 250;
        }
    }
}