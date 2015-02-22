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
    /// Contains all the methods that allow you to interact with the Parature Knowledge Base module.
    /// </summary>
    public class Article
    {
        /// <summary>
        /// Provides the Schema of the article module.
        /// </summary>
        public static ParaObjects.Article Schema(ParaCredentials pc)
        {
            ParaObjects.Article Article = new ParaObjects.Article();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetSchema(pc, ParaEnums.ParatureModule.Article);

            if (ar.HasException == false)
            {
                Article = ParaEntityParser.EntityFill<ParaObjects.Article>(ar.XmlReceived);
            }

            Article.ApiCallResponse = ar;
            Article.IsDirty = false;
            return Article;
        }


        /// <summary>
        /// Provides the capability to delete a Article.
        /// </summary>
        /// <param name="articleId">
        /// The id of the Article to delete
        /// </param>
        /// <param name="purge">
        /// If purge is set to true, the Article will be permanently deleted. Otherwise, it will just be 
        /// moved to the trash bin, so it will still be able to be restored from the service desk.
        ///</param>
        public static ApiCallResponse Delete(Int64 articleId, ParaCredentials pc, bool purge)
        {
            return ApiCallFactory.ObjectDelete(pc, ParaEnums.ParatureModule.Article, articleId, purge);
        }

        /// <summary>
        /// Creates a Parature Article. Requires an Object and a credentials object. Will return the Newly Created articleId
        /// </summary>
        public static ApiCallResponse Insert(ParaObjects.Article article, ParaCredentials pc)
        {
            ApiCallResponse ar = new ApiCallResponse();
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc = XmlGenerator.GenerateXml(article);
            ar = ApiCallFactory.ObjectCreateUpdate(pc, ParaEnums.ParatureModule.Article, doc, 0);
            article.Id = ar.Id;
            return ar;
        }

        /// <summary>
        /// Updates a Parature Article. Requires an Object and a credentials object.  Will return the updated articleId
        /// </summary>
        public static ApiCallResponse Update(ParaObjects.Article article, ParaCredentials pc)
        {
            ApiCallResponse ar = new ApiCallResponse();

            ar = ApiCallFactory.ObjectCreateUpdate(pc, ParaEnums.ParatureModule.Article, XmlGenerator.GenerateXml(article), article.Id);


            return ar;
            //return 0;
        }

        /// <summary>
        /// Returns an Article object with all the properties of an Article.
        /// </summary>
        /// <param name="articleId">
        ///The Article number that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="pc">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>
        /// <param name="requestDepth">
        /// For a simple customer request, please put 0. <br/>When Requesting a Customer, there might be related objects linked to that Customer: such as Account, etc. <br/>With a regular Customer detail call, generally only the ID and names of the second level objects are loaded. 
        /// </param>
        public static ParaObjects.Article GetDetails(Int64 articleId, ParaCredentials pc, ParaEnums.RequestDepth requestDepth)
        {
            ParaObjects.Article Article = new ParaObjects.Article();
            Article = FillDetails(articleId, pc, requestDepth, true);

            return Article;

        }

        /// <summary>
        /// Returns an Article object with all of its properties filled.
        /// </summary>
        /// <param name="articleId">
        ///The Article number that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="pc">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>
        public static ParaObjects.Article GetDetails(Int64 articleId, ParaCredentials pc)
        {

            ParaObjects.Article Article = new ParaObjects.Article();
            Article = FillDetails(articleId, pc, ParaEnums.RequestDepth.Standard, true);

            return Article;

        }

        /// <summary>
        /// Returns an article object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="articleXml">
        /// The Article XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaObjects.Article GetDetails(XmlDocument articleXml)
        {
            var article = ParaEntityParser.EntityFill<ParaObjects.Article>(articleXml);
            article.FullyLoaded = true;
            article.ApiCallResponse.XmlReceived = articleXml;
            article.ApiCallResponse.Id = article.Id;

            article.IsDirty = false;
            return article;
        }

        /// <summary>
        /// Returns an article list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="articleListXml">
        /// The Article List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<ParaObjects.Article> GetList(XmlDocument articleListXml)
        {
            var articlesList = ParaEntityParser.FillList<ParaObjects.Article>(articleListXml);

            articlesList.ApiCallResponse.XmlReceived = articleListXml;

            return articlesList;
        }

        /// <summary>
        /// Provides you with the capability to list Customers, following criteria you would set
        /// by instantiating a ModuleQuery.CustomerQuery object
        /// </summary>
        public static ParaEntityList<ParaObjects.Article> GetList(ParaCredentials ParaCredentials, ArticleQuery Query)
        {
            return FillList(ParaCredentials, Query, ParaEnums.RequestDepth.Standard);
        }

        /// <summary>
        /// Provides you with the capability to list Articles, following criteria you would set
        /// by instantiating a ModuleQuery.ArticleQuery object.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>
        public static ParaEntityList<ParaObjects.Article> GetList(ParaCredentials ParaCredentials, ArticleQuery Query, ParaEnums.RequestDepth RequestDepth)
        {
            return FillList(ParaCredentials, Query, RequestDepth);
        }
        /// <summary>
        /// Returns the first 25 Articles returned by the APIs.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>           
        public static ParaEntityList<ParaObjects.Article> GetList(ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
        {
            return FillList(ParaCredentials, null, RequestDepth);
        }
        /// <summary>
        /// Returns the first 25 Articles returned by the APIs.           
        /// </summary>
        public static ParaEntityList<ParaObjects.Article> GetList(ParaCredentials ParaCredentials)
        {
            return FillList(ParaCredentials, null, ParaEnums.RequestDepth.Standard);
        }

        /// <summary>
        /// Fills an Article list object.
        /// </summary>
        private static ParaEntityList<ParaObjects.Article> FillList(ParaCredentials ParaCredentials, ArticleQuery Query, ParaEnums.RequestDepth RequestDepth)
        {
            int requestdepth = (int)RequestDepth;
            if (Query == null)
            {
                Query = new ArticleQuery();
            }


            ApiCallResponse ar = new ApiCallResponse();
            var ArticlesList = new ParaEntityList<ParaObjects.Article>();

            if (Query.RetrieveAllRecords && Query.OptimizePageSize)
            {
                OptimizationResult rslt = ApiUtils.OptimizeObjectPageSize(ArticlesList, Query, ParaCredentials, requestdepth, ParaEnums.ParatureModule.Article);
                ar = rslt.apiResponse;
                Query = (ArticleQuery)rslt.Query;
                ArticlesList = ((ParaEntityList<ParaObjects.Article>)rslt.objectList);
                rslt = null;
            }
            else
            {
                ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Article, Query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    ArticlesList = ParaEntityParser.FillList<ParaObjects.Article>(ar.XmlReceived);
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
                        //ApiCallFactory.waitCheck(pc.Accountid);
                        Query.PageNumber = i;
                        //implement semaphore right here (in the thread pool instance to control the generation of threads
                        instance = new ThreadPool.ObjectList(ParaCredentials, ParaEnums.ParatureModule.Article, Query.BuildQueryArguments());
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
                        var objectlist = new ParaEntityList<ParaObjects.Article>();

                        if (ArticlesList.TotalItems > ArticlesList.Data.Count)
                        {
                            // We still need to pull data

                            // Getting next page's data
                            Query.PageNumber = Query.PageNumber + 1;

                            ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Article, Query.BuildQueryArguments());
                            if (ar.HasException == false)
                            {
                                objectlist = ParaEntityParser.FillList<ParaObjects.Article>(ar.XmlReceived);
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

        static ParaObjects.Article FillDetails(Int64 Articleid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth, bool MinimalisticLoad)
        {
            int requestdepth = (int)RequestDepth;
            ParaObjects.Article article = new ParaObjects.Article();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetDetail<ParaObjects.Article>(ParaCredentials, ParaEnums.ParatureModule.Article, Articleid);
            if (ar.HasException == false)
            {
                article = ParaEntityParser.EntityFill<ParaObjects.Article>(ar.XmlReceived);
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
        public class ArticleFolder
        {
            /// <summary>
            /// Provides the Schema of the article folder entity.
            /// </summary>
            /// <param name="ParaCredentials"></param>
            /// <returns></returns>
            public static ParaObjects.ArticleFolder Schema(ParaCredentials ParaCredentials)
            {
                ParaObjects.ArticleFolder ArticleFolder = new ParaObjects.ArticleFolder();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.EntityGetSchema(ParaCredentials, ParaEnums.ParatureEntity.ArticleFolder);

                if (ar.HasException == false)
                {
                    ArticleFolder = ParaEntityParser.EntityFill<ParaObjects.ArticleFolder>(ar.XmlReceived);
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
                ArticleFolderQuery afQuery = new ArticleFolderQuery();
                afQuery.PageSize = 5000;
                var Folders = new ParaEntityList<ParaObjects.ArticleFolder>();
                Folders = GetList(paracredentials, afQuery);
                foreach (ParaObjects.ArticleFolder folder in Folders.Data)
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
                ArticleFolderQuery afQuery = new ArticleFolderQuery();
                afQuery.AddStaticFieldFilter(ArticleFolderQuery.ArticleFolderStaticFields.Name, ParaEnums.QueryCriteria.Equal, ParentFolderId.ToString());
                afQuery.PageSize = 5000;
                var Folders = new ParaEntityList<ParaObjects.ArticleFolder>();
                Folders = GetList(paracredentials, afQuery);
                foreach (ParaObjects.ArticleFolder folder in Folders.Data)
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
                doc = XmlGenerator.GenerateXml(articleFolder);
                ar = ApiCallFactory.EntityCreateUpdate(ParaCredentials, ParaEnums.ParatureEntity.ArticleFolder, doc, 0);
                articleFolder.Id = ar.Id;
                return ar;
                //return 0;
            }

            /// <summary>
            /// Updates a Parature Article. Requires an Object and a credentials object.  Will return the updated Downloadid. Returns 0 if the Customer update operation fails.
            /// </summary>
            public static ApiCallResponse Update(ParaObjects.ArticleFolder articleFolder, ParaCredentials ParaCredentials)
            {
                ApiCallResponse ar = new ApiCallResponse();

                ar = ApiCallFactory.EntityCreateUpdate(ParaCredentials, ParaEnums.ParatureEntity.ArticleFolder, XmlGenerator.GenerateXml(articleFolder), articleFolder.Id);

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
            public static ParaObjects.ArticleFolder GetDetails(Int64 ArticleFolderid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
            {
                ParaObjects.ArticleFolder ArticleFolder = new ParaObjects.ArticleFolder();
                ArticleFolder = FillDetails(ArticleFolderid, ParaCredentials, RequestDepth);

                return ArticleFolder;

            }

            /// <summary>
            /// Returns an article folder object from a XML Document.  No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="ArticleFolderXml">
            /// The Article Folder XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.ArticleFolder GetDetails(XmlDocument ArticleFolderXml)
            {
                ParaObjects.ArticleFolder articleFolder = new ParaObjects.ArticleFolder();
                articleFolder = ParaEntityParser.EntityFill<ParaObjects.ArticleFolder>(ArticleFolderXml);
                articleFolder.FullyLoaded = true;

                articleFolder.ApiCallResponse.XmlReceived = ArticleFolderXml;
                articleFolder.ApiCallResponse.Id = articleFolder.Id;

                return articleFolder;
            }

            /// <summary>
            /// Provides you with the capability to list Article Folders.
            /// </summary>
            public static ParaEntityList<ParaObjects.ArticleFolder> GetList(ParaCredentials ParaCredentials)
            {
                ArticleFolderQuery eq = new ArticleFolderQuery();
                eq.RetrieveAllRecords = true;
                return FillList(ParaCredentials, eq, ParaEnums.RequestDepth.Standard);
            }

            /// <summary>
            /// Provides you with the capability to list Article Folders, following criteria you would set
            /// by instantiating a ModuleQuery.DownloadQuery object
            /// </summary>
            public static ParaEntityList<ParaObjects.ArticleFolder> GetList(ParaCredentials ParaCredentials, ArticleFolderQuery Query)
            {
                return FillList(ParaCredentials, Query, ParaEnums.RequestDepth.Standard);
            }

            /// <summary>
            /// Provides you with the capability to list Article Folders, following criteria you would set
            /// by instantiating a ModuleQuery.ArticleQuery object.
            /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
            /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
            /// the standard field depth.
            /// </summary>
            public static ParaEntityList<ParaObjects.ArticleFolder> GetList(ParaCredentials ParaCredentials, ArticleFolderQuery Query, ParaEnums.RequestDepth RequestDepth)
            {
                return FillList(ParaCredentials, Query, RequestDepth);
            }

            /// <summary>
            /// Returns an article folder list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="ArticleFoldersListXml">
            /// The Article Folder List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaEntityList<ParaObjects.ArticleFolder> GetList(XmlDocument ArticleFoldersListXml)
            {
                var articleFolderList = new ParaEntityList<ParaObjects.ArticleFolder>();
                articleFolderList = ParaEntityParser.FillList<ParaObjects.ArticleFolder>(ArticleFoldersListXml);

                articleFolderList.ApiCallResponse.XmlReceived = ArticleFoldersListXml;

                return articleFolderList;
            }

            /// <summary>
            /// Fills an Article list object.
            /// </summary>
            private static ParaEntityList<ParaObjects.ArticleFolder> FillList(ParaCredentials ParaCredentials, ArticleFolderQuery Query, ParaEnums.RequestDepth RequestDepth)
            {
                int requestdepth = (int)RequestDepth;
                if (Query == null)
                {
                    Query = new ArticleFolderQuery();
                }

                ApiCallResponse ar = new ApiCallResponse();
                var ArticleFoldersList = new ParaEntityList<ParaObjects.ArticleFolder>();

                ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureEntity.ArticleFolder, Query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    ArticleFoldersList = ParaEntityParser.FillList<ParaObjects.ArticleFolder>(ar.XmlReceived);
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
                                ArticleFoldersList.Data.AddRange(ParaEntityParser.FillList<ParaObjects.ArticleFolder>(ar.XmlReceived).Data);
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

            private static ParaObjects.ArticleFolder FillDetails(Int64 ArticleFolderid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
            {
                int requestdepth = (int)RequestDepth;
                ParaObjects.ArticleFolder ArticleFolder = new ParaObjects.ArticleFolder();
                //Customer = null;
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, ParaEnums.ParatureEntity.ArticleFolder, ArticleFolderid);
                if (ar.HasException == false)
                {
                    ArticleFolder = ParaEntityParser.EntityFill<ParaObjects.ArticleFolder>(ar.XmlReceived);
                    ArticleFolder.FullyLoaded = true;
                }
                else
                {
                    ArticleFolder.FullyLoaded = false;
                    ArticleFolder.Id = 0;
                }

                ArticleFolder.ApiCallResponse = ar;
                return ArticleFolder;
            }


            public static ParaObjects.ArticleFolder GetDetails(Int64 ArticleFolderid, ParaCredentials ParaCredentials)
            {

                ParaObjects.ArticleFolder ArticleFolder = new ParaObjects.ArticleFolder();
                ArticleFolder = FillDetails(ArticleFolderid, ParaCredentials, ParaEnums.RequestDepth.Standard);

                return ArticleFolder;
            }
        }

    }
}