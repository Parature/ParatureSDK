using System;
using System.Collections.Generic;
using System.Xml;
using ParatureSDK.ParaObjects;
using Download = ParatureSDK.ApiHandler.Download;

namespace ParatureSDK.XmlToObjectParser
{
    /// <summary>
    /// This class helps parse raw XML responses returned from the server to hard typed Download objects that you can use for further processing.
    /// </summary>
    internal partial class DownloadParser
    {
        /// <summary>
        /// This methods requires a Download xml file and returns a Download object. It should only by used for a retrieve operation.
        /// </summary>
        static internal ParaObjects.Download DownloadFill(XmlDocument xmlresp, int requestdepth, bool includeAllCustomFields, ParaCredentials ParaCredentials)
        {
            ParaObjects.Download Download = new ParaObjects.Download(true);
            XmlNode DownloadNode = xmlresp.DocumentElement;

            // Setting up the request level for all child items of an account.
            int childDepth = 0;
            if (requestdepth > 0)
            {
                childDepth = requestdepth - 1;
            }
            Download = ParaEntityParser.EntityFill<ParaObjects.Download>(xmlresp);
            Download.FullyLoaded = true;
            return Download;
        }

        /// <summary>
        /// This methods requires a Download list xml file and returns a DownloadsList oject. It should only by used for a List operation.
        /// </summary>
        static internal ParaEntityList<ParaObjects.Download> DownloadsFillList(XmlDocument xmlresp, Boolean MinimalisticLoad, int requestdepth, ParaCredentials ParaCredentials)
        {
            var DownloadsList = new ParaEntityList<ParaObjects.Download>();
            XmlNode DocNode = xmlresp.DocumentElement;

            // Setting up the request level for all child items of a Download.
            int childDepth = 0;
            if (requestdepth > 0)
            {
                childDepth = requestdepth - 1;
            }


            DownloadsList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());


            if (DocNode.Attributes["page-size"] != null)
            {
                // If this is a "TotalOnly" request, there are no other attributes than "Total"

                DownloadsList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                DownloadsList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                DownloadsList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
            }

            foreach (XmlNode xn in DocNode.ChildNodes)
            {
                var xDoc = new XmlDocument();
                xDoc.LoadXml(xn.OuterXml);
                //DownloadsList.Data.Add(DownloadFillNode(xn, childDepth, MinimalisticLoad, ParaCredentials));
                DownloadsList.Data.Add(ParaEntityParser.EntityFill<ParaObjects.Download>(xDoc));
            }
            return DownloadsList;
        }

        internal partial class DownloadFolderParser
        {
            /// <summary>
            /// This methods requires a DownloadFolder xml file and returns a DownloadFolder object. It should only by used for a retrieve operation.
            /// </summary>
            static internal DownloadFolder DownloadFolderFill(XmlDocument xmlresp, int requestdepth, ParaCredentials ParaCredentials)
            {
                DownloadFolder DownloadFolder = new DownloadFolder();
                XmlNode DownloadFolderNode = xmlresp.DocumentElement;

                // Setting up the request level for all child items of an account.
                int childDepth = 0;
                if (requestdepth > 0)
                {
                    childDepth = requestdepth - 1;
                }
                DownloadFolder = DownloadFolderFillNode(DownloadFolderNode, childDepth, ParaCredentials);
                DownloadFolder.FullyLoaded = true;
                return DownloadFolder;
            }

            /// <summary>
            /// This method requires a DownloadFolder list xml file and returns a DownloadFoldersList object. It should only by used for a List operation.
            /// </summary>
            static internal ParaEntityList<ParaObjects.DownloadFolder> DownloadFoldersFillList(XmlDocument xmlresp, int requestdepth, ParaCredentials ParaCredentials)
            {
                var DownloadFoldersList = new ParaEntityList<ParaObjects.DownloadFolder>();
                XmlNode DocNode = xmlresp.DocumentElement;

                // Setting up the request level for all child items of a DownloadFolder.
                int childDepth = 0;
                if (requestdepth > 0)
                {
                    childDepth = requestdepth - 1;
                }


                DownloadFoldersList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());


                if (DocNode.Attributes["page-size"] != null)
                {
                    // If this is a "TotalOnly" request, there are no other attributes than "Total"

                    DownloadFoldersList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                    DownloadFoldersList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                    DownloadFoldersList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
                }


                foreach (XmlNode xn in DocNode.ChildNodes)
                {
                    DownloadFoldersList.Data.Add(DownloadFolderFillNode(xn, childDepth, ParaCredentials));
                }
                return DownloadFoldersList;
            }

            /// <summary>
            /// This method accepts a DownloadFolder node and parses through the different items in it. it can be used to parse a DownloadFolder node, whether the node is returned from a simple read, or as part of a list call.
            /// </summary>
            static internal DownloadFolder DownloadFolderFillNode(XmlNode DownloadFolderNode, int childDepth, ParaCredentials ParaCredentials)
            {

                DownloadFolder DownloadFolder = new DownloadFolder();
                DownloadFolder.Id = Int64.Parse(DownloadFolderNode.Attributes["id"].InnerText.ToString());

                foreach (XmlNode child in DownloadFolderNode.ChildNodes)
                {
                    if (child.LocalName.ToLower() == "is_private")
                    {
                        DownloadFolder.Is_Private = Boolean.Parse(ParserUtils.NodeGetInnerText(child));
                    }

                    if (child.LocalName.ToLower() == "date_updated")
                    {
                        DownloadFolder.Date_Updated = ParserUtils.NodeGetInnerText(child);
                    }

                    if (child.LocalName.ToLower() == "description")
                    {
                        DownloadFolder.Description = ParserUtils.NodeGetInnerText(child);
                    }
                    if (child.LocalName.ToLower() == "name")
                    {
                        DownloadFolder.Name = ParserUtils.NodeGetInnerText(child);
                    }
                    if (child.LocalName.ToLower() == "parent_folder")
                    {
                        DownloadFolder pf = new DownloadFolder();

                        pf.Id = Int64.Parse(child.ChildNodes[0].Attributes["id"].Value.ToString());
                        pf.Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();

                        if (childDepth > 0)
                        {
                            pf = Download.DownloadFolder.GetDetails(Int64.Parse(child.ChildNodes[0].Attributes["id"].Value.ToString()), ParaCredentials, (ParaEnums.RequestDepth)childDepth);
                        }
                        DownloadFolder.Parent_Folder = pf;
                    }
                }
                return DownloadFolder;
            }
        }

    }
}