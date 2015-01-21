using System;
using System.Xml;
using ParatureAPI.ParaObjects;

namespace ParatureAPI.XmlToObjectParser
{
    /// <summary>
    /// Handles all XML parsing logic needed for the Role object
    /// </summary>
    internal partial class CustomerStatusParser
    {
        /// <summary>
        /// This methods requires a CustomerStatus xml file and returns a CustomerStatus object. It should only by used for a retrieve operation.
        /// </summary>
        /// <param name="xmlresp"></param>
        /// <returns></returns>
        static internal CustomerStatus CustomerStatusFill(XmlDocument xmlresp)
        {
            CustomerStatus CustomerStatus = new CustomerStatus();
            XmlNode CustomerStatusNode = xmlresp.DocumentElement;
            CustomerStatus = CustomerStatusFillNode(CustomerStatusNode);
            return CustomerStatus;
        }
        /// <summary>
        /// This method requires a CustomerStatus list xml file and returns a CustomerStatus list object. It should only by used for a List operation.
        /// </summary>
        /// <param name="xmlresp"></param>
        /// <returns></returns>
        static internal CustomerStatusList CustomerStatusFillList(XmlDocument xmlresp)
        {
            CustomerStatusList CustomerStatusList = new CustomerStatusList();
            XmlNode DocNode = xmlresp.DocumentElement;

            CustomerStatusList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());

            if (DocNode.Attributes["page-size"] != null)
            {
                // If this is a "TotalOnly" request, there are no other attributes than "Total"
                CustomerStatusList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                CustomerStatusList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                CustomerStatusList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
            }

            foreach (XmlNode xn in DocNode.ChildNodes)
            {
                CustomerStatusList.CustomerStatuses.Add(CustomerStatusFillNode(xn));
            }
            return CustomerStatusList;
        }

        /// <summary>
        /// This method accepts a CustomerStatus node and parses through the different items in it. it can be used to parse a CustomerStatus node, whether the node is returned from a simple read, or as part of a list call.
        /// </summary>
        static internal CustomerStatus CustomerStatusFillNode(XmlNode CustomerStatusNode)
        {

            CustomerStatus CustomerStatus = new CustomerStatus();
            CustomerStatus.StatusID = Int64.Parse(CustomerStatusNode.Attributes["id"].InnerText.ToString());

            foreach (XmlNode child in CustomerStatusNode.ChildNodes)
            {
                if (child.LocalName.ToLower() == "name")
                {
                    CustomerStatus.Name = ParserUtils.NodeGetInnerText(child);
                }
                else if (child.LocalName.ToLower() == "description")
                {
                    CustomerStatus.Description = ParserUtils.NodeGetInnerText(child);
                }
                else if (child.LocalName.ToLower() == "text")
                {
                    CustomerStatus.Text = ParserUtils.NodeGetInnerText(child);
                }
            }
            return CustomerStatus;
        }
    }
}