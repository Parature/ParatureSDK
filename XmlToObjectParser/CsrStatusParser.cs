using System;
using System.Xml;
using ParatureAPI.ParaObjects;

namespace ParatureAPI.XmlToObjectParser
{
    /// <summary>
    /// Handles all XML parsing logic needed for the Role object
    /// </summary>
    internal partial class CsrStatusParser
    {
        /// <summary>
        /// This methods requires a CsrStatus xml file and returns a CsrStatus object. It should only by used for a retrieve operation.
        /// </summary>
        /// <param name="xmlresp"></param>
        /// <returns></returns>
        static internal CsrStatus CsrStatusFill(XmlDocument xmlresp)
        {
            CsrStatus CsrStatus = new CsrStatus();
            XmlNode CsrStatusNode = xmlresp.DocumentElement;
            CsrStatus = CsrStatusFillNode(CsrStatusNode);
            return CsrStatus;
        }
        /// <summary>
        /// This method requires a CsrStatus list xml file and returns a CsrStatus list object. It should only by used for a List operation.
        /// </summary>
        /// <param name="xmlresp"></param>
        /// <returns></returns>
        static internal ParaEntityList<ParaObjects.CsrStatus> CsrStatusFillList(XmlDocument xmlresp)
        {
            var CsrStatusList = new ParaEntityList<ParaObjects.CsrStatus>();
            XmlNode DocNode = xmlresp.DocumentElement;

            CsrStatusList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());

            if (DocNode.Attributes["page-size"] != null)
            {
                // If this is a "TotalOnly" request, there are no other attributes than "Total"
                CsrStatusList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                CsrStatusList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                CsrStatusList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
            }

            foreach (XmlNode xn in DocNode.ChildNodes)
            {
                CsrStatusList.Data.Add(CsrStatusFillNode(xn));
            }
            return CsrStatusList;
        }

        /// <summary>
        /// This method accepts a CsrStatus node and parses through the different items in it. it can be used to parse a CsrStatus node, whether the node is returned from a simple read, or as part of a list call.
        /// </summary>
        static internal CsrStatus CsrStatusFillNode(XmlNode CsrStatusNode)
        {

            CsrStatus CsrStatus = new CsrStatus();
            CsrStatus.StatusID = Int64.Parse(CsrStatusNode.Attributes["id"].InnerText.ToString());

            foreach (XmlNode child in CsrStatusNode.ChildNodes)
            {
                if (child.LocalName.ToLower() == "name")
                {
                    CsrStatus.Name = ParserUtils.NodeGetInnerText(child);
                }
            }
            return CsrStatus;
        }
    }
}