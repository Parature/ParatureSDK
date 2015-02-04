using System;
using System.Collections.Generic;
using System.Xml;
using ParatureSDK.ParaObjects;
using Chat = ParatureSDK.ApiHandler.Chat;

namespace ParatureSDK.XmlToObjectParser
{
    /// <summary>
    /// This class helps parse raw XML responses returned from the server to hard typed Chat objects that you can use for further processing.
    /// </summary>
    internal class ChatParser
    {
        /// <summary>
        /// This methods requires a Chat xml file and returns a customer object. It should only by used for a retrieve operation.
        /// </summary>
        static internal ParaObjects.Chat ChatFill(XmlDocument xmlresp, Boolean MinimalisticLoad, int requestdepth, bool includeTranscripts, ParaCredentials ParaCredentials)
        {
            ParaObjects.Chat chat = new ParaObjects.Chat();
            XmlNode ObjectNode = xmlresp.DocumentElement;

            // Setting up the request level for all child items of an account.
            int childDepth = 0;
            if (requestdepth > 0)
            {
                childDepth = requestdepth - 1;
            }
            chat = ChatFillNode(ObjectNode,MinimalisticLoad, childDepth, includeTranscripts, ParaCredentials);
            chat.FullyLoaded = true;
            return chat;
        }

        /// <summary>
        /// This methods requires a Chat list xml file and returns a ChatsList oject. It should only by used for a List operation.
        /// </summary>
        static internal ParaEntityList<ParaObjects.Chat> ChatsFillList(XmlDocument xmlresp, Boolean MinimalisticLoad, bool includeTranscripts, int requestdepth, ParaCredentials ParaCredentials)
        {
            var ChatsList = new ParaEntityList<ParaObjects.Chat>();
            XmlNode DocNode = xmlresp.DocumentElement;

            // Setting up the request level for all child items of an account.
            int childDepth = 0;
            if (requestdepth > 0)
            {
                childDepth = requestdepth - 1;
            }




            ChatsList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());


            if (DocNode.Attributes["page-size"] != null)
            {
                // If this is a "TotalOnly" request, there are no other attributes than "Total"

                ChatsList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                ChatsList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                ChatsList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
            }



            foreach (XmlNode xn in DocNode.ChildNodes)
            {
                ChatsList.Data.Add(ChatFillNode(xn, MinimalisticLoad, childDepth, includeTranscripts, ParaCredentials));
            }
            return ChatsList;
        }

