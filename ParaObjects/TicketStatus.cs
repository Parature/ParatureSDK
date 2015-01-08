namespace ParatureAPI.ParaObjects
{
    public partial class TicketStatus : Status
    {
        public string Customer_Text = "";
        public ParaEnums.TicketStatusType StatusType = ParaEnums.TicketStatusType.All;


        public TicketStatus()
        {
        }

        public TicketStatus(TicketStatus ticketStatus)
            : base(ticketStatus)
        {
            this.StatusType = ticketStatus.StatusType;
            this.Customer_Text = ticketStatus.Customer_Text;
        }
    }
}