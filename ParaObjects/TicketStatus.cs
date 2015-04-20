using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects
{
    public class TicketStatus : Status
    {
        public string Customer_Text;
        [XmlAttribute("status-type")]
        public string StatusType;


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