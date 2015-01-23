using System;
using System.Xml;
using ParatureAPI.ParaObjects;

namespace ParatureAPI.XmlToObjectParser
{
    internal partial class AccountViewParser
    {

        /// <summary>
        /// This method requires a View list xml file and returns a ViewList object. It should only by used for a List operation.
        /// </summary>
        static internal ParaEntityList<ParaObjects.View> ViewFillList(XmlDocument xmlresp)
        {
            var viewList = new ParaEntityList<ParaObjects.View>();
            XmlNode DocNode = xmlresp.DocumentElement;

            viewList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());

            if (DocNode.Attributes["page-size"] != null)
            {
                // If this is a "TotalOnly" request, there are no other attributes than "Total"
                viewList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                viewList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                viewList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
            }

            foreach (XmlNode xn in DocNode.ChildNodes)
            {
                viewList.Data.Add(ViewFillNode(xn));
            }
            return viewList;
        }

        /// <summary>
        /// This method accepts a Queue node and parses through the different items in it. it can be used to parse a Csr node, whether the node is returned from a simple read, or as part of a list call.
        /// </summary>
        static internal View ViewFillNode(XmlNode QueueNode)
        {
            var view = new View();
            view.ID = Int32.Parse(QueueNode.Attributes["id"].InnerText.ToString());

            foreach (XmlNode child in QueueNode.ChildNodes)
            {
                if (child.LocalName.ToLower() == "name")
                {
                    view.Name = ParserUtils.NodeGetInnerText(child);
                }
            }
            return view;
        }
    }
}