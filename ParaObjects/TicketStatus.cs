namespace ParatureAPI.ParaObjects
{
    public class TicketStatus : Status
    {
        public string Customer_Text = "";
        public ParaEnums.TicketStatusType StatusType = ParaEnums.TicketStatusType.All;


        public TicketStatus()
        {
        }

        public TicketStatus(TicketStatus ticketStatus)
            : base(ticketStatus)
        {
            StatusType = ticketStatus.StatusType;
            Customer_Text = ticketStatus.Customer_Text;
        }
    }
}