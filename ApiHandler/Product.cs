using System;
using System.Threading;
using System.Xml;
using ParatureAPI.PagedData;
using ParatureAPI.ParaObjects;
using ParatureAPI.XmlToObjectParser;

namespace ParatureAPI.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Product module.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Provides the Schema of the Product module.
        /// </summary>
        public static ParaObjects.Product ProductSchema(ParaCredentials ParaCredentials)
        {
            ParaObjects.Product Product = new ParaObjects.Product();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetSchema(ParaCredentials, ParaEnums.ParatureModule.Product);

            if (ar.HasException == false)
            {
                Product = ProductParser.ProductFill(ar.xmlReceived, 0, false, ParaCredentials);
            }
            Product.ApiCallResponse = ar;
            return Product;
        }

        /// <summary>
        /// Gets an empty object with the scheam (custom fields, if any).  This call will also try to create a dummy
        /// record in order to determine if any of the custom fields have special validation rules (e.g. email, phone, url)
        /// and set the "dataType" of the custom field accordingly.
        /// </summary> 
        static public ParaObjects.Product ProductSchemaWithCustomFieldTypes(ParaCredentials ParaCredentials)
        {
            ParaObjects.Product Product = ProductSchema(ParaCredentials);

            Product = (ParaObjects.Product)ApiCallFactory.ObjectCheckCustomFieldTypes(ParaCredentials, ParaEnums.ParatureModule.Product, Product);

            return Product;
        }

        /// <summary>
        /// Provides the capability to delete a Product.
        /// </summary>
        /// <param name="Productid">
        /// The id of the Product to delete
        /// </param>
        /// <param name="purge">
        /// If purge is set to true, the Product will be permanently deleted. Otherwise, it will just be 
        /// moved to the trash bin, so it will still be able to be restored from the service desk.
        ///</param>
        public static ApiCallResponse ProductDelete(Int64 Productid, ParaCredentials ParaCredentials, bool purge)
        {
            return ApiCallFactory.ObjectDelete(ParaCredentials, ParaEnums.ParatureModule.Product, Productid, purge);
        }

        /// <summary>
        /// Creates a Parature Product. Requires an Object and a credentials object. Will return the Newly Created Productid. Returns 0 if the Product creation failed.
        /// </summary>
        public static ApiCallResponse ProductInsert(ParaObjects.Product Product, ParaCredentials ParaCredentials)
        {
            ApiCallResponse ar = new ApiCallResponse();
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc = XmlGenerator.ProductGenerateXml(Product);
            ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, ParaEnums.ParatureModule.Product, doc, 0);
            Product.Id = ar.Objectid;
            Product.uniqueIdentifier = ar.Objectid;
            return ar;
        }

        /// <summary>
        /// Updates a Parature Product. Requires an Object and a credentials object.  Will return the updated Productid. Returns 0 if the Product update operation failed.
        /// </summary>
        public static ApiCallResponse ProductUpdate(ParaObjects.Product Product, ParaCredentials ParaCredentials)
        {
            ApiCallResponse ar = new ApiCallResponse();

            ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, ParaEnums.ParatureModule.Product, XmlGenerator.ProductGenerateXml(Product), Product.Id);


            return ar;
            //return 0;
        }

        /// <summary>
        /// Returns a Product object with all the properties of a Product.
        /// </summary>
        /// <param name="Productid">
        ///The Product number that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="ParaCredentials">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>
        /// <param name="RequestDepth">
        /// Sets how deep should this call parse through related modules linked to the product you are calling.
        /// </param>
        public static ParaObjects.Product ProductGetDetails(Int64 Productid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
        {
            ParaObjects.Product Product = new ParaObjects.Product();
            Product = ProductFillDetails(Productid, ParaCredentials, RequestDepth, true);
            return Product;
        }

        /// <summary>
        /// Returns a Product object with all of its properties filled.
        /// </summary>
        /// <param name="Productid">
        /// The Product number that you would like to get the details of. 
        /// Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="ParaCredentials">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>
        public static ParaObjects.Product ProductGetDetails(Int64 Productid, ParaCredentials ParaCredentials)
        {
            ParaObjects.Product Product = new ParaObjects.Product();
            Product = ProductFillDetails(Productid, ParaCredentials, ParaEnums.RequestDepth.Standard, true);

            return Product;
        }

        /// <summary>
        /// Returns an product object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="ProductXML">
        /// The Product XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaObjects.Product ProductGetDetails(XmlDocument ProductXML)
        {
            ParaObjects.Product product = new ParaObjects.Product();
            product = ProductParser.ProductFill(ProductXML, 0, true, null);
            product.FullyLoaded = true;

            product.ApiCallResponse.xmlReceived = ProductXML;
            product.ApiCallResponse.Objectid = product.Id;

            product.IsDirty = false;
            return product;
        }

        /// <summary>
        /// Returns an product list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="ProductListXML">
        /// The Product List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ProductsList ProductsGetList(XmlDocument ProductListXML)
        {
            ProductsList productsList = new ProductsList();
            productsList = ProductParser.ProductsFillList(ProductListXML, true, 0, null);

            productsList.ApiCallResponse.xmlReceived = ProductListXML;

            return productsList;
        }

        /// <summary>
        /// Provides you with the capability to list Products, following criteria you would set
        /// by instantiating a ModuleQuery.ProductQuery object
        /// </summary>
        public static ProductsList ProductsGetList(ParaCredentials ParaCredentials, ModuleQuery.ProductQuery Query)
        {
            return ProductsFillList(ParaCredentials, Query, ParaEnums.RequestDepth.Standard);
        }

        /// <summary>
        /// Provides you with the capability to list Products, following criteria you would set
        /// by instantiating a ModuleQuery.ProductQuery object.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>
        public static ProductsList ProductsGetList(ParaCredentials ParaCredentials, ModuleQuery.ProductQuery Query, ParaEnums.RequestDepth RequestDepth)
        {
            return ProductsFillList(ParaCredentials, Query, RequestDepth);
        }

        /// <summary>
        /// Returns the list of the first 25 Products returned by the API.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>            
        public static ProductsList ProductsGetList(ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
        {
            return ProductsFillList(ParaCredentials, null, RequestDepth);
        }

        /// <summary>
        /// Returns the list of the first 25 Products returned by the API.
        /// </summary>
        public static ProductsList ProductsGetList(ParaCredentials ParaCredentials)
        {
            return ProductsFillList(ParaCredentials, null, ParaEnums.RequestDepth.Standard);
        }

        /// <summary>
        /// Fills an Customer list object.
        /// </summary>
        private static ProductsList ProductsFillList(ParaCredentials ParaCredentials, ModuleQuery.ProductQuery Query, ParaEnums.RequestDepth RequestDepth)
        {
            int requestdepth = (int)RequestDepth;
            if (Query == null)
            {
                Query = new ModuleQuery.ProductQuery();
            }
            // Making a schema call and returning all custom fields to be included in the call.
            if (Query.IncludeAllCustomFields == true)
            {
                ParaObjects.Product objschem = new ParaObjects.Product();
                objschem = ProductSchema(ParaCredentials);
                Query.IncludeCustomField(objschem.CustomFields);
            }

            ApiCallResponse ar = new ApiCallResponse();
            ProductsList ProductsList = new ProductsList();

            if (Query.RetrieveAllRecords && Query.OptimizePageSize)
            {
                OptimizationResult rslt = ApiUtils.OptimizeObjectPageSize(ProductsList, Query, ParaCredentials, requestdepth, ParaEnums.ParatureModule.Product);
                ar = rslt.apiResponse;
                Query = (ModuleQuery.ProductQuery)rslt.Query;
                ProductsList = ((ProductsList)rslt.objectList);
                rslt = null;
            }
            else
            {
                ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Product, Query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    ProductsList = ProductParser.ProductsFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);
                }
                ProductsList.ApiCallResponse = ar;
            }

            // Checking if the system needs to recursively call all of the data returned.
            if (Query.RetrieveAllRecords && !ar.HasException)
            {
                // A flag variable to check if we need to make more calls
                if (Query.OptimizeCalls)
                {
                    System.Threading.Thread t;
                    ThreadPool.ObjectList instance = null;
                    int callsRequired = (int)Math.Ceiling((double)(ProductsList.TotalItems / (double)ProductsList.PageSize));
                    for (int i = 2; i <= callsRequired; i++)
                    {
                        //ApiCallFactory.waitCheck(ParaCredentials.Accountid);
                        Query.PageNumber = i;
                        //implement semaphore right here (in the thread pool instance to control the generation of threads
                        instance = new ThreadPool.ObjectList(ParaCredentials, ParaEnums.ParatureModule.Product, Query.BuildQueryArguments(), requestdepth);
                        t = new System.Threading.Thread(delegate() { instance.Go(ProductsList); });
                        t.Start();
                    }

                    while (ProductsList.TotalItems > ProductsList.Products.Count)
                    {
                        Thread.Sleep(500);
                    }

                    ProductsList.ResultsReturned = ProductsList.Products.Count;
                    ProductsList.PageNumber = callsRequired;
                }
                else
                {
                    bool continueCalling = true;
                    while (continueCalling)
                    {
                        ProductsList objectlist = new ProductsList();

                        if (ProductsList.TotalItems > ProductsList.Products.Count)
                        {
                            // We still need to pull data

                            // Getting next page's data
                            Query.PageNumber = Query.PageNumber + 1;

                            ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Product, Query.BuildQueryArguments());

                            if (ar.HasException == false)
                            {
                                objectlist = ProductParser.ProductsFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);
                                ProductsList.Products.AddRange(objectlist.Products);
                                ProductsList.ResultsReturned = ProductsList.Products.Count;
                                ProductsList.PageNumber = Query.PageNumber;
                            }
                            else
                            {
                                // There is an error processing request
                                ProductsList.ApiCallResponse = ar;
                                continueCalling = false;
                            }
                        }
                        else
                        {
                            // That is it, pulled all the items.
                            continueCalling = false;
                            ProductsList.ApiCallResponse = ar;
                        }
                    }
                }
            }

            return ProductsList;

        }
                        
        private static ParaObjects.Product ProductFillDetails(Int64 Productid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth, bool MinimalisticLoad)
        {
            int requestdepth = (int)RequestDepth;
            ParaObjects.Product Product = new ParaObjects.Product();
            //Product = null;
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, ParaEnums.ParatureModule.Product, Productid);
            if (ar.HasException == false)
            {
                Product = ProductParser.ProductFill(ar.xmlReceived, requestdepth, MinimalisticLoad, ParaCredentials);
                Product.FullyLoaded = true;
            }
            else
            {
                Product.FullyLoaded = false;
                Product.Id = 0;
            }
            Product.ApiCallResponse = ar;
            Product.IsDirty = false;
            return Product;
        }


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
                EntityQuery.DownloadFolderQuery dfQuery = new EntityQuery.DownloadFolderQuery();
                dfQuery.PageSize = 5000;
                var Folders = new ParaEntityList<ParaObjects.DownloadFolder>();
                Folders = Download.DownloadFolder.DownloadFoldersGetList(paracredentials, dfQuery);
                foreach (DownloadFolder folder in Folders.Data)
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
                Folders = Download.DownloadFolder.DownloadFoldersGetList(paracredentials, dfQuery);
                foreach (DownloadFolder folder in Folders.Data)
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
            public static ApiCallResponse Insert(ParaObjects.ProductFolder ProductFolder, ParaCredentials ParaCredentials)
            {
                ApiCallResponse ar = new ApiCallResponse();
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc = XmlGenerator.ProductFolderGenerateXML(ProductFolder);
                ar = ApiCallFactory.EntityCreateUpdate(ParaCredentials, ParaEnums.ParatureEntity.ProductFolder, doc, 0);
                ProductFolder.FolderID = ar.Objectid;
                return ar;
            }

            /// <summary>
            /// Updates a Parature Product Folder. Requires an Object and a credentials object.  Will return the updated ProductFolderid. Returns 0 if the update operation failed.
            /// </summary>
            public static ApiCallResponse Update(ParaObjects.ProductFolder ProductFolder, ParaCredentials ParaCredentials)
            {
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, ParaEnums.ParatureModule.Product, XmlGenerator.ProductFolderGenerateXML(ProductFolder), ProductFolder.FolderID);
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
            public static ParaObjects.ProductFolder ProductFolderGetDetails(Int64 ProductFolderid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
            {
                ParaObjects.ProductFolder ProductFolder = new ParaObjects.ProductFolder();
                ProductFolder = ProductFolderFillDetails(ProductFolderid, ParaCredentials, RequestDepth);

                return ProductFolder;

            }

            /// <summary>
            /// Returns an productFolder object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="ProductFolderXML">
            /// The ProductFolder XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.ProductFolder ProductFolderGetDetails(XmlDocument ProductFolderXML)
            {
                ParaObjects.ProductFolder productFolder = new ParaObjects.ProductFolder();
                productFolder = ProductParser.ProductFolderParser.ProductFolderFill(ProductFolderXML, 0, null);
                productFolder.FullyLoaded = true;

                productFolder.ApiCallResponse.xmlReceived = ProductFolderXML;
                productFolder.ApiCallResponse.Objectid = productFolder.FolderID;

                return productFolder;
            }

            /// <summary>
            /// Provides you with the capability to list Product Folders
            /// </summary>
            public static ParaEntityList<ParaObjects.ProductFolder> ProductFoldersGetList(ParaCredentials ParaCredentials)
            {
                return ProductFoldersFillList(ParaCredentials, new EntityQuery.ProductFolderQuery(), ParaEnums.RequestDepth.Standard);
            }

            /// <summary>
            /// Provides you with the capability to list Downloads, following criteria you would set
            /// by instantiating a ModuleQuery.DownloadQuery object
            /// </summary>
            public static ParaEntityList<ParaObjects.ProductFolder> ProductFoldersGetList(ParaCredentials ParaCredentials, EntityQuery.ProductFolderQuery Query)
            {
                return ProductFoldersFillList(ParaCredentials, Query, ParaEnums.RequestDepth.Standard);
            }

            /// <summary>
            /// Provides you with the capability to list Downloads, following criteria you would set
            /// by instantiating a ModuleQuery.DownloadQuery object.
            /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
            /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
            /// the standard field depth.
            /// </summary>
            public static ParaEntityList<ParaObjects.ProductFolder> ProductFoldersGetList(ParaCredentials ParaCredentials, EntityQuery.ProductFolderQuery Query, ParaEnums.RequestDepth RequestDepth)
            {
                return ProductFoldersFillList(ParaCredentials, Query, RequestDepth);
            }

            /// <summary>
            /// Returns an productFolder list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="ProductFolderListXML">
            /// The ProductFolder List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaEntityList<ParaObjects.ProductFolder> ProductFoldersGetList(XmlDocument ProductFolderListXML)
            {
                var productFoldersList = new ParaEntityList<ParaObjects.ProductFolder>();
                productFoldersList = ProductParser.ProductFolderParser.ProductFoldersFillList(ProductFolderListXML, 0, null);

                productFoldersList.ApiCallResponse.xmlReceived = ProductFolderListXML;

                return productFoldersList;
            }

            /// <summary>
            /// Fills a ProductFolderList object.
            /// </summary>
            private static ParaEntityList<ParaObjects.ProductFolder> ProductFoldersFillList(ParaCredentials ParaCredentials, EntityQuery.ProductFolderQuery Query, ParaEnums.RequestDepth RequestDepth)
            {
                int requestdepth = (int)RequestDepth;
                if (Query == null)
                {
                    Query = new EntityQuery.ProductFolderQuery();
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
                        ProductFoldersList = ProductParser.ProductFolderParser.ProductFoldersFillList(ar.xmlReceived, requestdepth, ParaCredentials);
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
                            ProductFoldersList.Data.AddRange(ProductParser.ProductFolderParser.ProductFoldersFillList(ar.xmlReceived, requestdepth, ParaCredentials).Data);
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

            static ParaObjects.ProductFolder ProductFolderFillDetails(Int64 ProductFolderid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
            {
                int requestdepth = (int)RequestDepth;
                ParaObjects.ProductFolder ProductFolder = new ParaObjects.ProductFolder();
                //Customer = null;
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, ParaEnums.ParatureEntity.ProductFolder, ProductFolderid);
                if (ar.HasException == false)
                {
                    ProductFolder = ProductParser.ProductFolderParser.ProductFolderFill(ar.xmlReceived, requestdepth, ParaCredentials);
                    ProductFolder.FullyLoaded = true;
                }
                else
                {
                    ProductFolder.FullyLoaded = false;
                    ProductFolder.FolderID = 0;
                }

                ProductFolder.ApiCallResponse = ar;
                return ProductFolder;
            }


            public static ParaObjects.ProductFolder ProductFolderGetDetails(Int64 ProductFolderid, ParaCredentials ParaCredentials)
            {

                ParaObjects.ProductFolder ProductFolder = new ParaObjects.ProductFolder();
                ProductFolder = ProductFolderFillDetails(ProductFolderid, ParaCredentials, ParaEnums.RequestDepth.Standard);

                return ProductFolder;
            }
        }
    }
}