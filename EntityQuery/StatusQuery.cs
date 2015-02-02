namespace ParatureAPI.EntityQuery
{
    public partial class StatusQuery : ParaQuery
    {
        protected override void buildModuleSpecificFilter()
        {
            PageSize = 250;
        }
    }
}