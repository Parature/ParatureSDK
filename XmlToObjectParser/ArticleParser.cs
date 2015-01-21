using System;
using System.Xml;
using ParatureAPI.PagedData;
using ParatureAPI.ParaObjects;
using Article = ParatureAPI.ApiHandler.Article;

namespace ParatureAPI.XmlToObjectParser
{
    /// <summary>
    /// This class helps parse raw XML responses returned from the server to hard typed Article objects that you can use for further processing.
    /// </summary>
    internal partial class ArticleParser
    {
        /// <summary>
        /// This methods requires a Article xml file and returns a Article object. It should only by used for a retrieve operation.
        /// </summary>
        static internal ParaObjects.Article ArticleFill(XmlDocument xmlresp, int requestdepth, bool MinimalisticLoad, ParaCredentials ParaCredentials)
        {
            ParaObjects.Article Article = new ParaObjects.Article();
            XmlNode ArticleNode = xmlresp.DocumentElement;

            // Setting up the request level for all child items of an account.
            int childDepth = 0;
            if (requestdepth > 0)
            {
                childDepth = requestdepth - 1;
            }
            Article = ArticleFillNode(ArticleNode, childDepth, MinimalisticLoad, ParaCredentials);
            Article.FullyLoaded = true;
            return Article;
        }

        /// <summary>
        /// This methods requires a Article list xml file and returns a ArticlesList oject. It should only by used for a List operation.
        /// </summary>
        static internal ArticlesList ArticlesFillList(XmlDocument xmlresp, Boolean MinimalisticLoad, int requestdepth, ParaCredentials ParaCredentials)
        {
            ArticlesList ArticlesList = new ArticlesList();
            XmlNode DocNode = xmlresp.DocumentElement;

            // Setting up the request level for all child items of a Download.
            int childDepth = 0;
            if (requestdepth > 0)
            {
                childDepth = requestdepth - 1;
            }


            ArticlesList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());

            if (DocNode.Attributes["page-size"] != null)
            {
                // If this is a "TotalOnly" request, there are no other attributes than "Total"
                ArticlesList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                ArticlesList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                ArticlesList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
            }



            foreach (XmlNode xn in DocNode.ChildNodes)
            {
                ArticlesList.Articles.Add(ArticleFillNode(xn, childDepth, MinimalisticLoad, ParaCredentials));
            }
            return ArticlesList;
        }

