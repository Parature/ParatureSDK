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