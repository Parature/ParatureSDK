using System;
using System.Xml;
using ParatureAPI.PagedData;
using ParatureAPI.ParaHelper;
using ParatureAPI.ParaObjects;
using Product = ParatureAPI.ApiHandler.Product;

namespace ParatureAPI.XmlToObjectParser
{
    /// <summary>
    /// This class helps parse raw XML responses returned from the server to hard typed Product objects that you can use for further processing.
    /// </summary>
    internal class ProductParser
    {
        /// <summary>
        /// This methods requires a Product xml file and returns a product object. It should only by used for a retrieve operation.
        /// </summary>
        static internal ParaObjects.Product ProductFill(XmlDocument xmlresp, int requestdepth, bool MinimalisticLoad, ParaCredentials ParaCredentials)
        {
            ParaObjects.Product Product = new ParaObjects.Product();
            XmlNode ProductNode = xmlresp.DocumentElement;

            // Setting up the request level for all child items of an account.
            int childDepth = 0;
            if (requestdepth > 0)
            {
                childDepth = requestdepth - 1;
            }
            Product = ProductFillNode(ProductNode, childDepth, MinimalisticLoad, ParaCredentials);
            Product.FullyLoaded = true;
            return Product;
        }


        /// <summary>
        /// This methods requires a Product list xml file and returns a ProductsList oject. It should only by used for a List operation.
        /// </summary>
        static internal ParaEntityList<ParaObjects.Product> ProductsFillList(XmlDocument xmlresp, Boolean MinimalisticLoad, int requestdepth, ParaCredentials ParaCredentials)
        {
            var ProductsList = new ParaEntityList<ParaObjects.Product>();
            XmlNode DocNode = xmlresp.DocumentElement;

            // Setting up the request level for all child items of an account.
            int childDepth = 0;
            if (requestdepth > 0)
            {
                childDepth = requestdepth - 1;
            }

            ProductsList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());

            if (DocNode.Attributes["page-size"] != null)
            {
                // If this is a "TotalOnly" request, there are no other attributes than "Total"
                ProductsList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                ProductsList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                ProductsList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
            }

            foreach (XmlNode xn in DocNode.ChildNodes)
            {
                ProductsList.Data.Add(ProductFillNode(xn, childDepth, MinimalisticLoad, ParaCredentials));
            }
            return ProductsList;
        }

