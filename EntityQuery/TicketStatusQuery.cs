namespace ParatureAPI.EntityQuery
{
    public partial class TicketStatusQuery : ParaQuery
    {
        protected override void buildModuleSpecificFilter()
        {
            PageSize = 250;
        }
    }
}