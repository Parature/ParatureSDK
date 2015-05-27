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
    public static class Product
    {
        /// <summary>
        /// Provides the Schema of the Product module.
        /// </summary>
        public static ParaObjects.Product Schema(ParaCredentials creds)
        {
            var product = new ParaObjects.Product();
            var ar = ApiCallFactory.ObjectGetSchema(creds, ParaEnums.ParatureModule.Product);

            if (ar.HasException == false)
            {
                var purgedSchema = ApiUtils.RemoveStaticFieldsNodes(ar.XmlReceived);
                product = ParaEntityParser.EntityFill<ParaObjects.Product>(purgedSchema);
            }
            product.ApiCallResponse = ar;
            return product;
        }

        /// <summary>
        /// Gets an empty object with the scheam (custom fields, if any).  This call will also try to create a dummy
        /// record in order to determine if any of the custom fields have special validation rules (e.g. email, phone, url)
        /// and set the "dataType" of the custom field accordingly.
        /// </summary> 
        static public ParaObjects.Product SchemaWithCustomFieldTypes(ParaCredentials ParaCredentials)
        {
            ParaObjects.Product Product = Schema(ParaCredentials);

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
        public static ApiCallResponse Delete(Int64 Productid, ParaCredentials ParaCredentials, bool purge)
        {
            return ApiCallFactory.ObjectDelete(ParaCredentials, ParaEnums.ParatureModule.Product, Productid, purge);
        }

        /// <summary>
        /// Creates a Parature Product. Requires an Object and a credentials object. Will return the Newly Created Productid. Returns 0 if the Product creation failed.
        /// </summary>
        public static ApiCallResponse Insert(ParaObjects.Product Product, ParaCredentials ParaCredentials)
        {
            ApiCallResponse ar = new ApiCallResponse();
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc = XmlGenerator.GenerateXml(Product);
            ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, ParaEnums.ParatureModule.Product, doc, 0);
            Product.Id = ar.Id;
            return ar;
        }

        /// <summary>
        /// Updates a Parature Product. Requires an Object and a credentials object.  Will return the updated Productid. Returns 0 if the Product update operation failed.
        /// </summary>
        public static ApiCallResponse Update(ParaObjects.Product Product, ParaCredentials ParaCredentials)
        {
            ApiCallResponse ar = new ApiCallResponse();

            ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, ParaEnums.ParatureModule.Product, XmlGenerator.GenerateXml(Product), Product.Id);


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
        public static ParaObjects.Product GetDetails(Int64 Productid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
        {
            ParaObjects.Product Product = new ParaObjects.Product();
            Product = FillDetails(Productid, ParaCredentials, RequestDepth, true);
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
        public static ParaObjects.Product GetDetails(Int64 Productid, ParaCredentials ParaCredentials)
        {
            ParaObjects.Product Product = new ParaObjects.Product();
            Product = FillDetails(Productid, ParaCredentials, ParaEnums.RequestDepth.Standard, true);

            return Product;
        }

        /// <summary>
        /// Returns an product object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="ProductXML">
        /// The Product XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaObjects.Product GetDetails(XmlDocument ProductXML)
        {
            ParaObjects.Product product = new ParaObjects.Product();
            product = ParaEntityParser.EntityFill<ParaObjects.Product>(ProductXML);
            product.FullyLoaded = true;

            product.ApiCallResponse.XmlReceived = ProductXML;
            product.ApiCallResponse.Id = product.Id;

            product.IsDirty = false;
            return product;
        }

        /// <summary>
        /// Returns an product list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="ProductListXML">
        /// The Product List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<ParaObjects.Product> GetList(XmlDocument ProductListXML)
        {
            var productsList = new ParaEntityList<ParaObjects.Product>();
            productsList = ParaEntityParser.FillList<ParaObjects.Product>(ProductListXML);

            productsList.ApiCallResponse.XmlReceived = ProductListXML;

            return productsList;
        }

        /// <summary>
        /// Provides you with the capability to list Products, following criteria you would set
        /// by instantiating a ModuleQuery.ProductQuery object
        /// </summary>
        public static ParaEntityList<ParaObjects.Product> GetList(ParaCredentials ParaCredentials, ParaEntityQuery Query)
        {
            return FillList(ParaCredentials, Query, ParaEnums.RequestDepth.Standard);
        }

        /// <summary>
        /// Provides you with the capability to list Products, following criteria you would set
        /// by instantiating a ModuleQuery.ProductQuery object.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>
        public static ParaEntityList<ParaObjects.Product> GetList(ParaCredentials ParaCredentials, ParaEntityQuery Query, ParaEnums.RequestDepth RequestDepth)
        {
            return FillList(ParaCredentials, Query, RequestDepth);
        }

        /// <summary>
        /// Returns the list of the first 25 Products returned by the API.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>            
        public static ParaEntityList<ParaObjects.Product> GetList(ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
        {
            return FillList(ParaCredentials, null, RequestDepth);
        }

        /// <summary>
        /// Returns the list of the first 25 Products returned by the API.
        /// </summary>
        public static ParaEntityList<ParaObjects.Product> GetList(ParaCredentials ParaCredentials)
        {
            return FillList(ParaCredentials, null, ParaEnums.RequestDepth.Standard);
        }

        /// <summary>
        /// Fills an Customer list object.
        /// </summary>
        private static ParaEntityList<ParaObjects.Product> FillList(ParaCredentials ParaCredentials, ParaEntityQuery Query, ParaEnums.RequestDepth RequestDepth)
        {
            int requestdepth = (int)RequestDepth;
            if (Query == null)
            {
                Query = new ProductQuery();
            }
            // Making a schema call and returning all custom fields to be included in the call.
            if (Query.IncludeAllCustomFields == true)
            {
                ParaObjects.Product objschem = new ParaObjects.Product();
                objschem = Schema(ParaCredentials);
                Query.IncludeCustomField(objschem.CustomFields);
            }

            ApiCallResponse ar = new ApiCallResponse();
            var ProductsList = new ParaEntityList<ParaObjects.Product>();

            if (Query.RetrieveAllRecords && Query.OptimizePageSize)
            {
                OptimizationResult rslt = ApiUtils.OptimizeObjectPageSize(ProductsList, Query, ParaCredentials, requestdepth, ParaEnums.ParatureModule.Product);
                ar = rslt.apiResponse;
                Query = (ProductQuery)rslt.Query;
                ProductsList = ((ParaEntityList<ParaObjects.Product>)rslt.objectList);
                rslt = null;
            }
            else
            {
                ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Product, Query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    ProductsList = ParaEntityParser.FillList<ParaObjects.Product>(ar.XmlReceived);
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
                        instance = new ThreadPool.ObjectList(ParaCredentials, ParaEnums.ParatureModule.Product, Query.BuildQueryArguments());
                        t = new System.Threading.Thread(delegate() { instance.Go(ProductsList); });
                        t.Start();
                    }

                    while (ProductsList.TotalItems > ProductsList.Data.Count)
                    {
                        Thread.Sleep(500);
                    }

                    ProductsList.ResultsReturned = ProductsList.Data.Count;
                    ProductsList.PageNumber = callsRequired;
                }
                else
                {
                    bool continueCalling = true;
                    while (continueCalling)
                    {
                        var objectlist = new ParaEntityList<ParaObjects.Product>();

                        if (ProductsList.TotalItems > ProductsList.Data.Count)
                        {
                            // We still need to pull data

                            // Getting next page's data
                            Query.PageNumber = Query.PageNumber + 1;

                            ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Product, Query.BuildQueryArguments());

                            if (ar.HasException == false)
                            {
                                objectlist = ParaEntityParser.FillList<ParaObjects.Product>(ar.XmlReceived);
                                ProductsList.Data.AddRange(objectlist.Data);
                                ProductsList.ResultsReturned = ProductsList.Data.Count;
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
                        
        private static ParaObjects.Product FillDetails(Int64 Productid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth, bool MinimalisticLoad)
        {
            int requestdepth = (int)RequestDepth;
            var Product = new ParaObjects.Product();
            //Product = null;
            var ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetDetail<ParaObjects.Product>(ParaCredentials, ParaEnums.ParatureModule.Product, Productid);
            if (ar.HasException == false)
            {
                Product = ParaEntityParser.EntityFill<ParaObjects.Product>(ar.XmlReceived);
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