using System;
using System.Xml;
using ParatureSDK.ParaObjects;

namespace ParatureSDK.XmlToObjectParser
{
    /// <summary>
    /// Handles all XML parsing logic needed for the SLA object
    /// </summary>
    internal partial class SlaParser
    {
        /// <summary>
        /// This methods requires an Sla xml file and returns a Sla object. It should only by used for a retrieve operation.
        /// </summary>
        static internal Sla SlaFill(XmlDocument xmlresp)
        {
            Sla Sla = new Sla();
            XmlNode SlaNode = xmlresp.DocumentElement;
            Sla = SlaFillNode(SlaNode);
            return Sla;
        }

        /// <summary>
        /// This method requires an Sla list xml file and returns a Sla object. It should only by used for a List operation.
        /// </summary>
        static internal ParaEntityList<ParaObjects.Sla> SlasFillList(XmlDocument xmlresp)
        {
            var SlasList = new ParaEntityList<ParaObjects.Sla>();
            XmlNode DocNode = xmlresp.DocumentElement;

            SlasList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());


            if (DocNode.Attributes["page-size"] != null)
            {
                // If this is a "TotalOnly" request, there are no other attributes than "Total"
                SlasList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                SlasList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                SlasList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
            }


            foreach (XmlNode xn in DocNode.ChildNodes)
            {
                SlasList.Data.Add(SlaFillNode(xn));
            }
            return SlasList;
        }

        /// <summary>
        /// This method accepts a DownloadFolder node and parses through the different items in it. it can be used to parse a DownloadFolder node, whether the node is returned from a simple read, or as part of a list call.
        /// </summary>
        static internal Sla SlaFillNode(XmlNode SlaNode)
        {

            Sla Sla = new Sla();
            Sla.SlaID = Int64.Parse(SlaNode.Attributes["id"].InnerText.ToString());

            foreach (XmlNode child in SlaNode.ChildNodes)
            {
                if (child.LocalName.ToLower() == "name")
                {
                    Sla.Name = ParserUtils.NodeGetInnerText(child);
                }
            }
            return Sla;
        }
    }
}