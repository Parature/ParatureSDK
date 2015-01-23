using System;
using System.Threading;
using System.Xml;
using ParatureAPI.PagedData;
using ParatureAPI.ParaObjects;
using ParatureAPI.XmlToObjectParser;

namespace ParatureAPI.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Download module.
    /// </summary>
    public class Download
    {
        /// <summary>
        /// Provides the Schema of the download module.
        /// </summary>
        public static ParaObjects.Download DownloadSchema(ParaCredentials ParaCredentials)
        {
            ParaObjects.Download Download = new ParaObjects.Download(true);
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetSchema(ParaCredentials, ParaEnums.ParatureModule.Download);

            if (ar.HasException == false)
            {
                Download = DownloadParser.DownloadFill(ar.xmlReceived, 0, false, ParaCredentials);
            }

            Download.ApiCallResponse = ar;
            return Download;
        }

        /// <summary>
        /// Provides the capability to delete a Download.
        /// </summary>
        /// <param name="Downloadid">
        /// The id of the Download to delete
        /// </param>
        /// <param name="purge">
        /// If purge is set to true, the Download will be permanently deleted. Otherwise, it will just be 
        /// moved to the trash bin, so it will still be able to be restored from the service desk.
        ///</param>
        public static ApiCallResponse DownloadDelete(Int64 Downloadid, ParaCredentials ParaCredentials, bool purge)
        {
            return ApiCallFactory.ObjectDelete(ParaCredentials, ParaEnums.ParatureModule.Download, Downloadid, purge);
        }

        /// <summary>
        /// Creates a Parature Download. Requires an Object and a credentials object. Will return the Newly Created Downloadid. Returns 0 if the Customer creation fails.
        /// </summary>
        public static ApiCallResponse DownloadInsert(ParaObjects.Download Download, ParaCredentials ParaCredentials)
        {
            ApiCallResponse ar = new ApiCallResponse();
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc = XmlGenerator.DownloadGenerateXML(Download);
            ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, ParaEnums.ParatureModule.Download, doc, 0);
            Download.Id = ar.Objectid;
            Download.uniqueIdentifier = ar.Objectid;
            return ar;
        }

        /// <summary>
        /// Updates a Parature Download. Requires an Object and a credentials object.  Will return the updated Downloadid. Returns 0 if the Customer update operation fails.
        /// </summary>
        public static ApiCallResponse DownloadUpdate(ParaObjects.Download Download, ParaCredentials ParaCredentials)
        {
            ApiCallResponse ar = new ApiCallResponse();

            ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, ParaEnums.ParatureModule.Download, XmlGenerator.DownloadGenerateXML(Download), Download.Id);


            return ar;
            //return 0;
        }

        /// <summary>
        /// Returns a Download object with all of its properties.
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
        public static ParaObjects.Download DownloadGetDetails(Int64 Downloadid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
        {
            return DownloadFillDetails(Downloadid, ParaCredentials, RequestDepth, true);
        }

        /// <summary>
        /// Returns a Download object with all of its properties filled.
        /// </summary>
        /// <param name="Downloadid">
        ///The Customer number that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="ParaCredentials">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>
        public static ParaObjects.Download DownloadGetDetails(Int64 Downloadid, ParaCredentials ParaCredentials)
        {
            return DownloadFillDetails(Downloadid, ParaCredentials, ParaEnums.RequestDepth.Standard, true);
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
        /// <param name="DownloadXML">
        /// The Download XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaObjects.Download DownloadGetDetails(XmlDocument DownloadXML)
        {
            var download = new ParaObjects.Download(true);
            download = DownloadParser.DownloadFill(DownloadXML, 0, true, null);
            download.FullyLoaded = true;

            download.ApiCallResponse.xmlReceived = DownloadXML;
            download.ApiCallResponse.Objectid = download.Id;

            download.IsDirty = false;
            return download;
        }

        /// <summary>
        /// Returns an download list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="DownloadListXML">
        /// The Download List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<ParaObjects.Download> DownloadsGetList(XmlDocument DownloadListXML)
        {
            var downloadsList = new ParaEntityList<ParaObjects.Download>();
            downloadsList = DownloadParser.DownloadsFillList(DownloadListXML, true, 0, null);

            downloadsList.ApiCallResponse.xmlReceived = DownloadListXML;

            return downloadsList;
        }

        /// <summary>
        /// Provides you with the capability to list Downloads, following criteria you would set
        /// by instantiating a ModuleQuery.DownloadQuery object
        /// </summary>
        public static ParaEntityList<ParaObjects.Download> DownloadsGetList(ParaCredentials ParaCredentials, ModuleQuery.DownloadQuery Query)
        {
            return DownloadsFillList(ParaCredentials, Query, ParaEnums.RequestDepth.Standard);
        }

        /// <summary>
        /// Provides you with the capability to list Downloads, following criteria you would set
        /// by instantiating a ModuleQuery.DownloadQuery object.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>
        public static ParaEntityList<ParaObjects.Download> DownloadsGetList(ParaCredentials ParaCredentials, ModuleQuery.DownloadQuery Query, ParaEnums.RequestDepth RequestDepth)
        {
            return DownloadsFillList(ParaCredentials, Query, RequestDepth);
        }
        /// <summary>
        /// Returns a list of the first 25 Downloads returned by the APIs.
        /// </summary>

        public static ParaEntityList<ParaObjects.Download> DownloadsGetList(ParaCredentials ParaCredentials)
        {
            return DownloadsFillList(ParaCredentials, null, ParaEnums.RequestDepth.Standard);
        }
        /// <summary>
        /// Returns a list of the first 25 Downloads returned by the APIs.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>

        public static ParaEntityList<ParaObjects.Download> DownloadsGetList(ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
        {
            return DownloadsFillList(ParaCredentials, null, RequestDepth);
        }

        /// <summary>
        /// Fills a Download list object.
        /// </summary>
        private static ParaEntityList<ParaObjects.Download> DownloadsFillList(ParaCredentials ParaCredentials, ModuleQuery.DownloadQuery Query, ParaEnums.RequestDepth RequestDepth)
        {
            int requestdepth = (int)RequestDepth;
            if (Query == null)
            {
                Query = new ModuleQuery.DownloadQuery();
            }


            ApiCallResponse ar = new ApiCallResponse();
            var DownloadsList = new ParaEntityList<ParaObjects.Download>();

            if (Query.RetrieveAllRecords && Query.OptimizePageSize)
            {
                OptimizationResult rslt = ApiUtils.OptimizeObjectPageSize(DownloadsList, Query, ParaCredentials, requestdepth, ParaEnums.ParatureModule.Download);
                ar = rslt.apiResponse;
                Query = (ModuleQuery.DownloadQuery)rslt.Query;
                DownloadsList = ((ParaEntityList<ParaObjects.Download>)rslt.objectList);
                rslt = null;
            }
            else
            {
                ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Download, Query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    DownloadsList = DownloadParser.DownloadsFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);
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
                        instance = new ThreadPool.ObjectList(ParaCredentials, ParaEnums.ParatureModule.Download, Query.BuildQueryArguments(), requestdepth);
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
                                objectlist = DownloadParser.DownloadsFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);
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

        private static ParaObjects.Download DownloadFillDetails(Int64 Downloadid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth, bool MinimalisticLoad)
        {
            int requestdepth = (int)RequestDepth;
            ParaObjects.Download Download = new ParaObjects.Download(true);
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, ParaEnums.ParatureModule.Download, Downloadid);
            if (ar.HasException == false)
            {
                Download = DownloadParser.DownloadFill(ar.xmlReceived, requestdepth, MinimalisticLoad, ParaCredentials);
                Download.FullyLoaded = true;
            }
            else
            {
                Download.FullyLoaded = false;
                Download.Id = 0;
            }

            Download.ApiCallResponse = ar;
            Download.IsDirty = false;
            return Download;
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
                EntityQuery.DownloadFolderQuery dfQuery = new EntityQuery.DownloadFolderQuery();
                dfQuery.PageSize = 5000;
                var Folders = new ParaEntityList<ParaObjects.DownloadFolder>();
                Folders = DownloadFoldersGetList(paracredentials, dfQuery);
                foreach (ParaObjects.DownloadFolder folder in Folders.Data)
                {
                    if (String.Compare(folder.Name, FolderName, IgnoreCase) == 0)
                    {
                        id = folder.FolderID;
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
                EntityQuery.DownloadFolderQuery dfQuery = new EntityQuery.DownloadFolderQuery();
                dfQuery.AddStaticFieldFilter(EntityQuery.DownloadFolderQuery.DownloadFolderStaticFields.ParentFolder, ParaEnums.QueryCriteria.Equal, ParentFolderId.ToString());
                dfQuery.PageSize = 5000;
                var Folders = new ParaEntityList<ParaObjects.DownloadFolder>();
                Folders = DownloadFoldersGetList(paracredentials, dfQuery);
                foreach (ParaObjects.DownloadFolder folder in Folders.Data)
                {
                    if (String.Compare(folder.Name, FolderName, IgnoreCase) == 0)
                    {
                        id = folder.FolderID;
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
                doc = XmlGenerator.DownloadFolderGenerateXML(downloadfolder);
                ar = ApiCallFactory.EntityCreateUpdate(ParaCredentials, ParaEnums.ParatureEntity.DownloadFolder, doc, 0);
                downloadfolder.FolderID = ar.Objectid;
                return ar;
            }

            /// <summary>
            /// Updates a Parature Download. Requires an Object and a credentials object.  Will return the updated Downloadid. Returns 0 if the Customer update operation fails.
            /// </summary>
            public static ApiCallResponse Update(ParaObjects.DownloadFolder downloadfolder, ParaCredentials ParaCredentials)
            {
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.EntityCreateUpdate(ParaCredentials, ParaEnums.ParatureEntity.DownloadFolder, XmlGenerator.DownloadFolderGenerateXML(downloadfolder), downloadfolder.FolderID);

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
            public static ParaObjects.DownloadFolder DownloadFolderGetDetails(Int64 DownloadFolderid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
            {
                ParaObjects.DownloadFolder DownloadFolder = new ParaObjects.DownloadFolder();
                DownloadFolder = DownloadFolderFillDetails(DownloadFolderid, ParaCredentials, RequestDepth);

                return DownloadFolder;

            }


            /// <summary>
            /// Provides you with the capability to list Downloads, following criteria you would set
            /// by instantiating a ModuleQuery.DownloadQuery object
            /// </summary>
            public static ParaEntityList<ParaObjects.DownloadFolder> DownloadFoldersGetList(ParaCredentials ParaCredentials, EntityQuery.DownloadFolderQuery Query)
            {
                return DownloadFoldersFillList(ParaCredentials, Query, ParaEnums.RequestDepth.Standard);
            }

            /// <summary>
            /// Provides you with the capability to list Downloads, following criteria you would set
            /// by instantiating a ModuleQuery.DownloadQuery object.
            /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
            /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
            /// the standard field depth.
            /// </summary>
            public static ParaEntityList<ParaObjects.DownloadFolder> DownloadFoldersGetList(ParaCredentials ParaCredentials, EntityQuery.DownloadFolderQuery Query, ParaEnums.RequestDepth RequestDepth)
            {
                return DownloadFoldersFillList(ParaCredentials, Query, RequestDepth);
            }

            /// <summary>
            /// Fills an Download list object.
            /// </summary>
            private static ParaEntityList<ParaObjects.DownloadFolder> DownloadFoldersFillList(ParaCredentials ParaCredentials, EntityQuery.DownloadFolderQuery Query, ParaEnums.RequestDepth RequestDepth)
            {
                int requestdepth = (int)RequestDepth;
                var DownloadFoldersList = new ParaEntityList<ParaObjects.DownloadFolder>();
                //DownloadsList = null;
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureEntity.DownloadFolder, Query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    DownloadFoldersList = DownloadParser.DownloadFolderParser.DownloadFoldersFillList(ar.xmlReceived, requestdepth, ParaCredentials);
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

                            objectlist = DownloadParser.DownloadFolderParser.DownloadFoldersFillList(ar.xmlReceived,0, ParaCredentials);

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

            static ParaObjects.DownloadFolder DownloadFolderFillDetails(Int64 DownloadFolderid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
            {
                int requestdepth = (int)RequestDepth;
                ParaObjects.DownloadFolder DownloadFolder = new ParaObjects.DownloadFolder();
                //Customer = null;
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, ParaEnums.ParatureEntity.DownloadFolder, DownloadFolderid);
                if (ar.HasException == false)
                {
                    DownloadFolder = DownloadParser.DownloadFolderParser.DownloadFolderFill(ar.xmlReceived, requestdepth, ParaCredentials);
                    DownloadFolder.FullyLoaded = true;
                }
                else
                {
                    DownloadFolder.FullyLoaded = false;
                    DownloadFolder.FolderID = 0;
                }

                DownloadFolder.ApiCallResponse = ar;
                return DownloadFolder;
            }

            /// <summary>
            /// Returns an downloadFolder object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="DownloadFolderXML">
            /// The DownloadFolder XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.DownloadFolder DownloadFolderGetDetails(XmlDocument DownloadFolderXML)
            {
                ParaObjects.DownloadFolder downloadFolder = new ParaObjects.DownloadFolder();
                downloadFolder = DownloadParser.DownloadFolderParser.DownloadFolderFill(DownloadFolderXML, 0, null);
                downloadFolder.FullyLoaded = true;

                downloadFolder.ApiCallResponse.xmlReceived = DownloadFolderXML;
                downloadFolder.ApiCallResponse.Objectid = downloadFolder.FolderID;

                return downloadFolder;
            }

            /// <summary>
            /// Returns an downloadFolder list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="DownloadFolderListXML">
            /// The DownloadFolder List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaEntityList<ParaObjects.DownloadFolder> DownloadFoldersGetList(XmlDocument DownloadFolderListXML)
            {
                var downloadFoldersList = new ParaEntityList<ParaObjects.DownloadFolder>();
                downloadFoldersList = DownloadParser.DownloadFolderParser.DownloadFoldersFillList(DownloadFolderListXML, 0, null);

                downloadFoldersList.ApiCallResponse.xmlReceived = DownloadFolderListXML;

                return downloadFoldersList;
            }

            public static ParaObjects.DownloadFolder DownloadFolderGetDetails(Int64 DownloadFolderid, ParaCredentials ParaCredentials)
            {

                ParaObjects.DownloadFolder DownloadFolder = new ParaObjects.DownloadFolder();
                DownloadFolder = DownloadFolderFillDetails(DownloadFolderid, ParaCredentials, ParaEnums.RequestDepth.Standard);

                return DownloadFolder;
            }
        }
    }
}