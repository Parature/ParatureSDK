using System;
using System.Threading;
using System.Xml;
using ParatureAPI.PagedData;
using ParatureAPI.ParaObjects;
using Action = ParatureAPI.ParaObjects.Action;

namespace ParatureAPI.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Asset module.
    /// </summary>
    public class Asset
    {

        /// <summary>
        /// Provides the Schema of the asset module.
        /// </summary>
        public static ParaObjects.Asset AssetSchema(ParaCredentials ParaCredentials)
        {
            ParaObjects.Asset Asset = new ParaObjects.Asset();

            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetSchema(ParaCredentials, ParaEnums.ParatureModule.Asset);

            if (ar.HasException == false)
            {
                Asset = XmlToObjectParser.AssetParser.AssetFill(ar.xmlReceived, false, 0, ParaCredentials);
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
        static public ParaObjects.Asset AssetSchemaWithCustomFieldTypes(ParaCredentials ParaCredentials)
        {
            ParaObjects.Asset asset = AssetSchema(ParaCredentials);

            asset = (ParaObjects.Asset)ApiCallFactory.ObjectCheckCustomFieldTypes(ParaCredentials, ParaEnums.ParatureModule.Asset, asset);

            return asset;
        }

        /// <summary>
        /// Creates a Parature Ticket. Requires an Object and a credentials object. Will return the Newly Created Assetid. Returns 0 if the Asset creation fails.
        /// </summary>
        public static ApiCallResponse AssetInsert(ParaObjects.Asset Asset, ParaCredentials ParaCredentials)
        {
            ApiCallResponse ar = new ApiCallResponse();
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc = XmlGenerator.AssetGenerateXml(Asset);
            ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, ParaEnums.ParatureModule.Asset, doc, 0);
            Asset.Assetid = ar.Objectid;
            Asset.uniqueIdentifier = ar.Objectid;
            return ar;
        }

        /// <summary>
        /// Updates a Parature Asset. Requires an Object and a credentials object.  Will return the updated Assetid. Returns 0 if the Asset update operation failed.
        /// </summary>
        public static ApiCallResponse AssetUpdate(ParaObjects.Asset Asset, ParaCredentials ParaCredentials)
        {
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, ParaEnums.ParatureModule.Asset, XmlGenerator.AssetGenerateXml(Asset), Asset.Assetid);
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
        public static ApiCallResponse AssetDelete(Int64 Assetid, ParaCredentials ParaCredentials, bool purge)
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
        public static ParaObjects.Asset AssetGetDetails(Int64 Assetid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
        {
            ParaObjects.Asset Asset = new ParaObjects.Asset();
            Asset = AssetFillDetails(Assetid, ParaCredentials, RequestDepth);
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
        public static ParaObjects.Asset AssetGetDetails(Int64 Assetid, ParaCredentials ParaCredentials)
        {
            return AssetGetDetails(Assetid, ParaCredentials, ParaEnums.RequestDepth.Standard);
        }

        /// <summary>
        /// Returns an asset object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="AssetXML">
        /// The Asset XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaObjects.Asset AssetGetDetails(XmlDocument AssetXML)
        {
            ParaObjects.Asset asset = new ParaObjects.Asset();
            asset = XmlToObjectParser.AssetParser.AssetFill(AssetXML, true, 0, null);
            asset.FullyLoaded = true;

            asset.ApiCallResponse.xmlReceived = AssetXML;
            asset.ApiCallResponse.Objectid = asset.Assetid;

            asset.IsDirty = true;
            return asset;
        }

        /// <summary>
        /// Returns an assets list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="AssetsListXML">
        /// The Asset List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static AssetsList AssetsGetList(XmlDocument AssetsListXML)
        {
            AssetsList assetsList = new AssetsList();
            assetsList = XmlToObjectParser.AssetParser.AssetsFillList(AssetsListXML, true, 0, null);

            assetsList.ApiCallResponse.xmlReceived = AssetsListXML;

            return assetsList;
        }

        /// <summary>
        /// Provides you with the capability to list Assets, following criteria you would set
        /// by instantiating a ModuleQuery.AssetQuery object
        /// </summary>
        public static AssetsList AssetsGetList(ParaCredentials ParaCredentials, ModuleQuery.AssetQuery Query)
        {
            return AssetsFillList(ParaCredentials, Query, ParaEnums.RequestDepth.Standard);
        }

        /// <summary>
        /// Gets the first 25 assets returned by the API.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>
        public static AssetsList AssetsGetList(ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
        {
            return AssetsFillList(ParaCredentials, null, RequestDepth);
        }
        /// <summary>
        /// Gets the first 25 assets returned by the API.
        /// </summary>            
        public static AssetsList AssetsGetList(ParaCredentials ParaCredentials)
        {
            return AssetsFillList(ParaCredentials, null, ParaEnums.RequestDepth.Standard);
        }
        /// <summary>
        /// Provides you with the capability to list Assets, following criteria you would set
        /// by instantiating a ModuleQuery.AssetQuery object.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>
        public static AssetsList AssetsGetList(ParaCredentials ParaCredentials, ModuleQuery.AssetQuery Query, ParaEnums.RequestDepth RequestDepth)
        {
            return AssetsFillList(ParaCredentials, Query, RequestDepth);
        }

        /// <summary>
        /// Fills an Asset list object.
        /// </summary>
        private static AssetsList AssetsFillList(ParaCredentials ParaCredentials, ModuleQuery.AssetQuery Query, ParaEnums.RequestDepth RequestDepth)
        {
            int requestdepth = (int)RequestDepth;
            if (Query == null)
            {
                Query = new ModuleQuery.AssetQuery();
            }
            // Making a schema call and returning all custom fields to be included in the call.
            if (Query.IncludeAllCustomFields == true)
            {
                ParaObjects.Asset objschem = new ParaObjects.Asset();
                objschem = AssetSchema(ParaCredentials);
                Query.IncludeCustomField(objschem.CustomFields);
            }

            ApiCallResponse ar = new ApiCallResponse();
            AssetsList AssetsList = new AssetsList();

            if (Query.RetrieveAllRecords && Query.OptimizePageSize)
            {
                OptimizationResult rslt = ApiUtils.OptimizeObjectPageSize(AssetsList, Query, ParaCredentials, requestdepth, ParaEnums.ParatureModule.Asset);
                ar = rslt.apiResponse;
                Query = (ModuleQuery.AssetQuery)rslt.Query;
                AssetsList = ((AssetsList)rslt.objectList);
                rslt = null;
            }
            else
            {
                ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Asset, Query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    AssetsList = XmlToObjectParser.AssetParser.AssetsFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);
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

                    while (AssetsList.TotalItems > AssetsList.Assets.Count)
                    {
                        Thread.Sleep(500);
                    }

                    AssetsList.ResultsReturned = AssetsList.Assets.Count;
                    AssetsList.PageNumber = callsRequired;
                }
                else
                {
                    bool continueCalling = true;
                    while (continueCalling)
                    {
                        AssetsList objectlist = new AssetsList();

                        if (AssetsList.TotalItems > AssetsList.Assets.Count)
                        {
                            // We still need to pull data

                            // Getting next page's data
                            Query.PageNumber = Query.PageNumber + 1;

                            ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Asset, Query.BuildQueryArguments());

                            if (ar.HasException == false)
                            {
                                objectlist = XmlToObjectParser.AssetParser.AssetsFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);
                                AssetsList.Assets.AddRange(objectlist.Assets);
                                AssetsList.ResultsReturned = AssetsList.Assets.Count;
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

        private static ParaObjects.Asset AssetFillDetails(Int64 Assetid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
        {
            int requestdepth = (int)RequestDepth;
            ParaObjects.Asset Asset = new ParaObjects.Asset();
            ApiCallResponse ar = new ApiCallResponse();


            ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, ParaEnums.ParatureModule.Asset, Assetid);

            if (ar.HasException == false)
            {
                Asset = XmlToObjectParser.AssetParser.AssetFill(ar.xmlReceived, true, requestdepth, ParaCredentials);
                Asset.FullyLoaded = true;
            }
            else
            {
                Asset.FullyLoaded = false;
                Asset.Assetid = 0;
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