        /// <summary>
        /// This methods accepts a Article node and parse through the different items in it. it can be used to parse a Article node, whether the node is returned from a simple read, or as part of a list call.
        /// </summary>
        static internal ParaObjects.Article ArticleFillNode(XmlNode ArticleNode, int childDepth, bool MinimalisticLoad, ParaCredentials ParaCredentials)
        {
            bool isSchema = false;
            ParaObjects.Article Article = new ParaObjects.Article();
            if (ArticleNode.Attributes["id"] != null)
            {
                Article.Id = Int64.Parse(ArticleNode.Attributes["id"].InnerText.ToString());
                Article.uniqueIdentifier = Article.Id;
            }
            else
            {
                isSchema = true;
            }

            if (ArticleNode.Attributes["service-desk-uri"] != null)
            {
                Article.serviceDeskUri = ArticleNode.Attributes["service-desk-uri"].InnerText.ToString();
            }

            foreach (XmlNode child in ArticleNode.ChildNodes)
            {
                if (isSchema == false)
                {
                    if (child.LocalName.ToLower() == "permissions")
                    {
                        foreach (XmlNode n in child.ChildNodes)
                        {
                            Sla sla = new Sla();
                            sla.SlaID = Int64.Parse(n.Attributes["id"].Value);
                            sla.Name = n.ChildNodes[0].InnerText.ToString();
                            Article.Permissions.Add(sla);
                        }
                    }
                    if (child.LocalName.ToLower() == "products")
                    {
                        foreach (XmlNode n in child.ChildNodes)
                        {
                            ParaObjects.Product product = new ParaObjects.Product();
                            product.Id = Int64.Parse(n.Attributes["id"].Value);
                            product.Name = n.ChildNodes[0].InnerText.ToString();
                            Article.Products.Add(product);
                        }
                    }
                    if (child.LocalName.ToLower() == "answer")
                    {
                        Article.Answer = ParserUtils.NodeGetInnerText(child);
                    }
                    if (child.LocalName.ToLower() == "created_by")
                    {
                        Article.Created_By.Id = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value.ToString());
                        Article.Created_By.Full_Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                    }

                    if (child.LocalName.ToLower() == "date_created")
                    {
                        Article.Date_Created = DateTime.Parse(ParserUtils.NodeGetInnerText(child));
                    }
                    if (child.LocalName.ToLower() == "date_updated")
                    {
                        Article.Date_Updated = DateTime.Parse(ParserUtils.NodeGetInnerText(child));
                    }
                    if (child.LocalName.ToLower() == "expiration_date")
                    {
                        Article.Expiration_Date = DateTime.Parse(ParserUtils.NodeGetInnerText(child));
                    }
                    if (child.LocalName.ToLower() == "modified_by")
                    {
                        Article.Modified_By.Id = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value.ToString());
                        Article.Modified_By.Full_Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                    }
                    if (child.LocalName.ToLower() == "folders")
                    {
                        foreach (XmlNode n in child.ChildNodes)
                        {
                            Folder folder = new Folder();
                            folder.FolderID = Int64.Parse(n.Attributes["id"].Value);
                            folder.Name = n.ChildNodes[0].InnerText.ToString();
                            Article.Folders.Add(folder);
                        }
                    }
                    if (child.LocalName.ToLower() == "published")
                    {
                        Article.Published = Boolean.Parse(ParserUtils.NodeGetInnerText(child));
                    }
                    if (child.LocalName.ToLower() == "question")
                    {
                        Article.Question = ParserUtils.NodeGetInnerText(child);
                    }
                    if (child.LocalName.ToLower() == "rating")
                    {
                        Article.Rating = Int32.Parse(ParserUtils.NodeGetInnerText(child));
                    }
                    if (child.LocalName.ToLower() == "times_viewed")
                    {
                        Article.Times_Viewed = Int32.Parse(ParserUtils.NodeGetInnerText(child));
                    }
                }
            }
            return Article;
        }

        internal partial class ArticleFolderParser
        {
            /// <summary>
            /// This methods requires a DownloadFolder xml file and returns a DownloadFolder object. It should only by used for a retrieve operation.
            /// </summary>
            static internal ArticleFolder ArticleFolderFill(XmlDocument xmlresp, int requestdepth, ParaCredentials ParaCredentials)
            {
                ArticleFolder ArticleFolder = new ArticleFolder();
                XmlNode ArticleFolderNode = xmlresp.DocumentElement;

                // Setting up the request level for all child items of an account.
                int childDepth = 0;
                if (requestdepth > 0)
                {
                    childDepth = requestdepth - 1;
                }
                ArticleFolder = ArticleFolderFillNode(ArticleFolderNode, childDepth, ParaCredentials);
                ArticleFolder.FullyLoaded = true;
                return ArticleFolder;
            }

            /// <summary>
            /// This method requires a DownloadFolder list xml file and returns a DownloadFoldersList object. It should only by used for a List operation.
            /// </summary>
            static internal ArticleFoldersList ArticleFoldersFillList(XmlDocument xmlresp, int requestdepth, ParaCredentials ParaCredentials)
            {
                ArticleFoldersList ArticleFoldersList = new ArticleFoldersList();
                XmlNode DocNode = xmlresp.DocumentElement;

                // Setting up the request level for all child items of a DownloadFolder.
                int childDepth = 0;
                if (requestdepth > 0)
                {
                    childDepth = requestdepth - 1;
                }


                ArticleFoldersList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());

                if (DocNode.Attributes["page-size"] != null)
                {
                    // If this is a "TotalOnly" request, there are no other attributes than "Total"
                    ArticleFoldersList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                    ArticleFoldersList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                    ArticleFoldersList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
                }


                foreach (XmlNode xn in DocNode.ChildNodes)
                {
                    ArticleFoldersList.ArticleFolders.Add(ArticleFolderFillNode(xn, childDepth, ParaCredentials));
                }
                return ArticleFoldersList;
            }

            /// <summary>
            /// This method accepts a DownloadFolder node and parses through the different items in it. it can be used to parse a DownloadFolder node, whether the node is returned from a simple read, or as part of a list call.
            /// </summary>
            static internal ArticleFolder ArticleFolderFillNode(XmlNode ArticleFolderNode, int childDepth, ParaCredentials ParaCredentials)
            {

                ArticleFolder ArticleFolder = new ArticleFolder();

                bool isSchema = false;

                if (ArticleFolderNode.Attributes["id"] != null)
                {
                    ArticleFolder.FolderID = Int64.Parse(ArticleFolderNode.Attributes["id"].InnerText.ToString());
                }
                else
                {
                    isSchema = true;
                }

                ArticleFolder.FullyLoaded = true;
                foreach (XmlNode child in ArticleFolderNode.ChildNodes)
                {
                    if (isSchema == false)
                    {
                        if (child.LocalName.ToLower() == "name")
                        {
                            ArticleFolder.Name = ParserUtils.NodeGetInnerText(child);
                        }
                        if (child.LocalName.ToLower() == "description")
                        {
                            ArticleFolder.Description = ParserUtils.NodeGetInnerText(child);
                        }
                        if (child.LocalName.ToLower() == "is_private")
                        {
                            ArticleFolder.Is_Private = Boolean.Parse(ParserUtils.NodeGetInnerText(child));
                        }
                        if (child.LocalName.ToLower() == "parent_folder")
                        {
                            ArticleFolder pf = new ArticleFolder();

                            pf.FolderID = Int64.Parse(child.ChildNodes[0].Attributes["id"].Value.ToString());
                            pf.Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();

                            if (childDepth > 0)
                            {
                                pf = Article.ArticleFolder.ArticleFolderGetDetails(Int64.Parse(child.ChildNodes[0].Attributes["id"].Value.ToString()), ParaCredentials, (ParaEnums.RequestDepth)childDepth);
                            }
                            ArticleFolder.Parent_Folder = pf;
                        }
                    }
                }
                return ArticleFolder;
            }
        }
    }
}