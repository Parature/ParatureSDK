namespace ParatureAPI.EntityQuery
{
    public partial class CsrStatusQuery : ParaQuery
    {
        protected override void buildModuleSpecificFilter()
        {
            PageSize = 250;
        }
    }
}