using System;
using System.Collections.Generic;
using System.Xml;
using ParatureAPI.PagedData;
using ParatureAPI.ParaObjects;
using Action = ParatureAPI.ParaObjects.Action;
using Product = ParatureAPI.ApiHandler.Product;

namespace ParatureAPI.XmlToObjectParser
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
            Ticket = TicketFillNode(TicketNode, childDepth, includeAllCustomFields, ParaCredentials);
            Ticket.FullyLoaded = true;
            return Ticket;
        }

        /// <summary>
        /// This methods requires a Ticket list xml file and returns a TicketsList oject. It should only by used for a List operation.
        /// </summary>
        static internal TicketsList TicketsFillList(XmlDocument xmlresp, Boolean MinimalisticLoad, int requestdepth, ParaCredentials ParaCredentials)
        {
            TicketsList TicketsList = new TicketsList();
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
                TicketsList.Tickets.Add(TicketFillNode(xn, childDepth, MinimalisticLoad, ParaCredentials));
            }
            return TicketsList;
        }

        /// <summary>
        /// This methods accepts a ticket node and parse through the different items in it. it can be used to parse a ticket node, whether the node is returned from a simple read, or as part of a list call.
        /// </summary>
        static internal ParaObjects.Ticket TicketFillNode(XmlNode Node, int childDepth, bool MinimalisticLoad, ParaCredentials ParaCredentials)
        {

            ParaObjects.Ticket Ticket = new ParaObjects.Ticket();
            bool isSchema = false;
            if (Node.Attributes["id"] != null)
            {
                Ticket.Id = Int64.Parse(Node.Attributes["id"].InnerText.ToString());
                Ticket.uniqueIdentifier = Ticket.Id;
                Ticket.Ticket_Parent = new ParaObjects.Ticket();
                isSchema = false;
            }
            else
            {
                isSchema = true;
            }

            if (Node.Attributes["service-desk-uri"] != null)
            {
                Ticket.serviceDeskUri = Node.Attributes["service-desk-uri"].InnerText.ToString();
            }

            foreach (XmlNode child in Node.ChildNodes)
            {
                if (isSchema == false)
                {
                    if (child.LocalName.ToLower() == "ticket_status")
                    {
                        Ticket.Ticket_Status.StatusID = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value);
                        Ticket.Ticket_Status.Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                    }
                    if (child.LocalName.ToLower() == "ticket_number")
                    {
                        Ticket.Ticket_Number = ParserUtils.NodeGetInnerText(child);
                    }

                    //Take care of the attachments.
                    if (child.LocalName.ToLower() == "ticket_attachments")
                    {
                        Ticket.Ticket_Attachments = CommonParser.FillAttachments(child);
                    }

                    if (child.LocalName.ToLower() == "entered_by")
                    {
                        Ticket.Entered_By.Id = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value);
                        Ticket.Entered_By.Full_Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                    }

                    if (child.LocalName.ToLower() == "assigned_to")
                    {
                        Ticket.Assigned_To.Id = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value);
                        Ticket.Assigned_To.Full_Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                    }
                    if (child.LocalName.ToLower() == "email_notification")
                    {
                        Ticket.Email_Notification = Boolean.Parse(ParserUtils.NodeGetInnerText(child));
                    }

                    // DJERAME
                    if (child.LocalName.ToLower() == "email_notification_additional_contact")
                    {
                        Ticket.Email_Notification_Additional_Contact = Boolean.Parse(ParserUtils.NodeGetInnerText(child));
                    }

                    if (child.LocalName.ToLower() == "hide_from_customer")
                    {
                        Ticket.Hide_From_Customer = Boolean.Parse(ParserUtils.NodeGetInnerText(child));
                    }

                    if (child.LocalName.ToLower() == "tou")
                    {
                        Ticket.Email_Notification = Boolean.Parse(ParserUtils.NodeGetInnerText(child));
                    }

                    if (child.LocalName.ToLower() == "date_created")
                    {
                        Ticket.Date_Created = DateTime.Parse(ParserUtils.NodeGetInnerText(child));
                    }
                    if (child.LocalName.ToLower() == "date_updated")
                    {
                        Ticket.Date_Updated = DateTime.Parse(ParserUtils.NodeGetInnerText(child));
                    }
                    if (child.LocalName.ToLower() == "cc_customer")
                    {
                        // List of CCed Customers

                        string result = ParserUtils.NodeGetInnerText(child);
                        if (String.IsNullOrEmpty(result) == false)
                        {
                            Ticket.Cc_Customer = ParserUtils.ParseStringCollection(result);
                        }
                    }
                    if (child.LocalName.ToLower() == "cc_csr")
                    {
                        // List of CCed CSRs

                        string result = ParserUtils.NodeGetInnerText(child);
                        if (String.IsNullOrEmpty(result) == false)
                        {
                            Ticket.Cc_Csr = ParserUtils.ParseStringCollection(result);
                        }
                    }

                    if (child.LocalName.ToLower() == "ticket_customer")
                    {
                        // Fill the Customer details
                        ParaObjects.Customer Customer = new ParaObjects.Customer();

                        Ticket.Ticket_Customer = CustomerParser.CustomerFillNode(child.ChildNodes[0], childDepth, MinimalisticLoad, ParaCredentials);
                        if (childDepth > 0)
                        {
                            Ticket.Ticket_Customer = ApiHandler.Customer.CustomerGetDetails(Ticket.Ticket_Customer.Id, ParaCredentials, (ParaEnums.RequestDepth)childDepth);
                        }
                        Ticket.Ticket_Customer.FullyLoaded = ParserUtils.ObjectFullyLoaded(childDepth);
                    }


                    if (child.LocalName.ToLower() == "additional_contact")
                    {
                        // Fill the Customer details
                        ParaObjects.Customer Customer = new ParaObjects.Customer();

                        Ticket.Additional_Contact = CustomerParser.CustomerFillNode(child.ChildNodes[0], childDepth, MinimalisticLoad, ParaCredentials);
                        if (childDepth > 0)
                        {
                            Ticket.Additional_Contact = ApiHandler.Customer.CustomerGetDetails(Ticket.Additional_Contact.Id, ParaCredentials, (ParaEnums.RequestDepth)childDepth);
                        }
                        Ticket.Additional_Contact.FullyLoaded = ParserUtils.ObjectFullyLoaded(childDepth);
                    }
                        
                    if (child.LocalName.ToLower() == "ticket_parent")
                    {
                        // Fill the Parent Ticket details
                        string result = child.ChildNodes[0].Attributes["href"].InnerText.ToString();
                        if (String.IsNullOrEmpty(result) == false)
                        {
                            ParaObjects.Ticket PTicket = new ParaObjects.Ticket();

                            if (child.ChildNodes[0].HasChildNodes && child.ChildNodes[0].ChildNodes[0].LocalName.ToLower() == "ticket_number")
                            {
                                PTicket.Ticket_Number = ParserUtils.NodeGetInnerText(child.ChildNodes[0].ChildNodes[0]);
                            }

                            char[] splitter = { '/' };
                            String[] hrefs;
                            hrefs = result.Split(splitter);
                            PTicket.Id = Int64.Parse(child.ChildNodes[0].Attributes["id"].InnerText.ToString());
                            PTicket.Department.Id = Int64.Parse(hrefs[hrefs.Length - 3]);
                            if (childDepth > 0)
                            {
                                PTicket = ApiHandler.Ticket.TicketGetDetails(PTicket.Id, false, ParaCredentials, (ParaEnums.RequestDepth)childDepth);
                            }
                            PTicket.Ticket_Number = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                            PTicket.FullyLoaded = ParserUtils.ObjectFullyLoaded(childDepth);
                            Ticket.Ticket_Parent = PTicket;
                        }

                    }
                    if (child.LocalName.ToLower() == "ticket_children")
                    {
                        List<ParaObjects.Ticket> Ticket_Children = new List<ParaObjects.Ticket>();

                        foreach (XmlNode ChildTicketNode in child.ChildNodes)
                        {
                            string result = ChildTicketNode.Attributes["href"].InnerText.ToString();
                                
                            ParaObjects.Ticket ChildTicket = new ParaObjects.Ticket();

                            if (ChildTicketNode.HasChildNodes && ChildTicketNode.ChildNodes[0].LocalName.ToLower() == "ticket_number")
                            {
                                ChildTicket.Ticket_Number = ParserUtils.NodeGetInnerText(ChildTicketNode.ChildNodes[0]);
                            }

                            char[] splitter = { '/' };
                            String[] hrefs;
                            hrefs = result.Split(splitter);
                            ChildTicket.Id = Int64.Parse(hrefs[hrefs.Length - 1]);
                            ChildTicket.Department.Id = Int64.Parse(hrefs[hrefs.Length - 3]);
                                
                            if (childDepth > 0)
                            {
                                ChildTicket = ApiHandler.Ticket.TicketGetDetails(ChildTicket.Id, false, ParaCredentials, (ParaEnums.RequestDepth)childDepth);
                            }
                            ChildTicket.FullyLoaded = ParserUtils.ObjectFullyLoaded(childDepth);
                            Ticket_Children.Add(ChildTicket);
                        }
                        Ticket.Ticket_Children = Ticket_Children;
                    }

                    if (child.LocalName.ToLower() == "related_chats")
                    {
                        var Related_Chats = new List<ParaObjects.Chat>();

                        foreach (XmlNode chatNode in child.ChildNodes)
                        {
                            var chat = new ParaObjects.Chat();
                            chat.Id = Int32.Parse(chatNode.Attributes["id"].Value);
                            chat.Chat_Number = Int32.Parse(chatNode.Attributes["id"].Value);
                            Related_Chats.Add(chat);
                        }

                        Ticket.Related_Chats = Related_Chats;
                    }

                    if (child.LocalName.ToLower() == "department")
                    {
                        Ticket.Department.Id = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value);
                        Ticket.Department.Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                    }

                    if (child.LocalName.ToLower() == "ticket_queue")
                    {
                        Ticket.Ticket_Queue.QueueID = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value);
                        Ticket.Ticket_Queue.Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                    }
                    if (child.LocalName.ToLower() == "ticket_asset")
                    {
                        if (childDepth > 0)
                        {
                            //TODO, Call Asset
                            Ticket.Ticket_Asset.Id = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value);
                            Ticket.Ticket_Asset.uniqueIdentifier = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value);
                        }
                        else
                        {
                            Ticket.Ticket_Asset.Id = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value);
                            Ticket.Ticket_Asset.uniqueIdentifier = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value);
                        }
                        Ticket.Ticket_Asset.FullyLoaded = ParserUtils.ObjectFullyLoaded(childDepth);
                    }

                    if (child.LocalName.ToLower() == "ticket_product")
                    {
                        Ticket.Ticket_Product.Id = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value);

                        if (childDepth > 0)
                        {
                            Ticket.Ticket_Product = Product.ProductGetDetails(Ticket.Ticket_Product.Id, ParaCredentials, (ParaEnums.RequestDepth)childDepth - 1);
                        }
                        Ticket.Ticket_Product.FullyLoaded = ParserUtils.ObjectFullyLoaded(childDepth);
                    }

                    if (child.LocalName.ToLower() == "ticket_sla")
                    {
                        if (child.ChildNodes[0] != null && child.ChildNodes[0].Attributes["id"] != null)
                        {
                            Ticket.Ticket_Sla.SlaID = Int64.Parse(child.ChildNodes[0].Attributes["id"].Value.ToString());
                            Ticket.Ticket_Sla.Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                        }
                    }

                    if (child.LocalName.ToLower() == "actions")
                    {

                        foreach (XmlNode actionchild in child.ChildNodes)
                        {
                            Action availableaction = new Action();
                            availableaction.Id = Int32.Parse(actionchild.Attributes["id"].Value);
                            availableaction.Name = actionchild.Attributes["name"].Value.ToString();
                            if (childDepth > 0)
                            {
                                // WHEN WE PARSE ACTIONS, RETURN THIS ONE.

                            }
                            availableaction.FullyLoaded = false;
                            Ticket.Actions.Add(availableaction);
                        }
                    }
                    if (child.LocalName.ToLower() == "actionhistory")
                    {
                        Ticket.ActionHistory = CommonParser.FillActionHistory(child);
                    }

                }


                if (child.LocalName.ToLower() == "custom_field")
                {
                    Ticket.Fields.Add(CommonParser.FillCustomField(false, child));
                }


            }
            return Ticket;
        }


        internal class TicketStatusParser
        {
            /// <summary>
            /// This methods requires a DownloadFolder xml file and returns a DownloadFolder object. It should only by used for a retrieve operation.
            /// </summary>
            static internal TicketStatus TicketStatusFill(XmlDocument xmlresp, ParaCredentials ParaCredentials)
            {
                TicketStatus ticketstatus = new TicketStatus();
                XmlNode MainNode = xmlresp.DocumentElement;

                ticketstatus = TicketStatusFillNode(MainNode, 0);

                return ticketstatus;
            }

            /// <summary>
            /// This method requires a DownloadFolder list xml file and returns a DownloadFoldersList object. It should only by used for a List operation.
            /// </summary>
            static internal TicketStatusList TicketStatusFillList(XmlDocument xmlresp, int requestdepth, ParaCredentials ParaCredentials)
            {
                TicketStatusList TicketStatusList = new TicketStatusList();
                XmlNode DocNode = xmlresp.DocumentElement;

                // Setting up the request level for all child items of a DownloadFolder.

                TicketStatusList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());

                if (DocNode.Attributes["page-size"] != null)
                {
                    // If this is a "TotalOnly" request, there are no other attributes than "Total"
                    TicketStatusList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                    TicketStatusList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                    TicketStatusList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
                }



                foreach (XmlNode xn in DocNode.ChildNodes)
                {
                    TicketStatusList.TicketStatuses.Add(TicketStatusFillNode(xn, 0));
                }
                return TicketStatusList;
            }

            /// <summary>
            /// This method accepts a DownloadFolder node and parses through the different items in it. it can be used to parse a DownloadFolder node, whether the node is returned from a simple read, or as part of a list call.
            /// </summary>
            static internal TicketStatus TicketStatusFillNode(XmlNode Node, int childDepth)
            {

                TicketStatus TicketStatus = new TicketStatus();
                TicketStatus.StatusID = Int64.Parse(Node.Attributes["id"].InnerText.ToString());
                TicketStatus.StatusType = (ParaEnums.TicketStatusType)Enum.Parse(typeof(ParaEnums.TicketStatusType), Node.Attributes["status-type"].InnerText.ToString());
                foreach (XmlNode child in Node.ChildNodes)
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
}