using System;
using System.Collections.Generic;
using System.Xml;
using ParatureAPI.PagedData;
using ParatureAPI.ParaHelper;
using ParatureAPI.ParaObjects;
using Account = ParatureAPI.ApiHandler.Account;

namespace ParatureAPI.XmlToObjectParser
{
    /// <summary>
    /// This class helps parse raw XML responses returned from the server to hard typed Account objects that you can use for further processing.
    /// </summary>
    internal class AccountParser
    {
        /// <summary>
        /// This methods requires an account xml file and returns an account object. It should only by used for a retrieve operation.
        /// </summary>
        static internal ParaObjects.Account AccountFill(XmlDocument xmlresp, int requestdepth, bool MinimalisticLoad, ParaCredentials ParaCredentials)
        {
            ParaObjects.Account account = new ParaObjects.Account();
            XmlNode AccountNode = xmlresp.DocumentElement;

            // Setting up the request level for all child items of an account.
            int childDepth = 0;
            if (requestdepth > 0)
            {
                childDepth = requestdepth - 1;
            }

            account = AccountFillNode(AccountNode, childDepth, MinimalisticLoad, ParaCredentials);
            account.FullyLoaded = true;
            return account;
        }

        /// <summary>
        /// This methods requires an account list xml file and returns an AccountsList oject. It should only by used for a List operation.
        /// </summary>
        static internal ParaEntityList<ParaObjects.Account> AccountsFillList(XmlDocument xmlresp, Boolean MinimalisticLoad, int requestdepth, ParaCredentials ParaCredentials)
        {
            var AccountsList = new ParaEntityList<ParaObjects.Account>();
            XmlNode DocNode = xmlresp.DocumentElement;

            // Setting up the request level for all child items of an account.
            int childDepth = 0;
            if (requestdepth > 0)
            {
                childDepth = requestdepth - 1;
            }


            AccountsList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());

            if (DocNode.Attributes["page-size"] != null)
            {
                // If this is a "TotalOnly" request, there are no other attributes than "Total"
                AccountsList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                AccountsList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                AccountsList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
            }


            foreach (XmlNode xn in DocNode.ChildNodes)
            {
                AccountsList.Data.Add(AccountFillNode(xn, childDepth, MinimalisticLoad, ParaCredentials));
            }
            return AccountsList;
        }

        /// <summary>
        /// This methods accepts an account node and parse through the different items in it. it can be used to parse an account node, whether the node is returned from a simple read, or as part of a list call.
        /// </summary>
        static internal ParaObjects.Account AccountFillNode(XmlNode AccountNode, int childDepth, bool MinimalisticLoad, ParaCredentials ParaCredentials)
        {
            ParaObjects.Account account = new ParaObjects.Account();
            account.Viewable_Account = new List<ParaObjects.Account>();

            bool isSchema = false;

            if (AccountNode.Attributes["id"] != null)
            {
                account.Id = Int64.Parse(AccountNode.Attributes["id"].InnerText.ToString());
                account.uniqueIdentifier = account.Id;
                isSchema = false;
            }
            else
            {
                isSchema = true;
            }

            if (AccountNode.Attributes["service-desk-uri"] != null)
            {
                account.serviceDeskUri = AccountNode.Attributes["service-desk-uri"].InnerText.ToString();
            }

            account.FullyLoaded = true;
            foreach (XmlNode child in AccountNode.ChildNodes)
            {
                if (isSchema == false)
                {
                    if (child.LocalName.ToLower() == "account_name")
                    {
                        account.Account_Name = HelperMethods.SafeHtmlDecode(child.InnerText.ToString());
                    }

                    if (child.LocalName.ToLower() == "date_created")
                    {
                        account.Date_Created = DateTime.Parse(child.InnerText.ToString());
                    }

                    if (child.LocalName.ToLower() == "date_updated")
                    {
                        account.Date_Updated = DateTime.Parse(child.InnerText.ToString());
                    }

                    if (child.LocalName.ToLower() == "owned_by")
                    {
                        if (child.ChildNodes[0].Attributes["id"] != null)
                        {
                            account.Owned_By.Id = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value.ToString());
                            account.Owned_By.Full_Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                        }
                    }

                    if (child.LocalName.ToLower() == "sla")
                    {
                        if (child.ChildNodes[0].Attributes["id"] != null)
                        {
                            account.Sla.SlaID = Int64.Parse(child.ChildNodes[0].Attributes["id"].Value.ToString());
                            account.Sla.Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                        }
                    }

                    if (child.LocalName.ToLower() == "default_customer_role")
                    {
                        if (child.ChildNodes[0].Attributes["id"] != null)
                        {
                            account.Default_Customer_Role.RoleID = Int64.Parse(child.ChildNodes[0].Attributes["id"].Value.ToString());
                            account.Default_Customer_Role.Name = child.ChildNodes[0].InnerText.ToString();
                        }
                    }

                    if (child.LocalName.ToLower() == "modified_by")
                    {
                        if (child.ChildNodes[0].Attributes["id"] != null)
                        {
                            account.Modified_By.Id = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value.ToString());
                            account.Modified_By.Full_Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                        }
                    }
                    if (child.LocalName.ToLower() == "shown_accounts")
                    {
                        foreach (XmlNode vnode in child.ChildNodes)
                        {
                            if (vnode.LocalName.ToLower() == "account")
                            {
                                ParaObjects.Account acc = new ParaObjects.Account();
                                acc.Id = Int64.Parse(vnode.Attributes["id"].Value.ToString());
                                acc.Account_Name = vnode.ChildNodes[0].InnerText.ToString();

                                if (childDepth > 0)
                                {
                                    acc = Account.GetDetails(acc.Id, ParaCredentials, (ParaEnums.RequestDepth)childDepth);
                                }

                                acc.FullyLoaded = ParserUtils.ObjectFullyLoaded(childDepth);

                                account.Viewable_Account.Add(acc);

                            }
                        }


                    }
                }

                if (child.LocalName.ToLower() == "custom_field")
                {
                    account.Fields.Add(CommonParser.FillCustomField(MinimalisticLoad, child));
                }
            }
            return account;
        }

    }
}