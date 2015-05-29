using System;
using System.Threading;
using System.Xml;
using ParatureSDK.EntityQuery;
using ParatureSDK.ModuleQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Download module.
    /// </summary>
    public static class Download
    {
        /// <summary>
        /// Provides the Schema of the download module.
        /// </summary>
        public static ParaObjects.Download Schema(ParaCredentials creds)
        {
            var download = new ParaObjects.Download(true);
            var ar = ApiCallFactory.ObjectGetSchema(creds, ParaEnums.ParatureModule.Download);

            if (ar.HasException == false)
            {
                var hasMultipleFolders = HasMultipleFoldersAndConvert(ar.XmlReceived);
                var purgedSchema = ApiUtils.RemoveStaticFieldsNodes(ar.XmlReceived);
                download = ParaEntityParser.EntityFill<ParaObjects.Download>(purgedSchema);
                download.MultipleFolders = hasMultipleFolders;
            }

            download.ApiCallResponse = ar;
            return download;
        }

        ///  <summary>
        ///  Provides the capability to delete a Download.
        ///  </summary>
        ///  <param name="downloadId">
        ///  The id of the Download to delete
        ///  </param>
        /// <param name="creds"></param>
        /// <param name="purge">
        ///  If purge is set to true, the Download will be permanently deleted. Otherwise, it will just be 
        ///  moved to the trash bin, so it will still be able to be restored from the service desk.
        /// </param>
        public static ApiCallResponse Delete(Int64 downloadId, ParaCredentials creds, bool purge)
        {
            return ApiCallFactory.ObjectDelete(creds, ParaEnums.ParatureModule.Download, downloadId, purge);
        }

        /// <summary>
        /// Creates a Parature Download. Requires an Object and a credentials object. Will return the Newly Created Downloadid. Returns 0 if the Customer creation fails.
        /// </summary>
        public static ApiCallResponse Insert(ParaObjects.Download download, ParaCredentials creds)
        {
            var doc = XmlGenerator.GenerateXml(download);
            var ar = ApiCallFactory.ObjectCreateUpdate(creds, ParaEnums.ParatureModule.Download, doc, 0);
            download.Id = ar.Id;
            return ar;
        }

        /// <summary>
        /// Updates a Parature Download. Requires an Object and a credentials object.  Will return the updated Downloadid. Returns 0 if the Customer update operation fails.
        /// </summary>
        public static ApiCallResponse Update(ParaObjects.Download download, ParaCredentials creds)
        {
            var ar = ApiCallFactory.ObjectCreateUpdate(creds, ParaEnums.ParatureModule.Download, XmlGenerator.GenerateXml(download), download.Id);

            return ar;
        }

        /// <summary>
        /// Returns a Download object with all of its properties.
        /// </summary>
        /// <param name="downloadId">
        ///The Download number that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="creds">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>
        /// <param name="requestDepth">
        /// For a simple Download request, please put 0. <br/>When Requesting a Download, there might be related objects linked to that Download: such as Products, etc. <br/>With a regular Download detail call, generally only the ID and names of the second level objects are loaded. 
        /// </param>
        public static ParaObjects.Download GetDetails(Int64 downloadId, ParaCredentials creds, ParaEnums.RequestDepth requestDepth)
        {
            return FillDetails(downloadId, creds);
        }

        /// <summary>
        /// Returns a Download object with all of its properties filled.
        /// </summary>
        /// <param name="downloadId">
        ///The Customer number that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="creds">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>
        public static ParaObjects.Download GetDetails(Int64 downloadId, ParaCredentials creds)
        {
            return FillDetails(downloadId, creds);
        }

        internal static Attachment DownloadUploadFile(ParaCredentials ParaCredentials, string text, string contentType, string FileName)
        {
            return ApiUtils.UploadFile(ParaEnums.ParatureModule.Download, ParaCredentials, text, contentType, FileName);
        }

        internal static Attachment DownloadUploadFile(ParaCredentials ParaCredentials, Byte[] Attachment, string contentType, string FileName)
        {
            return ApiUtils.UploadFile(ParaEnums.ParatureModule.Download, ParaCredentials, Attachment, contentType, FileName);
        }

        internal static Attachment DownloadUploadFile(ParaCredentials ParaCredentials, System.Net.Mail.Attachment Attachment)
        {
            return ApiUtils.UploadFile(ParaEnums.ParatureModule.Download, ParaCredentials, Attachment);
        }

        /// <summary>
        /// Returns an download object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="downloadXml">
        /// The Download XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaObjects.Download GetDetails(XmlDocument downloadXml)
        {
            var download = new ParaObjects.Download(true);
            var hasMultipleFolders = HasMultipleFoldersAndConvert(downloadXml);
            download = ParaEntityParser.EntityFill<ParaObjects.Download>(downloadXml);
            download.MultipleFolders = hasMultipleFolders;
            download.FullyLoaded = true;

            download.ApiCallResponse.XmlReceived = downloadXml;
            download.ApiCallResponse.Id = download.Id;

            download.IsDirty = false;
            return download;
        }

        /// <summary>
        /// Returns an download list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="DownloadListXML">
        /// The Download List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<ParaObjects.Download> GetList(XmlDocument DownloadListXML)
        {
            var downloadsList = new ParaEntityList<ParaObjects.Download>();
            downloadsList = ParaEntityParser.FillList<ParaObjects.Download>(DownloadListXML);

            downloadsList.ApiCallResponse.XmlReceived = DownloadListXML;

            return downloadsList;
        }

        /// <summary>
        /// Provides you with the capability to list Downloads, following criteria you would set
        /// by instantiating a ModuleQuery.DownloadQuery object
        /// </summary>
        public static ParaEntityList<ParaObjects.Download> GetList(ParaCredentials ParaCredentials, DownloadQuery Query)
        {
            return FillList(ParaCredentials, Query, ParaEnums.RequestDepth.Standard);
        }

        /// <summary>
        /// Provides you with the capability to list Downloads, following criteria you would set
        /// by instantiating a ModuleQuery.DownloadQuery object.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>
        public static ParaEntityList<ParaObjects.Download> GetList(ParaCredentials ParaCredentials, DownloadQuery Query, ParaEnums.RequestDepth RequestDepth)
        {
            return FillList(ParaCredentials, Query, RequestDepth);
        }
        /// <summary>
        /// Returns a list of the first 25 Downloads returned by the APIs.
        /// </summary>

        public static ParaEntityList<ParaObjects.Download> GetList(ParaCredentials ParaCredentials)
        {
            return FillList(ParaCredentials, null, ParaEnums.RequestDepth.Standard);
        }
        /// <summary>
        /// Returns a list of the first 25 Downloads returned by the APIs.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>

        public static ParaEntityList<ParaObjects.Download> GetList(ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
        {
            return FillList(ParaCredentials, null, RequestDepth);
        }

        /// <summary>
        /// Fills a Download list object.
        /// </summary>
        private static ParaEntityList<ParaObjects.Download> FillList(ParaCredentials ParaCredentials, DownloadQuery Query, ParaEnums.RequestDepth RequestDepth)
        {
            int requestdepth = (int)RequestDepth;
            if (Query == null)
            {
                Query = new DownloadQuery();
            }

            ApiCallResponse ar = new ApiCallResponse();
            var DownloadsList = new ParaEntityList<ParaObjects.Download>();

            if (Query.RetrieveAllRecords && Query.OptimizePageSize)
            {
                OptimizationResult rslt = ApiUtils.OptimizeObjectPageSize(DownloadsList, Query, ParaCredentials, ParaEnums.ParatureModule.Download);
                ar = rslt.apiResponse;
                Query = (DownloadQuery)rslt.Query;
                DownloadsList = ((ParaEntityList<ParaObjects.Download>)rslt.objectList);
                rslt = null;
            }
            else
            {
                ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Download, Query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    DownloadsList = ParaEntityParser.FillListDownload(ar.XmlReceived);
                }
                DownloadsList.ApiCallResponse = ar;
            }

            // Checking if the system needs to recursively call all of the data returned.
            if (Query.RetrieveAllRecords && !ar.HasException)
            {
                // A flag variable to check if we need to make more calls
                if (Query.OptimizeCalls)
                {
                    System.Threading.Thread t;
                    ThreadPool.ObjectList instance = null;
                    int callsRequired = (int)Math.Ceiling((double)(DownloadsList.TotalItems / (double)DownloadsList.PageSize));
                    for (int i = 2; i <= callsRequired; i++)
                    {
                        //ApiCallFactory.waitCheck(ParaCredentials.Accountid);
                        Query.PageNumber = i;
                        //implement semaphore right here (in the thread pool instance to control the generation of threads
                        instance = new ThreadPool.ObjectList(ParaCredentials, ParaEnums.ParatureModule.Download, Query.BuildQueryArguments());
                        t = new System.Threading.Thread(delegate() { instance.Go(DownloadsList); });
                        t.Start();
                    }

                    while (DownloadsList.TotalItems > DownloadsList.Data.Count)
                    {
                        Thread.Sleep(500);
                    }

                    DownloadsList.ResultsReturned = DownloadsList.Data.Count;
                    DownloadsList.PageNumber = callsRequired;
                }
                else
                {
                    bool continueCalling = true;
                    while (continueCalling)
                    {
                        var objectlist = new ParaEntityList<ParaObjects.Download>();

                        if (DownloadsList.TotalItems > DownloadsList.Data.Count)
                        {
                            // We still need to pull data

                            // Getting next page's data
                            Query.PageNumber = Query.PageNumber + 1;

                            ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Download, Query.BuildQueryArguments());

                            if (ar.HasException == false)
                            {
                                objectlist = ParaEntityParser.FillListDownload(ar.XmlReceived);
                                DownloadsList.Data.AddRange(objectlist.Data);
                                DownloadsList.ResultsReturned = DownloadsList.Data.Count;
                                DownloadsList.PageNumber = Query.PageNumber;
                            }
                            else
                            {
                                // There is an error processing request
                                DownloadsList.ApiCallResponse = ar;
                                continueCalling = false;
                            }
                        }
                        else
                        {
                            // That is it, pulled all the items.
                            continueCalling = false;
                            DownloadsList.ApiCallResponse = ar;
                        }
                    }
                }
            }

            return DownloadsList;
        }

        private static ParaObjects.Download FillDetails(Int64 downloadId, ParaCredentials creds)
        {
            //Note: Download folders is WEIRD.
            var download = new ParaObjects.Download(true);
            var ar = ApiCallFactory.ObjectGetDetail<ParaObjects.Download>(creds, ParaEnums.ParatureModule.Download, downloadId);
            if (ar.HasException == false)
            {
                var xmlReceived = ar.XmlReceived;
                var hasMultipleFolders = HasMultipleFoldersAndConvert(xmlReceived);
                download = ParaEntityParser.EntityFill<ParaObjects.Download>(ar.XmlReceived);
                download.MultipleFolders = hasMultipleFolders;
                download.FullyLoaded = true;
            }
            else
            {
                download.FullyLoaded = false;
                download.Id = 0;
            }

            download.ApiCallResponse = ar;
            download.IsDirty = false;
            return download;
        }

        /// <summary>
        /// Modify the XML so it can be parsed, and check to see if it supports multiple folders
        /// </summary>
        /// <param name="xmlReceived"></param>
        /// <returns></returns>
        internal static bool HasMultipleFoldersAndConvert(XmlDocument xmlReceived)
        {
            var foldersNode = xmlReceived.SelectSingleNode("/Download/Folders");
            bool hasMultipleFolders;
            if (foldersNode != null)
            {
                hasMultipleFolders = true;
            }
            else
            {
                var singleFolderNode = xmlReceived.SelectSingleNode("/Download/Folder");
                if (singleFolderNode != null && singleFolderNode.OwnerDocument != null && singleFolderNode.ParentNode != null)
                {
                    //replace the <Folder> with <Folders> for our parser
                    var dlFolders = singleFolderNode.InnerXml;
                    var doc = new XmlDocument();
                    doc.LoadXml(string.Format("<Folders>{0}</Folders>", dlFolders));
                    var newNode = singleFolderNode.OwnerDocument.ImportNode(doc.DocumentElement, true);
                    singleFolderNode.ParentNode.ReplaceChild(newNode, singleFolderNode);
                }
                hasMultipleFolders = false;
            }
            return hasMultipleFolders;
        }

        /// <summary>
        /// Contains all the methods needed to work with the download module's folders.
        /// </summary>
        public class DownloadFolder
        {

            /// <summary>
            /// Locates a folder with the name provided, will return the id if found. Otherwise, it will return 0.
            /// </summary>
            /// <param name="FolderName">
            /// The name of the folder you are looking for.
            /// </param>                
            /// <param name="IgnoreCase">
            /// Whether or not this methods needs to ignore case when looking for the folder name or not.
            /// </param>
            /// <returns></returns>
            public static Int64 FolderGetIdByName(string FolderName, bool IgnoreCase, ParaCredentials paracredentials)
            {
                Int64 id = 0;
                DownloadFolderQuery dfQuery = new DownloadFolderQuery();
                dfQuery.PageSize = 5000;
                var Folders = new ParaEntityList<ParaObjects.DownloadFolder>();
                Folders = GetList(paracredentials, dfQuery);
                foreach (ParaObjects.DownloadFolder folder in Folders.Data)
                {
                    if (String.Compare(folder.Name, FolderName, IgnoreCase) == 0)
                    {
                        id = folder.Id;
                        break;
                    }
                }
                return id;
            }

            /// <summary>
            /// Locates a folder with the name provided and which has the parent folder of your choice, will return the id if found. Otherwise, it will return 0.
            /// </summary>
            /// <param name="FolderName">
            /// The name of the folder you are looking for.
            /// </param>                
            /// <param name="IgnoreCase">
            /// Whether or not this methods needs to ignore case when looking for the folder name or not.
            /// </param>
            /// <param name="ParentFolderId">
            /// The parent folder under which to look for a folder by name.
            /// </param>
            /// <returns></returns>
            public static Int64 FolderGetIdByName(string FolderName, bool IgnoreCase, ParaCredentials paracredentials, Int64 ParentFolderId)
            {
                Int64 id = 0;
                DownloadFolderQuery dfQuery = new DownloadFolderQuery();
                dfQuery.AddStaticFieldFilter(DownloadFolderQuery.DownloadFolderStaticFields.ParentFolder, ParaEnums.QueryCriteria.Equal, ParentFolderId.ToString());
                dfQuery.PageSize = 5000;
                var Folders = new ParaEntityList<ParaObjects.DownloadFolder>();
                Folders = GetList(paracredentials, dfQuery);
                foreach (ParaObjects.DownloadFolder folder in Folders.Data)
                {
                    if (String.Compare(folder.Name, FolderName, IgnoreCase) == 0)
                    {
                        id = folder.Id;
                        break;
                    }
                }
                return id;
            }


            /// <summary>
            /// Creates a Parature Download Folder. Requires an Object and a credentials object. Will return the Newly Created Downloadid. Returns 0 if the Customer creation fails.
            /// </summary>
            public static ApiCallResponse Insert(ParaObjects.DownloadFolder downloadfolder, ParaCredentials ParaCredentials)
            {
                ApiCallResponse ar = new ApiCallResponse();
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc = XmlGenerator.GenerateXml(downloadfolder);
                ar = ApiCallFactory.EntityCreateUpdate(ParaCredentials, ParaEnums.ParatureEntity.DownloadFolder, doc, 0);
                downloadfolder.Id = ar.Id;
                return ar;
            }

            /// <summary>
            /// Updates a Parature Download. Requires an Object and a credentials object.  Will return the updated Downloadid. Returns 0 if the Customer update operation fails.
            /// </summary>
            public static ApiCallResponse Update(ParaObjects.DownloadFolder downloadfolder, ParaCredentials ParaCredentials)
            {
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.EntityCreateUpdate(ParaCredentials, ParaEnums.ParatureEntity.DownloadFolder, XmlGenerator.GenerateXml(downloadfolder), downloadfolder.Id);

                return ar;
            }

            /// <summary>
            /// Returns a Download object with all the properties of a customer.
            /// </summary>
            /// <param name="Downloadid">
            ///The Download number that you would like to get the details of. 
            ///Value Type: <see cref="Int64" />   (System.Int64)
            ///</param>
            /// <param name="ParaCredentials">
            /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            /// </param>
            /// <param name="RequestDepth">
            /// For a simple Download request, please put 0. <br/>When Requesting a Download, there might be related objects linked to that Download: such as Products, etc. <br/>With a regular Download detail call, generally only the ID and names of the second level objects are loaded. 
            /// </param>
            public static ParaObjects.DownloadFolder GetDetails(Int64 DownloadFolderid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
            {
                ParaObjects.DownloadFolder DownloadFolder = new ParaObjects.DownloadFolder();
                DownloadFolder = FillDetails(DownloadFolderid, ParaCredentials, RequestDepth);

                return DownloadFolder;

            }


            /// <summary>
            /// Provides you with the capability to list Downloads, following criteria you would set
            /// by instantiating a ModuleQuery.DownloadQuery object
            /// </summary>
            public static ParaEntityList<ParaObjects.DownloadFolder> GetList(ParaCredentials ParaCredentials, DownloadFolderQuery Query)
            {
                return FillList(ParaCredentials, Query, ParaEnums.RequestDepth.Standard);
            }

            /// <summary>
            /// Provides you with the capability to list Downloads, following criteria you would set
            /// by instantiating a ModuleQuery.DownloadQuery object.
            /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
            /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
            /// the standard field depth.
            /// </summary>
            public static ParaEntityList<ParaObjects.DownloadFolder> GetList(ParaCredentials ParaCredentials, DownloadFolderQuery Query, ParaEnums.RequestDepth RequestDepth)
            {
                return FillList(ParaCredentials, Query, RequestDepth);
            }

            /// <summary>
            /// Fills an Download list object.
            /// </summary>
            private static ParaEntityList<ParaObjects.DownloadFolder> FillList(ParaCredentials ParaCredentials, DownloadFolderQuery Query, ParaEnums.RequestDepth RequestDepth)
            {
                int requestdepth = (int)RequestDepth;
                var DownloadFoldersList = new ParaEntityList<ParaObjects.DownloadFolder>();
                //DownloadsList = null;
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureEntity.DownloadFolder, Query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    DownloadFoldersList = ParaEntityParser.FillList<ParaObjects.DownloadFolder>(ar.XmlReceived);
                }
                DownloadFoldersList.ApiCallResponse = ar;


                // Checking if the system needs to recursively call all of the data returned.
                if (Query.RetrieveAllRecords)
                {
                    bool continueCalling = true;
                    while (continueCalling)
                    {
                        var objectlist = new ParaEntityList<ParaObjects.DownloadFolder>();

                        if (DownloadFoldersList.TotalItems > DownloadFoldersList.Data.Count)
                        {
                            // We still need to pull data

                            // Getting next page's data
                            Query.PageNumber = Query.PageNumber + 1;

                            ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureEntity.DownloadFolder, Query.BuildQueryArguments());

                            objectlist = ParaEntityParser.FillList<ParaObjects.DownloadFolder>(ar.XmlReceived);

                            if (objectlist.Data.Count == 0)
                            {
                                continueCalling = false;
                            }

                            DownloadFoldersList.Data.AddRange(objectlist.Data);
                            DownloadFoldersList.ResultsReturned = DownloadFoldersList.Data.Count;
                            DownloadFoldersList.PageNumber = Query.PageNumber;
                        }
                        else
                        {
                            // That is it, pulled all the items.
                            continueCalling = false;
                            DownloadFoldersList.ApiCallResponse = ar;
                        }
                    }
                }


                return DownloadFoldersList;
            }

            static ParaObjects.DownloadFolder FillDetails(Int64 DownloadFolderid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
            {
                int requestdepth = (int)RequestDepth;
                var downloadFolder = new ParaObjects.DownloadFolder();
                //Customer = null;
                var ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, ParaEnums.ParatureEntity.DownloadFolder, DownloadFolderid);
                if (ar.HasException == false)
                {
                    downloadFolder = ParaEntityParser.EntityFill<ParaObjects.DownloadFolder>(ar.XmlReceived);
                    downloadFolder.FullyLoaded = true;
                }
                else
                {
                    downloadFolder.FullyLoaded = false;
                    downloadFolder.Id = 0;
                }

                downloadFolder.ApiCallResponse = ar;
                return downloadFolder;
            }

            /// <summary>
            /// Returns an downloadFolder object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="DownloadFolderXML">
            /// The DownloadFolder XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.DownloadFolder GetDetails(XmlDocument DownloadFolderXML)
            {
                ParaObjects.DownloadFolder downloadFolder = new ParaObjects.DownloadFolder();
                downloadFolder = ParaEntityParser.EntityFill<ParaObjects.DownloadFolder>(DownloadFolderXML);
                downloadFolder.FullyLoaded = true;

                downloadFolder.ApiCallResponse.XmlReceived = DownloadFolderXML;
                downloadFolder.ApiCallResponse.Id = downloadFolder.Id;

                return downloadFolder;
            }

            /// <summary>
            /// Returns an downloadFolder list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="DownloadFolderListXML">
            /// The DownloadFolder List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaEntityList<ParaObjects.DownloadFolder> GetList(XmlDocument DownloadFolderListXML)
            {
                var downloadFoldersList = new ParaEntityList<ParaObjects.DownloadFolder>();
                downloadFoldersList = ParaEntityParser.FillList<ParaObjects.DownloadFolder>(DownloadFolderListXML);

                downloadFoldersList.ApiCallResponse.XmlReceived = DownloadFolderListXML;

                return downloadFoldersList;
            }

            public static ParaObjects.DownloadFolder GetDetails(Int64 DownloadFolderid, ParaCredentials ParaCredentials)
            {

                ParaObjects.DownloadFolder DownloadFolder = new ParaObjects.DownloadFolder();
                DownloadFolder = FillDetails(DownloadFolderid, ParaCredentials, ParaEnums.RequestDepth.Standard);

                return DownloadFolder;
            }
        }
    }
}