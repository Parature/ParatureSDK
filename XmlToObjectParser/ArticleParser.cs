using System;
using System.Xml;
using ParatureSDK.ParaObjects;
using Article = ParatureSDK.ApiHandler.Article;

namespace ParatureSDK.XmlToObjectParser
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
            Article = ParaEntityParser.EntityFill<ParaObjects.Article>(xmlresp);
            Article.FullyLoaded = true;
            return Article;
        }

        /// <summary>
        /// This methods requires a Article list xml file and returns a ArticlesList oject. It should only by used for a List operation.
        /// </summary>
        static internal ParaEntityList<ParaObjects.Article> ArticlesFillList(XmlDocument xmlresp, Boolean MinimalisticLoad, int requestdepth, ParaCredentials ParaCredentials)
        {
            var ArticlesList = new ParaEntityList<ParaObjects.Article>();
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
                var xDoc = new XmlDocument();
                xDoc.LoadXml(xn.OuterXml);
                //ArticlesList.Data.Add(ArticleFillNode(xn, childDepth, MinimalisticLoad, ParaCredentials));
                ArticlesList.Data.Add(ParaEntityParser.EntityFill<ParaObjects.Article>(xDoc));
            }
            return ArticlesList;
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
            static internal ParaEntityList<ParaObjects.ArticleFolder> ArticleFoldersFillList(XmlDocument xmlresp, int requestdepth, ParaCredentials ParaCredentials)
            {
                var ArticleFoldersList = new ParaEntityList<ArticleFolder>();
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
                    ArticleFoldersList.Data.Add(ArticleFolderFillNode(xn, childDepth, ParaCredentials));
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
                    ArticleFolder.Id = Int64.Parse(ArticleFolderNode.Attributes["id"].InnerText.ToString());
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

                            pf.Id = Int64.Parse(child.ChildNodes[0].Attributes["id"].Value.ToString());
                            pf.Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();

                            if (childDepth > 0)
                            {
                                pf = Article.ArticleFolder.GetDetails(Int64.Parse(child.ChildNodes[0].Attributes["id"].Value.ToString()), ParaCredentials, (ParaEnums.RequestDepth)childDepth);
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