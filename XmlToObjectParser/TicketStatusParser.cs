using System;
using System.Xml;
using ParatureSDK.ParaObjects;

namespace ParatureSDK.XmlToObjectParser
{
    internal partial class TicketStatusParser
    {
        /// <summary>
        /// This methods requires a Ticket Status xml file and returns a Ticket Status object. It should only by used for a retrieve operation.
        /// </summary>
        static internal TicketStatus TicketStatusFill(XmlDocument xmlresp)
        {
            TicketStatus ticketStatus = new TicketStatus();
            XmlNode ticketStatusNode = xmlresp.DocumentElement;
            ticketStatus = TicketStatusFillNode(ticketStatusNode);
            return ticketStatus;
        }

        /// <summary>
        /// This method requires a Ticket Status list xml file and returns a Ticket Status object. It should only by used for a List operation.
        /// </summary>
        static internal ParaEntityList<ParaObjects.TicketStatus> TicketStatusFillList(XmlDocument xmlresp)
        {
            var ticketStatusList = new ParaEntityList<ParaObjects.TicketStatus>();
            XmlNode DocNode = xmlresp.DocumentElement;


            ticketStatusList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());

            if (DocNode.Attributes["page-size"] != null)
            {
                // If this is a "TotalOnly" request, there are no other attributes than "Total"
                ticketStatusList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                ticketStatusList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                ticketStatusList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
            }

            foreach (XmlNode xn in DocNode.ChildNodes)
            {
                ticketStatusList.Data.Add(TicketStatusFillNode(xn));
            }
            return ticketStatusList;
        }

        /// <summary>
        /// This method accepts a Csr node and parses through the different items in it. it can be used to parse a Csr node, whether the node is returned from a simple read, or as part of a list call.
        /// </summary>
        static internal TicketStatus TicketStatusFillNode(XmlNode TicketStatusNode)
        {

            TicketStatus TicketStatus = new TicketStatus();
            TicketStatus.StatusID = Int64.Parse(TicketStatusNode.Attributes["id"].InnerText.ToString());
            TicketStatus.StatusType = (ParaEnums.TicketStatusType)Enum.Parse(typeof(ParaEnums.TicketStatusType), TicketStatusNode.Attributes["status-type"].InnerText.ToString());

            foreach (XmlNode child in TicketStatusNode.ChildNodes)
            {
                if (child.LocalName.ToLower() == "customer_text")
                {
                    TicketStatus.Customer_Text = ParserUtils.NodeGetInnerText(child);
                }
                if (child.LocalName.ToLower() == "name")
                {
                    TicketStatus.Name = ParserUtils.NodeGetInnerText(child);
                }
            }
            return TicketStatus;
        }
    }
}