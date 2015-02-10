using System;
using System.Threading;
using System.Xml;
using ParatureSDK.ModuleQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;
using Action = ParatureSDK.ParaObjects.Action;

namespace ParatureSDK.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Asset module.
    /// </summary>
    public class Asset
    {

        /// <summary>
        /// Provides the Schema of the asset module.
        /// </summary>
        public static ParaObjects.Asset Schema(ParaCredentials ParaCredentials)
        {
            var Asset = new ParaObjects.Asset();

            var ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetSchema(ParaCredentials, ParaEnums.ParatureModule.Asset);

            if (ar.HasException == false)
            {
                Asset = ParaEntityParser.EntityFill<ParaObjects.Asset>(ar.xmlReceived);
            }
            Asset.ApiCallResponse = ar;
            Asset.IsDirty = false;
            return Asset;
        }

        /// <summary>
        /// Gets an empty object with the scheam (custom fields, if any).  This call will also try to create a dummy
        /// record in order to determine if any of the custom fields have special validation rules (e.g. email, phone, url)
        /// and set the "dataType" of the custom field accordingly.
        /// </summary> 
        static public ParaObjects.Asset SchemaWithCustomFieldTypes(ParaCredentials ParaCredentials)
        {
            ParaObjects.Asset asset = Schema(ParaCredentials);

            asset = (ParaObjects.Asset)ApiCallFactory.ObjectCheckCustomFieldTypes(ParaCredentials, ParaEnums.ParatureModule.Asset, asset);

            return asset;
        }

        /// <summary>
        /// Creates a Parature Ticket. Requires an Object and a credentials object. Will return the Newly Created Assetid. Returns 0 if the Asset creation fails.
        /// </summary>
        public static ApiCallResponse Insert(ParaObjects.Asset Asset, ParaCredentials ParaCredentials)
        {
            var ar = new ApiCallResponse();
            var doc = new System.Xml.XmlDocument();
            doc = XmlGenerator.AssetGenerateXml(Asset);
            ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, ParaEnums.ParatureModule.Asset, doc, 0);
            Asset.Id = ar.Objectid;
            return ar;
        }

        /// <summary>
        /// Updates a Parature Asset. Requires an Object and a credentials object.  Will return the updated Assetid. Returns 0 if the Asset update operation failed.
        /// </summary>
        public static ApiCallResponse Update(ParaObjects.Asset Asset, ParaCredentials ParaCredentials)
        {
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, ParaEnums.ParatureModule.Asset, XmlGenerator.AssetGenerateXml(Asset), Asset.Id);
            return ar;
        }

        /// <summary>
        /// Provides the capability to delete a Asset.
        /// </summary>
        /// <param name="Assetid">
        /// The id of the Asset to delete
        /// </param>
        /// <param name="purge">
        /// If purge is set to true, the Asset will be permanently deleted. Otherwise, it will just be 
        /// moved to the trash bin, so it will still be able to be restored from the service desk.
        ///</param>
        public static ApiCallResponse Delete(Int64 Assetid, ParaCredentials ParaCredentials, bool purge)
        {
            return ApiCallFactory.ObjectDelete(ParaCredentials, ParaEnums.ParatureModule.Asset, Assetid, purge);
        }


        /// <summary>
        /// Returns a Asset object with all the properties of an Asset.
        /// </summary>
        /// <param name="Assetid">
        ///The Asset number that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>           
        /// <param name="ParaCredentials">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>
        /// <param name="RequestDepth">
        /// For a simple Asset request, please put 0. <br/>When Requesting an Asset, there might be related objects linked to that Asset: such as Customer Owner, Product, etc. <br/>With a regular Asset detail call, generally only the ID and names of the extra objects are loaded. 
        /// <example>For example, the call will return a Asset.CustomerOwner object, but only the Name and ID values will be filled. All of the other properties of the Customer object will be empty. If you select RequestDepth=1, then we will go one level deeper into our request and will therefore retrieve the Customer's detail for you. Customers might be part of an account, so if you select RequestDepth=2, we will go to an even deeper level and return the full account object with all of its properties, etc.<br/> Please make sure you only request the depth you need, as this might make your request slower. </example>
        /// </param>
        public static ParaObjects.Asset GetDetails(Int64 Assetid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
        {
            ParaObjects.Asset Asset = new ParaObjects.Asset();
            Asset = FillDetails(Assetid, ParaCredentials, RequestDepth);
            return Asset;
        }


        /// <summary>
        /// Returns a Asset object with all the properties of an Asset.
        /// </summary>
        /// <param name="Assetid">
        ///The Asset number that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>           
        /// <param name="ParaCredentials">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>           
        public static ParaObjects.Asset GetDetails(Int64 Assetid, ParaCredentials ParaCredentials)
        {
            return GetDetails(Assetid, ParaCredentials, ParaEnums.RequestDepth.Standard);
        }

        /// <summary>
        /// Returns an asset object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="AssetXML">
        /// The Asset XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaObjects.Asset GetDetails(XmlDocument AssetXML)
        {
            ParaObjects.Asset asset = new ParaObjects.Asset();
            asset = ParaEntityParser.EntityFill<ParaObjects.Asset>(AssetXML);
            asset.FullyLoaded = true;

            asset.ApiCallResponse.xmlReceived = AssetXML;
            asset.ApiCallResponse.Objectid = asset.Id;

            asset.IsDirty = true;
            return asset;
        }

        /// <summary>
        /// Returns an assets list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="AssetsListXML">
        /// The Asset List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<ParaObjects.Asset> GetList(XmlDocument AssetsListXML)
        {
            var assetsList = new ParaEntityList<ParaObjects.Asset>();
            assetsList = ParaEntityParser.FillList<ParaObjects.Asset>(AssetsListXML);

            assetsList.ApiCallResponse.xmlReceived = AssetsListXML;

            return assetsList;
        }

        /// <summary>
        /// Provides you with the capability to list Assets, following criteria you would set
        /// by instantiating a ModuleQuery.AssetQuery object
        /// </summary>
        public static ParaEntityList<ParaObjects.Asset> GetList(ParaCredentials ParaCredentials, AssetQuery Query)
        {
            return FillList(ParaCredentials, Query, ParaEnums.RequestDepth.Standard);
        }

        /// <summary>
        /// Gets the first 25 assets returned by the API.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>
        public static ParaEntityList<ParaObjects.Asset> GetList(ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
        {
            return FillList(ParaCredentials, null, RequestDepth);
        }
        /// <summary>
        /// Gets the first 25 assets returned by the API.
        /// </summary>            
        public static ParaEntityList<ParaObjects.Asset> GetList(ParaCredentials ParaCredentials)
        {
            return FillList(ParaCredentials, null, ParaEnums.RequestDepth.Standard);
        }
        /// <summary>
        /// Provides you with the capability to list Assets, following criteria you would set
        /// by instantiating a ModuleQuery.AssetQuery object.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>
        public static ParaEntityList<ParaObjects.Asset> GetList(ParaCredentials ParaCredentials, AssetQuery Query, ParaEnums.RequestDepth RequestDepth)
        {
            return FillList(ParaCredentials, Query, RequestDepth);
        }

        /// <summary>
        /// Fills an Asset list object.
        /// </summary>
        private static ParaEntityList<ParaObjects.Asset> FillList(ParaCredentials ParaCredentials, AssetQuery Query, ParaEnums.RequestDepth RequestDepth)
        {
            int requestdepth = (int)RequestDepth;
            if (Query == null)
            {
                Query = new AssetQuery();
            }
            // Making a schema call and returning all custom fields to be included in the call.
            if (Query.IncludeAllCustomFields == true)
            {
                ParaObjects.Asset objschem = new ParaObjects.Asset();
                objschem = Schema(ParaCredentials);
                Query.IncludeCustomField(objschem.CustomFields);
            }

            ApiCallResponse ar = new ApiCallResponse();
            var AssetsList = new ParaEntityList<ParaObjects.Asset>();

            if (Query.RetrieveAllRecords && Query.OptimizePageSize)
            {
                OptimizationResult rslt = ApiUtils.OptimizeObjectPageSize(AssetsList, Query, ParaCredentials, requestdepth, ParaEnums.ParatureModule.Asset);
                ar = rslt.apiResponse;
                Query = (AssetQuery)rslt.Query;
                AssetsList = ((ParaEntityList<ParaObjects.Asset>)rslt.objectList);
                rslt = null;
            }
            else
            {
                ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Asset, Query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    AssetsList = ParaEntityParser.FillList<ParaObjects.Asset>(ar.xmlReceived);
                }
                AssetsList.ApiCallResponse = ar;
            }

            // Checking if the system needs to recursively call all of the data returned.
            if (Query.RetrieveAllRecords && !ar.HasException)
            {
                // A flag variable to check if we need to make more calls
                if (Query.OptimizeCalls)
                {
                    System.Threading.Thread t;
                    ThreadPool.ObjectList instance = null;
                    int callsRequired = (int)Math.Ceiling((double)(AssetsList.TotalItems / (double)AssetsList.PageSize));
                    for (int i = 2; i <= callsRequired; i++)
                    {
                        //ApiCallFactory.waitCheck(ParaCredentials.Accountid);
                        Query.PageNumber = i;
                        //implement semaphore right here (in the thread pool instance to control the generation of threads
                        instance = new ThreadPool.ObjectList(ParaCredentials, ParaEnums.ParatureModule.Asset, Query.BuildQueryArguments(), requestdepth);
                        t = new System.Threading.Thread(delegate() { instance.Go(AssetsList); });
                        t.Start();
                    }

                    while (AssetsList.TotalItems > AssetsList.Data.Count)
                    {
                        Thread.Sleep(500);
                    }

                    AssetsList.ResultsReturned = AssetsList.Data.Count;
                    AssetsList.PageNumber = callsRequired;
                }
                else
                {
                    bool continueCalling = true;
                    while (continueCalling)
                    {
                        var objectlist = new ParaEntityList<ParaObjects.Asset>();

                        if (AssetsList.TotalItems > AssetsList.Data.Count)
                        {
                            // We still need to pull data

                            // Getting next page's data
                            Query.PageNumber = Query.PageNumber + 1;

                            ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Asset, Query.BuildQueryArguments());

                            if (ar.HasException == false)
                            {
                                objectlist = ParaEntityParser.FillList<ParaObjects.Asset>(ar.xmlReceived);
                                AssetsList.Data.AddRange(objectlist.Data);
                                AssetsList.ResultsReturned = AssetsList.Data.Count;
                                AssetsList.PageNumber = Query.PageNumber;
                            }
                            else
                            {
                                // There is an error processing request
                                AssetsList.ApiCallResponse = ar;
                                continueCalling = false;
                            }

                        }
                        else
                        {
                            // That is it, pulled all the items.
                            continueCalling = false;
                            AssetsList.ApiCallResponse = ar;
                        }
                    }
                }
            }

            return AssetsList;

        }

        private static ParaObjects.Asset FillDetails(Int64 Assetid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
        {
            int requestdepth = (int)RequestDepth;
            ParaObjects.Asset Asset = new ParaObjects.Asset();
            ApiCallResponse ar = new ApiCallResponse();


            ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, ParaEnums.ParatureModule.Asset, Assetid);

            if (ar.HasException == false)
            {
                Asset = ParaEntityParser.EntityFill<ParaObjects.Asset>(ar.xmlReceived);
                Asset.FullyLoaded = true;
            }
            else
            {
                Asset.FullyLoaded = false;
                Asset.Id = 0;
            }
            Asset.ApiCallResponse = ar;
            Asset.IsDirty = false;
            return Asset;
        }

        /// <summary>
        /// Runs an Asset action.
        /// </summary>
        /// <param name="Assetid">
        /// The Asset you would like to run this action on.
        /// </param>
        /// <param name="Action">
        /// The action object your would like to run on this ticket.
        /// </param>
        /// <param name="ParaCredentials">
        /// Your credentials object.
        /// </param>
        /// <returns></returns>
        public static ApiCallResponse ActionRun(Int64 Assetid, Action Action, ParaCredentials ParaCredentials)
        {
            Action.actionType = ParaEnums.ActionType.Other;
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiUtils.ActionRun(Assetid, Action, ParaCredentials, ParaEnums.ParatureModule.Asset);
            return ar;
        }
    }
}