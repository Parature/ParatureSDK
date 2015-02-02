namespace ParatureAPI.EntityQuery
{
    public partial class RoleQuery : ParaQuery
    {
        protected override void buildModuleSpecificFilter()
        {
            PageSize = 250;
        }
    }
}