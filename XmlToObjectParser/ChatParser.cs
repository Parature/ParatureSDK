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
            chat = ParaEntityParser.EntityFill<ParaObjects.Chat>(xmlresp);
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
                var xDoc = new XmlDocument();
                xDoc.LoadXml(xn.OuterXml);
                //ChatsList.Data.Add(ChatFillNode(xn, MinimalisticLoad, childDepth, includeTranscripts, ParaCredentials));
                ChatsList.Data.Add(ParaEntityParser.EntityFill<ParaObjects.Chat>(xDoc));
            }
            return ChatsList;
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