        /// <summary>
        /// This methods accepts a Product node and parse through the different items in it. it can be used to parse a product node, whether the node is returned from a simple read, or as part of a list call.
        /// </summary>
        static internal ParaObjects.Product ProductFillNode(XmlNode Node, int childDepth, bool MinimalisticLoad, ParaCredentials ParaCredentials)
        {

            var Product = new ParaObjects.Product();
            bool isSchema = false;

            if (Node.Attributes["id"] != null)
            {
                Product.Id = Int64.Parse(Node.Attributes["id"].InnerText.ToString());
                Product.uniqueIdentifier = Product.Id;
                isSchema = false;
            }
            else
            {
                isSchema = true;
            }

            if (Node.Attributes["service-desk-uri"] != null)
            {
                Product.serviceDeskUri = Node.Attributes["service-desk-uri"].InnerText.ToString();
            }

            foreach (XmlNode child in Node.ChildNodes)
            {
                //Date_Created
                if (isSchema == false)
                {
                    if (child.LocalName.ToLower() == "date_created")
                    {
                        Product.Date_Created = DateTime.Parse(ParserUtils.NodeGetInnerText(child));
                    }
                    if (child.LocalName.ToLower() == "date_updated")
                    {
                        Product.Date_Updated = DateTime.Parse(ParserUtils.NodeGetInnerText(child));
                    }

                    if (child.LocalName.ToLower() == "folder")
                    {
                        Product.Folder.FolderID = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value);
                        Product.Folder.Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                        Product.Folder.FullyLoaded = false;
                    }


                    if (child.LocalName.ToLower() == "visible")
                    {
                        Product.Visible = Boolean.Parse(ParserUtils.NodeGetInnerText(child));
                    }
                    if (child.LocalName.ToLower() == "instock")
                    {
                        Product.InStock = Boolean.Parse(ParserUtils.NodeGetInnerText(child));
                    }


                    if (child.LocalName.ToLower() == "sku")
                    {
                        Product.Sku = ParserUtils.NodeGetInnerText(child);
                    }

                    if (child.LocalName.ToLower() == "price")
                    {
                        Product.Price = ParserUtils.NodeGetInnerText(child);
                    }

                    if (child.LocalName.ToLower() == "shortdesc")
                    {
                        Product.Shortdesc = ParserUtils.NodeGetInnerText(child);
                    }

                    if (child.LocalName.ToLower() == "name")
                    {
                        Product.Name = HelperMethods.SafeHtmlDecode(ParserUtils.NodeGetInnerText(child));
                    }
                    if (child.LocalName.ToLower() == "longdesc")
                    {
                        Product.Longdesc = ParserUtils.NodeGetInnerText(child);
                    }


                }
                if (child.LocalName.ToLower() == "custom_field")
                {
                    Product.Fields.Add(CommonParser.FillCustomField(MinimalisticLoad, child));
                }

            }
            return Product;
        }
        internal class ProductFolderParser
        {
            /// <summary>
            /// This method requires a ProductFolder xml file and returns a ProductFolder object. It should only by used for a retrieve operation.
            /// </summary>
            static internal ProductFolder ProductFolderFill(XmlDocument xmlresp, int requestdepth, ParaCredentials ParaCredentials)
            {
                ProductFolder ProductFolder = new ProductFolder();
                XmlNode ProductFolderNode = xmlresp.DocumentElement;

                // Setting up the request level for all child items of an account.
                int childDepth = 0;
                if (requestdepth > 0)
                {
                    childDepth = requestdepth - 1;
                }
                ProductFolder = ProductFolderFillNode(ProductFolderNode, childDepth, ParaCredentials);
                ProductFolder.FullyLoaded = true;
                return ProductFolder;
            }

            /// <summary>
            /// This method requires a ProductFolder list xml file and returns a ProductFoldersList object. It should only by used for a List operation.
            /// </summary>
            static internal ParaEntityList<ParaObjects.ProductFolder> ProductFoldersFillList(XmlDocument xmlresp, int requestdepth, ParaCredentials ParaCredentials)
            {
                var ProductFoldersList = new ParaEntityList<ParaObjects.ProductFolder>();
                XmlNode DocNode = xmlresp.DocumentElement;

                // Setting up the request level for all child items of a DownloadFolder.
                int childDepth = 0;
                if (requestdepth > 0)
                {
                    childDepth = requestdepth - 1;
                }

                ProductFoldersList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());

                if (DocNode.Attributes["page-size"] != null)
                {
                    // If this is a "TotalOnly" request, there are no other attributes than "Total"
                    ProductFoldersList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                    ProductFoldersList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                    ProductFoldersList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
                }


                foreach (XmlNode xn in DocNode.ChildNodes)
                {
                    ProductFoldersList.Data.Add(ProductFolderFillNode(xn, childDepth, ParaCredentials));
                }
                return ProductFoldersList;
            }

            /// <summary>
            /// This method accepts a ProductFolder node and parses through the different items in it. it can be used to parse a ProductFolder node, whether the node is returned from a simple read, or as part of a list call.
            /// </summary>
            static internal ProductFolder ProductFolderFillNode(XmlNode ProductFolderNode, int childDepth, ParaCredentials ParaCredentials)
            {

                ProductFolder ProductFolder = new ProductFolder();
                ProductFolder.FolderID = Int64.Parse(ProductFolderNode.Attributes["id"].InnerText.ToString());

                foreach (XmlNode child in ProductFolderNode.ChildNodes)
                {
                    if (child.LocalName.ToLower() == "is_private")
                    {
                        ProductFolder.Is_Private = Boolean.Parse(ParserUtils.NodeGetInnerText(child));
                    }

                    if (child.LocalName.ToLower() == "date_updated")
                    {
                        ProductFolder.Date_Updated = ParserUtils.NodeGetInnerText(child);
                    }

                    if (child.LocalName.ToLower() == "description")
                    {
                        ProductFolder.Description = ParserUtils.NodeGetInnerText(child);
                    }
                    if (child.LocalName.ToLower() == "name")
                    {
                        ProductFolder.Name = ParserUtils.NodeGetInnerText(child);
                    }
                    if (child.LocalName.ToLower() == "parent_folder")
                    {
                        ProductFolder pf = new ProductFolder();

                        pf.FolderID = Int64.Parse(child.ChildNodes[0].Attributes["id"].Value.ToString());
                        pf.Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();

                        if (childDepth > 0)
                        {
                            pf = Product.ProductFolder.GetDetails(Int64.Parse(child.ChildNodes[0].Attributes["id"].Value.ToString()), ParaCredentials, (ParaEnums.RequestDepth)childDepth);
                        }
                        ProductFolder.Parent_Folder = pf;
                    }
                }
                return ProductFolder;
            }
        }

    }
}