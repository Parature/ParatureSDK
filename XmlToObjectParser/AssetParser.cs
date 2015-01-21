using System;
using System.Xml;
using ParatureAPI.ApiHandler;
using ParatureAPI.PagedData;
using ParatureAPI.ParaHelper;

namespace ParatureAPI.XmlToObjectParser
{
    /// <summary>
    /// This class helps parse raw XML responses returned from the server to hard typed Asset objects that you can use for further processing.
    /// </summary>
    internal partial class AssetParser
    {
        /// <summary>
        /// This methods requires an Asset xml file and returns a product object. It should only by used for a retrieve operation.
        /// </summary>
        static internal ParaObjects.Asset AssetFill(XmlDocument xmlresp, Boolean MinimalisticLoad, int requestdepth, ParaCredentials ParaCredentials)
        {
            ParaObjects.Asset Asset = new ParaObjects.Asset();
            XmlNode AssetNode = xmlresp.DocumentElement;

            // Setting up the request level for all child items of an account.
            int childDepth = 0;
            if (requestdepth > 0)
            {
                childDepth = requestdepth - 1;
            }
            Asset = AssetFillNode(AssetNode, MinimalisticLoad, childDepth, ParaCredentials);
            Asset.FullyLoaded = true;
            return Asset;
        }

        /// <summary>
        /// This methods requires an Asset list xml file and returns an AssetsList oject. It should only by used for a List operation.
        /// </summary>
        static internal AssetsList AssetsFillList(XmlDocument xmlresp, Boolean MinimalisticLoad, int requestdepth, ParaCredentials ParaCredentials)
        {
            AssetsList AssetsList = new AssetsList();
            XmlNode DocNode = xmlresp.DocumentElement;

            // Setting up the request level for all child items of an account.
            int childDepth = 0;
            if (requestdepth > 0)
            {
                childDepth = requestdepth - 1;
            }


            AssetsList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());

            if (DocNode.Attributes["page-size"] != null)
            {
                // If this is a "TotalOnly" request, there are no other attributes than "Total"
                AssetsList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                AssetsList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                AssetsList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
            }



            foreach (XmlNode xn in DocNode.ChildNodes)
            {
                AssetsList.Assets.Add(AssetFillNode(xn, MinimalisticLoad, childDepth, ParaCredentials));
            }
            return AssetsList;
        }

        /// <summary>
        /// This methods accepts an Asset node and parse through the different items in it. it can be used to parse a Asset node, whether the node is returned from a simple read, or as part of a list call.
        /// </summary>
        static internal ParaObjects.Asset AssetFillNode(XmlNode Node, Boolean MinimalisticLoad, int childDepth, ParaCredentials ParaCredentials)
        {

            ParaObjects.Asset Asset = new ParaObjects.Asset();
            bool isSchema = false;
            if (Node.Attributes["id"] != null)
            {
                Asset.Id = Int64.Parse(Node.Attributes["id"].InnerText.ToString());
                Asset.uniqueIdentifier = Asset.Id;
                isSchema = false;
            }
            else
            {
                isSchema = true;
            }

            if (Node.Attributes["service-desk-uri"] != null)
            {
                Asset.serviceDeskUri = Node.Attributes["service-desk-uri"].InnerText.ToString();
            }

            if (Node.Attributes["uid"] != null)
            {
                Asset.uid = Node.Attributes["uid"].InnerText.ToString();
            }


            foreach (XmlNode child in Node.ChildNodes)
            {
                if (isSchema == false)
                {
                    if (child.LocalName.ToLower() == "account_owner")
                    {
                        Asset.Account_Owner.Id = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value);
                        Asset.Account_Owner.Account_Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();

                        if (childDepth > 0)
                        {
                            Asset.Account_Owner = Account.AccountGetDetails(Asset.Account_Owner.Id, ParaCredentials, (ParaEnums.RequestDepth)childDepth - 1);
                        }
                        Asset.Account_Owner.FullyLoaded = ParserUtils.ObjectFullyLoaded(childDepth);
                    }

                    if (child.LocalName.ToLower() == "created_by")
                    {
                        Asset.Created_By.Id = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value);
                        Asset.Created_By.Full_Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                    }
                    if (child.LocalName.ToLower() == "modified_by")
                    {
                        Asset.Modified_By.Id = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value);
                        Asset.Modified_By.Full_Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                    }
                    if (child.LocalName.ToLower() == "customer_owner")
                    {
                        Asset.Customer_Owner.Id = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value);

                        if (childDepth > 0)
                        {
                            Asset.Customer_Owner = Customer.CustomerGetDetails(Asset.Customer_Owner.Id, ParaCredentials, (ParaEnums.RequestDepth)childDepth - 1);
                        }
                        Asset.Customer_Owner.FullyLoaded = ParserUtils.ObjectFullyLoaded(childDepth);

                        //Not sure about this one
                        Asset.Customer_Owner.First_Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                    }

                    if (child.LocalName.ToLower() == "product")
                    {
                        Asset.Product.Id = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value);
                        Asset.Product.Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                        if (childDepth > 0)
                        {
                            Asset.Product = Product.ProductGetDetails(Asset.Product.Id, ParaCredentials, (ParaEnums.RequestDepth)childDepth - 1);
                        }
                        Asset.Product.FullyLoaded = ParserUtils.ObjectFullyLoaded(childDepth);
                    }
                    if (child.LocalName.ToLower() == "date_created")
                    {
                        Asset.Date_Created = DateTime.Parse(ParserUtils.NodeGetInnerText(child));
                    }
                    if (child.LocalName.ToLower() == "date_updated")
                    {
                        Asset.Date_Updated = DateTime.Parse(ParserUtils.NodeGetInnerText(child));
                    }
                    if (child.LocalName.ToLower() == "serial_number")
                    {
                        Asset.Serial_Number = ParserUtils.NodeGetInnerText(child);
                    }
                    if (child.LocalName.ToLower() == "name")
                    {
                        Asset.Name = HelperMethods.SafeHtmlDecode(ParserUtils.NodeGetInnerText(child));
                    }
                    if (child.LocalName.ToLower() == "status")
                    {
                        Asset.Status.StatusID = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value);
                        Asset.Status.Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                    }
                }

                if (child.LocalName.ToLower() == "custom_field")
                {
                    Asset.Fields.Add(CommonParser.FillCustomField(MinimalisticLoad, child));
                }
            }
            return Asset;
        }
    }
}