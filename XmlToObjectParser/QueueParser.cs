using System;
using System.Xml;
using ParatureAPI.ParaObjects;

namespace ParatureAPI.XmlToObjectParser
{
    internal partial class QueueParser
    {
        /// <summary>
        /// This methods requires a Queue xml file and returns a Queue object. It should only by used for a retrieve operation.
        /// </summary>
        static internal Queue QueueFill(XmlDocument xmlresp)
        {
            Queue queue = new Queue();
            XmlNode queueNode = xmlresp.DocumentElement;
            queue = QueueFillNode(queueNode);
            return queue;
        }

        /// <summary>
        /// This method requires a Queue list xml file and returns a Queue object. It should only by used for a List operation.
        /// </summary>
        static internal ParaEntityList<ParaObjects.Queue> QueueFillList(XmlDocument xmlresp)
        {
            var queueList = new ParaEntityList<ParaObjects.Queue>();
            XmlNode DocNode = xmlresp.DocumentElement;


            queueList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());

            if (DocNode.Attributes["page-size"] != null)
            {
                // If this is a "TotalOnly" request, there are no other attributes than "Total"
                queueList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                queueList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                queueList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
            }


            foreach (XmlNode xn in DocNode.ChildNodes)
            {
                queueList.Data.Add(QueueFillNode(xn));
            }
            return queueList;
        }

        /// <summary>
        /// This method accepts a Queue node and parses through the different items in it. it can be used to parse a Csr node, whether the node is returned from a simple read, or as part of a list call.
        /// </summary>
        static internal Queue QueueFillNode(XmlNode QueueNode)
        {
            Queue Queue = new Queue();
            Queue.QueueID = Int32.Parse(QueueNode.Attributes["id"].InnerText.ToString());

            foreach (XmlNode child in QueueNode.ChildNodes)
            {
                if (child.LocalName.ToLower() == "name")
                {
                    Queue.Name = ParserUtils.NodeGetInnerText(child);
                }
            }
            return Queue;
        }
    }
}