using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using ParatureAPI.PagedData;
using ParatureAPI.ParaObjects;
using ParatureAPI.XmlToObjectParser;

namespace ParatureAPI.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Knowledge Base module.
    /// </summary>
    public class Article
    {
        /// <summary>
        /// Provides the Schema of the article module.
        /// </summary>
        public static ParaObjects.Article ArticleSchema(ParaCredentials ParaCredentials)
        {
            ParaObjects.Article Article = new ParaObjects.Article();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetSchema(ParaCredentials, ParaEnums.ParatureModule.Article);

            if (ar.HasException == false)
            {
                Article = ArticleParser.ArticleFill(ar.xmlReceived, 0, false, ParaCredentials);
            }

            Article.ApiCallResponse = ar;
            Article.IsDirty = false;
            return Article;
        }


        /// <summary>
        /// Provides the capability to delete a Article.
        /// </summary>
        /// <param name="Articleid">
        /// The id of the Article to delete
        /// </param>
        /// <param name="purge">
        /// If purge is set to true, the Article will be permanently deleted. Otherwise, it will just be 
        /// moved to the trash bin, so it will still be able to be restored from the service desk.
        ///</param>
        public static ApiCallResponse ArticleDelete(Int64 Articleid, ParaCredentials ParaCredentials, bool purge)
        {
            return ApiCallFactory.ObjectDelete(ParaCredentials, ParaEnums.ParatureModule.Article, Articleid, purge);
        }

        /// <summary>
        /// Creates a Parature Article. Requires an Object and a credentials object. Will return the Newly Created Articleid
        /// </summary>
        public static ApiCallResponse ArticleInsert(ParaObjects.Article Article, ParaCredentials ParaCredentials)
        {
            ApiCallResponse ar = new ApiCallResponse();
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc = XmlGenerator.ArticleGenerateXml(Article);
            ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, ParaEnums.ParatureModule.Article, doc, 0);
            Article.Id = ar.Objectid;
            Article.uniqueIdentifier = ar.Objectid;
            return ar;
        }

        /// <summary>
        /// Updates a Parature Article. Requires an Object and a credentials object.  Will return the updated Articleid
        /// </summary>
        public static ApiCallResponse ArticleUpdate(ParaObjects.Article Article, ParaCredentials ParaCredentials)
        {
            ApiCallResponse ar = new ApiCallResponse();

            ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, ParaEnums.ParatureModule.Article, XmlGenerator.ArticleGenerateXml(Article), Article.Id);


            return ar;
            //return 0;
        }

        /// <summary>
        /// Returns an Article object with all the properties of an Article.
        /// </summary>
        /// <param name="Articleid">
        ///The Article number that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="ParaCredentials">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>
        /// <param name="RequestDepth">
        /// For a simple customer request, please put 0. <br/>When Requesting a Customer, there might be related objects linked to that Customer: such as Account, etc. <br/>With a regular Customer detail call, generally only the ID and names of the second level objects are loaded. 
        /// </param>
        public static ParaObjects.Article ArticleGetDetails(Int64 Articleid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
        {
            ParaObjects.Article Article = new ParaObjects.Article();
            Article = ArticleFillDetails(Articleid, ParaCredentials, RequestDepth, true);

            return Article;

        }

        /// <summary>
        /// Returns an Article object with all of its properties filled.
        /// </summary>
        /// <param name="Articleid">
        ///The Article number that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="ParaCredentials">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>
        public static ParaObjects.Article ArticleGetDetails(Int64 Articleid, ParaCredentials ParaCredentials)
        {

            ParaObjects.Article Article = new ParaObjects.Article();
            Article = ArticleFillDetails(Articleid, ParaCredentials, ParaEnums.RequestDepth.Standard, true);

            return Article;

        }

        /// <summary>
        /// Returns an article object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="ArticleXML">
        /// The Article XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaObjects.Article ArticleGetDetails(XmlDocument ArticleXML)
        {
            ParaObjects.Article article = new ParaObjects.Article();
            article = ArticleParser.ArticleFill(ArticleXML, 0, true, null);
            article.FullyLoaded = true;

            article.ApiCallResponse.xmlReceived = ArticleXML;
            article.ApiCallResponse.Objectid = article.Id;

            article.IsDirty = false;
            return article;
        }

        /// <summary>
        /// Returns an article list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="ArticleListXML">
        /// The Article List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ArticlesList ArticlesGetList(XmlDocument ArticleListXML)
        {
            ArticlesList articlesList = new ArticlesList();
            articlesList = ArticleParser.ArticlesFillList(ArticleListXML, true, 0, null);

            articlesList.ApiCallResponse.xmlReceived = ArticleListXML;

            return articlesList;
        }

        /// <summary>
        /// Provides you with the capability to list Customers, following criteria you would set
        /// by instantiating a ModuleQuery.CustomerQuery object
        /// </summary>
        public static ArticlesList ArticlesGetList(ParaCredentials ParaCredentials, ModuleQuery.ArticleQuery Query)
        {
            return ArticlesFillList(ParaCredentials, Query, ParaEnums.RequestDepth.Standard);
        }

        /// <summary>
        /// Provides you with the capability to list Articles, following criteria you would set
        /// by instantiating a ModuleQuery.ArticleQuery object.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>
        public static ArticlesList ArticlesGetList(ParaCredentials ParaCredentials, ModuleQuery.ArticleQuery Query, ParaEnums.RequestDepth RequestDepth)
        {
            return ArticlesFillList(ParaCredentials, Query, RequestDepth);
        }
        /// <summary>
        /// Returns the first 25 Articles returned by the APIs.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>           
        public static ArticlesList ArticlesGetList(ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
        {
            return ArticlesFillList(ParaCredentials, null, RequestDepth);
        }
        /// <summary>
        /// Returns the first 25 Articles returned by the APIs.           
        /// </summary>
        public static ArticlesList ArticlesGetList(ParaCredentials ParaCredentials)
        {
            return ArticlesFillList(ParaCredentials, null, ParaEnums.RequestDepth.Standard);
        }

        /// <summary>
        /// Fills an Article list object.
        /// </summary>
        private static ArticlesList ArticlesFillList(ParaCredentials ParaCredentials, ModuleQuery.ArticleQuery Query, ParaEnums.RequestDepth RequestDepth)
        {
            int requestdepth = (int)RequestDepth;
            if (Query == null)
            {
                Query = new ModuleQuery.ArticleQuery();
            }


            ApiCallResponse ar = new ApiCallResponse();
            ArticlesList ArticlesList = new ArticlesList();

            if (Query.RetrieveAllRecords && Query.OptimizePageSize)
            {
                OptimizationResult rslt = ApiUtils.OptimizeObjectPageSize(ArticlesList, Query, ParaCredentials, requestdepth, ParaEnums.ParatureModule.Article);
                ar = rslt.apiResponse;
                Query = (ModuleQuery.ArticleQuery)rslt.Query;
                ArticlesList = ((ArticlesList)rslt.objectList);
                rslt = null;
            }
            else
            {
                ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Article, Query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    ArticlesList = ArticleParser.ArticlesFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);
                }
                ArticlesList.ApiCallResponse = ar;
            }

            // Checking if the system needs to recursively call all of the data returned.
            if (Query.RetrieveAllRecords && !ar.HasException)
            {
                // A flag variable to check if we need to make more calls
                if (Query.OptimizeCalls)
                {
                    System.Threading.Thread t;
                    ThreadPool.ObjectList instance = null;
                    int callsRequired = (int)Math.Ceiling((double)(ArticlesList.TotalItems / (double)ArticlesList.PageSize));
                    for (int i = 2; i <= callsRequired; i++)
                    {
                        //ApiCallFactory.waitCheck(ParaCredentials.Accountid);
                        Query.PageNumber = i;
                        //implement semaphore right here (in the thread pool instance to control the generation of threads
                        instance = new ThreadPool.ObjectList(ParaCredentials, ParaEnums.ParatureModule.Article, Query.BuildQueryArguments(), requestdepth);
                        t = new System.Threading.Thread(delegate() { instance.Go(ArticlesList); });
                        t.Start();
                    }

                    while (ArticlesList.TotalItems > ArticlesList.Data.Count)
                    {
                        Thread.Sleep(500);
                    }

                    ArticlesList.ResultsReturned = ArticlesList.Data.Count;
                    ArticlesList.PageNumber = callsRequired;
                }
                else
                {
                    bool continueCalling = true;
                    while (continueCalling)
                    {
                        ArticlesList objectlist = new ArticlesList();

                        if (ArticlesList.TotalItems > ArticlesList.Data.Count)
                        {
                            // We still need to pull data

                            // Getting next page's data
                            Query.PageNumber = Query.PageNumber + 1;

                            ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Article, Query.BuildQueryArguments());
                            if (ar.HasException == false)
                            {
                                objectlist = ArticleParser.ArticlesFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);
                                ArticlesList.Data.AddRange(objectlist.Data);
                                ArticlesList.ResultsReturned = ArticlesList.Data.Count;
                                ArticlesList.PageNumber = Query.PageNumber;
                            }
                            else
                            {
                                // There is an error processing request
                                ArticlesList.ApiCallResponse = ar;
                                continueCalling = false;
                            }
                        }
                        else
                        {
                            // That is it, pulled all the items.
                            continueCalling = false;
                            ArticlesList.ApiCallResponse = ar;
                        }
                    }
                }
            }

            return ArticlesList;
        }

        static ParaObjects.Article ArticleFillDetails(Int64 Articleid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth, bool MinimalisticLoad)
        {
            int requestdepth = (int)RequestDepth;
            ParaObjects.Article article = new ParaObjects.Article();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, ParaEnums.ParatureModule.Article, Articleid);
            if (ar.HasException == false)
            {
                article = ArticleParser.ArticleFill(ar.xmlReceived, requestdepth, MinimalisticLoad, ParaCredentials);
                article.FullyLoaded = true;
            }
            else
            {
                article.FullyLoaded = false;
                article.Id = 0;
            }
            article.ApiCallResponse = ar;
            return article;
        }

        /// <summary>
        /// Contains all the methods needed to work with the download module's folders.
        /// </summary>
        public partial class ArticleFolder
        {
            /// <summary>
            /// Provides the Schema of the article folder entity.
            /// </summary>
            /// <param name="ParaCredentials"></param>
            /// <returns></returns>
            public static ParaObjects.ArticleFolder ArticleFolderSchema(ParaCredentials ParaCredentials)
            {
                ParaObjects.ArticleFolder ArticleFolder = new ParaObjects.ArticleFolder();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.EntityGetSchema(ParaCredentials, ParaEnums.ParatureEntity.ArticleFolder);

                if (ar.HasException == false)
                {
                    ArticleFolder = ArticleParser.ArticleFolderParser.ArticleFolderFill(ar.xmlReceived, 0, ParaCredentials);
                }
                ArticleFolder.ApiCallResponse = ar;
                return ArticleFolder;
            }

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
                EntityQuery.ArticleFolderQuery afQuery = new EntityQuery.ArticleFolderQuery();
                afQuery.PageSize = 5000;
                var Folders = new ParaEntityList<ParaObjects.ArticleFolder>();
                Folders = ArticleFoldersGetList(paracredentials, afQuery);
                foreach (ParaObjects.ArticleFolder folder in Folders.Data)
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
                EntityQuery.ArticleFolderQuery afQuery = new EntityQuery.ArticleFolderQuery();
                afQuery.AddStaticFieldFilter(EntityQuery.ArticleFolderQuery.ArticleFolderStaticFields.Name, ParaEnums.QueryCriteria.Equal, ParentFolderId.ToString());
                afQuery.PageSize = 5000;
                var Folders = new ParaEntityList<ParaObjects.ArticleFolder>();
                Folders = ArticleFoldersGetList(paracredentials, afQuery);
                foreach (ParaObjects.ArticleFolder folder in Folders.Data)
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
            /// Provides the capability to delete an Article Folder.
            /// </summary>
            /// <param name="articleFolder">
            /// The id of the Article Folder to delete
            /// </param>
            public static ApiCallResponse Delete(Int64 Folderid, ParaCredentials ParaCredentials)
            {
                return ApiCallFactory.EntityDelete(ParaCredentials, ParaEnums.ParatureEntity.ArticleFolder, Folderid);
            }

            /// <summary>
            /// Creates a Parature Article Folder. Requires an Object and a credentials object. Will return the Newly Created Downloadid. Returns 0 if the Customer creation fails.
            /// </summary>
            public static ApiCallResponse Insert(ParaObjects.ArticleFolder articleFolder, ParaCredentials ParaCredentials)
            {
                ApiCallResponse ar = new ApiCallResponse();
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc = XmlGenerator.ArticleFolderGenerateXML(articleFolder);
                ar = ApiCallFactory.EntityCreateUpdate(ParaCredentials, ParaEnums.ParatureEntity.ArticleFolder, doc, 0);
                articleFolder.FolderID = ar.Objectid;
                return ar;
                //return 0;
            }

            /// <summary>
            /// Updates a Parature Article. Requires an Object and a credentials object.  Will return the updated Downloadid. Returns 0 if the Customer update operation fails.
            /// </summary>
            public static ApiCallResponse Update(ParaObjects.ArticleFolder articleFolder, ParaCredentials ParaCredentials)
            {
                ApiCallResponse ar = new ApiCallResponse();

                ar = ApiCallFactory.EntityCreateUpdate(ParaCredentials, ParaEnums.ParatureEntity.ArticleFolder, XmlGenerator.ArticleFolderGenerateXML(articleFolder), articleFolder.FolderID);

                return ar;
                //return 0;
            }

            /// <summary>
            /// Returns a Article object with all the properties of a customer.
            /// </summary>
            /// <param name="ArticleFolderid">
            ///The Article number that you would like to get the details of. 
            ///Value Type: <see cref="Int64" />   (System.Int64)
            ///</param>
            /// <param name="ParaCredentials">
            /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            /// </param>
            /// <param name="RequestDepth">
            /// For a simple Article request, please put 0. <br/>When Requesting a Article, there might be related objects linked to that Article: such as Products, etc. <br/>With a regular Download detail call, generally only the ID and names of the second level objects are loaded. 
            /// </param>
            public static ParaObjects.ArticleFolder ArticleFolderGetDetails(Int64 ArticleFolderid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
            {
                ParaObjects.ArticleFolder ArticleFolder = new ParaObjects.ArticleFolder();
                ArticleFolder = ArticleFolderFillDetails(ArticleFolderid, ParaCredentials, RequestDepth);

                return ArticleFolder;

            }

            /// <summary>
            /// Returns an article folder object from a XML Document.  No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="ArticleFolderXml">
            /// The Article Folder XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.ArticleFolder ArticleFolderGetDetails(XmlDocument ArticleFolderXml)
            {
                ParaObjects.ArticleFolder articleFolder = new ParaObjects.ArticleFolder();
                articleFolder = ArticleParser.ArticleFolderParser.ArticleFolderFill(ArticleFolderXml, 0, null);
                articleFolder.FullyLoaded = true;

                articleFolder.ApiCallResponse.xmlReceived = ArticleFolderXml;
                articleFolder.ApiCallResponse.Objectid = articleFolder.FolderID;

                return articleFolder;
            }

            /// <summary>
            /// Provides you with the capability to list Article Folders.
            /// </summary>
            public static ParaEntityList<ParaObjects.ArticleFolder> ArticleFoldersGetList(ParaCredentials ParaCredentials)
            {
                EntityQuery.ArticleFolderQuery eq = new EntityQuery.ArticleFolderQuery();
                eq.RetrieveAllRecords = true;
                return ArticleFoldersFillList(ParaCredentials, eq, ParaEnums.RequestDepth.Standard);
            }

            /// <summary>
            /// Provides you with the capability to list Article Folders, following criteria you would set
            /// by instantiating a ModuleQuery.DownloadQuery object
            /// </summary>
            public static ParaEntityList<ParaObjects.ArticleFolder> ArticleFoldersGetList(ParaCredentials ParaCredentials, EntityQuery.ArticleFolderQuery Query)
            {
                return ArticleFoldersFillList(ParaCredentials, Query, ParaEnums.RequestDepth.Standard);
            }

            /// <summary>
            /// Provides you with the capability to list Article Folders, following criteria you would set
            /// by instantiating a ModuleQuery.ArticleQuery object.
            /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
            /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
            /// the standard field depth.
            /// </summary>
            public static ParaEntityList<ParaObjects.ArticleFolder> ArticleFoldersGetList(ParaCredentials ParaCredentials, EntityQuery.ArticleFolderQuery Query, ParaEnums.RequestDepth RequestDepth)
            {
                return ArticleFoldersFillList(ParaCredentials, Query, RequestDepth);
            }

            /// <summary>
            /// Returns an article folder list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="ArticleFoldersListXml">
            /// The Article Folder List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaEntityList<ParaObjects.ArticleFolder> ArticleFoldersGetList(XmlDocument ArticleFoldersListXml)
            {
                var articleFolderList = new ParaEntityList<ParaObjects.ArticleFolder>();
                articleFolderList = ArticleParser.ArticleFolderParser.ArticleFoldersFillList(ArticleFoldersListXml, 0, null);

                articleFolderList.ApiCallResponse.xmlReceived = ArticleFoldersListXml;

                return articleFolderList;
            }

            /// <summary>
            /// Fills an Article list object.
            /// </summary>
            private static ParaEntityList<ParaObjects.ArticleFolder> ArticleFoldersFillList(ParaCredentials ParaCredentials, EntityQuery.ArticleFolderQuery Query, ParaEnums.RequestDepth RequestDepth)
            {
                int requestdepth = (int)RequestDepth;
                if (Query == null)
                {
                    Query = new EntityQuery.ArticleFolderQuery();
                }

                ApiCallResponse ar = new ApiCallResponse();
                var ArticleFoldersList = new ParaEntityList<ParaObjects.ArticleFolder>();

                ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureEntity.ArticleFolder, Query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    ArticleFoldersList = ArticleParser.ArticleFolderParser.ArticleFoldersFillList(ar.xmlReceived, requestdepth, ParaCredentials);
                }

                ArticleFoldersList.ApiCallResponse = ar;

                // Checking if the system needs to recursively call all of the data returned.
                if (Query.RetrieveAllRecords && !ar.HasException)
                {
                    bool continueCalling = true;
                    while (continueCalling)
                    {

                        if (ArticleFoldersList.TotalItems > ArticleFoldersList.Data.Count)
                        {
                            // Getting next page's data
                            Query.PageNumber = Query.PageNumber + 1;

                            ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureEntity.ArticleFolder, Query.BuildQueryArguments());

                            if (ar.HasException == false)
                            {
                                ArticleFoldersList.Data.AddRange(ArticleParser.ArticleFolderParser.ArticleFoldersFillList(ar.xmlReceived, requestdepth, ParaCredentials).Data);
                                ArticleFoldersList.ResultsReturned = ArticleFoldersList.Data.Count;
                                ArticleFoldersList.PageNumber = Query.PageNumber;
                            }
                            else
                            {
                                // There is an error processing request
                                ArticleFoldersList.ApiCallResponse = ar;
                                continueCalling = false;
                            }
                        }
                        else
                        {
                            // That is it, pulled all the items.
                            continueCalling = false;
                            ArticleFoldersList.ApiCallResponse = ar;
                        }
                    }
                }

                return ArticleFoldersList;
            }

            private static ParaObjects.ArticleFolder ArticleFolderFillDetails(Int64 ArticleFolderid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
            {
                int requestdepth = (int)RequestDepth;
                ParaObjects.ArticleFolder ArticleFolder = new ParaObjects.ArticleFolder();
                //Customer = null;
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, ParaEnums.ParatureEntity.ArticleFolder, ArticleFolderid);
                if (ar.HasException == false)
                {
                    ArticleFolder = ArticleParser.ArticleFolderParser.ArticleFolderFill(ar.xmlReceived, requestdepth, ParaCredentials);
                    ArticleFolder.FullyLoaded = true;
                }
                else
                {
                    ArticleFolder.FullyLoaded = false;
                    ArticleFolder.FolderID = 0;
                }

                ArticleFolder.ApiCallResponse = ar;
                return ArticleFolder;
            }


            public static ParaObjects.ArticleFolder ArticleFolderGetDetails(Int64 ArticleFolderid, ParaCredentials ParaCredentials)
            {

                ParaObjects.ArticleFolder ArticleFolder = new ParaObjects.ArticleFolder();
                ArticleFolder = ArticleFolderFillDetails(ArticleFolderid, ParaCredentials, ParaEnums.RequestDepth.Standard);

                return ArticleFolder;
            }
        }

    }
}