        /// <summary>
        /// This methods accepts a Chat node and parse through the different items in it. it can be used to parse a Chat node, whether the node is returned from a simple read, or as part of a list call.
        /// </summary>
        static internal ParaObjects.Chat ChatFillNode(XmlNode ChatNode, Boolean MinimalisticLoad, int childDepth, bool includeTranscripts, ParaCredentials ParaCredentials)
        {

            ParaObjects.Chat chat = new ParaObjects.Chat();
            bool isSchema = false;
            if (ChatNode.Attributes["id"] != null)
            {
                isSchema = false;
                chat.Id = Int64.Parse(ChatNode.Attributes["id"].InnerText.ToString());
                chat.uniqueIdentifier = chat.Id;
            }
            else
            {
                isSchema = true;
            }

            if (ChatNode.Attributes["service-desk-uri"] != null)
            {
                chat.serviceDeskUri = ChatNode.Attributes["service-desk-uri"].InnerText.ToString();
            }

            foreach (XmlNode child in ChatNode.ChildNodes)
            {
                if (isSchema == false)
                {

                    if (child.LocalName.ToLower() == "browser_language")
                    {
                        chat.Browser_Language = ParserUtils.NodeGetInnerText(child);
                    }


                    if (child.LocalName.ToLower() == "browser_type")
                    {
                        chat.Browser_Type = ParserUtils.NodeGetInnerText(child);
                    }


                    if (child.LocalName.ToLower() == "browser_version")
                    {
                        chat.Browser_Version = ParserUtils.NodeGetInnerText(child);
                    }


                    if (child.LocalName.ToLower() == "chat_number")
                    {
                        long num;
                        Int64.TryParse(ParserUtils.NodeGetInnerText(child), out num);
                        chat.Chat_Number = num;
                    }

                    if (child.LocalName.ToLower() == "customer")
                    {
                        // Fill the Customer details
                        ParaObjects.Customer Customer = new ParaObjects.Customer();

                        chat.Customer = CustomerParser.CustomerFillNode(child.ChildNodes[0], childDepth, true, ParaCredentials);
                        if (childDepth > 0)
                        {
                            chat.Customer = ApiHandler.Customer.GetDetails(chat.Customer.Id, ParaCredentials, (ParaEnums.RequestDepth)childDepth);
                        }
                        chat.Customer.FullyLoaded = ParserUtils.ObjectFullyLoaded(childDepth);
                    }

                    if (child.LocalName.ToLower() == "email")
                    {
                        chat.Email = ParserUtils.NodeGetInnerText(child);
                    }

                    if (child.LocalName.ToLower() == "date_created")
                    {
                        chat.Date_Created = DateTime.Parse(ParserUtils.NodeGetInnerText(child));
                    }

                    if (child.LocalName.ToLower() == "date_ended")
                    {
                        chat.Date_Ended = DateTime.Parse(ParserUtils.NodeGetInnerText(child));
                    }

                    if (child.LocalName.ToLower() == "initial_csr")
                    {
                        if (child.ChildNodes[0] != null && child.ChildNodes[0].Attributes["id"] != null)
                        {
                            chat.Initial_Csr.Id = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value.ToString());
                            chat.Initial_Csr.Full_Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                        }
                    }

                    if (child.LocalName.ToLower() == "ip_address")
                    {
                        chat.Ip_Address = ParserUtils.NodeGetInnerText(child);
                    }

                    if (child.LocalName.ToLower() == "is_anonymous")
                    {
                        bool anon;
                        Boolean.TryParse(ParserUtils.NodeGetInnerText(child), out anon);
                        chat.Is_Anonymous = anon;
                    }

                    if (child.LocalName.ToLower() == "referrer_url")
                    {
                        chat.Referrer_Url = ParserUtils.NodeGetInnerText(child);
                    }

                    if (child.LocalName.ToLower() == "related_tickets")
                    {
                        if (child.ChildNodes[0] != null && child.ChildNodes[0].Attributes["id"] != null)
                        {
                            Int32 counter = 0;
                            chat.Related_Tickets = new List<ParaObjects.Ticket>();

                            while (child.ChildNodes[counter] != null && child.ChildNodes[counter].Attributes["id"] != null)
                            {
                                ParaObjects.Ticket ticket = new ParaObjects.Ticket();

                                ticket.Id = Int32.Parse(child.ChildNodes[counter].Attributes["id"].Value.ToString());
                                ticket.Ticket_Number = child.ChildNodes[counter].ChildNodes[0].InnerText.ToString();

                                chat.Related_Tickets.Add(ticket);

                                counter++;
                            }
                        }
                    }

                    if (child.LocalName.ToLower() == "sla_violations")
                    {
                        var vio = Int32.MinValue;
                        Int32.TryParse(ParserUtils.NodeGetInnerText(child), out vio);
                        chat.Sla_Violation = vio;
                    }
                        
                    if (child.LocalName.ToLower() == "status")
                    {
                        if (child.ChildNodes[0] != null && child.ChildNodes[0].Attributes["id"] != null)
                        {
                            chat.Status.StatusID = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value.ToString());
                            chat.Status.Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                        }
                    }

                    if (child.LocalName.ToLower() == "summary")
                    {
                        chat.Summary = ParserUtils.NodeGetInnerText(child);
                    }
                    if (child.LocalName.ToLower() == "user_agent")
                    {
                        chat.User_Agent = ParserUtils.NodeGetInnerText(child);
                    }

                }

                if (child.LocalName.ToLower() == "custom_field")
                {
                    chat.Fields.Add(CommonParser.FillCustomField(MinimalisticLoad, child));
                }
            }

            if (includeTranscripts && chat.Id>0)
            {
                // Load transcripts
                chat.ChatTranscripts = Chat.ChatTranscripts(chat.Id, ParaCredentials);
            }

            return chat;
        }

        static internal List<ChatTranscript> ChatTranscriptsFillList(XmlDocument ChatTranscriptDoc)
        {
            List<ChatTranscript> transcripts = new List<ChatTranscript>();

            XmlNode ChatTranscriptNode = ChatTranscriptDoc.DocumentElement;

            foreach (XmlNode xn in ChatTranscriptNode.ChildNodes)
            {
                ChatTranscript transcript = new ChatTranscript();
                transcript.IsInternal = true;
                if (xn.Attributes["internal"] != null)
                {
                    Boolean.TryParse(xn.Attributes["internal"].InnerText.ToString(), out transcript.IsInternal);
                }
                foreach (XmlNode xnc in xn.ChildNodes)
                {
                    // Looking at each message nodes
                    switch (xnc.LocalName.ToLower())
                    {
                        case "system":
                            transcript.Performer = ParaEnums.ActionHistoryPerformerType.System;
                            break;

                        case "customer":
                            transcript.Performer = ParaEnums.ActionHistoryPerformerType.Customer;
                            transcript.CustomerName = ParserUtils.NodeGetInnerText(xnc);
                            break;
                        case "csr":
                            transcript.Performer = ParaEnums.ActionHistoryPerformerType.Csr;
                            transcript.CsrName = ParserUtils.NodeGetInnerText(xnc);
                            break;

                        case "text":
                            transcript.Text = ParserUtils.NodeGetInnerText(xnc);
                            break;
                        case "timestamp":
                            DateTime.TryParse(ParserUtils.NodeGetInnerText(xnc), out transcript.Timestamp);
                            break;
                    }   
                }

                transcripts.Add(transcript);
            }

            return transcripts;
        }
    }
}