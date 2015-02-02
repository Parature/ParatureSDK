namespace ParatureAPI.EntityQuery
{
    public partial class CustomerStatusQuery : ParaQuery
    {
        protected override void buildModuleSpecificFilter()
        {
            PageSize = 250;
        }
    }
}