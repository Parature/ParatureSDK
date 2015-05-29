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
    /// Contains all the methods that allow you to interact with the Parature Product module.
    /// </summary>
    public class Product : FirstLevelApiHandler<ParaObjects.Product, ProductQuery>
    {
        private static ParaEnums.ParatureModule _module = ParaEnums.ParatureModule.Product;

        /// <summary>
        /// Contains all the methods needed to work with the download module's folders.
        /// </summary>
        public class ProductFolder
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
                Folders = Download.DownloadFolder.GetList(paracredentials, dfQuery);
                foreach (DownloadFolder folder in Folders.Data)
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
                Folders = Download.DownloadFolder.GetList(paracredentials, dfQuery);
                foreach (DownloadFolder folder in Folders.Data)
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
            public static ApiCallResponse Insert(ParaObjects.ProductFolder ProductFolder, ParaCredentials ParaCredentials)
            {
                ApiCallResponse ar = new ApiCallResponse();
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc = XmlGenerator.GenerateXml(ProductFolder);
                ar = ApiCallFactory.EntityCreateUpdate(ParaCredentials, ParaEnums.ParatureEntity.ProductFolder, doc, 0);
                ProductFolder.Id = ar.Id;
                return ar;
            }

            /// <summary>
            /// Updates a Parature Product Folder. Requires an Object and a credentials object.  Will return the updated ProductFolderid. Returns 0 if the update operation failed.
            /// </summary>
            public static ApiCallResponse Update(ParaObjects.ProductFolder ProductFolder, ParaCredentials ParaCredentials)
            {
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, ParaEnums.ParatureModule.Product, XmlGenerator.GenerateXml(ProductFolder), ProductFolder.Id);
                return ar;
            }

            public static ApiCallResponse Delete(long objectId, ParaCredentials ParaCredentials)
            {
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.EntityDelete(ParaCredentials, ParaEnums.ParatureEntity.ProductFolder, objectId);
                return ar;
            }

            /// <summary>
            /// Returns a ProductFolder object with all the properties filled.
            /// </summary>
            /// <param name="ProductFolderid">
            ///The Download number that you would like to get the details of. 
            ///Value Type: <see cref="Int64" />   (System.Int64)
            ///</param>
            /// <param name="ParaCredentials">
            /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            /// </param>
            /// <param name="RequestDepth">
            /// For a simple Download request, please put 0. <br/>When Requesting a Download, there might be related objects linked to that Download: such as Products, etc. <br/>With a regular Download detail call, generally only the ID and names of the second level objects are loaded. 
            /// </param>
            public static ParaObjects.ProductFolder GetDetails(Int64 ProductFolderid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
            {
                ParaObjects.ProductFolder ProductFolder = new ParaObjects.ProductFolder();
                ProductFolder = FillDetails(ProductFolderid, ParaCredentials, RequestDepth);

                return ProductFolder;

            }

            /// <summary>
            /// Returns an productFolder object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="ProductFolderXML">
            /// The ProductFolder XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.ProductFolder GetDetails(XmlDocument ProductFolderXML)
            {
                ParaObjects.ProductFolder productFolder = new ParaObjects.ProductFolder();
                productFolder = ParaEntityParser.EntityFill<ParaObjects.ProductFolder>(ProductFolderXML);
                productFolder.FullyLoaded = true;

                productFolder.ApiCallResponse.XmlReceived = ProductFolderXML;
                productFolder.ApiCallResponse.Id = productFolder.Id;

                return productFolder;
            }

            /// <summary>
            /// Provides you with the capability to list Product Folders
            /// </summary>
            public static ParaEntityList<ParaObjects.ProductFolder> GetList(ParaCredentials ParaCredentials)
            {
                return FillList(ParaCredentials, new ProductFolderQuery(), ParaEnums.RequestDepth.Standard);
            }

            /// <summary>
            /// Provides you with the capability to list Downloads, following criteria you would set
            /// by instantiating a ModuleQuery.DownloadQuery object
            /// </summary>
            public static ParaEntityList<ParaObjects.ProductFolder> GetList(ParaCredentials ParaCredentials, ProductFolderQuery Query)
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
            public static ParaEntityList<ParaObjects.ProductFolder> GetList(ParaCredentials ParaCredentials, ProductFolderQuery Query, ParaEnums.RequestDepth RequestDepth)
            {
                return FillList(ParaCredentials, Query, RequestDepth);
            }

            /// <summary>
            /// Returns an productFolder list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="ProductFolderListXML">
            /// The ProductFolder List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaEntityList<ParaObjects.ProductFolder> GetList(XmlDocument ProductFolderListXML)
            {
                var productFoldersList = new ParaEntityList<ParaObjects.ProductFolder>();
                productFoldersList = ParaEntityParser.FillList<ParaObjects.ProductFolder>(ProductFolderListXML);

                productFoldersList.ApiCallResponse.XmlReceived = ProductFolderListXML;

                return productFoldersList;
            }

            /// <summary>
            /// Fills a ProductFolderList object.
            /// </summary>
            private static ParaEntityList<ParaObjects.ProductFolder> FillList(ParaCredentials ParaCredentials, ProductFolderQuery Query, ParaEnums.RequestDepth RequestDepth)
            {
                int requestdepth = (int)RequestDepth;
                if (Query == null)
                {
                    Query = new ProductFolderQuery();
                }

                ApiCallResponse ar = new ApiCallResponse();
                var ProductFoldersList = new ParaEntityList<ParaObjects.ProductFolder>();

                if (Query.RetrieveAllRecords && Query.OptimizePageSize)
                {
                }
                else
                {
                    ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureEntity.ProductFolder, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        ProductFoldersList = ParaEntityParser.FillList<ParaObjects.ProductFolder>(ar.XmlReceived);
                    }
                    ProductFoldersList.ApiCallResponse = ar;
                }

                // Checking if the system needs to recursively call all of the data returned.
                if (Query.RetrieveAllRecords && !ar.HasException)
                {
                    bool continueCalling = true;
                    while (continueCalling)
                    {

                        if (ProductFoldersList.TotalItems > ProductFoldersList.Data.Count)
                        {
                            // We still need to pull data

                            // Getting next page's data
                            Query.PageNumber = Query.PageNumber + 1;

                            ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureEntity.ProductFolder, Query.BuildQueryArguments());
                            ProductFoldersList.Data.AddRange(ParaEntityParser.FillList<ParaObjects.ProductFolder>(ar.XmlReceived).Data);
                            ProductFoldersList.ResultsReturned = ProductFoldersList.Data.Count;
                            ProductFoldersList.PageNumber = Query.PageNumber;
                        }
                        else
                        {
                            // That is it, pulled all the items.
                            continueCalling = false;
                            ProductFoldersList.ApiCallResponse = ar;
                        }
                        /*
                            }
                            */ 
                    }
                }

                return ProductFoldersList;
            }

            static ParaObjects.ProductFolder FillDetails(Int64 ProductFolderid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
            {
                int requestdepth = (int)RequestDepth;
                ParaObjects.ProductFolder ProductFolder = new ParaObjects.ProductFolder();
                //Customer = null;
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, ParaEnums.ParatureEntity.ProductFolder, ProductFolderid);
                if (ar.HasException == false)
                {
                    ProductFolder = ParaEntityParser.EntityFill<ParaObjects.ProductFolder>(ar.XmlReceived);
                    ProductFolder.FullyLoaded = true;
                }
                else
                {
                    ProductFolder.FullyLoaded = false;
                    ProductFolder.Id = 0;
                }

                ProductFolder.ApiCallResponse = ar;
                return ProductFolder;
            }


            public static ParaObjects.ProductFolder GetDetails(Int64 ProductFolderid, ParaCredentials ParaCredentials)
            {

                ParaObjects.ProductFolder ProductFolder = new ParaObjects.ProductFolder();
                ProductFolder = FillDetails(ProductFolderid, ParaCredentials, ParaEnums.RequestDepth.Standard);

                return ProductFolder;
            }
        }
    }
}