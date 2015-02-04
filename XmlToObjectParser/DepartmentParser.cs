using System;
using System.Xml;
using ParatureSDK.ParaObjects;

namespace ParatureSDK.XmlToObjectParser
{
    /// <summary>
    /// Handles all XML parsing logic needed for the Department object
    /// </summary>
    internal partial class DepartmentParser
    {
        /// <summary>
        /// This methods requires a Department xml file and returns a Department object. It should only by used for a retrieve operation.
        /// </summary>
        static internal Department DepartmentFill(XmlDocument xmlresp)
        {
            Department department = new Department();
            XmlNode departmentNode = xmlresp.DocumentElement;
            department = DepartmentFillNode(departmentNode);
            return department;
        }

        /// <summary>
        /// This method requires a Department list xml file and returns a Department object. It should only by used for a List operation.
        /// </summary>
        static internal ParaEntityList<ParaObjects.Department> DepartmentsFillList(XmlDocument xmlresp)
        {
            var departmentsList = new ParaEntityList<ParaObjects.Department>();
            XmlNode DocNode = xmlresp.DocumentElement;

            departmentsList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());

            if (DocNode.Attributes["page-size"] != null)
            {
                // If this is a "TotalOnly" request, there are no other attributes than "Total"
                departmentsList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                departmentsList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                departmentsList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
            }

            foreach (XmlNode xn in DocNode.ChildNodes)
            {
                departmentsList.Data.Add(DepartmentFillNode(xn));
            }
            return departmentsList;
        }

        /// <summary>
        /// This method accepts a department node and parses through the different items in it. it can be used to parse a department node, whether the node is returned from a simple read, or as part of a list call.
        /// </summary>
        static internal Department DepartmentFillNode(XmlNode DepartmentNode)
        {

            Department department = new Department();
            department.Id = Int64.Parse(DepartmentNode.Attributes["id"].InnerText.ToString());

            foreach (XmlNode child in DepartmentNode.ChildNodes)
            {
                if (child.LocalName.ToLower() == "name")
                {
                    department.Name = ParserUtils.NodeGetInnerText(child);
                }
                if (child.LocalName.ToLower() == "description")
                {
                    department.Description = ParserUtils.NodeGetInnerText(child);
                }

            }
            return department;
        }
    }
}