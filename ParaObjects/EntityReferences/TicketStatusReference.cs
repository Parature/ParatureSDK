using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects.EntityReferences
{
    public class TicketStatusReference: StatusReference
    {
        /// <summary>
        /// Status of a ticket
        /// </summary>
        [XmlElement("Status", Namespace="Ticket") ]
        public TicketStatus TicketStatus { get; set; }
    }
}
