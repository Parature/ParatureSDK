using System;
using System.Xml;
using ParatureSDK.ParaObjects;

namespace ParatureSDK.XmlToObjectParser
{
    /// <summary>
    /// This class takes care of parsing the different XMLs returned when sending an attachment.
    /// </summary>
    internal partial class AttachmentParser
    {
        static internal string AttachmentGetUrlToPost(XmlDocument doc)
        {
            if (doc != null && doc.DocumentElement.HasAttribute("href"))
            {
                return doc.DocumentElement.Attributes["href"].InnerText.ToString();
            }
            else
            {
                throw new Exception("Could not locate the URL in " + doc == null ? "null document" : doc.OuterXml.ToString());
            }
        }

        static internal Attachment AttachmentFill(XmlDocument doc)
        {
            Attachment attachment = new Attachment();

            if (doc.DocumentElement.ChildNodes[0].LocalName.ToLower() == "passed")
            {
                attachment.HasException = false;
            }
            else
            {
                attachment.HasException = true;
            }

            XmlNode file = doc.DocumentElement.ChildNodes[0].ChildNodes[0];
            foreach (XmlNode node in file.ChildNodes)
            {
                if (node.LocalName.ToLower() == "filename")
                {
                    attachment.Name = node.InnerText;
                }
                else if (node.LocalName.ToLower() == "guid")
                {
                    attachment.GUID = node.InnerText;
                }
                else if (node.LocalName.ToLower() == "error")
                {
                    attachment.Error = node.InnerText;
                }
            }
            file = null;
            return attachment;
        }

    }
}