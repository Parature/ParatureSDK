using System;
using System.Xml;
using ParatureSDK.ParaObjects;

namespace ParatureSDK.XmlToObjectParser
{
    /// <summary>
    /// Handles all XML parsing logic needed for the Role object
    /// </summary>
    internal partial class StatusParser
    {
        /// <summary>
        /// This methods requires a Status xml file and returns a Status object. It should only by used for a retrieve operation.
        /// </summary>
        /// <param name="xmlresp"></param>
        /// <returns></returns>
        static internal Status StatusFill(XmlDocument xmlresp)
        {
            Status Status = new Status();
            XmlNode StatusNode = xmlresp.DocumentElement;
            Status = StatusFillNode(StatusNode);
            return Status;
        }
        /// <summary>
        /// This method requires a Status list xml file and returns a Status list object. It should only by used for a List operation.
        /// </summary>
        /// <param name="xmlresp"></param>
        /// <returns></returns>
        static internal ParaEntityList<ParaObjects.Status> StatusFillList(XmlDocument xmlresp)
        {
            var StatusList = new ParaEntityList<ParaObjects.Status>();
            XmlNode DocNode = xmlresp.DocumentElement;

            StatusList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());

            if (DocNode.Attributes["page-size"] != null)
            {
                // If this is a "TotalOnly" request, there are no other attributes than "Total"
                StatusList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                StatusList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                StatusList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
            }

            foreach (XmlNode xn in DocNode.ChildNodes)
            {
                StatusList.Data.Add(StatusFillNode(xn));
            }
            return StatusList;
        }

        /// <summary>
        /// This method accepts a Status node and parses through the different items in it. it can be used to parse a Status node, whether the node is returned from a simple read, or as part of a list call.
        /// </summary>
        static internal Status StatusFillNode(XmlNode StatusNode)
        {

            Status Status = new Status();
            Status.Id = Int64.Parse(StatusNode.Attributes["id"].InnerText.ToString());

            foreach (XmlNode child in StatusNode.ChildNodes)
            {
                if (child.LocalName.ToLower() == "name")
                {
                    Status.Name = ParserUtils.NodeGetInnerText(child);
                }
            }
            return Status;
        }
    }
}