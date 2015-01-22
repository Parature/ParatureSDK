using System;
using System.Xml;
using ParatureAPI.ParaObjects;

namespace ParatureAPI.XmlToObjectParser
{
    /// <summary>
    /// Handles all XML parsing logic needed for the Role object
    /// </summary>
    internal partial class RoleParser
    {
        /// <summary>
        /// This methods requires a Role xml file and returns a Role object. It should only by used for a retrieve operation.
        /// </summary>
        /// <param name="xmlresp"></param>
        /// <returns></returns>
        static internal Role RoleFill(XmlDocument xmlresp)
        {
            Role Role = new Role();
            XmlNode RoleNode = xmlresp.DocumentElement;
            Role = RoleFillNode(RoleNode);
            return Role;
        }
        /// <summary>
        /// This method requires a Role list xml file and returns a Role list object. It should only by used for a List operation.
        /// </summary>
        /// <param name="xmlresp"></param>
        /// <returns></returns>
        static internal ParaEntityList<ParaObjects.Role> RolesFillList(XmlDocument xmlresp)
        {
            var RolesList = new ParaEntityList<ParaObjects.Role>();
            XmlNode DocNode = xmlresp.DocumentElement;

            RolesList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());

            if (DocNode.Attributes["page-size"] != null)
            {
                // If this is a "TotalOnly" request, there are no other attributes than "Total"
                RolesList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                RolesList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                RolesList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
            }

            foreach (XmlNode xn in DocNode.ChildNodes)
            {
                RolesList.Data.Add(RoleFillNode(xn));
            }
            return RolesList;
        }

        /// <summary>
        /// This method accepts a Role node and parses through the different items in it. it can be used to parse a Role node, whether the node is returned from a simple read, or as part of a list call.
        /// </summary>
        static internal Role RoleFillNode(XmlNode RoleNode)
        {

            Role Role = new Role();
            Role.RoleID = Int64.Parse(RoleNode.Attributes["id"].InnerText.ToString());

            foreach (XmlNode child in RoleNode.ChildNodes)
            {
                if (child.LocalName.ToLower() == "name")
                {
                    Role.Name = ParserUtils.NodeGetInnerText(child);
                }
                else if (child.LocalName.ToLower() == "description")
                {
                    Role.Description = ParserUtils.NodeGetInnerText(child);
                }
            }
            return Role;
        }
    }
}