using System;
using System.Xml;
using ParatureSDK.ParaObjects;

namespace ParatureSDK.XmlToObjectParser
{
    /// <summary>
    /// This class takes care of parsing the different XMLs returned when sending an attachment.
    /// </summary>
    internal class AttachmentParser
    {
        static internal string AttachmentGetUrlToPost(XmlDocument doc)
        {
            if (doc != null && doc.DocumentElement.HasAttribute("href"))
            {
                return doc.DocumentElement.Attributes["href"].InnerText;
            }
            else
            {
                throw new Exception(doc.OuterXml);
            }
        }
    }
}