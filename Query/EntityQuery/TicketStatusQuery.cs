namespace ParatureSDK.Query.EntityQuery
{
    public class TicketStatusQuery : ParaQuery
    {
        protected override void BuildModuleSpecificFilter()
        {
            PageSize = 250;
        }
    }
}