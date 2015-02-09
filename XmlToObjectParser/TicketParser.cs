using System;
using System.Collections.Generic;
using System.Xml;
using ParatureSDK.ParaObjects;
using ParatureSDK.ParaObjects.EntityReferences;
using Action = ParatureSDK.ParaObjects.Action;
using Product = ParatureSDK.ApiHandler.Product;

namespace ParatureSDK.XmlToObjectParser
{
    /// <summary>
    /// This class helps parse raw XML responses returned from the server to hard typed Ticket objects that you can use for further processing.
    /// </summary>
    internal class TicketParser
    {
        /// <summary>
        /// This methods requires a ticket xml file and returns an ticket object. It should only by used for a retrieve operation.
        /// </summary>
        static internal ParaObjects.Ticket TicketFill(XmlDocument xmlresp, int requestdepth, bool includeAllCustomFields, ParaCredentials ParaCredentials)
        {
            ParaObjects.Ticket Ticket = new ParaObjects.Ticket();
            XmlNode TicketNode = xmlresp.DocumentElement;

            // Setting up the request level for all child items of an account.
            int childDepth = 0;
            if (requestdepth > 0)
            {
                childDepth = requestdepth - 1;
            }
            Ticket = ParaEntityParser.EntityFill<Ticket>(xmlresp);
            Ticket.FullyLoaded = true;
            return Ticket;
        }

        /// <summary>
        /// This methods requires a Ticket list xml file and returns a TicketsList oject. It should only by used for a List operation.
        /// </summary>
        static internal ParaEntityList<ParaObjects.Ticket> TicketsFillList(XmlDocument xmlresp, Boolean MinimalisticLoad, int requestdepth, ParaCredentials ParaCredentials)
        {
            var TicketsList = new ParaEntityList<ParaObjects.Ticket>();
            XmlNode DocNode = xmlresp.DocumentElement;

            int childDepth = 0;
            if (requestdepth > 0)
            {
                childDepth = requestdepth - 1;
            }

            TicketsList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());

            if (DocNode.Attributes["page-size"] != null)
            {
                // If this is a "TotalOnly" request, there are no other attributes than "Total"
                TicketsList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                TicketsList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
                TicketsList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
            }


            foreach (XmlNode xn in DocNode.ChildNodes)
            {
                var xDoc = new XmlDocument();
                xDoc.LoadXml(xn.OuterXml);
                //TicketsList.Data.Add(TicketFillNode(xn, childDepth, MinimalisticLoad, ParaCredentials));
                TicketsList.Data.Add(ParaEntityParser.EntityFill<Ticket>(xDoc));
            }
            return TicketsList;
        }
    }
}