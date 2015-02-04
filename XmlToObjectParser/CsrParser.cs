using System;
using System.Xml;
using ParatureSDK.ParaObjects;

namespace ParatureSDK.XmlToObjectParser
{
    /// <summary>
    /// Handles all XML parsing logic needed for the CSR object
    /// </summary>
    internal partial class CsrParser
    {
        /// <summary>
        /// This methods requires a CSR xml file and returns a CSR object. It should only by used for a retrieve operation.
        /// </summary>
        static internal ParaObjects.Csr CsrFill(XmlDocument xmlresp)
        {
            ParaObjects.Csr Csr = new ParaObjects.Csr();
            XmlNode CsrNode = xmlresp.DocumentElement;
            Csr = CsrFillNode(CsrNode);
            return Csr;
        }

        /// <summary>
        /// This method requires a Csr list xml file and returns a Csr object. It should only by used for a List operation.
        /// </summary>
        static internal ParaEntityList<ParaObjects.Csr> CsrsFillList(XmlDocument xmlresp)
        {
            var CsrsList = new ParaEntityList<ParaObjects.Csr>();
            XmlNode DocNode = xmlresp.DocumentElement;


            CsrsList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());


            if (DocNode.Attributes["page-size"] != null)
            {
                // If this is a "TotalOnly" request, there are no other attributes than "Total"
                CsrsList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                CsrsList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                CsrsList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
            }




            foreach (XmlNode xn in DocNode.ChildNodes)
            {
                CsrsList.Data.Add(CsrFillNode(xn));
            }
            return CsrsList;
        }

        /// <summary>
        /// This method accepts a Csr node and parses through the different items in it. it can be used to parse a Csr node, whether the node is returned from a simple read, or as part of a list call.
        /// </summary>
        static internal ParaObjects.Csr CsrFillNode(XmlNode CsrNode)
        {

            ParaObjects.Csr Csr = new ParaObjects.Csr();

            bool isSchema = false;

            if (CsrNode.Attributes["id"] != null)
            {
                Csr.Id = Int64.Parse(CsrNode.Attributes["id"].InnerText.ToString());
                Csr.uniqueIdentifier = Csr.Id;
            }
            else
            {
                isSchema = true;
            }

            if (CsrNode.Attributes["service-desk-uri"] != null)
            {
                Csr.serviceDeskUri = CsrNode.Attributes["service-desk-uri"].InnerText.ToString();
            }

            foreach (XmlNode child in CsrNode.ChildNodes)
            {
                if (isSchema == false)
                {
                    if (child.LocalName.ToLower() == "email")
                    {
                        Csr.Email = ParserUtils.NodeGetInnerText(child);
                    }
                    else if (child.LocalName.ToLower() == "fax")
                    {
                        Csr.Fax = ParserUtils.NodeGetInnerText(child);
                    }
                    else if (child.LocalName.ToLower() == "full_name")
                    {
                        Csr.Full_Name = ParserUtils.NodeGetInnerText(child);
                    }
                    else if (child.LocalName.ToLower() == "phone_1")
                    {
                        Csr.Phone_1 = ParserUtils.NodeGetInnerText(child);
                    }
                    else if (child.LocalName.ToLower() == "phone_2")
                    {
                        Csr.Phone_2 = ParserUtils.NodeGetInnerText(child);
                    }
                    else if (child.LocalName.ToLower() == "screen_name")
                    {
                        Csr.Screen_Name = ParserUtils.NodeGetInnerText(child);
                    }
                    else if (child.LocalName.ToLower() == "date_created")
                    {
                        Csr.Date_Created = DateTime.Parse(ParserUtils.NodeGetInnerText(child));
                    }
                    else if (child.LocalName.ToLower() == "date_format")
                    {
                        Csr.Date_Format = ParserUtils.NodeGetInnerText(child);
                    }
                    else if (child.LocalName.ToLower() == "role")
                    {
                        for (int i = 0; i < child.ChildNodes.Count; i++)
                        {
                            if (child.ChildNodes[i] != null && child.ChildNodes[i].Attributes["id"] != null)
                            {
                                Csr.Role.Add(new Role(
                                    Int64.Parse(child.ChildNodes[i].Attributes["id"].Value.ToString()),
                                    child.ChildNodes[i].ChildNodes[0].InnerText.ToString(),
                                    ""));
                            }
                        }
                    }
                    else if (child.LocalName.ToLower() == "status")
                    {
                        if (child.ChildNodes[0] != null && child.ChildNodes[0].Attributes["id"] != null)
                        {
                            Csr.Status.StatusID = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value.ToString());
                            Csr.Status.Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                        }
                    }
                    else if (child.LocalName.ToLower() == "timezone")
                    {
                        if (child.ChildNodes[0] != null && child.ChildNodes[0].Attributes["id"] != null)
                        {
                            Csr.Timezone.TimezoneID = Int64.Parse(child.ChildNodes[0].Attributes["id"].Value.ToString());
                            Csr.Timezone.Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                        }
                    }
                    else
                    {
                        Console.Read();
                    }
                }
            }
            return Csr;
        }
    }
}