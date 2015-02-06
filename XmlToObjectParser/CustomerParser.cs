using System;
using System.Xml;
using ParatureSDK.ParaHelper;
using ParatureSDK.ParaObjects;
using Account = ParatureSDK.ApiHandler.Account;

namespace ParatureSDK.XmlToObjectParser
{
    /// <summary>
    /// This class helps parse raw XML responses returned from the server to hard typed Customer objects that you can use for further processing.
    /// </summary>
    internal class CustomerParser
    {
        /// <summary>
        /// This methods requires a Customer xml file and returns a customer object. It should only by used for a retrieve operation.
        /// </summary>
        static internal ParaObjects.Customer CustomerFill(XmlDocument xmlresp, int requestdepth, bool includeAllCustomFields, ParaCredentials ParaCredentials)
        {
            ParaObjects.Customer Customer = new ParaObjects.Customer();
            XmlNode CustomerNode = xmlresp.DocumentElement;

            // Setting up the request level for all child items of an account.
            int childDepth = 0;
            if (requestdepth > 0)
            {
                childDepth = requestdepth - 1;
            }
            Customer = CustomerFillNode(CustomerNode, childDepth, includeAllCustomFields, ParaCredentials);
            Customer.FullyLoaded = true;
            return Customer;
        }

        /// <summary>
        /// This methods requires a Customer list xml file and returns an CustomersList oject. It should only by used for a List operation.
        /// </summary>
        static internal ParaEntityList<ParaObjects.Customer> CustomersFillList(XmlDocument xmlresp, Boolean MinimalisticLoad, int requestdepth, ParaCredentials ParaCredentials)
        {
            var CustomersList = new ParaEntityList<ParaObjects.Customer>();
            XmlNode DocNode = xmlresp.DocumentElement;

            // Setting up the request level for all child items of an account.
            int childDepth = 0;
            if (requestdepth > 0)
            {
                childDepth = requestdepth - 1;
            }


            CustomersList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());


            if (DocNode.Attributes["page-size"] != null)
            {
                // If this is a "TotalOnly" request, there are no other attributes than "Total"

                CustomersList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                CustomersList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                CustomersList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
            }



            foreach (XmlNode xn in DocNode.ChildNodes)
            {
                CustomersList.Data.Add(CustomerFillNode(xn, childDepth, MinimalisticLoad, ParaCredentials));
            }
            return CustomersList;
        }

        /// <summary>
        /// This methods accepts a customer node and parse through the different items in it. it can be used to parse a customer node, whether the node is returned from a simple read, or as part of a list call.
        /// </summary>
        static internal ParaObjects.Customer CustomerFillNode(XmlNode CustomerNode, int childDepth, Boolean MinimalisticLoad, ParaCredentials ParaCredentials)
        {

            ParaObjects.Customer Customer = new ParaObjects.Customer();
            bool isSchema = false;
            if (CustomerNode.Attributes["id"] != null)
            {
                isSchema = false;
                Customer.Id = Int64.Parse(CustomerNode.Attributes["id"].InnerText.ToString());
                Customer.uniqueIdentifier = Customer.Id;
            }
            else
            {
                isSchema = true;
            }

            if (CustomerNode.Attributes["service-desk-uri"] != null)
            {
                Customer.serviceDeskUri = CustomerNode.Attributes["service-desk-uri"].InnerText.ToString();
            }

            foreach (XmlNode child in CustomerNode.ChildNodes)
            {
                if (isSchema == false)
                {
                    if (child.LocalName.ToLower() == "status")
                    {
                        if (child.ChildNodes[0] != null && child.ChildNodes[0].Attributes["id"] != null)
                        {
                            var status = new Status
                            {
                                Name = child.ChildNodes[0].ChildNodes[0].InnerText,
                                StatusID = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value)
                            };
                            Customer.Status = status;
                        }
                    }
                    if (child.LocalName.ToLower() == "account")
                    {
                        // Fill the account details
                        Customer.Account = new ParaObjects.Account();
                        Customer.Account = AccountParser.AccountFillNode(child.ChildNodes[0], childDepth, MinimalisticLoad, ParaCredentials);
                        if (childDepth > 0)
                        {
                            Customer.Account = Account.GetDetails(Customer.Account.Id, ParaCredentials, (ParaEnums.RequestDepth)childDepth);
                        }

                        Customer.Account.FullyLoaded = ParserUtils.ObjectFullyLoaded(childDepth);
                    }

                    if (child.LocalName.ToLower() == "customer_role")
                    {
                        if (child.ChildNodes[0] != null && child.ChildNodes[0].Attributes["id"] != null)
                        {
                            var role = new Role
                            {
                                Name = child.ChildNodes[0].ChildNodes[0].InnerText,
                                RoleID = Int64.Parse(child.ChildNodes[0].Attributes["id"].Value)
                            };
                            Customer.Customer_Role = role;
                        }
                    }

                    if (child.LocalName.ToLower() == "sla")
                    {
                        if (child.ChildNodes[0] != null && child.ChildNodes[0].Attributes["id"] != null)
                        {
                            var sla = new Sla
                            {
                                Name = child.ChildNodes[0].ChildNodes[0].InnerText,
                                SlaID = Int64.Parse(child.ChildNodes[0].Attributes["id"].Value)
                            };
                            Customer.Sla = sla;
                        }
                    }
                    if (child.LocalName.ToLower() == "date_visited")
                    {
                        Customer.Date_Visited = DateTime.Parse(ParserUtils.NodeGetInnerText(child));
                    }


                    if (child.LocalName.ToLower() == "date_created")
                    {
                        Customer.Date_Created = DateTime.Parse(ParserUtils.NodeGetInnerText(child));
                    }

                    if (child.LocalName.ToLower() == "date_updated")
                    {
                        Customer.Date_Updated = DateTime.Parse(ParserUtils.NodeGetInnerText(child));
                    }

                    if (child.LocalName.ToLower() == "email")
                    {
                        Customer.Email = HelperMethods.SafeHtmlDecode(ParserUtils.NodeGetInnerText(child));
                    }
                    if (child.LocalName.ToLower() == "first_name")
                    {
                        Customer.First_Name = HelperMethods.SafeHtmlDecode(ParserUtils.NodeGetInnerText(child));
                    }
                    if (child.LocalName.ToLower() == "last_name")
                    {
                        Customer.Last_Name = HelperMethods.SafeHtmlDecode(ParserUtils.NodeGetInnerText(child));
                    }
                    if (child.LocalName.ToLower() == "user_name")
                    {
                        Customer.User_Name = HelperMethods.SafeHtmlDecode(ParserUtils.NodeGetInnerText(child));
                    }
                }

                if (child.LocalName.ToLower() == "custom_field")
                {
                    Customer.Fields.Add(CommonParser.FillCustomField(MinimalisticLoad, child));
                }

            }
            return Customer;
        }
    }
}