using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;
using System.Net;
using System.IO;


namespace ParatureAPI
{

    /// <summary>
    /// Manages all interactions with the Parature APIs.
    /// </summary>
    public class ApiHandler
    {

        /// <summary>
        /// Allows the user to quickly post an XML, and get back the API response.
        /// </summary>
        /// <param name="xmlDocument">
        /// The XML File to Post, for a Create or an Update
        /// </param>
        /// <param name="module">
        /// This is to be selected from "Paraenums.ParatureModule" to decide which module to use (Ticket, Account, etc...)
        /// </param>
        /// <param name="Objectid">
        /// If this is a create, just pass 0, otherwise, pass the objectid to update
        /// </param>
        /// <param name="pc">
        /// ParatureCredentials Class
        /// </param>
        /// <returns></returns>
        public static ParaObjects.ApiCallResponse SubmitGenericRequest(System.Xml.XmlDocument xmlDocument, Paraenums.ParatureModule module, Int64 Objectid, ParaCredentials pc)
        {
            return ApiCallFactory.ObjectCreateUpdate(pc, module, xmlDocument, Objectid);
        }

        private static ParaObjects.PagedData objectFillList(ParaCredentials ParaCredentials, ModuleQuery.CustomerQuery Query, Paraenums.RequestDepth RequestDepth, Paraenums.ParatureModule Module)
        {
            // CustomerFillList, AccountFillList, ProductFillList, AssetFillList, 
            // TicketFillList, and ArticleFillList are all very similar, they should be refactored
            // out into one generic objectFillList to reduce the redundancy
            //  -- RShuford 4/22/09
            throw new Exception("Not Implemented");
        }

        public static int determineOptimizedObjectPageSize(ParaQuery Query, ParaCredentials pc, Paraenums.ParatureModule module)
        {
            ParaObjects.ApiCallResponse tempAr;
            ParaObjects.PagedData tempObjectList;

            System.Diagnostics.Stopwatch callStopWatch = new System.Diagnostics.Stopwatch();

            int bestTimePerRecord = 0;
            int testTimePerRecord = 0;
            int bestPageSize = 0;
            int testPageSize = 0;
            int bestPagesRequired = 0;
            int testPagesRequired = 0;

            while (true)
            {
                callStopWatch.Reset();
                callStopWatch.Start();
                tempAr = ApiCallFactory.ObjectGetList(pc, module, Query.BuildQueryArguments());
                callStopWatch.Stop();

                tempObjectList = xmlToObjectParser.ObjectFillList(tempAr.xmlReceived,Query.MinimalisticLoad, (int)Paraenums.RequestDepth.Standard, pc, module);

                testTimePerRecord = (int)(callStopWatch.ElapsedMilliseconds / Query.PageSize);
                testPagesRequired = (int)Math.Ceiling((double)tempObjectList.TotalItems / (double)Query.PageSize);
                testPageSize = Query.PageSize;
                // if the improvment is less than 25 percent lets just take it
                if (false && tempAr.HasException == false)
                {
                    break;
                }
                else
                {
                    bestPageSize = testPageSize;
                    bestPagesRequired = testPagesRequired;
                    bestTimePerRecord = testTimePerRecord;

                    if (((testPageSize + bestPageSize) / 2) == Query.PageSize)
                    {

                    }
                    Query.PageSize = (testPageSize + bestPageSize) / 2;
                }
            }
            //double runningTotal;
            //for (int i = 0; i < 500; i++)
            //{
            //    runningTotal = 0.0;
            //    for (int j = 0; j < 10; j++)
            //    {
            //        callStopWatch.Reset();
            //        callStopWatch.Start();
            //        tempAr = ApiCallFactory.ObjectGetList(ParaCredentails, module, Query.BuildQueryArguments());
            //        callStopWatch.Stop();
            //        runningTotal += (double)(callStopWatch.ElapsedMilliseconds);
            //    }
            //}

            return bestPageSize;
        }

        // This Optimize Page Size should be enhanced to get the Exact best pagesize
        // this will take more time, and as such should be the kind of thing that the application runs once,
        // then stores the results and reuses them from then on.
        // Because it will be used once then reused, there should be several calls of each page size to ensure accuracy.
        // The optimizeObjectCalls method should also be built out to determine the best case number of 
        // calls depending on current server load.
        private static ParaObjects.OptimizationResult optimizeObjectPageSize(ParaObjects.PagedData objectList, ParaQuery Query, ParaCredentials ParaCredentials, int requestdepth, Paraenums.ParatureModule module)
        {
            ParaObjects.OptimizationResult rtn = new ParaObjects.OptimizationResult();
            rtn.Query = Query;
            rtn.objectList = objectList;
            //Two distinct sets of logic are required for optimization.  The two methods are as described:
            //OptimizeCalls requires us to compensate for 5 parallel asynchronous threads.  This is best 
            //  approached by making a series of test calls and calculating the required pages size to achieve 
            //  call duration equal to the thread spawn interval multiplied by the thread limit.  Obviously this
            //  requires revision, but it’s good enough for now.
            //NonOptimizeCalls requires us to calculate the minimum total call time by taking the current 
            //  call time multiplied by the calls required.  The calls required is calculated by dividing the 
            //  total records by the page size then rounded up. Currently we do this by stepping through a 
            //  fixed set of test calls, but this should be refactored to a more dynamic calculation.
            //ParaObjects.PagedData objectList;
            ParaObjects.PagedData tempObjectList;
            ParaObjects.ApiCallResponse tempAR;
            double testTimePerPage = 0.0;    //units are milliseconds
            double testTimePagesRequired = 0.0;
            System.Diagnostics.Stopwatch callStopWatch = new System.Diagnostics.Stopwatch();
            double masterTimePerPage = 0.0;  //units are milliseconds
            double masterPagesRequired = 0.0;

            rtn.apiResponse = new ParaObjects.ApiCallResponse();

            if (rtn.Query.OptimizeCalls)
            {
                List<int> pageSizeSampleSet = new List<int>();

                pageSizeSampleSet.Add(rtn.Query.OptimizePageSizeSeed); //seed

                for (int i = 0; i < rtn.Query.OptimizePageSizeTestCalls; i++)
                {
                    rtn.Query.PageSize = pageSizeSampleSet[i];

                    callStopWatch.Reset();
                    callStopWatch.Start();
                    tempAR = ApiCallFactory.ObjectGetList(ParaCredentials, module, rtn.Query.BuildQueryArguments());
                    callStopWatch.Stop();

                    tempObjectList = xmlToObjectParser.ObjectFillList(tempAR.xmlReceived, rtn.Query.MinimalisticLoad, requestdepth, ParaCredentials, module);

                    testTimePerPage = (int)(callStopWatch.ElapsedMilliseconds);

                    if (i == (rtn.Query.OptimizePageSizeTestCalls - 1) && tempAR.HasException == false)
                    {
                        int total = 0;
                        for (int j = 1; j < pageSizeSampleSet.Count; j++)
                        {
                            total = total + pageSizeSampleSet[j];
                        }
                        rtn.Query.PageSize = (total / (pageSizeSampleSet.Count - 1));
                        break;
                    }
                    else
                    {   // ((5 * 500) == Golden Number
                        pageSizeSampleSet.Add((int)((5 * 500) / (testTimePerPage / pageSizeSampleSet[i])));
                    }
                }

                //first page call
                rtn.apiResponse = ApiCallFactory.ObjectGetList(ParaCredentials, module, rtn.Query.BuildQueryArguments());

                rtn.objectList = xmlToObjectParser.ObjectFillList(rtn.apiResponse.xmlReceived, rtn.Query.MinimalisticLoad, requestdepth, ParaCredentials, module);

                pageSizeSampleSet = null;
            }
            else
            {
                int[] pageSizeSampleSet = { 50, 100, 150, 200, 250, 300, 350, 400, 450, 500 };

                for (int i = 0; i < pageSizeSampleSet.Length; i++)
                {
                    rtn.Query.PageSize = pageSizeSampleSet[i];

                    callStopWatch.Reset();
                    callStopWatch.Start();
                    tempAR = ApiCallFactory.ObjectGetList(ParaCredentials, module, rtn.Query.BuildQueryArguments());
                    callStopWatch.Stop();

                    tempObjectList = xmlToObjectParser.ObjectFillList(tempAR.xmlReceived, rtn.Query.MinimalisticLoad, requestdepth, ParaCredentials, module);

                    testTimePerPage = (int)(callStopWatch.ElapsedMilliseconds);
                    testTimePagesRequired = (int)Math.Ceiling((double)tempObjectList.TotalItems / (double)pageSizeSampleSet[i]);
                    // if the improvment is less than 25 percent lets just take it
                    if ((((masterPagesRequired * masterTimePerPage) / (testTimePagesRequired * testTimePerPage)) < 1.25) && i != 0 && tempAR.HasException == false)
                    {
                        rtn.Query.PageSize = pageSizeSampleSet[(i - 1)];
                        break;
                    }
                    else
                    {
                        masterTimePerPage = testTimePerPage;
                        masterPagesRequired = testTimePagesRequired;
                        rtn.objectList = tempObjectList;
                        rtn.apiResponse = tempAR;
                    }
                }
                pageSizeSampleSet = null;
            }

            tempAR = null;
            tempObjectList = null;
            callStopWatch = null;

            return rtn;
        }

        /// <summary>
        /// Contains all the methods that allow you to interact with the Parature Account module.
        /// </summary>
        public partial class Account
        {
            /// <summary>
            /// Create a Parature Account. Requires an Object and a credentials object. Will return the Newly Created Accountid. Returns 0 if the account creation fails.
            /// </summary>
            public static ParaObjects.ApiCallResponse AccountInsert(ParaObjects.Account account, ParaCredentials paraCredentials)
            {
                var ar = new ParaObjects.ApiCallResponse();
                var doc = new XmlDocument();
                doc = xmlgenerator.AccountGenerateXML(account);
                ar = ApiCallFactory.ObjectCreateUpdate(paraCredentials, Paraenums.ParatureModule.Account, doc, 0);
                account.Accountid = ar.Objectid;
                account.uniqueIdentifier = ar.Objectid;
                return ar;
            }

            /// <summary>
            /// Update a Parature Account. Requires an Object and a credentials object.  Will return the updated Accountid. Returns 0 if the account update operation fails.
            /// </summary>
            public static ParaObjects.ApiCallResponse AccountUpdate(ParaObjects.Account account, ParaCredentials paraCredentials)
            {
                var ar = new ParaObjects.ApiCallResponse();
                ar = ApiCallFactory.ObjectCreateUpdate(paraCredentials, Paraenums.ParatureModule.Account, xmlgenerator.AccountGenerateXML(account), account.Accountid);
                return ar;
            }

            /// <summary>
            /// Returns an account object with all the properties of an account.
            /// </summary>
            /// <param name="accountid">
            ///The account number that you would like to get the details of. 
            ///Value Type: <see cref="Int64" />   (System.Int64)
            ///</param>
            /// <param name="ParaCredentials">
            /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            /// </param>
            /// <param name="RequestDepth">
            /// For a simple account request, please put 0. <br/>When Requesting an account, there might be related objects linked to that Account: such as products, viewable accounts, etc. <br/>With a regular Account detail call, generally only the ID and names of the extra objects are loaded. 
            /// <example>For example, the call will return an Account.Product object, but only the Name and ID values will be filled. All of the other properties of the product object will be empty. If you select RequestDepth=1, then we will go one level deeper into our request and will therefore retrieve the product's detail for you. Products might have assets linked to them, so if you select RequestDepth=2, we will go to an even deeped level and return all the assets properties for that product, etc.<br/> Please make sure you only request the depth you need, as this might make your request slower. </example>
            /// </param>
            public static ParaObjects.Account AccountGetDetails(Int64 accountid, ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth)
            {
                ParaObjects.Account account = new ParaObjects.Account();
                account = AccountFillDetails(accountid, ParaCredentials, RequestDepth, true);
                return account;
            }


            /// <summary>
            /// Returns an account object from an XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="AccountXML">
            /// The Account XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.Account AccountGetDetails(XmlDocument AccountXML)
            {
                ParaObjects.Account account = new ParaObjects.Account();
                account = xmlToObjectParser.AccountParser.AccountFill(AccountXML, 0, true, null);
                account.FullyLoaded = true;

                account.ApiCallResponse.xmlReceived = AccountXML;
                account.ApiCallResponse.Objectid = account.Accountid;

                account.IsDirty = false;
                return account;
            }

            /// <summary>
            /// Returns an account object with all the properties of an account.
            /// </summary>
            /// <param name="accountid">
            ///The account number that you would like to get the details of. 
            ///Value Type: <see cref="Int64" />   (System.Int64)
            ///</param>
            /// <param name="ParaCredentials">
            /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            /// </param>
            public static ParaObjects.Account AccountGetDetails(Int64 accountid, ParaCredentials ParaCredentials)
            {

                ParaObjects.Account account = new ParaObjects.Account();
                account = AccountFillDetails(accountid, ParaCredentials, Paraenums.RequestDepth.Standard, true);

                return account;

            }

            /// <summary>
            /// Provides you with the capability to list accounts, following criteria you would set
            /// by instantiating a ModuleQuery.AccountQuery object
            /// </summary>
            public static ParaObjects.AccountsList AccountsGetList(ParaCredentials ParaCredentials, ModuleQuery.AccountQuery Query)
            {
                return AccountsFillList(ParaCredentials, Query, Paraenums.RequestDepth.Standard);
            }

            /// <summary>
            /// Returns an accounts list object from an XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="AccountListXML">
            /// The Account List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.AccountsList AccountsGetList(XmlDocument AccountListXML)
            {
                ParaObjects.AccountsList accountsList = new ParaObjects.AccountsList();
                accountsList = xmlToObjectParser.AccountParser.AccountsFillList(AccountListXML, true, 0, null);

                accountsList.ApiCallResponse.xmlReceived = AccountListXML;

                return accountsList;
            }

            /// <summary>
            /// Provides you with the capability to list accounts, following criteria you would set
            /// by instantiating a ModuleQuery.AccountQuery object.
            /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
            /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
            /// the standard field depth.
            /// </summary>
            public static ParaObjects.AccountsList AccountsGetList(ParaCredentials ParaCredentials, ModuleQuery.AccountQuery Query, Paraenums.RequestDepth RequestDepth)
            {
                return AccountsFillList(ParaCredentials, Query, RequestDepth);
            }

            /// <summary>
            /// Will return the first 25 accounts returned by the APIs.
            /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
            /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
            /// the standard field depth.
            /// </summary>  
            public static ParaObjects.AccountsList AccountsGetList(ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth)
            {
                return AccountsFillList(ParaCredentials, null, RequestDepth);
            }
            /// <summary>
            /// Will return the first 25 accounts returned by the APIs.
            /// </summary>            
            public static ParaObjects.AccountsList AccountsGetList(ParaCredentials ParaCredentials)
            {
                return AccountsFillList(ParaCredentials, null, Paraenums.RequestDepth.Standard);
            }


            /// <summary>
            /// Provides the capability to delete an Account.
            /// </summary>
            /// <param name="Accountid">
            /// The id of the Account to delete
            /// </param>
            /// <param name="purge">
            /// If purge is set to true, the account will be permanently deleted. Otherwise, it will just be 
            /// moved to the trash bin, so it will still be able to be restored from the service desk.
            ///</param>
            public static ParaObjects.ApiCallResponse AccountDelete(Int64 Accountid, ParaCredentials ParaCredentials, bool purge)
            {
                return ApiCallFactory.ObjectDelete(ParaCredentials, Paraenums.ParatureModule.Account, Accountid, purge);
            }


            /// <summary>
            /// Fills an account list object.
            /// </summary>
            private static ParaObjects.AccountsList AccountsFillList(ParaCredentials ParaCredentials, ModuleQuery.AccountQuery Query, Paraenums.RequestDepth RequestDepth)
            {
                int requestdepth = (int)RequestDepth;
                if (Query == null)
                {
                    Query = new ModuleQuery.AccountQuery();
                }

                // Making a schema call and returning all custom fields to be included in the call.
                if (Query.IncludeAllCustomFields == true)
                {
                    ParaObjects.Account objschem = new ParaObjects.Account();
                    objschem = ApiHandler.Account.AccountSchema(ParaCredentials);
                    Query.IncludeCustomField(objschem.CustomFields);
                }
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ParaObjects.AccountsList AccountsList = new ParaObjects.AccountsList();

                if (Query.RetrieveAllRecords && Query.OptimizePageSize)
                {
                    ParaObjects.OptimizationResult rslt = optimizeObjectPageSize(AccountsList, Query, ParaCredentials, requestdepth, Paraenums.ParatureModule.Account);
                    ar = rslt.apiResponse;
                    Query = (ModuleQuery.AccountQuery)rslt.Query;
                    AccountsList = ((ParaObjects.AccountsList)rslt.objectList);
                    rslt = null;
                }
                else
                {
                    ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureModule.Account, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        AccountsList = xmlToObjectParser.AccountParser.AccountsFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);
                    }
                    AccountsList.ApiCallResponse = ar;
                }

                // Checking if the system needs to recursively call all of the data returned.
                if (Query.RetrieveAllRecords && !ar.HasException)
                {
                    // A flag variable to check if we need to make more calls
                    if (Query.OptimizeCalls)
                    {
                        System.Threading.Thread t;
                        ThreadPool.ObjectList instance = null;
                        int callsRequired = (int)Math.Ceiling((double)(AccountsList.TotalItems / (double)AccountsList.PageSize));
                        for (int i = 2; i <= callsRequired; i++)
                        {
                            //ApiCallFactory.waitCheck(ParaCredentials.Accountid);
                            Query.PageNumber = i;
                            //implement semaphore right here (in the thread pool instance to control the generation of threads
                            instance = new ThreadPool.ObjectList(ParaCredentials, Paraenums.ParatureModule.Account, Query.BuildQueryArguments(), requestdepth);
                            t = new System.Threading.Thread(delegate() { instance.Go(AccountsList); });
                            t.Start();
                        }

                        while (AccountsList.TotalItems > AccountsList.Accounts.Count)
                        {
                            System.Threading.Thread.Sleep(500);
                        }

                        AccountsList.ResultsReturned = AccountsList.Accounts.Count;
                        AccountsList.PageNumber = callsRequired;
                    }
                    else
                    {
                        bool continueCalling = true;
                        while (continueCalling)
                        {
                            ParaObjects.AccountsList objectlist = new ParaObjects.AccountsList();

                            if (AccountsList.TotalItems > AccountsList.Accounts.Count)
                            {
                                // We still need to pull data

                                // Getting next page's data
                                Query.PageNumber = Query.PageNumber + 1;

                                ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureModule.Account, Query.BuildQueryArguments());
                                if (ar.HasException==false)
                                {
                                    objectlist = xmlToObjectParser.AccountParser.AccountsFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);
                                    AccountsList.Accounts.AddRange(objectlist.Accounts);
                                    AccountsList.ResultsReturned = AccountsList.Accounts.Count;
                                    AccountsList.PageNumber = Query.PageNumber;
                                }
                                else
                                {
                                    // There is an error processing request
                                    AccountsList.ApiCallResponse = ar;
                                    continueCalling = false;
                                }

                            }
                            else
                            {
                                // That is it, pulled all the items.
                                continueCalling = false;
                                AccountsList.ApiCallResponse = ar;
                            }
                        }
                    }
                }

                return AccountsList;
            }

            static ParaObjects.Account AccountFillDetails(Int64 accountid, ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth, bool MinimalisticLoad)
            {
                int requestdepth = (int)RequestDepth;
                ParaObjects.Account account = new ParaObjects.Account();
                //account = null;
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, Paraenums.ParatureModule.Account, accountid);
                if (ar.HasException == false)
                {
                    account = xmlToObjectParser.AccountParser.AccountFill(ar.xmlReceived, requestdepth, MinimalisticLoad, ParaCredentials);
                    account.FullyLoaded = true;
                }
                else
                {
                    account.FullyLoaded = false;
                    account.Accountid = 0;
                }
                account.ApiCallResponse = ar;
                account.IsDirty = false;
                return account;
            }

            /// <summary>
            /// Gets an empty object with the scheam (custom fields, if any).
            /// </summary>            
            static public ParaObjects.Account AccountSchema(ParaCredentials ParaCredentials)
            {
                ParaObjects.Account account = new ParaObjects.Account();
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ar = ApiCallFactory.ObjectGetSchema(ParaCredentials, Paraenums.ParatureModule.Account);

                if (ar.HasException == false)
                {
                    account = xmlToObjectParser.AccountParser.AccountFill(ar.xmlReceived, 0, false, ParaCredentials);

                }

                account.ApiCallResponse = ar;
                return account;
            }

            /// <summary>
            /// Gets an empty object with the scheam (custom fields, if any).  This call will also try to create a dummy
            /// record in order to determine if any of the custom fields have special validation rules (e.g. email, phone, url)
            /// and set the "dataType" of the custom field accordingly.
            /// </summary> 
            static public ParaObjects.Account AccountSchemaWithCustomFieldTypes(ParaCredentials ParaCredentials)
            {
                ParaObjects.Account account = AccountSchema(ParaCredentials);

                account = (ParaObjects.Account)ApiCallFactory.ObjectCheckCustomFieldTypes(ParaCredentials, Paraenums.ParatureModule.Account, account);

                return account;
            }          
        }

        /// <summary>
        /// Contains all the methods that allow you to interact with the Parature Product module.
        /// </summary>
        public partial class Product
        {

            /// <summary>
            /// Provides the Schema of the Product module.
            /// </summary>
            public static ParaObjects.Product ProductSchema(ParaCredentials ParaCredentials)
            {
                ParaObjects.Product Product = new ParaObjects.Product();
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ar = ApiCallFactory.ObjectGetSchema(ParaCredentials, Paraenums.ParatureModule.Product);

                if (ar.HasException == false)
                {
                    Product = xmlToObjectParser.ProductParser.ProductFill(ar.xmlReceived, 0, false, ParaCredentials);
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

                Product = (ParaObjects.Product)ApiCallFactory.ObjectCheckCustomFieldTypes(ParaCredentials, Paraenums.ParatureModule.Product, Product);

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
            public static ParaObjects.ApiCallResponse ProductDelete(Int64 Productid, ParaCredentials ParaCredentials, bool purge)
            {
                return ApiCallFactory.ObjectDelete(ParaCredentials, Paraenums.ParatureModule.Product, Productid, purge);
            }


            /// <summary>
            /// Creates a Parature Product. Requires an Object and a credentials object. Will return the Newly Created Productid. Returns 0 if the Product creation failed.
            /// </summary>
            public static ParaObjects.ApiCallResponse ProductInsert(ParaObjects.Product Product, ParaCredentials ParaCredentials)
            {
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc = xmlgenerator.ProductGenerateXML(Product);
                ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, Paraenums.ParatureModule.Product, doc, 0);
                Product.productid = ar.Objectid;
                Product.uniqueIdentifier = ar.Objectid;
                return ar;
            }

            /// <summary>
            /// Updates a Parature Product. Requires an Object and a credentials object.  Will return the updated Productid. Returns 0 if the Product update operation failed.
            /// </summary>
            public static ParaObjects.ApiCallResponse ProductUpdate(ParaObjects.Product Product, ParaCredentials ParaCredentials)
            {
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();

                ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, Paraenums.ParatureModule.Product, xmlgenerator.ProductGenerateXML(Product), Product.productid);


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
            public static ParaObjects.Product ProductGetDetails(Int64 Productid, ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth)
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
                Product = ProductFillDetails(Productid, ParaCredentials, Paraenums.RequestDepth.Standard, true);

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
                product = xmlToObjectParser.ProductParser.ProductFill(ProductXML, 0, true, null);
                product.FullyLoaded = true;

                product.ApiCallResponse.xmlReceived = ProductXML;
                product.ApiCallResponse.Objectid = product.productid;

                product.IsDirty = false;
                return product;
            }

            /// <summary>
            /// Returns an product list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="ProductListXML">
            /// The Product List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.ProductsList ProductsGetList(XmlDocument ProductListXML)
            {
                ParaObjects.ProductsList productsList = new ParaObjects.ProductsList();
                productsList = xmlToObjectParser.ProductParser.ProductsFillList(ProductListXML, true, 0, null);

                productsList.ApiCallResponse.xmlReceived = ProductListXML;

                return productsList;
            }

            /// <summary>
            /// Provides you with the capability to list Products, following criteria you would set
            /// by instantiating a ModuleQuery.ProductQuery object
            /// </summary>
            public static ParaObjects.ProductsList ProductsGetList(ParaCredentials ParaCredentials, ModuleQuery.ProductQuery Query)
            {
                return ProductsFillList(ParaCredentials, Query, Paraenums.RequestDepth.Standard);
            }

            /// <summary>
            /// Provides you with the capability to list Products, following criteria you would set
            /// by instantiating a ModuleQuery.ProductQuery object.
            /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
            /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
            /// the standard field depth.
            /// </summary>
            public static ParaObjects.ProductsList ProductsGetList(ParaCredentials ParaCredentials, ModuleQuery.ProductQuery Query, Paraenums.RequestDepth RequestDepth)
            {
                return ProductsFillList(ParaCredentials, Query, RequestDepth);
            }

            /// <summary>
            /// Returns the list of the first 25 Products returned by the API.
            /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
            /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
            /// the standard field depth.
            /// </summary>            
            public static ParaObjects.ProductsList ProductsGetList(ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth)
            {
                return ProductsFillList(ParaCredentials, null, RequestDepth);
            }

            /// <summary>
            /// Returns the list of the first 25 Products returned by the API.
            /// </summary>
            public static ParaObjects.ProductsList ProductsGetList(ParaCredentials ParaCredentials)
            {
                return ProductsFillList(ParaCredentials, null, Paraenums.RequestDepth.Standard);
            }

            /// <summary>
            /// Fills an Customer list object.
            /// </summary>
            private static ParaObjects.ProductsList ProductsFillList(ParaCredentials ParaCredentials, ModuleQuery.ProductQuery Query, Paraenums.RequestDepth RequestDepth)
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
                    objschem = ApiHandler.Product.ProductSchema(ParaCredentials);
                    Query.IncludeCustomField(objschem.CustomFields);
                }

                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ParaObjects.ProductsList ProductsList = new ParaObjects.ProductsList();

                if (Query.RetrieveAllRecords && Query.OptimizePageSize)
                {
                    ParaObjects.OptimizationResult rslt = optimizeObjectPageSize(ProductsList, Query, ParaCredentials, requestdepth, Paraenums.ParatureModule.Product);
                    ar = rslt.apiResponse;
                    Query = (ModuleQuery.ProductQuery)rslt.Query;
                    ProductsList = ((ParaObjects.ProductsList)rslt.objectList);
                    rslt = null;
                }
                else
                {
                    ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureModule.Product, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        ProductsList = xmlToObjectParser.ProductParser.ProductsFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);
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
                            instance = new ThreadPool.ObjectList(ParaCredentials, Paraenums.ParatureModule.Product, Query.BuildQueryArguments(), requestdepth);
                            t = new System.Threading.Thread(delegate() { instance.Go(ProductsList); });
                            t.Start();
                        }

                        while (ProductsList.TotalItems > ProductsList.Products.Count)
                        {
                            System.Threading.Thread.Sleep(500);
                        }

                        ProductsList.ResultsReturned = ProductsList.Products.Count;
                        ProductsList.PageNumber = callsRequired;
                    }
                    else
                    {
                        bool continueCalling = true;
                        while (continueCalling)
                        {
                            ParaObjects.ProductsList objectlist = new ParaObjects.ProductsList();

                            if (ProductsList.TotalItems > ProductsList.Products.Count)
                            {
                                // We still need to pull data

                                // Getting next page's data
                                Query.PageNumber = Query.PageNumber + 1;

                                ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureModule.Product, Query.BuildQueryArguments());

                                if (ar.HasException == false)
                                {
                                objectlist = xmlToObjectParser.ProductParser.ProductsFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);
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
                        
            static ParaObjects.Product ProductFillDetails(Int64 Productid, ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth, bool MinimalisticLoad)
            {
                int requestdepth = (int)RequestDepth;
                ParaObjects.Product Product = new ParaObjects.Product();
                //Product = null;
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, Paraenums.ParatureModule.Product, Productid);
                if (ar.HasException == false)
                {
                    Product = xmlToObjectParser.ProductParser.ProductFill(ar.xmlReceived, requestdepth, MinimalisticLoad, ParaCredentials);
                    Product.FullyLoaded = true;
                }
                else
                {
                    Product.FullyLoaded = false;
                    Product.productid = 0;
                }
                Product.ApiCallResponse = ar;
                Product.IsDirty = false;
                return Product;
            }


            /// <summary>
            /// Contains all the methods needed to work with the download module's folders.
            /// </summary>
            public partial class ProductFolder
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
                    ParaObjects.DownloadFoldersList Folders = new ParaObjects.DownloadFoldersList();
                    Folders = ApiHandler.Download.DownloadFolder.DownloadFoldersGetList(paracredentials, dfQuery);
                    foreach (ParaObjects.DownloadFolder folder in Folders.DownloadFolders)
                    {
                        if (string.Compare(folder.Name, FolderName, IgnoreCase) == 0)
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
                    dfQuery.AddStaticFieldFilter(EntityQuery.DownloadFolderQuery.DownloadFolderStaticFields.ParentFolder, Paraenums.QueryCriteria.Equal, ParentFolderId.ToString());
                    dfQuery.PageSize = 5000;
                    ParaObjects.DownloadFoldersList Folders = new ParaObjects.DownloadFoldersList();
                    Folders = ApiHandler.Download.DownloadFolder.DownloadFoldersGetList(paracredentials, dfQuery);
                    foreach (ParaObjects.DownloadFolder folder in Folders.DownloadFolders)
                    {
                        if (string.Compare(folder.Name, FolderName, IgnoreCase) == 0)
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
                public static ParaObjects.ApiCallResponse Insert(ParaObjects.ProductFolder ProductFolder, ParaCredentials ParaCredentials)
                {
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    doc = xmlgenerator.ProductFolderGenerateXML(ProductFolder);
                    ar = ApiCallFactory.EntityCreateUpdate(ParaCredentials, Paraenums.ParatureEntity.ProductFolder, doc, 0);
                    ProductFolder.FolderID = ar.Objectid;
                    return ar;
                }

                /// <summary>
                /// Updates a Parature Product Folder. Requires an Object and a credentials object.  Will return the updated ProductFolderid. Returns 0 if the update operation failed.
                /// </summary>
                public static ParaObjects.ApiCallResponse Update(ParaObjects.ProductFolder ProductFolder, ParaCredentials ParaCredentials)
                {
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, Paraenums.ParatureModule.Product, xmlgenerator.ProductFolderGenerateXML(ProductFolder), ProductFolder.FolderID);
                    return ar;
                }

                public static ParaObjects.ApiCallResponse Delete(long objectId, ParaCredentials ParaCredentials)
                {
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.EntityDelete(ParaCredentials, Paraenums.ParatureEntity.ProductFolder, objectId);
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
                public static ParaObjects.ProductFolder ProductFolderGetDetails(Int64 ProductFolderid, ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth)
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
                    productFolder = xmlToObjectParser.ProductParser.ProductFolderParser.ProductFolderFill(ProductFolderXML, 0, null);
                    productFolder.FullyLoaded = true;

                    productFolder.ApiCallResponse.xmlReceived = ProductFolderXML;
                    productFolder.ApiCallResponse.Objectid = productFolder.FolderID;

                    return productFolder;
                }

                /// <summary>
                /// Provides you with the capability to list Product Folders
                /// </summary>
                public static ParaObjects.ProductFoldersList ProductFoldersGetList(ParaCredentials ParaCredentials)
                {
                    return ProductFoldersFillList(ParaCredentials, new EntityQuery.ProductFolderQuery(), Paraenums.RequestDepth.Standard);
                }

                /// <summary>
                /// Provides you with the capability to list Downloads, following criteria you would set
                /// by instantiating a ModuleQuery.DownloadQuery object
                /// </summary>
                public static ParaObjects.ProductFoldersList ProductFoldersGetList(ParaCredentials ParaCredentials, EntityQuery.ProductFolderQuery Query)
                {
                    return ProductFoldersFillList(ParaCredentials, Query, Paraenums.RequestDepth.Standard);
                }

                /// <summary>
                /// Provides you with the capability to list Downloads, following criteria you would set
                /// by instantiating a ModuleQuery.DownloadQuery object.
                /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
                /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
                /// the standard field depth.
                /// </summary>
                public static ParaObjects.ProductFoldersList ProductFoldersGetList(ParaCredentials ParaCredentials, EntityQuery.ProductFolderQuery Query, Paraenums.RequestDepth RequestDepth)
                {
                    return ProductFoldersFillList(ParaCredentials, Query, RequestDepth);
                }

                /// <summary>
                /// Returns an productFolder list object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="ProductFolderListXML">
                /// The ProductFolder List XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.ProductFoldersList ProductFoldersGetList(XmlDocument ProductFolderListXML)
                {
                    ParaObjects.ProductFoldersList productFoldersList = new ParaObjects.ProductFoldersList();
                    productFoldersList = xmlToObjectParser.ProductParser.ProductFolderParser.ProductFoldersFillList(ProductFolderListXML, 0, null);

                    productFoldersList.ApiCallResponse.xmlReceived = ProductFolderListXML;

                    return productFoldersList;
                }

                /// <summary>
                /// Fills a ProductFolderList object.
                /// </summary>
                private static ParaObjects.ProductFoldersList ProductFoldersFillList(ParaCredentials ParaCredentials, EntityQuery.ProductFolderQuery Query, Paraenums.RequestDepth RequestDepth)
                {
                    /*
                    int requestdepth = (int)RequestDepth;
                    ParaObjects.ProductFoldersList ProductFoldersList = new ParaObjects.ProductFoldersList();
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureEntity.ProductFolder, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        ProductFoldersList = xmlToObjectParser.ProductParser.ProductFolderParser.ProductFoldersFillList(ar.xmlReceived, requestdepth, ParaCredentials);
                    }
                    ProductFoldersList.ApiCallResponse = ar;                   
                    return ProductFoldersList;
                     */


                    int requestdepth = (int)RequestDepth;
                    if (Query == null)
                    {
                        Query = new EntityQuery.ProductFolderQuery();
                    }

                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ParaObjects.ProductFoldersList ProductFoldersList = new ParaObjects.ProductFoldersList();

                    if (Query.RetrieveAllRecords && Query.OptimizePageSize)
                    {
                        /*
                        ParaObjects.OptimizationResult rslt = optimizeObjectPageSize(ProductFolderList, Query, ParaCredentials, requestdepth, Paraenums.ParatureModule.Customer);
                        ar = rslt.apiResponse;
                        Query = (ModuleQuery.CustomerQuery)rslt.Query;
                        CustomersList = ((ParaObjects.CustomersList)rslt.objectList);
                        rslt = null;
                        */ 
                    }
                    else
                    {
                        ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureEntity.ProductFolder, Query.BuildQueryArguments());
                        if (ar.HasException == false)
                        {
                            ProductFoldersList = xmlToObjectParser.ProductParser.ProductFolderParser.ProductFoldersFillList(ar.xmlReceived, requestdepth, ParaCredentials);
                            //CustomersList = xmlToObjectParser.CustomerParser.CustomersFillList(ar.xmlReceived, requestdepth, ParaCredentials);
                        }
                        ProductFoldersList.ApiCallResponse = ar;
                    }

                    // Checking if the system needs to recursively call all of the data returned.
                    if (Query.RetrieveAllRecords && !ar.HasException)
                    {
                        // A flag variable to check if we need to make more calls
                        /*
                        if (Query.OptimizeCalls)
                        {
                            System.Threading.Thread t;
                            ThreadPool.ObjectList instance = null;
                            int callsRequired = (int)Math.Ceiling((double)(ProductFoldersList.TotalItems / (double)ProductFoldersList.PageSize));
                            for (int i = 2; i <= callsRequired; i++)
                            {
                                //ApiCallFactory.waitCheck(ParaCredentials.Accountid);
                                Query.PageNumber = i;
                                //implement semaphore right here (in the thread pool instance to control the generation of threads
                                instance = new ThreadPool.ObjectList(ParaCredentials, Paraenums.ParatureEntity.ProductFolder, Query.BuildQueryArguments(), requestdepth);
                                t = new System.Threading.Thread(delegate() { instance.Go(ProductFoldersList); });
                                t.Start();
                            }

                            while (CustomersList.TotalItems > CustomersList.Customers.Count)
                            {
                                System.Threading.Thread.Sleep(500);
                            }

                            CustomersList.ResultsReturned = CustomersList.Customers.Count;
                            CustomersList.PageNumber = callsRequired;
                        }
                        else
                        {
                         */ 
                            bool continueCalling = true;
                            while (continueCalling)
                            {

                                if (ProductFoldersList.TotalItems > ProductFoldersList.ProductFolders.Count)
                                {
                                    // We still need to pull data

                                    // Getting next page's data
                                    Query.PageNumber = Query.PageNumber + 1;

                                    ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureEntity.ProductFolder, Query.BuildQueryArguments());
                                    ProductFoldersList.ProductFolders.AddRange(xmlToObjectParser.ProductParser.ProductFolderParser.ProductFoldersFillList(ar.xmlReceived, requestdepth, ParaCredentials).ProductFolders);
                                    ProductFoldersList.ResultsReturned = ProductFoldersList.ProductFolders.Count;
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

                static ParaObjects.ProductFolder ProductFolderFillDetails(Int64 ProductFolderid, ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth)
                {
                    int requestdepth = (int)RequestDepth;
                    ParaObjects.ProductFolder ProductFolder = new ParaObjects.ProductFolder();
                    //Customer = null;
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, Paraenums.ParatureEntity.ProductFolder, ProductFolderid);
                    if (ar.HasException == false)
                    {
                        ProductFolder = xmlToObjectParser.ProductParser.ProductFolderParser.ProductFolderFill(ar.xmlReceived, requestdepth, ParaCredentials);
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
                    ProductFolder = ProductFolderFillDetails(ProductFolderid, ParaCredentials, Paraenums.RequestDepth.Standard);

                    return ProductFolder;
                }
            }
        }


        /// <summary>
        /// Contains all the methods that allow you to interact with the Parature Chat module.
        /// </summary>
        public partial class Chat
        {
            /// <summary>
            /// Returns a Chat object with all the properties of a chat.
            /// </summary>
            /// <param name="chatid">
            ///The chat number that you would like to get the details of. 
            ///Value Type: <see cref="Int64" />   (System.Int64)
            ///</param>
            /// <param name="ParaCredentials">
            /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            /// </param>
            public static ParaObjects.Chat ChatGetDetails(Int64 chatid, ParaCredentials ParaCredentials)
            {
                ParaObjects.Chat chat = new ParaObjects.Chat();
                chat = ChatFillDetails(chatid, ParaCredentials, false, false);

                return chat;

            }

            /// <summary>
            /// Returns a Chat object with all the properties of a chat.
            /// </summary>
            /// <param name="chatid">
            ///The chat number that you would like to get the details of. 
            ///Value Type: <see cref="Int64" />   (System.Int64)
            ///</param>
            /// <param name="ParaCredentials">
            /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            /// </param>
            /// <param name="IncludeHistory">
            /// Whether to include the chat history (action history) for this particular chat
            /// </param>
            /// <param name="IncludeTranscripts">
            /// Whether to include the chat transcript (chat discussion) for this particular chat 
            /// </param>
            public static ParaObjects.Chat ChatGetDetails(Int64 chatid, ParaCredentials ParaCredentials, Boolean IncludeHistory, Boolean IncludeTranscripts)
            {

                ParaObjects.Chat chat = new ParaObjects.Chat();
                chat = ChatFillDetails(chatid, ParaCredentials, IncludeHistory, IncludeTranscripts);

                return chat;

            }


            public static ParaObjects.ChatList ChatGetList(ParaCredentials ParaCredentials, Boolean IncludeTranscripts, ModuleQuery.ChatQuery Query)
            {
                return ChatFillList(ParaCredentials, IncludeTranscripts, Query, Paraenums.RequestDepth.Standard);
            }


            public static ParaObjects.ChatList ChatGetList(ParaCredentials ParaCredentials, Boolean IncludeTranscripts, Boolean IncludeHistory)
            {
                return ChatFillList(ParaCredentials, IncludeTranscripts, null, Paraenums.RequestDepth.Standard);
            }


            private static ParaObjects.ChatList ChatFillList(ParaCredentials ParaCredentials, Boolean IncludeTranscripts, ModuleQuery.ChatQuery Query, Paraenums.RequestDepth RequestDepth)
            {
                int requestdepth = (int)RequestDepth;
                if (Query == null)
                {
                    Query = new ModuleQuery.ChatQuery();
                }
                // Making a schema call and returning all custom fields to be included in the call.
                if (Query.IncludeAllCustomFields)
                {
                    ParaObjects.Customer objschem = new ParaObjects.Customer();
                    objschem = ApiHandler.Customer.CustomerSchema(ParaCredentials);
                    Query.IncludeCustomField(objschem.CustomFields);
                }
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ParaObjects.ChatList ChatList = new ParaObjects.ChatList();

                if (Query.RetrieveAllRecords && Query.OptimizePageSize)
                {
                    ParaObjects.OptimizationResult rslt = optimizeObjectPageSize(ChatList, Query, ParaCredentials, requestdepth, Paraenums.ParatureModule.Customer);
                    ar = rslt.apiResponse;
                    Query = (ModuleQuery.ChatQuery)rslt.Query;
                    ChatList = ((ParaObjects.ChatList)rslt.objectList);
                    rslt = null;
                }
                else
                {
                    ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureModule.Chat, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        ChatList = xmlToObjectParser.ChatParser.ChatsFillList(ar.xmlReceived, Query.MinimalisticLoad, IncludeTranscripts, requestdepth, ParaCredentials);
                    }
                    ChatList.ApiCallResponse = ar;
                }

                // Checking if the system needs to recursively call all of the data returned.
                if (Query.RetrieveAllRecords && !ar.HasException)
                {
                    // A flag variable to check if we need to make more calls
                    if (Query.OptimizeCalls)
                    {
                        System.Threading.Thread t;
                        ThreadPool.ObjectList instance = null;
                        int callsRequired = (int)Math.Ceiling((double)(ChatList.TotalItems / (double)ChatList.PageSize));
                        for (int i = 2; i <= callsRequired; i++)
                        {
                            //ApiCallFactory.waitCheck(ParaCredentials.Accountid);
                            Query.PageNumber = i;
                            //implement semaphore right here (in the thread pool instance to control the generation of threads
                            instance = new ThreadPool.ObjectList(ParaCredentials, Paraenums.ParatureModule.Customer, Query.BuildQueryArguments(), requestdepth);
                            t = new System.Threading.Thread(delegate() { instance.Go(ChatList, IncludeTranscripts); });
                            t.Start();
                        }

                        while (ChatList.TotalItems > ChatList.chats.Count)
                        {
                            System.Threading.Thread.Sleep(500);
                        }

                        ChatList.ResultsReturned = ChatList.chats.Count;
                        ChatList.PageNumber = callsRequired;
                    }
                    else
                    {
                        bool continueCalling = true;
                        while (continueCalling)
                        {

                            if (ChatList.TotalItems > ChatList.chats.Count)
                            {
                                // We still need to pull data

                                // Getting next page's data
                                Query.PageNumber = Query.PageNumber + 1;

                                ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureModule.Customer, Query.BuildQueryArguments());
                                if (ar.HasException == false)
                                {
                                    ChatList.chats.AddRange(xmlToObjectParser.ChatParser.ChatsFillList(ar.xmlReceived, Query.MinimalisticLoad, IncludeTranscripts, requestdepth, ParaCredentials).chats);
                                    ChatList.ResultsReturned = ChatList.chats.Count;
                                    ChatList.PageNumber = Query.PageNumber;
                                }
                                else
                                {
                                    continueCalling = false;
                                    ChatList.ApiCallResponse = ar;
                                    break;
                                }
                            }
                            else
                            {
                                // That is it, pulled all the items.
                                continueCalling = false;
                                ChatList.ApiCallResponse = ar;
                            }
                        }
                    }
                }

                return ChatList;
            }

            
            static ParaObjects.Chat ChatFillDetails(Int64 chatid, ParaCredentials ParaCredentials, Boolean IncludeHistory, Boolean IncludeTranscripts)
            {
                ParaObjects.Chat chat = new ParaObjects.Chat();
                //Customer = null;
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ArrayList arl = new ArrayList();
                if (IncludeHistory)
                {
                    arl.Add("_history_=true");
                }
                ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, Paraenums.ParatureModule.Chat, chatid, arl);
                if (ar.HasException == false)
                {
                    chat = xmlToObjectParser.ChatParser.ChatFill(ar.xmlReceived, true, 0, IncludeTranscripts, ParaCredentials);
                    chat.FullyLoaded = true;
                }
                else
                {
                    chat.FullyLoaded = false;
                    chat.ChatID = 0;
                }
                chat.ApiCallResponse = ar;
                chat.IsDirty = false;
                return chat;
            }
            
            public static ParaObjects.Chat ChatSchema(ParaCredentials ParaCredentials)
            {
                ParaObjects.Chat chat = new ParaObjects.Chat();
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ar = ApiCallFactory.ObjectGetSchema(ParaCredentials, Paraenums.ParatureModule.Chat);

                if (ar.HasException == false)
                {
                    chat = xmlToObjectParser.ChatParser.ChatFill(ar.xmlReceived, false, 0,false, ParaCredentials);
                }
                chat.ApiCallResponse = ar;
                return chat;
            }

            /// <summary>
            /// Gets an empty object with the scheam (custom fields, if any).  This call will also try to create a dummy
            /// record in order to determine if any of the custom fields have special validation rules (e.g. email, phone, url)
            /// and set the "dataType" of the custom field accordingly.
            /// </summary> 
            static public ParaObjects.Chat ChatSchemaWithCustomFieldTypes(ParaCredentials ParaCredentials)
            {
                ParaObjects.Chat chat = ChatSchema(ParaCredentials);

                chat = (ParaObjects.Chat)ApiCallFactory.ObjectCheckCustomFieldTypes(ParaCredentials, Paraenums.ParatureModule.Chat, chat);

                return chat;
            }

            static public List<ParaObjects.ChatTranscript> ChatTranscripts(Int64 ChatID, ParaCredentials pc)
            {
                List<ParaObjects.ChatTranscript> transcripts = new List<ParaObjects.ChatTranscript>() ;
                ParaObjects.ApiCallResponse ar = ApiCallFactory.ObjectGetDetail(pc, Paraenums.ParatureEntity.ChatTranscript, ChatID);

                if (ar.HasException == false)
                {
                    transcripts = xmlToObjectParser.ChatParser.ChatTranscriptsFillList(ar.xmlReceived);
                }

                return transcripts;
            }
        }


        public partial class Csr
        {
            /// <summary>
            /// Provides the Schema of the CSR module.
            /// </summary>
            public static ParaObjects.Csr CsrSchema(ParaCredentials ParaCredentials)
            {
                ParaObjects.Csr Csr = new ParaObjects.Csr();
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ar = ApiCallFactory.ObjectGetSchema(ParaCredentials, Paraenums.ParatureModule.Csr);

                if (ar.HasException == false)
                {
                    Csr = xmlToObjectParser.CsrParser.CsrFill(ar.xmlReceived);
                }
                Csr.ApiCallResponse = ar;
                Csr.IsDirty = false;
                return Csr;
            }

            /// <summary>
            /// Provides the capability to delete a CSR.
            /// </summary>
            /// <param name="CsrId">
            /// The id of the CSR to delete
            /// </param>
            public static ParaObjects.ApiCallResponse CsrDelete(Int64 CsrId, ParaCredentials ParaCredentials)
            {
                return ApiCallFactory.ObjectDelete(ParaCredentials, Paraenums.ParatureModule.Csr, CsrId, true);
            }

            /// <summary>
            /// Returns an Csr object with all of its properties filled.
            /// </summary>
            /// <param name="Csrid">
            ///The Csr number that you would like to get the details of. 
            ///Value Type: <see cref="Int64" />   (System.Int64)
            ///</param>
            /// <param name="ParaCredentials">
            /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            /// </param>               
            public static ParaObjects.Csr CsrGetDetails(Int64 Csrid, ParaCredentials ParaCredentials)
            {
                ParaObjects.Csr Csr = new ParaObjects.Csr();
                Csr = CsrFillDetails(Csrid, ParaCredentials);
                return Csr;
            }

            /// <summary>
            /// Returns an csr object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="CsrXML">
            /// The Csr XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.Csr CsrGetDetails(XmlDocument CsrXML)
            {
                ParaObjects.Csr csr = new ParaObjects.Csr();
                csr = xmlToObjectParser.CsrParser.CsrFill(CsrXML);

                return csr;
            }

            /// <summary>
            /// Creates a Parature CSR. Requires an Object and a credentials object. Will return the Newly Created CSR ID
            /// </summary>
            public static ParaObjects.ApiCallResponse CsrInsert(ParaObjects.Csr Csr, ParaCredentials ParaCredentials)
            {
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc = xmlgenerator.CsrGenerateXML(Csr);
                ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, Paraenums.ParatureModule.Csr, doc, 0);
                Csr.CsrID = ar.Objectid;
                return ar;
            }

            /// <summary>
            /// Returns an csr list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="CsrListXML">
            /// The Csr List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.CsrsList CsrsGetList(XmlDocument CsrListXML)
            {
                ParaObjects.CsrsList csrsList = new ParaObjects.CsrsList();
                csrsList = xmlToObjectParser.CsrParser.CsrsFillList(CsrListXML);

                csrsList.ApiCallResponse.xmlReceived = CsrListXML;

                return csrsList;
            }

            /// <summary>
            /// Get the list of Csrs from within your Parature license.
            /// </summary>
            public static ParaObjects.CsrsList CsrsGetList(ParaCredentials ParaCredentials)
            {
                return CsrFillList(ParaCredentials, new ModuleQuery.CsrQuery());
            }

            /// <summary>
            /// Updates a Parature Csr. Requires a Csr object and a ParaCredentials object.  Will return the updated Csrid
            /// </summary>
            public static ParaObjects.ApiCallResponse CsrUpdate(ParaObjects.Csr Csr, ParaCredentials ParaCredentials)
            {
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();

                ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, Paraenums.ParatureModule.Csr, xmlgenerator.CsrGenerateXML(Csr), Csr.CsrID);


                return ar;
                //return 0;
            }

            /// <summary>
            /// Get the list of Csrs from within your Parature license.
            /// </summary>
            public static ParaObjects.CsrsList CsrsGetList(ParaCredentials ParaCredentials, ModuleQuery.CsrQuery Query)
            {
                return CsrFillList(ParaCredentials, Query);
            }
            /// <summary>
            /// Fills a Sla list object.
            /// </summary>
            private static ParaObjects.CsrsList CsrFillList(ParaCredentials ParaCredentials, ModuleQuery.CsrQuery Query)
            {

                ParaObjects.CsrsList CsrsList = new ParaObjects.CsrsList();
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureModule.Csr, Query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    CsrsList = xmlToObjectParser.CsrParser.CsrsFillList(ar.xmlReceived);
                }

                CsrsList.ApiCallResponse = ar;


                    // Checking if the system needs to recursively call all of the data returned.
                if (Query.RetrieveAllRecords) 
                    {
                        bool continueCalling = true;
                        while (continueCalling)
                        {
                            ParaObjects.CsrsList objectlist = new ParaObjects.CsrsList();

                            if (CsrsList.TotalItems > CsrsList.Csrs.Count)
                            {
                                // We still need to pull data

                                // Getting next page's data
                                Query.PageNumber = Query.PageNumber + 1;

                                ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureModule.Csr, Query.BuildQueryArguments());

                                objectlist = xmlToObjectParser.CsrParser.CsrsFillList(ar.xmlReceived);

                                if (objectlist.Csrs.Count == 0)
                                {
                                    continueCalling = false;
                                }

                                CsrsList.Csrs.AddRange(objectlist.Csrs);
                                CsrsList.ResultsReturned = CsrsList.Csrs.Count;
                                CsrsList.PageNumber = Query.PageNumber;
                            }
                            else
                            {
                                // That is it, pulled all the items.
                                continueCalling = false;
                                CsrsList.ApiCallResponse = ar;
                            }
                        }
                    }

                return CsrsList;
            }
            static ParaObjects.Csr CsrFillDetails(Int64 Csrid, ParaCredentials ParaCredentials)
            {
                ParaObjects.Csr Csr = new ParaObjects.Csr();
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, Paraenums.ParatureModule.Csr, Csrid);
                if (ar.HasException == false)
                {
                    Csr = xmlToObjectParser.CsrParser.CsrFill(ar.xmlReceived);
                }
                else
                {

                    Csr.CsrID = 0;
                }

                Csr.ApiCallResponse = ar;

                return Csr;
            }

            /// <summary>
            /// Contains all the methods needed to work with the Ticket statuses.
            /// </summary>
            public partial class CsrStatus
            {
                /// <summary>
                /// Returns a filled Csr status object.
                /// </summary>
                /// <param name="CsrStatusid">
                ///The Status id that you would like to get the details of. 
                ///Value Type: <see cref="Int64" />   (System.Int64)
                ///</param>
                /// <param name="ParaCredentials">
                /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
                /// </param>                
                public static ParaObjects.CsrStatus CsrStatusGetDetails(Int64 CsrStatusid, ParaCredentials ParaCredentials)
                {
                    ParaObjects.CsrStatus CsrStatus = new ParaObjects.CsrStatus();
                    CsrStatus = CsrStatusFillDetails(CsrStatusid, ParaCredentials);
                    return CsrStatus;
                }

                /// <summary>
                /// Returns an CsrStatus object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="CsrStatusXML">
                /// The CsrStatus XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.CsrStatus CsrStatusGetDetails(XmlDocument CsrStatusXML)
                {
                    ParaObjects.CsrStatus CsrStatus = new ParaObjects.CsrStatus();
                    CsrStatus = xmlToObjectParser.CsrStatusParser.CsrStatusFill(CsrStatusXML);

                    CsrStatus.ApiCallResponse.xmlReceived = CsrStatusXML;
                    CsrStatus.ApiCallResponse.Objectid = CsrStatus.StatusID;

                    return CsrStatus;
                }

                /// <summary>
                /// Returns an CsrStatus list object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="CsrStatusListXML">
                /// The CsrStatus List XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.CsrStatusList CsrStatusGetList(XmlDocument CsrStatusListXML)
                {
                    ParaObjects.CsrStatusList CsrStatussList = new ParaObjects.CsrStatusList();
                    CsrStatussList = xmlToObjectParser.CsrStatusParser.CsrStatusFillList(CsrStatusListXML);

                    CsrStatussList.ApiCallResponse.xmlReceived = CsrStatusListXML;

                    return CsrStatussList;
                }

                /// <summary>
                /// Provides you with the capability to list statuses
                /// </summary>
                public static ParaObjects.CsrStatusList CsrStatusGetList(ParaCredentials ParaCredentials)
                {
                    return CsrStatusFillList(ParaCredentials);
                }


                /// <summary>
                /// Fills an Csr Status object.
                /// </summary>
                private static ParaObjects.CsrStatusList CsrStatusFillList(ParaCredentials ParaCredentials)
                {

                    ParaObjects.CsrStatusList CsrStatusList = new ParaObjects.CsrStatusList();
                    //DownloadsList = null;
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectSecondLevelGetList(ParaCredentials, Paraenums.ParatureModule.Csr, Paraenums.ParatureEntity.status, new ArrayList(0));
                    if (ar.HasException == false)
                    {
                        CsrStatusList = xmlToObjectParser.CsrStatusParser.CsrStatusFillList(ar.xmlReceived);
                    }
                    CsrStatusList.ApiCallResponse = ar;
                    return CsrStatusList;
                }

                static ParaObjects.CsrStatus CsrStatusFillDetails(Int64 CsrStatusid, ParaCredentials ParaCredentials)
                {

                    ParaObjects.CsrStatus CsrStatus = new ParaObjects.CsrStatus();
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, Paraenums.ParatureEntity.CsrStatus, CsrStatusid);
                    if (ar.HasException == false)
                    {
                        CsrStatus = xmlToObjectParser.CsrStatusParser.CsrStatusFill(ar.xmlReceived);
                    }
                    else
                    {
                        CsrStatus.StatusID = 0;
                    }

                    CsrStatus.ApiCallResponse = ar;
                    return CsrStatus;
                }

            }
        }

        /// <summary>
        /// Contains all the methods that allow you to interact with the Parature Customer module.
        /// </summary>
        public partial class Customer
        {

            /// <summary>
            /// Provides the capability to delete a Customer.
            /// </summary>
            /// <param name="Customerid">
            /// The id of the Customer to delete
            /// </param>
            /// <param name="purge">
            /// If purge is set to true, the Customer will be permanently deleted. Otherwise, it will just be 
            /// moved to the trash bin, so it will still be able to be restored from the service desk.
            ///</param>
            public static ParaObjects.ApiCallResponse CustomerDelete(Int64 Customerid, ParaCredentials ParaCredentials, bool purge)
            {
                return ApiCallFactory.ObjectDelete(ParaCredentials, Paraenums.ParatureModule.Customer, Customerid, purge);
            }


            /// <summary>
            /// Creates a Parature Customer. Requires an Object and a credentials object. Will return the Newly Created Customerid. Returns 0 if the Customer creation fails.
            /// </summary>
            public static ParaObjects.ApiCallResponse CustomerInsert(ParaObjects.Customer customer, ParaCredentials ParaCredentials)
            {
                return CustomerInsert(customer, ParaCredentials, false, false);
            }


            /// <summary>
            /// Creates a Parature Customer. Requires an Object and a credentials object. Need to specific whether to include a customer notification for the insert, as well as if the password should be included in the Notificaiton. 
            /// Will return the Newly Created Customerid. Returns 0 if the Customer creation fails.
            /// </summary>
            public static ParaObjects.ApiCallResponse CustomerInsert(ParaObjects.Customer customer, ParaCredentials ParaCredentials, bool NotifyCustomer, bool IncludePasswordInNotification)
            {
                // Extra arguments for the customer module
                ArrayList arguments = new ArrayList();
                arguments.Add("_notify_=" + NotifyCustomer.ToString().ToLower());

                // Only if the user selects to notify the customer that we include the password.
                if (NotifyCustomer == true)
                {
                    // This is currently a bug. Needs to be uncommented when fixed.
                    arguments.Add("_includePassword_=" + IncludePasswordInNotification.ToString().ToLower());
                }

                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc = xmlgenerator.customerGenerateXML(customer);
                ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, Paraenums.ParatureModule.Customer, doc, 0, arguments);
                customer.customerid = ar.Objectid;
                customer.uniqueIdentifier = ar.Objectid;
                return ar;
            }


            /// <summary>
            /// Updates a Parature Customer. Requires an Object and a credentials object.  Will return the updated Customerid. Returns 0 if the Customer update operation fails.
            /// </summary>
            public static ParaObjects.ApiCallResponse CustomerUpdate(ParaObjects.Customer Customer, ParaCredentials ParaCredentials)
            {
                return CustomerUpdate(Customer, ParaCredentials, false, false);
            }

            /// <summary>
            /// Updates a Parature Customer. Requires an Object and a credentials object. Need to specific whether to include a customer notification for the insert, as well as if the password should be included in the Notificaiton. Will return the updated Customerid. Returns 0 if the Customer update operation fails.
            /// </summary>
            public static ParaObjects.ApiCallResponse CustomerUpdate(ParaObjects.Customer Customer, ParaCredentials ParaCredentials, bool NotifyCustomer, bool IncludePasswordInNotification)
            {
                // Extra arguments for the customer module
                ArrayList arguments = new ArrayList();
                arguments.Add("_notify_=" + NotifyCustomer.ToString().ToLower());
                // Only if the user selects to notify the customer that we include the password.
                if (NotifyCustomer == true)
                {
                    //arguments.Add("_include_password_=" + IncludePasswordInNotification.ToString().ToLower());
                }
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, Paraenums.ParatureModule.Customer, xmlgenerator.customerGenerateXML(Customer), Customer.customerid, arguments);
                return ar;
            }

            /// <summary>
            /// Returns a customer object with all the properties of a customer.
            /// </summary>
            /// <param name="customerid">
            ///The customer number that you would like to get the details of. 
            ///Value Type: <see cref="Int64" />   (System.Int64)
            ///</param>
            /// <param name="ParaCredentials">
            /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            /// </param>
            /// <param name="RequestDepth">
            /// For a simple customer request, please put 0. <br/>When Requesting a Customer, there might be related objects linked to that Customer: such as Account, etc. <br/>With a regular Customer detail call, generally only the ID and names of the second level objects are loaded. 
            /// </param>
            public static ParaObjects.Customer CustomerGetDetails(Int64 customerid, ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth)
            {
                ParaObjects.Customer Customer = new ParaObjects.Customer();
                Customer = CustomerFillDetails(customerid, ParaCredentials, RequestDepth, true);

                return Customer;

            }

            /// <summary>
            /// Returns a Customer object with all of its properties filled.
            /// </summary>
            /// <param name="Customerid">
            ///The Customer number that you would like to get the details of. 
            ///Value Type: <see cref="Int64" />   (System.Int64)
            ///</param>
            /// <param name="ParaCredentials">
            /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            /// </param>
            public static ParaObjects.Customer CustomerGetDetails(Int64 Customerid, ParaCredentials ParaCredentials)
            {

                ParaObjects.Customer Customer = new ParaObjects.Customer();
                Customer = CustomerFillDetails(Customerid, ParaCredentials, Paraenums.RequestDepth.Standard, true);

                return Customer;

            }

            /// <summary>
            /// Returns an customer object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="CustomerXML">
            /// The Customer XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.Customer CustomerGetDetails(XmlDocument CustomerXML)
            {
                ParaObjects.Customer customer = new ParaObjects.Customer();
                customer = xmlToObjectParser.CustomerParser.CustomerFill(CustomerXML, 0, true, null);
                customer.FullyLoaded = true;

                customer.ApiCallResponse.xmlReceived = CustomerXML;
                customer.ApiCallResponse.Objectid = customer.customerid;

                customer.IsDirty = false;
                return customer;
            }

            /// <summary>
            /// Returns an customer list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="CustomerListXML">
            /// The Customer List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.CustomersList CustomersGetList(XmlDocument CustomerListXML)
            {
                ParaObjects.CustomersList customersList = new ParaObjects.CustomersList();
                customersList = xmlToObjectParser.CustomerParser.CustomersFillList(CustomerListXML, true, 0, null);

                customersList.ApiCallResponse.xmlReceived = CustomerListXML;

                return customersList;
            }

            /// <summary>
            /// Provides you with the capability to list Customers, following criteria you would set
            /// by instantiating a ModuleQuery.CustomerQuery object
            /// </summary>
            public static ParaObjects.CustomersList CustomersGetList(ParaCredentials ParaCredentials, ModuleQuery.CustomerQuery Query)
            {
                return CustomersFillList(ParaCredentials, Query, Paraenums.RequestDepth.Standard);
            }

            /// <summary>
            /// Provides you with the capability to list Customers, following criteria you would set
            /// by instantiating a ModuleQuery.CustomerQuery object.
            /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
            /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
            /// the standard field depth.
            /// </summary>
            public static ParaObjects.CustomersList CustomersGetList(ParaCredentials ParaCredentials, ModuleQuery.CustomerQuery Query, Paraenums.RequestDepth RequestDepth)
            {
                return CustomersFillList(ParaCredentials, Query, RequestDepth);
            }

            /// <summary>
            /// Returns the list of the first 25 customers returned by the API.
            /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
            /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
            /// the standard field depth.
            /// </summary>            
            public static ParaObjects.CustomersList CustomersGetList(ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth)
            {
                return CustomersFillList(ParaCredentials, null, RequestDepth);
            }

            /// <summary>
            /// Returns the list of the first 25 customers returned by the API.
            /// </summary>
            public static ParaObjects.CustomersList CustomersGetList(ParaCredentials ParaCredentials)
            {
                return CustomersFillList(ParaCredentials, null, Paraenums.RequestDepth.Standard);
            }

            /// <summary>
            /// Fills an Customer list object.
            /// </summary>
            private static ParaObjects.CustomersList CustomersFillList(ParaCredentials ParaCredentials, ModuleQuery.CustomerQuery Query, Paraenums.RequestDepth RequestDepth)
            {
                int requestdepth = (int)RequestDepth;
                if (Query == null)
                {
                    Query = new ModuleQuery.CustomerQuery();
                }
                // Making a schema call and returning all custom fields to be included in the call.
                if (Query.IncludeAllCustomFields)
                {
                    ParaObjects.Customer objschem = new ParaObjects.Customer();
                    objschem = ApiHandler.Customer.CustomerSchema(ParaCredentials);
                    Query.IncludeCustomField(objschem.CustomFields);
                }
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ParaObjects.CustomersList CustomersList = new ParaObjects.CustomersList();

                if (Query.RetrieveAllRecords && Query.OptimizePageSize)
                {
                    ParaObjects.OptimizationResult rslt = optimizeObjectPageSize(CustomersList, Query, ParaCredentials, requestdepth, Paraenums.ParatureModule.Customer);
                    ar = rslt.apiResponse;
                    Query = (ModuleQuery.CustomerQuery)rslt.Query;
                    CustomersList = ((ParaObjects.CustomersList)rslt.objectList);
                    rslt = null;
                }
                else
                {
                    ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureModule.Customer, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        CustomersList = xmlToObjectParser.CustomerParser.CustomersFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);
                    }
                    CustomersList.ApiCallResponse = ar;
                }

                // Checking if the system needs to recursively call all of the data returned.
                if (Query.RetrieveAllRecords && !ar.HasException)
                {
                    // A flag variable to check if we need to make more calls
                    if (Query.OptimizeCalls)
                    {
                        System.Threading.Thread t;
                        ThreadPool.ObjectList instance = null;
                        int callsRequired = (int)Math.Ceiling((double)(CustomersList.TotalItems / (double)CustomersList.PageSize));
                        for (int i = 2; i <= callsRequired; i++)
                        {
                            //ApiCallFactory.waitCheck(ParaCredentials.Accountid);
                            Query.PageNumber = i;
                            //implement semaphore right here (in the thread pool instance to control the generation of threads
                            instance = new ThreadPool.ObjectList(ParaCredentials, Paraenums.ParatureModule.Customer, Query.BuildQueryArguments(), requestdepth);
                            t = new System.Threading.Thread(delegate() { instance.Go(CustomersList); });
                            t.Start();
                        }

                        while (CustomersList.TotalItems > CustomersList.Customers.Count)
                        {
                            System.Threading.Thread.Sleep(500);
                        }

                        CustomersList.ResultsReturned = CustomersList.Customers.Count;
                        CustomersList.PageNumber = callsRequired;
                    }
                    else
                    {
                        bool continueCalling = true;
                        while (continueCalling)
                        {

                            if (CustomersList.TotalItems > CustomersList.Customers.Count)
                            {
                                // We still need to pull data

                                // Getting next page's data
                                Query.PageNumber = Query.PageNumber + 1;

                                ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureModule.Customer, Query.BuildQueryArguments());
                                if (ar.HasException == false)
                                {
                                    CustomersList.Customers.AddRange(xmlToObjectParser.CustomerParser.CustomersFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials).Customers);
                                    CustomersList.ResultsReturned = CustomersList.Customers.Count;
                                    CustomersList.PageNumber = Query.PageNumber;
                                }
                                else
                                {
                                    continueCalling = false;
                                    CustomersList.ApiCallResponse = ar;
                                    break;
                                }
                            }
                            else
                            {
                                // That is it, pulled all the items.
                                continueCalling = false;
                                CustomersList.ApiCallResponse = ar;
                            }
                        }
                    }
                }

                return CustomersList;
            }

            static ParaObjects.Customer CustomerFillDetails(Int64 Customerid, ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth, bool MinimalisticLoad)
            {
                int requestdepth = (int)RequestDepth;
                ParaObjects.Customer Customer = new ParaObjects.Customer();
                //Customer = null;
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, Paraenums.ParatureModule.Customer, Customerid);
                if (ar.HasException == false)
                {
                    Customer = xmlToObjectParser.CustomerParser.CustomerFill(ar.xmlReceived, requestdepth, MinimalisticLoad, ParaCredentials);
                    Customer.FullyLoaded = true;
                }
                else
                {
                    Customer.FullyLoaded = false;
                    Customer.customerid = 0;
                }
                Customer.ApiCallResponse = ar;
                Customer.IsDirty = false;
                return Customer;
            }


            public static ParaObjects.Customer CustomerSchema(ParaCredentials ParaCredentials)
            {
                ParaObjects.Customer Customer = new ParaObjects.Customer();
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ar = ApiCallFactory.ObjectGetSchema(ParaCredentials, Paraenums.ParatureModule.Customer);

                if (ar.HasException == false)
                {
                    Customer = xmlToObjectParser.CustomerParser.CustomerFill(ar.xmlReceived, 0, false, ParaCredentials);
                }
                Customer.ApiCallResponse = ar;
                return Customer;
            }

            /// <summary>
            /// Gets an empty object with the scheam (custom fields, if any).  This call will also try to create a dummy
            /// record in order to determine if any of the custom fields have special validation rules (e.g. email, phone, url)
            /// and set the "dataType" of the custom field accordingly.
            /// </summary> 
            static public ParaObjects.Customer CustomerSchemaWithCustomFieldTypes(ParaCredentials ParaCredentials)
            {
                ParaObjects.Customer Customer = CustomerSchema(ParaCredentials);

                Customer = (ParaObjects.Customer)ApiCallFactory.ObjectCheckCustomFieldTypes(ParaCredentials, Paraenums.ParatureModule.Customer, Customer);

                return Customer;
            }

        }

        /// <summary>
        /// Contains all the methods that allow you to interact with the Parature Ticket module.
        /// </summary>
        public partial class Ticket
        {
            /// <summary>
            /// Provides the Schema of the Ticket module.
            /// </summary>
            public static ParaObjects.Ticket TicketSchema(ParaCredentials ParaCredentials)
            {
                ParaObjects.Ticket Ticket = new ParaObjects.Ticket();
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ar = ApiCallFactory.ObjectGetSchema(ParaCredentials, Paraenums.ParatureModule.Ticket);

                if (ar.HasException == false)
                {
                    Ticket = xmlToObjectParser.TicketParser.TicketFill(ar.xmlReceived, 0, false, ParaCredentials);
                }
                Ticket.ApiCallResponse = ar;
                return Ticket;
            }

            /// <summary>
            /// Gets an empty object with the scheam (custom fields, if any).  This call will also try to create a dummy
            /// record in order to determine if any of the custom fields have special validation rules (e.g. email, phone, url)
            /// and set the "dataType" of the custom field accordingly.
            /// </summary> 
            static public ParaObjects.Ticket TicketSchemaWithCustomFieldTypes(ParaCredentials ParaCredentials)
            {
                ParaObjects.Ticket Ticket = TicketSchema(ParaCredentials);

                Ticket = (ParaObjects.Ticket)ApiCallFactory.ObjectCheckCustomFieldTypes(ParaCredentials, Paraenums.ParatureModule.Ticket, Ticket);

                return Ticket;
            }

            /// <summary>
            /// Creates a Parature Ticket. Requires an Object and a credentials object. Will return the Newly Created Customerid. Returns 0 if the Customer creation fails.
            /// </summary>
            public static ParaObjects.ApiCallResponse TicketInsert(ParaObjects.Ticket Ticket, ParaCredentials ParaCredentials)
            {
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc = xmlgenerator.TicketGenerateXML(Ticket);
                ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, Paraenums.ParatureModule.Ticket, doc, 0);
                Ticket.id = ar.Objectid;
                Ticket.uniqueIdentifier = ar.Objectid;
                return ar;
            }

            /// <summary>
            /// Updates a Parature Ticket. Requires an Object and a credentials object.  Will return the updated Ticketid. Returns 0 if the Customer update operation failed.
            /// </summary>
            public static ParaObjects.ApiCallResponse TicketUpdate(ParaObjects.Ticket Ticket, ParaCredentials ParaCredentials)
            {
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, Paraenums.ParatureModule.Ticket, xmlgenerator.TicketGenerateXML(Ticket), Ticket.id);
                return ar;
            }

            /// <summary>
            /// Provides the capability to delete a Ticket.
            /// </summary>
            /// <param name="Ticketid">
            /// The id of the Ticket to delete
            /// </param>
            /// <param name="purge">
            /// If purge is set to true, the ticket will be permanently deleted. Otherwise, it will just be 
            /// moved to the trash bin, so it will still be able to be restored from the service desk.
            ///</param>
            public static ParaObjects.ApiCallResponse TicketDelete(Int64 Ticketid, ParaCredentials ParaCredentials, bool purge)
            {
                return ApiCallFactory.ObjectDelete(ParaCredentials, Paraenums.ParatureModule.Ticket, Ticketid, purge);
            }

            /// <summary>
            /// Returns a Ticket object with all the properties of an customer.
            /// </summary>
            /// <param name="TicketNumber">
            ///The Ticket number that you would like to get the details of. 
            ///Value Type: <see cref="Int64" />   (System.Int64)
            ///</param>
            /// <param name="IncludeHistory">
            /// Indicates whether or not to return the Ticket action history. Please keep in mind that the action history for certain tickets 
            /// can be very large, and therefore might slow down the operation.
            /// </param>
            /// <param name="ParaCredentials">
            /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            /// </param>
            /// <param name="RequestDepth">
            /// For a simple Ticket request, please put 0. <br/>When Requesting a Ticket, there might be related objects linked to that Ticket: such as Customer, AssignedToCsr, etc. <br/>With a regular Ticket detail call, generally only the ID and names of the extra objects are loaded. 
            /// <example>For example, the call will return a Ticket.Customer object, but only the Name and ID values will be filled. All of the other properties of the Customer object will be empty. If you select RequestDepth=1, then we will go one level deeper into our request and will therefore retrieve the Customer's detail for you. Customers might be part of an account, so if you select RequestDepth=2, we will go to an even deeper level and return the full account object with all of its properties, etc.<br/> Please make sure you only request the depth you need, as this might make your request slower. </example>
            /// </param>
            public static ParaObjects.Ticket TicketGetDetails(Int64 TicketNumber, bool IncludeHistory, ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth)
            {
                ParaObjects.Ticket Ticket = new ParaObjects.Ticket();
                Ticket = TicketFillDetails(TicketNumber, ParaCredentials, RequestDepth, true, IncludeHistory);
                return Ticket;
            }

            /// <summary>
            /// Returns a Ticket object with all of its details.
            /// </summary>
            /// <param name="IncludeHistory">
            /// Indicates whether or not to return the Ticket action history. Please keep in mind that the action history for certain tickets 
            /// can be very large, and therefore might slow down the operation.
            /// </param> 
            /// <param name="TicketNumber">
            ///The Ticket number that you would like to get the details of. 
            ///Value Type: <see cref="Int64" />   (System.Int64)
            ///</param>
            /// <param name="ParaCredentials">
            /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            /// </param>
            public static ParaObjects.Ticket TicketGetDetails(Int64 TicketNumber, bool IncludeHistory, ParaCredentials ParaCredentials)
            {

                ParaObjects.Ticket Ticket = new ParaObjects.Ticket();
                Ticket = TicketFillDetails(TicketNumber, ParaCredentials, Paraenums.RequestDepth.Standard, true, IncludeHistory);
                return Ticket;
            }

            /// <summary>
            /// Returns an ticket object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="TicketXML">
            /// The Ticket XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.Ticket TicketGetDetails(XmlDocument TicketXML)
            {
                ParaObjects.Ticket ticket = new ParaObjects.Ticket();
                ticket = xmlToObjectParser.TicketParser.TicketFill(TicketXML, 0, true, null);
                ticket.FullyLoaded = true;

                ticket.ApiCallResponse.xmlReceived = TicketXML;
                ticket.ApiCallResponse.Objectid = ticket.id;

                ticket.IsDirty = false;
                return ticket;
            }

            /// <summary>
            /// Returns an ticket list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="TicketListXML">
            /// The Ticket List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.TicketsList TicketsGetList(XmlDocument TicketListXML)
            {
                ParaObjects.TicketsList ticketsList = new ParaObjects.TicketsList();
                ticketsList = xmlToObjectParser.TicketParser.TicketsFillList(TicketListXML, true, 0, null);

                ticketsList.ApiCallResponse.xmlReceived = TicketListXML;

                return ticketsList;
            }

            /// <summary>
            /// Provides you with the capability to list the 25 first Tickets returned by the APIs.
            /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
            /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
            /// the standard field depth.
            /// </summary>
            public static ParaObjects.TicketsList TicketsGetList(ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth)
            {
                return TicketsFillList(ParaCredentials, null, RequestDepth);
            }

            /// <summary>
            /// Provides you with the capability to list Tickets. This will only list the first 25 tickets returned by the Api.
            /// </summary>
            public static ParaObjects.TicketsList TicketsGetList(ParaCredentials ParaCredentials)
            {
                return TicketsFillList(ParaCredentials, null, Paraenums.RequestDepth.Standard);
            }

            /// <summary>
            /// Provides you with the capability to list Tickets, following criteria you would set
            /// by instantiating a ModuleQuery.CustomerQuery object
            /// </summary>
            public static ParaObjects.TicketsList TicketsGetList(ParaCredentials ParaCredentials, ModuleQuery.TicketQuery Query)
            {
                return TicketsFillList(ParaCredentials, Query, Paraenums.RequestDepth.Standard);
            }

            /// <summary>
            /// Provides you with the capability to list Tickets, following criteria you would set
            /// by instantiating a ModuleQuery.TicketQuery object.
            /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
            /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
            /// the standard field depth.
            /// </summary>
            public static ParaObjects.TicketsList TicketsGetList(ParaCredentials ParaCredentials, ModuleQuery.TicketQuery Query, Paraenums.RequestDepth RequestDepth)
            {
                return TicketsFillList(ParaCredentials, Query, RequestDepth);
            }

            /// <summary>
            /// Fills an Ticket list object.
            /// </summary>
            private static ParaObjects.TicketsList TicketsFillList(ParaCredentials ParaCredentials, ModuleQuery.TicketQuery Query, Paraenums.RequestDepth RequestDepth)
            {
                int requestdepth = (int)RequestDepth;
                if (Query == null)
                {
                    Query = new ModuleQuery.TicketQuery();
                }

                // Making a schema call and returning all custom fields to be included in the call.
                if (Query.IncludeAllCustomFields == true)
                {
                    ParaObjects.Ticket objschem = new ParaObjects.Ticket();
                    objschem = ApiHandler.Ticket.TicketSchema(ParaCredentials);
                    Query.IncludeCustomField(objschem.CustomFields);
                }
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ParaObjects.TicketsList TicketsList = new ParaObjects.TicketsList();

                if (Query.RetrieveAllRecords && Query.OptimizePageSize)
                {
                    ParaObjects.OptimizationResult rslt = optimizeObjectPageSize(TicketsList, Query, ParaCredentials, requestdepth, Paraenums.ParatureModule.Ticket);
                    ar = rslt.apiResponse;
                    Query = (ModuleQuery.TicketQuery)rslt.Query;
                    TicketsList = ((ParaObjects.TicketsList)rslt.objectList);
                    rslt = null;
                }
                else
                {
                    ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureModule.Ticket, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        TicketsList = xmlToObjectParser.TicketParser.TicketsFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);
                    }
                    TicketsList.ApiCallResponse = ar;
                }

                // Checking if the system needs to recursively call all of the data returned.
                if (Query.RetrieveAllRecords && !ar.HasException)
                {
                    // A flag variable to check if we need to make more calls
                    if (Query.OptimizeCalls)
                    {
                        System.Threading.Thread t;
                        ThreadPool.ObjectList instance = null;
                        int callsRequired = (int)Math.Ceiling((double)(TicketsList.TotalItems / (double)TicketsList.PageSize));
                        for (int i = 2; i <= callsRequired; i++)
                        {
                            //ApiCallFactory.waitCheck(ParaCredentials.Accountid);
                            Query.PageNumber = i;
                            //implement semaphore right here (in the thread pool instance to control the generation of threads
                            instance = new ThreadPool.ObjectList(ParaCredentials, Paraenums.ParatureModule.Ticket, Query.BuildQueryArguments(), requestdepth);
                            t = new System.Threading.Thread(delegate() { instance.Go(TicketsList); });
                            t.Start();
                        }

                        while (TicketsList.TotalItems > TicketsList.Tickets.Count)
                        {
                            System.Threading.Thread.Sleep(500);
                        }

                        TicketsList.ResultsReturned = TicketsList.Tickets.Count;
                        TicketsList.PageNumber = callsRequired;
                    }
                    else
                    {
                        bool continueCalling = true;
                        while (continueCalling)
                        {
                            ParaObjects.TicketsList objectlist = new ParaObjects.TicketsList();

                            if (TicketsList.TotalItems > TicketsList.Tickets.Count)
                            {
                                // We still need to pull data

                                // Getting next page's data
                                Query.PageNumber = Query.PageNumber + 1;

                                ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureModule.Ticket, Query.BuildQueryArguments());


                                if (ar.HasException == false)
                                {
                                    objectlist = xmlToObjectParser.TicketParser.TicketsFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);

                                    TicketsList.Tickets.AddRange(objectlist.Tickets);
                                    TicketsList.ResultsReturned = TicketsList.Tickets.Count;
                                    TicketsList.PageNumber = Query.PageNumber;
                                }
                                else
                                {
                                    // There is an error processing request
                                    TicketsList.ApiCallResponse = ar;
                                    continueCalling = false;
                                }
                            }
                            else
                            {
                                // That is it, pulled all the items.
                                continueCalling = false;
                                TicketsList.ApiCallResponse = ar;
                            }
                        }
                    }
                }

                return TicketsList;

            }

            internal static ParaObjects.Attachment TicketAddAttachment(ParaCredentials ParaCredentials, Byte[] Attachment, string contentType, string FileName)
            {
                return ApiHandler.UploadFile(Paraenums.ParatureModule.Ticket, ParaCredentials, Attachment, contentType, FileName);
            }
            internal static ParaObjects.Attachment TicketAddAttachment(ParaCredentials ParaCredentials, string text, string contentType, string FileName)
            {
                return ApiHandler.UploadFile(Paraenums.ParatureModule.Ticket, ParaCredentials, text, contentType, FileName);
            }

            static ParaObjects.Ticket TicketFillDetails(Int64 TicketNumber, ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth, bool MinimalisticLoad, bool IncludeHistory)
            {
                int requestdepth = (int)RequestDepth;
                ParaObjects.Ticket Ticket = new ParaObjects.Ticket();
                //Ticket = null;
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();

                if (IncludeHistory == true)
                {
                    ArrayList arl = new ArrayList();
                    arl.Add("_history_=true");
                    ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, Paraenums.ParatureModule.Ticket, TicketNumber, arl);
                }
                else
                {
                    ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, Paraenums.ParatureModule.Ticket, TicketNumber);
                }


                if (ar.HasException == false)
                {
                    Ticket = xmlToObjectParser.TicketParser.TicketFill(ar.xmlReceived, requestdepth, MinimalisticLoad, ParaCredentials);
                    Ticket.FullyLoaded = true;
                }
                else
                {
                    Ticket.FullyLoaded = false;
                    Ticket.id = 0;
                }
                Ticket.ApiCallResponse = ar;
                Ticket.IsDirty = false;
                return Ticket;
            }

            /// <summary>
            /// Grabs a Ticket to the CSR that is making the call.
            /// </summary>
            /// <param name="Ticketid">
            /// The Ticket you would like to run this action on.
            /// </param>
            /// <param name="actionid">
            /// The id of action Grab in your license.
            /// </param>
            /// <param name="ParaCredentials">
            /// Your credentials object.
            /// </param>
            /// <returns></returns>
            public static ParaObjects.ApiCallResponse ActionRunGrab(Int64 Ticketid, Int64 actionid, ParaCredentials ParaCredentials)
            {
                ParaObjects.Action Action = new ParaObjects.Action();
                Action.ActionID = actionid;
                Action.actionType = Paraenums.ActionType.Grab;
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ar = ApiHandler.ActionRun(Ticketid, Action, ParaCredentials, Paraenums.ParatureModule.Ticket);
                return ar;
            }

            /// <summary>
            /// Assigns a Ticket to a specific CSR.
            /// </summary>
            /// <param name="Ticketid">
            /// The Ticket you would like to run this action on.
            /// </param>
            /// <param name="Action">
            /// The action object your would like to run on this ticket.
            /// </param>
            /// <param name="ParaCredentials">
            /// Your credentials object.
            /// </param>
            /// <param name="CsrID">
            /// The CSR you would like to assign this ticket to.
            /// </param>
            /// <returns></returns>
            public static ParaObjects.ApiCallResponse ActionRunAssignToCsr(Int64 Ticketid, ParaObjects.Action Action, Int64 CsrID, ParaCredentials ParaCredentials)
            {
                Action.actionType = Paraenums.ActionType.Assign;
                Action.AssigntToCsrid = CsrID;
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ar = ApiHandler.ActionRun(Ticketid, Action, ParaCredentials, Paraenums.ParatureModule.Ticket);
                return ar;
            }

            /// <summary>
            /// Assigns a Ticket to a specific Queue.
            /// </summary>
            /// <param name="Ticketid">
            /// The Ticket you would like to run this action on.
            /// </param>
            /// <param name="Action">
            /// The action object your would like to run on this ticket.
            /// </param>
            /// <param name="ParaCredentials">
            /// Your credentials object.
            /// </param>
            /// <param name="QueueID">
            /// The Queue you would like to assign this ticket to.
            /// </param>
            /// <returns></returns>
            public static ParaObjects.ApiCallResponse ActionRunAssignToQueue(Int64 Ticketid, ParaObjects.Action Action, Int64 QueueID, ParaCredentials ParaCredentials)
            {
                Action.actionType = Paraenums.ActionType.Assign_Queue;
                Action.AssignToQueueid = QueueID;
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ar = ApiHandler.ActionRun(Ticketid, Action, ParaCredentials, Paraenums.ParatureModule.Ticket);
                return ar;
            }

            /// <summary>
            /// Assigns a Ticket to a specific Queue.
            /// </summary>
            /// <param name="Ticketid">
            /// The Ticket you would like to run this action on.
            /// </param>
            /// <param name="Action">
            /// The action object your would like to run on this ticket.
            /// </param>
            /// <param name="ParaCredentials">
            /// Your credentials object.
            /// </param>
            /// <returns></returns>
            public static ParaObjects.ApiCallResponse ActionRun(Int64 Ticketid, ParaObjects.Action Action, ParaCredentials ParaCredentials)
            {
                Action.actionType = Paraenums.ActionType.Other;
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ar = ApiHandler.ActionRun(Ticketid, Action, ParaCredentials, Paraenums.ParatureModule.Ticket);
                return ar;
            }
        }

        /// <summary>
        /// Contains all the methods that allow you to interact with the Parature Asset module.
        /// </summary>
        public partial class Asset
        {

            /// <summary>
            /// Provides the Schema of the asset module.
            /// </summary>
            public static ParaObjects.Asset AssetSchema(ParaCredentials ParaCredentials)
            {
                ParaObjects.Asset Asset = new ParaObjects.Asset();

                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ar = ApiCallFactory.ObjectGetSchema(ParaCredentials, Paraenums.ParatureModule.Asset);

                if (ar.HasException == false)
                {
                    Asset = xmlToObjectParser.AssetParser.AssetFill(ar.xmlReceived, false, 0, ParaCredentials);
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

                asset = (ParaObjects.Asset)ApiCallFactory.ObjectCheckCustomFieldTypes(ParaCredentials, Paraenums.ParatureModule.Asset, asset);

                return asset;
            }

            /// <summary>
            /// Creates a Parature Ticket. Requires an Object and a credentials object. Will return the Newly Created Assetid. Returns 0 if the Asset creation fails.
            /// </summary>
            public static ParaObjects.ApiCallResponse AssetInsert(ParaObjects.Asset Asset, ParaCredentials ParaCredentials)
            {
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc = xmlgenerator.AssetGenerateXML(Asset);
                ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, Paraenums.ParatureModule.Asset, doc, 0);
                Asset.Assetid = ar.Objectid;
                Asset.uniqueIdentifier = ar.Objectid;
                return ar;
            }

            /// <summary>
            /// Updates a Parature Asset. Requires an Object and a credentials object.  Will return the updated Assetid. Returns 0 if the Asset update operation failed.
            /// </summary>
            public static ParaObjects.ApiCallResponse AssetUpdate(ParaObjects.Asset Asset, ParaCredentials ParaCredentials)
            {
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, Paraenums.ParatureModule.Asset, xmlgenerator.AssetGenerateXML(Asset), Asset.Assetid);
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
            public static ParaObjects.ApiCallResponse AssetDelete(Int64 Assetid, ParaCredentials ParaCredentials, bool purge)
            {
                return ApiCallFactory.ObjectDelete(ParaCredentials, Paraenums.ParatureModule.Asset, Assetid, purge);
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
            public static ParaObjects.Asset AssetGetDetails(Int64 Assetid, ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth)
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
                return AssetGetDetails(Assetid, ParaCredentials, Paraenums.RequestDepth.Standard);
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
                asset = xmlToObjectParser.AssetParser.AssetFill(AssetXML, true, 0, null);
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
            public static ParaObjects.AssetsList AssetsGetList(XmlDocument AssetsListXML)
            {
                ParaObjects.AssetsList assetsList = new ParaObjects.AssetsList();
                assetsList = xmlToObjectParser.AssetParser.AssetsFillList(AssetsListXML, true, 0, null);

                assetsList.ApiCallResponse.xmlReceived = AssetsListXML;

                return assetsList;
            }

            /// <summary>
            /// Provides you with the capability to list Assets, following criteria you would set
            /// by instantiating a ModuleQuery.AssetQuery object
            /// </summary>
            public static ParaObjects.AssetsList AssetsGetList(ParaCredentials ParaCredentials, ModuleQuery.AssetQuery Query)
            {
                return AssetsFillList(ParaCredentials, Query, Paraenums.RequestDepth.Standard);
            }

            /// <summary>
            /// Gets the first 25 assets returned by the API.
            /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
            /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
            /// the standard field depth.
            /// </summary>
            public static ParaObjects.AssetsList AssetsGetList(ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth)
            {
                return AssetsFillList(ParaCredentials, null, RequestDepth);
            }
            /// <summary>
            /// Gets the first 25 assets returned by the API.
            /// </summary>            
            public static ParaObjects.AssetsList AssetsGetList(ParaCredentials ParaCredentials)
            {
                return AssetsFillList(ParaCredentials, null, Paraenums.RequestDepth.Standard);
            }
            /// <summary>
            /// Provides you with the capability to list Assets, following criteria you would set
            /// by instantiating a ModuleQuery.AssetQuery object.
            /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
            /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
            /// the standard field depth.
            /// </summary>
            public static ParaObjects.AssetsList AssetsGetList(ParaCredentials ParaCredentials, ModuleQuery.AssetQuery Query, Paraenums.RequestDepth RequestDepth)
            {
                return AssetsFillList(ParaCredentials, Query, RequestDepth);
            }

            /// <summary>
            /// Fills an Asset list object.
            /// </summary>
            private static ParaObjects.AssetsList AssetsFillList(ParaCredentials ParaCredentials, ModuleQuery.AssetQuery Query, Paraenums.RequestDepth RequestDepth)
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
                    objschem = ApiHandler.Asset.AssetSchema(ParaCredentials);
                    Query.IncludeCustomField(objschem.CustomFields);
                }

                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ParaObjects.AssetsList AssetsList = new ParaObjects.AssetsList();

                if (Query.RetrieveAllRecords && Query.OptimizePageSize)
                {
                    ParaObjects.OptimizationResult rslt = optimizeObjectPageSize(AssetsList, Query, ParaCredentials, requestdepth, Paraenums.ParatureModule.Asset);
                    ar = rslt.apiResponse;
                    Query = (ModuleQuery.AssetQuery)rslt.Query;
                    AssetsList = ((ParaObjects.AssetsList)rslt.objectList);
                    rslt = null;
                }
                else
                {
                    ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureModule.Asset, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        AssetsList = xmlToObjectParser.AssetParser.AssetsFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);
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
                            instance = new ThreadPool.ObjectList(ParaCredentials, Paraenums.ParatureModule.Asset, Query.BuildQueryArguments(), requestdepth);
                            t = new System.Threading.Thread(delegate() { instance.Go(AssetsList); });
                            t.Start();
                        }

                        while (AssetsList.TotalItems > AssetsList.Assets.Count)
                        {
                            System.Threading.Thread.Sleep(500);
                        }

                        AssetsList.ResultsReturned = AssetsList.Assets.Count;
                        AssetsList.PageNumber = callsRequired;
                    }
                    else
                    {
                        bool continueCalling = true;
                        while (continueCalling)
                        {
                            ParaObjects.AssetsList objectlist = new ParaObjects.AssetsList();

                            if (AssetsList.TotalItems > AssetsList.Assets.Count)
                            {
                                // We still need to pull data

                                // Getting next page's data
                                Query.PageNumber = Query.PageNumber + 1;

                                ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureModule.Asset, Query.BuildQueryArguments());

                                if (ar.HasException == false)
                                {
                                objectlist = xmlToObjectParser.AssetParser.AssetsFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);
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

            static ParaObjects.Asset AssetFillDetails(Int64 Assetid, ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth)
            {
                int requestdepth = (int)RequestDepth;
                ParaObjects.Asset Asset = new ParaObjects.Asset();
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();


                ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, Paraenums.ParatureModule.Asset, Assetid);

                if (ar.HasException == false)
                {
                    Asset = xmlToObjectParser.AssetParser.AssetFill(ar.xmlReceived, true, requestdepth, ParaCredentials);
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
            public static ParaObjects.ApiCallResponse ActionRun(Int64 Assetid, ParaObjects.Action Action, ParaCredentials ParaCredentials)
            {
                Action.actionType = Paraenums.ActionType.Other;
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ar = ApiHandler.ActionRun(Assetid, Action, ParaCredentials, Paraenums.ParatureModule.Asset);
                return ar;
            }
        }

        /// <summary>
        /// Contains all the methods that allow you to interact with the Parature Download module.
        /// </summary>
        public partial class Download
        {
            /// <summary>
            /// Provides the Schema of the download module.
            /// </summary>
            public static ParaObjects.Download DownloadSchema(ParaCredentials ParaCredentials)
            {
                ParaObjects.Download Download = new ParaObjects.Download(true);
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ar = ApiCallFactory.ObjectGetSchema(ParaCredentials, Paraenums.ParatureModule.Download);

                if (ar.HasException == false)
                {
                    Download = xmlToObjectParser.DownloadParser.DownloadFill(ar.xmlReceived, 0, false, ParaCredentials);
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
            public static ParaObjects.ApiCallResponse DownloadDelete(Int64 Downloadid, ParaCredentials ParaCredentials, bool purge)
            {
                return ApiCallFactory.ObjectDelete(ParaCredentials, Paraenums.ParatureModule.Download, Downloadid, purge);
            }

            /// <summary>
            /// Creates a Parature Download. Requires an Object and a credentials object. Will return the Newly Created Downloadid. Returns 0 if the Customer creation fails.
            /// </summary>
            public static ParaObjects.ApiCallResponse DownloadInsert(ParaObjects.Download Download, ParaCredentials ParaCredentials)
            {
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc = xmlgenerator.DownloadGenerateXML(Download);
                ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, Paraenums.ParatureModule.Download, doc, 0);
                Download.Downloadid = ar.Objectid;
                Download.uniqueIdentifier = ar.Objectid;
                return ar;
            }

            /// <summary>
            /// Updates a Parature Download. Requires an Object and a credentials object.  Will return the updated Downloadid. Returns 0 if the Customer update operation fails.
            /// </summary>
            public static ParaObjects.ApiCallResponse DownloadUpdate(ParaObjects.Download Download, ParaCredentials ParaCredentials)
            {
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();

                ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, Paraenums.ParatureModule.Download, xmlgenerator.DownloadGenerateXML(Download), Download.Downloadid);


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
            public static ParaObjects.Download DownloadGetDetails(Int64 Downloadid, ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth)
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
                return DownloadFillDetails(Downloadid, ParaCredentials, Paraenums.RequestDepth.Standard, true);
            }
            internal static ParaObjects.Attachment DownloadUploadFile(ParaCredentials ParaCredentials, string text, string contentType, string FileName)
            {
                return ApiHandler.UploadFile(Paraenums.ParatureModule.Download, ParaCredentials, text, contentType, FileName);
            }

            internal static ParaObjects.Attachment DownloadUploadFile(ParaCredentials ParaCredentials, Byte[] Attachment, string contentType, string FileName)
            {
                return ApiHandler.UploadFile(Paraenums.ParatureModule.Download, ParaCredentials, Attachment, contentType, FileName);
            }

            internal static ParaObjects.Attachment DownloadUploadFile(ParaCredentials ParaCredentials, System.Net.Mail.Attachment Attachment)
            {
                return ApiHandler.UploadFile(Paraenums.ParatureModule.Download, ParaCredentials, Attachment);
            }

            /// <summary>
            /// Returns an download object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="DownloadXML">
            /// The Download XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.Download DownloadGetDetails(XmlDocument DownloadXML)
            {
                ParaObjects.Download download = new ParaObjects.Download(true);
                download = xmlToObjectParser.DownloadParser.DownloadFill(DownloadXML, 0, true, null);
                download.FullyLoaded = true;

                download.ApiCallResponse.xmlReceived = DownloadXML;
                download.ApiCallResponse.Objectid = download.Downloadid;

                download.IsDirty = false;
                return download;
            }

            /// <summary>
            /// Returns an download list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="DownloadListXML">
            /// The Download List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.DownloadsList DownloadsGetList(XmlDocument DownloadListXML)
            {
                ParaObjects.DownloadsList downloadsList = new ParaObjects.DownloadsList();
                downloadsList = xmlToObjectParser.DownloadParser.DownloadsFillList(DownloadListXML, true, 0, null);

                downloadsList.ApiCallResponse.xmlReceived = DownloadListXML;

                return downloadsList;
            }

            /// <summary>
            /// Provides you with the capability to list Downloads, following criteria you would set
            /// by instantiating a ModuleQuery.DownloadQuery object
            /// </summary>
            public static ParaObjects.DownloadsList DownloadsGetList(ParaCredentials ParaCredentials, ModuleQuery.DownloadQuery Query)
            {
                return DownloadsFillList(ParaCredentials, Query, Paraenums.RequestDepth.Standard);
            }

            /// <summary>
            /// Provides you with the capability to list Downloads, following criteria you would set
            /// by instantiating a ModuleQuery.DownloadQuery object.
            /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
            /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
            /// the standard field depth.
            /// </summary>
            public static ParaObjects.DownloadsList DownloadsGetList(ParaCredentials ParaCredentials, ModuleQuery.DownloadQuery Query, Paraenums.RequestDepth RequestDepth)
            {
                return DownloadsFillList(ParaCredentials, Query, RequestDepth);
            }
            /// <summary>
            /// Returns a list of the first 25 Downloads returned by the APIs.
            /// </summary>
            public static ParaObjects.DownloadsList DownloadsGetList(ParaCredentials ParaCredentials)
            {
                return DownloadsFillList(ParaCredentials, null, Paraenums.RequestDepth.Standard);
            }
            /// <summary>
            /// Returns a list of the first 25 Downloads returned by the APIs.
            /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
            /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
            /// the standard field depth.
            /// </summary>
            public static ParaObjects.DownloadsList DownloadsGetList(ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth)
            {
                return DownloadsFillList(ParaCredentials, null, RequestDepth);
            }

            /// <summary>
            /// Fills a Download list object.
            /// </summary>
            private static ParaObjects.DownloadsList DownloadsFillList(ParaCredentials ParaCredentials, ModuleQuery.DownloadQuery Query, Paraenums.RequestDepth RequestDepth)
            {
                int requestdepth = (int)RequestDepth;
                if (Query == null)
                {
                    Query = new ModuleQuery.DownloadQuery();
                }


                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ParaObjects.DownloadsList DownloadsList = new ParaObjects.DownloadsList();

                if (Query.RetrieveAllRecords && Query.OptimizePageSize)
                {
                    ParaObjects.OptimizationResult rslt = optimizeObjectPageSize(DownloadsList, Query, ParaCredentials, requestdepth, Paraenums.ParatureModule.Download);
                    ar = rslt.apiResponse;
                    Query = (ModuleQuery.DownloadQuery)rslt.Query;
                    DownloadsList = ((ParaObjects.DownloadsList)rslt.objectList);
                    rslt = null;
                }
                else
                {
                    ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureModule.Download, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        DownloadsList = xmlToObjectParser.DownloadParser.DownloadsFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);
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
                            instance = new ThreadPool.ObjectList(ParaCredentials, Paraenums.ParatureModule.Download, Query.BuildQueryArguments(), requestdepth);
                            t = new System.Threading.Thread(delegate() { instance.Go(DownloadsList); });
                            t.Start();
                        }

                        while (DownloadsList.TotalItems > DownloadsList.Downloads.Count)
                        {
                            System.Threading.Thread.Sleep(500);
                        }

                        DownloadsList.ResultsReturned = DownloadsList.Downloads.Count;
                        DownloadsList.PageNumber = callsRequired;
                    }
                    else
                    {
                        bool continueCalling = true;
                        while (continueCalling)
                        {
                            ParaObjects.DownloadsList objectlist = new ParaObjects.DownloadsList();

                            if (DownloadsList.TotalItems > DownloadsList.Downloads.Count)
                            {
                                // We still need to pull data

                                // Getting next page's data
                                Query.PageNumber = Query.PageNumber + 1;

                                ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureModule.Download, Query.BuildQueryArguments());

                                if (ar.HasException == false)
                                {
                                    objectlist = xmlToObjectParser.DownloadParser.DownloadsFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);
                                    DownloadsList.Downloads.AddRange(objectlist.Downloads);
                                    DownloadsList.ResultsReturned = DownloadsList.Downloads.Count;
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

            static ParaObjects.Download DownloadFillDetails(Int64 Downloadid, ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth, bool MinimalisticLoad)
            {
                int requestdepth = (int)RequestDepth;
                ParaObjects.Download Download = new ParaObjects.Download(true);
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, Paraenums.ParatureModule.Download, Downloadid);
                if (ar.HasException == false)
                {
                    Download = xmlToObjectParser.DownloadParser.DownloadFill(ar.xmlReceived, requestdepth, MinimalisticLoad, ParaCredentials);
                    Download.FullyLoaded = true;
                }
                else
                {
                    Download.FullyLoaded = false;
                    Download.Downloadid = 0;
                }

                Download.ApiCallResponse = ar;
                Download.IsDirty = false;
                return Download;
            }

            /// <summary>
            /// Contains all the methods needed to work with the download module's folders.
            /// </summary>
            public partial class DownloadFolder
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
                    ParaObjects.DownloadFoldersList Folders = new ParaObjects.DownloadFoldersList();
                    Folders = ApiHandler.Download.DownloadFolder.DownloadFoldersGetList(paracredentials, dfQuery);
                    foreach (ParaObjects.DownloadFolder folder in Folders.DownloadFolders)
                    {
                        if (string.Compare(folder.Name, FolderName, IgnoreCase) == 0)
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
                    dfQuery.AddStaticFieldFilter(EntityQuery.DownloadFolderQuery.DownloadFolderStaticFields.ParentFolder, Paraenums.QueryCriteria.Equal, ParentFolderId.ToString());
                    dfQuery.PageSize = 5000;
                    ParaObjects.DownloadFoldersList Folders = new ParaObjects.DownloadFoldersList();
                    Folders = ApiHandler.Download.DownloadFolder.DownloadFoldersGetList(paracredentials, dfQuery);
                    foreach (ParaObjects.DownloadFolder folder in Folders.DownloadFolders)
                    {
                        if (string.Compare(folder.Name, FolderName, IgnoreCase) == 0)
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
                public static ParaObjects.ApiCallResponse Insert(ParaObjects.DownloadFolder downloadfolder, ParaCredentials ParaCredentials)
                {
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    doc = xmlgenerator.DownloadFolderGenerateXML(downloadfolder);
                    ar = ApiCallFactory.EntityCreateUpdate(ParaCredentials, Paraenums.ParatureEntity.DownloadFolder, doc, 0);
                    downloadfolder.FolderID = ar.Objectid;
                    return ar;
                }

                /// <summary>
                /// Updates a Parature Download. Requires an Object and a credentials object.  Will return the updated Downloadid. Returns 0 if the Customer update operation fails.
                /// </summary>
                public static ParaObjects.ApiCallResponse Update(ParaObjects.DownloadFolder downloadfolder, ParaCredentials ParaCredentials)
                {
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.EntityCreateUpdate(ParaCredentials, Paraenums.ParatureEntity.DownloadFolder, xmlgenerator.DownloadFolderGenerateXML(downloadfolder), downloadfolder.FolderID);

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
                public static ParaObjects.DownloadFolder DownloadFolderGetDetails(Int64 DownloadFolderid, ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth)
                {
                    ParaObjects.DownloadFolder DownloadFolder = new ParaObjects.DownloadFolder();
                    DownloadFolder = DownloadFolderFillDetails(DownloadFolderid, ParaCredentials, RequestDepth);

                    return DownloadFolder;

                }


                /// <summary>
                /// Provides you with the capability to list Downloads, following criteria you would set
                /// by instantiating a ModuleQuery.DownloadQuery object
                /// </summary>
                public static ParaObjects.DownloadFoldersList DownloadFoldersGetList(ParaCredentials ParaCredentials, EntityQuery.DownloadFolderQuery Query)
                {
                    return DownloadFoldersFillList(ParaCredentials, Query, Paraenums.RequestDepth.Standard);
                }

                /// <summary>
                /// Provides you with the capability to list Downloads, following criteria you would set
                /// by instantiating a ModuleQuery.DownloadQuery object.
                /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
                /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
                /// the standard field depth.
                /// </summary>
                public static ParaObjects.DownloadFoldersList DownloadFoldersGetList(ParaCredentials ParaCredentials, EntityQuery.DownloadFolderQuery Query, Paraenums.RequestDepth RequestDepth)
                {
                    return DownloadFoldersFillList(ParaCredentials, Query, RequestDepth);
                }

                /// <summary>
                /// Fills an Download list object.
                /// </summary>
                private static ParaObjects.DownloadFoldersList DownloadFoldersFillList(ParaCredentials ParaCredentials, EntityQuery.DownloadFolderQuery Query, Paraenums.RequestDepth RequestDepth)
                {
                    int requestdepth = (int)RequestDepth;
                    ParaObjects.DownloadFoldersList DownloadFoldersList = new ParaObjects.DownloadFoldersList();
                    //DownloadsList = null;
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureEntity.DownloadFolder, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        DownloadFoldersList = xmlToObjectParser.DownloadParser.DownloadFolderParser.DownloadFoldersFillList(ar.xmlReceived, requestdepth, ParaCredentials);
                    }
                    DownloadFoldersList.ApiCallResponse = ar;


                    // Checking if the system needs to recursively call all of the data returned.
                    if (Query.RetrieveAllRecords)
                    {
                        bool continueCalling = true;
                        while (continueCalling)
                        {
                            ParaObjects.DownloadFoldersList objectlist = new ParaObjects.DownloadFoldersList();

                            if (DownloadFoldersList.TotalItems > DownloadFoldersList.DownloadFolders.Count)
                            {
                                // We still need to pull data

                                // Getting next page's data
                                Query.PageNumber = Query.PageNumber + 1;

                                ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureEntity.DownloadFolder, Query.BuildQueryArguments());

                                objectlist = xmlToObjectParser.DownloadParser.DownloadFolderParser.DownloadFoldersFillList(ar.xmlReceived,0, ParaCredentials);

                                if (objectlist.DownloadFolders.Count == 0)
                                {
                                    continueCalling = false;
                                }

                                DownloadFoldersList.DownloadFolders.AddRange(objectlist.DownloadFolders);
                                DownloadFoldersList.ResultsReturned = DownloadFoldersList.DownloadFolders.Count;
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

                static ParaObjects.DownloadFolder DownloadFolderFillDetails(Int64 DownloadFolderid, ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth)
                {
                    int requestdepth = (int)RequestDepth;
                    ParaObjects.DownloadFolder DownloadFolder = new ParaObjects.DownloadFolder();
                    //Customer = null;
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, Paraenums.ParatureEntity.DownloadFolder, DownloadFolderid);
                    if (ar.HasException == false)
                    {
                        DownloadFolder = xmlToObjectParser.DownloadParser.DownloadFolderParser.DownloadFolderFill(ar.xmlReceived, requestdepth, ParaCredentials);
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
                    downloadFolder = xmlToObjectParser.DownloadParser.DownloadFolderParser.DownloadFolderFill(DownloadFolderXML, 0, null);
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
                public static ParaObjects.DownloadFoldersList DownloadFoldersGetList(XmlDocument DownloadFolderListXML)
                {
                    ParaObjects.DownloadFoldersList downloadFoldersList = new ParaObjects.DownloadFoldersList();
                    downloadFoldersList = xmlToObjectParser.DownloadParser.DownloadFolderParser.DownloadFoldersFillList(DownloadFolderListXML, 0, null);

                    downloadFoldersList.ApiCallResponse.xmlReceived = DownloadFolderListXML;

                    return downloadFoldersList;
                }


                public static ParaObjects.DownloadFolder DownloadFolderGetDetails(Int64 DownloadFolderid, ParaCredentials ParaCredentials)
                {

                    ParaObjects.DownloadFolder DownloadFolder = new ParaObjects.DownloadFolder();
                    DownloadFolder = DownloadFolderFillDetails(DownloadFolderid, ParaCredentials, Paraenums.RequestDepth.Standard);

                    return DownloadFolder;
                }
            }
        }

        /// <summary>
        /// Contains all the methods that allow you to interact with the Parature Knowledge Base module.
        /// </summary>
        public partial class Article
        {
            /// <summary>
            /// Provides the Schema of the article module.
            /// </summary>
            public static ParaObjects.Article ArticleSchema(ParaCredentials ParaCredentials)
            {
                ParaObjects.Article Article = new ParaObjects.Article();
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ar = ApiCallFactory.ObjectGetSchema(ParaCredentials, Paraenums.ParatureModule.Article);

                if (ar.HasException == false)
                {
                    Article = xmlToObjectParser.ArticleParser.ArticleFill(ar.xmlReceived, 0, false, ParaCredentials);
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
            public static ParaObjects.ApiCallResponse ArticleDelete(Int64 Articleid, ParaCredentials ParaCredentials, bool purge)
            {
                return ApiCallFactory.ObjectDelete(ParaCredentials, Paraenums.ParatureModule.Article, Articleid, purge);
            }

            /// <summary>
            /// Creates a Parature Article. Requires an Object and a credentials object. Will return the Newly Created Articleid
            /// </summary>
            public static ParaObjects.ApiCallResponse ArticleInsert(ParaObjects.Article Article, ParaCredentials ParaCredentials)
            {
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc = xmlgenerator.ArticleGenerateXML(Article);
                ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, Paraenums.ParatureModule.Article, doc, 0);
                Article.Articleid = ar.Objectid;
                Article.uniqueIdentifier = ar.Objectid;
                return ar;
            }

            /// <summary>
            /// Updates a Parature Article. Requires an Object and a credentials object.  Will return the updated Articleid
            /// </summary>
            public static ParaObjects.ApiCallResponse ArticleUpdate(ParaObjects.Article Article, ParaCredentials ParaCredentials)
            {
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();

                ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, Paraenums.ParatureModule.Article, xmlgenerator.ArticleGenerateXML(Article), Article.Articleid);


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
            public static ParaObjects.Article ArticleGetDetails(Int64 Articleid, ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth)
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
                Article = ArticleFillDetails(Articleid, ParaCredentials, Paraenums.RequestDepth.Standard, true);

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
                article = xmlToObjectParser.ArticleParser.ArticleFill(ArticleXML, 0, true, null);
                article.FullyLoaded = true;

                article.ApiCallResponse.xmlReceived = ArticleXML;
                article.ApiCallResponse.Objectid = article.Articleid;

                article.IsDirty = false;
                return article;
            }

            /// <summary>
            /// Returns an article list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="ArticleListXML">
            /// The Article List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.ArticlesList ArticlesGetList(XmlDocument ArticleListXML)
            {
                ParaObjects.ArticlesList articlesList = new ParaObjects.ArticlesList();
                articlesList = xmlToObjectParser.ArticleParser.ArticlesFillList(ArticleListXML, true, 0, null);

                articlesList.ApiCallResponse.xmlReceived = ArticleListXML;

                return articlesList;
            }

            /// <summary>
            /// Provides you with the capability to list Customers, following criteria you would set
            /// by instantiating a ModuleQuery.CustomerQuery object
            /// </summary>
            public static ParaObjects.ArticlesList ArticlesGetList(ParaCredentials ParaCredentials, ModuleQuery.ArticleQuery Query)
            {
                return ArticlesFillList(ParaCredentials, Query, Paraenums.RequestDepth.Standard);
            }

            /// <summary>
            /// Provides you with the capability to list Articles, following criteria you would set
            /// by instantiating a ModuleQuery.ArticleQuery object.
            /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
            /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
            /// the standard field depth.
            /// </summary>
            public static ParaObjects.ArticlesList ArticlesGetList(ParaCredentials ParaCredentials, ModuleQuery.ArticleQuery Query, Paraenums.RequestDepth RequestDepth)
            {
                return ArticlesFillList(ParaCredentials, Query, RequestDepth);
            }
            /// <summary>
            /// Returns the first 25 Articles returned by the APIs.
            /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
            /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
            /// the standard field depth.
            /// </summary>           
            public static ParaObjects.ArticlesList ArticlesGetList(ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth)
            {
                return ArticlesFillList(ParaCredentials, null, RequestDepth);
            }
            /// <summary>
            /// Returns the first 25 Articles returned by the APIs.           
            /// </summary>
            public static ParaObjects.ArticlesList ArticlesGetList(ParaCredentials ParaCredentials)
            {
                return ArticlesFillList(ParaCredentials, null, Paraenums.RequestDepth.Standard);
            }

            /// <summary>
            /// Fills an Article list object.
            /// </summary>
            private static ParaObjects.ArticlesList ArticlesFillList(ParaCredentials ParaCredentials, ModuleQuery.ArticleQuery Query, Paraenums.RequestDepth RequestDepth)
            {
                int requestdepth = (int)RequestDepth;
                if (Query == null)
                {
                    Query = new ModuleQuery.ArticleQuery();
                }


                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ParaObjects.ArticlesList ArticlesList = new ParaObjects.ArticlesList();

                if (Query.RetrieveAllRecords && Query.OptimizePageSize)
                {
                    ParaObjects.OptimizationResult rslt = optimizeObjectPageSize(ArticlesList, Query, ParaCredentials, requestdepth, Paraenums.ParatureModule.Article);
                    ar = rslt.apiResponse;
                    Query = (ModuleQuery.ArticleQuery)rslt.Query;
                    ArticlesList = ((ParaObjects.ArticlesList)rslt.objectList);
                    rslt = null;
                }
                else
                {
                    ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureModule.Article, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        ArticlesList = xmlToObjectParser.ArticleParser.ArticlesFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);
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
                            instance = new ThreadPool.ObjectList(ParaCredentials, Paraenums.ParatureModule.Article, Query.BuildQueryArguments(), requestdepth);
                            t = new System.Threading.Thread(delegate() { instance.Go(ArticlesList); });
                            t.Start();
                        }

                        while (ArticlesList.TotalItems > ArticlesList.Articles.Count)
                        {
                            System.Threading.Thread.Sleep(500);
                        }

                        ArticlesList.ResultsReturned = ArticlesList.Articles.Count;
                        ArticlesList.PageNumber = callsRequired;
                    }
                    else
                    {
                        bool continueCalling = true;
                        while (continueCalling)
                        {
                            ParaObjects.ArticlesList objectlist = new ParaObjects.ArticlesList();

                            if (ArticlesList.TotalItems > ArticlesList.Articles.Count)
                            {
                                // We still need to pull data

                                // Getting next page's data
                                Query.PageNumber = Query.PageNumber + 1;

                                ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureModule.Article, Query.BuildQueryArguments());
                                if (ar.HasException == false)
                                {
                                    objectlist = xmlToObjectParser.ArticleParser.ArticlesFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);
                                    ArticlesList.Articles.AddRange(objectlist.Articles);
                                    ArticlesList.ResultsReturned = ArticlesList.Articles.Count;
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

            static ParaObjects.Article ArticleFillDetails(Int64 Articleid, ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth, bool MinimalisticLoad)
            {
                int requestdepth = (int)RequestDepth;
                ParaObjects.Article article = new ParaObjects.Article();
                ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, Paraenums.ParatureModule.Article, Articleid);
                if (ar.HasException == false)
                {
                    article = xmlToObjectParser.ArticleParser.ArticleFill(ar.xmlReceived, requestdepth, MinimalisticLoad, ParaCredentials);
                    article.FullyLoaded = true;
                }
                else
                {
                    article.FullyLoaded = false;
                    article.Articleid = 0;
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
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.EntityGetSchema(ParaCredentials, Paraenums.ParatureEntity.ArticleFolder);

                    if (ar.HasException == false)
                    {
                        ArticleFolder = xmlToObjectParser.ArticleParser.ArticleFolderParser.ArticleFolderFill(ar.xmlReceived, 0, ParaCredentials);
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
                    ParaObjects.ArticleFoldersList Folders = new ParaObjects.ArticleFoldersList();
                    Folders = ApiHandler.Article.ArticleFolder.ArticleFoldersGetList(paracredentials, afQuery);
                    foreach (ParaObjects.ArticleFolder folder in Folders.ArticleFolders)
                    {
                        if (string.Compare(folder.Name, FolderName, IgnoreCase) == 0)
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
                    afQuery.AddStaticFieldFilter(EntityQuery.ArticleFolderQuery.ArticleFolderStaticFields.Name, Paraenums.QueryCriteria.Equal, ParentFolderId.ToString());
                    afQuery.PageSize = 5000;
                    ParaObjects.ArticleFoldersList Folders = new ParaObjects.ArticleFoldersList();
                    Folders = ApiHandler.Article.ArticleFolder.ArticleFoldersGetList(paracredentials, afQuery);
                    foreach (ParaObjects.ArticleFolder folder in Folders.ArticleFolders)
                    {
                        if (string.Compare(folder.Name, FolderName, IgnoreCase) == 0)
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
                public static ParaObjects.ApiCallResponse Delete(Int64 Folderid, ParaCredentials ParaCredentials)
                {
                    return ApiCallFactory.EntityDelete(ParaCredentials, Paraenums.ParatureEntity.ArticleFolder, Folderid);
                }

                /// <summary>
                /// Creates a Parature Article Folder. Requires an Object and a credentials object. Will return the Newly Created Downloadid. Returns 0 if the Customer creation fails.
                /// </summary>
                public static ParaObjects.ApiCallResponse Insert(ParaObjects.ArticleFolder articleFolder, ParaCredentials ParaCredentials)
                {
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    doc = xmlgenerator.ArticleFolderGenerateXML(articleFolder);
                    ar = ApiCallFactory.EntityCreateUpdate(ParaCredentials, Paraenums.ParatureEntity.ArticleFolder, doc, 0);
                    articleFolder.FolderID = ar.Objectid;
                    return ar;
                    //return 0;
                }

                /// <summary>
                /// Updates a Parature Article. Requires an Object and a credentials object.  Will return the updated Downloadid. Returns 0 if the Customer update operation fails.
                /// </summary>
                public static ParaObjects.ApiCallResponse Update(ParaObjects.ArticleFolder articleFolder, ParaCredentials ParaCredentials)
                {
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();

                    ar = ApiCallFactory.EntityCreateUpdate(ParaCredentials, Paraenums.ParatureEntity.ArticleFolder, xmlgenerator.ArticleFolderGenerateXML(articleFolder), articleFolder.FolderID);

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
                public static ParaObjects.ArticleFolder ArticleFolderGetDetails(Int64 ArticleFolderid, ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth)
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
                    articleFolder = xmlToObjectParser.ArticleParser.ArticleFolderParser.ArticleFolderFill(ArticleFolderXml, 0, null);
                    articleFolder.FullyLoaded = true;

                    articleFolder.ApiCallResponse.xmlReceived = ArticleFolderXml;
                    articleFolder.ApiCallResponse.Objectid = articleFolder.FolderID;

                    return articleFolder;
                }

                /// <summary>
                /// Provides you with the capability to list Article Folders.
                /// </summary>
                public static ParaObjects.ArticleFoldersList ArticleFoldersGetList(ParaCredentials ParaCredentials)
                {
                    EntityQuery.ArticleFolderQuery eq = new EntityQuery.ArticleFolderQuery();
                    eq.RetrieveAllRecords = true;
                    return ArticleFoldersFillList(ParaCredentials, eq, Paraenums.RequestDepth.Standard);
                }

                /// <summary>
                /// Provides you with the capability to list Article Folders, following criteria you would set
                /// by instantiating a ModuleQuery.DownloadQuery object
                /// </summary>
                public static ParaObjects.ArticleFoldersList ArticleFoldersGetList(ParaCredentials ParaCredentials, EntityQuery.ArticleFolderQuery Query)
                {
                    return ArticleFoldersFillList(ParaCredentials, Query, Paraenums.RequestDepth.Standard);
                }

                /// <summary>
                /// Provides you with the capability to list Article Folders, following criteria you would set
                /// by instantiating a ModuleQuery.ArticleQuery object.
                /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
                /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
                /// the standard field depth.
                /// </summary>
                public static ParaObjects.ArticleFoldersList ArticleFoldersGetList(ParaCredentials ParaCredentials, EntityQuery.ArticleFolderQuery Query, Paraenums.RequestDepth RequestDepth)
                {
                    return ArticleFoldersFillList(ParaCredentials, Query, RequestDepth);
                }

                /// <summary>
                /// Returns an article folder list object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="ArticleFoldersListXml">
                /// The Article Folder List XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.ArticleFoldersList ArticleFoldersGetList(XmlDocument ArticleFoldersListXml)
                {
                    ParaObjects.ArticleFoldersList articleFolderList = new ParaObjects.ArticleFoldersList();
                    articleFolderList = xmlToObjectParser.ArticleParser.ArticleFolderParser.ArticleFoldersFillList(ArticleFoldersListXml, 0, null);

                    articleFolderList.ApiCallResponse.xmlReceived = ArticleFoldersListXml;

                    return articleFolderList;
                }

                /// <summary>
                /// Fills an Article list object.
                /// </summary>
                private static ParaObjects.ArticleFoldersList ArticleFoldersFillList(ParaCredentials ParaCredentials, EntityQuery.ArticleFolderQuery Query, Paraenums.RequestDepth RequestDepth)
                {
                    /*
                    int requestdepth = (int)RequestDepth;
                    ParaObjects.ArticleFoldersList ArticleFoldersList = new ParaObjects.ArticleFoldersList();
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureEntity.ArticleFolder, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        ArticleFoldersList = xmlToObjectParser.ArticleParser.ArticleFolderParser.ArticleFoldersFillList(ar.xmlReceived, requestdepth, ParaCredentials);
                    }



                    ArticleFoldersList.ApiCallResponse = ar;
                    return ArticleFoldersList;
                    */





                    int requestdepth = (int)RequestDepth;
                    if (Query == null)
                    {
                        Query = new EntityQuery.ArticleFolderQuery();
                    }

                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ParaObjects.ArticleFoldersList ArticleFoldersList = new ParaObjects.ArticleFoldersList();

                    ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureEntity.ArticleFolder, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        ArticleFoldersList = xmlToObjectParser.ArticleParser.ArticleFolderParser.ArticleFoldersFillList(ar.xmlReceived, requestdepth, ParaCredentials);
                    }

                    ArticleFoldersList.ApiCallResponse = ar;

                    // Checking if the system needs to recursively call all of the data returned.
                    if (Query.RetrieveAllRecords && !ar.HasException)
                    {
                        bool continueCalling = true;
                        while (continueCalling)
                        {

                            if (ArticleFoldersList.TotalItems > ArticleFoldersList.ArticleFolders.Count)
                            {
                                // Getting next page's data
                                Query.PageNumber = Query.PageNumber + 1;

                                ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureEntity.ArticleFolder, Query.BuildQueryArguments());

                                if (ar.HasException == false)
                                {
                                    ArticleFoldersList.ArticleFolders.AddRange(xmlToObjectParser.ArticleParser.ArticleFolderParser.ArticleFoldersFillList(ar.xmlReceived, requestdepth, ParaCredentials).ArticleFolders);
                                    ArticleFoldersList.ResultsReturned = ArticleFoldersList.ArticleFolders.Count;
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

                static ParaObjects.ArticleFolder ArticleFolderFillDetails(Int64 ArticleFolderid, ParaCredentials ParaCredentials, Paraenums.RequestDepth RequestDepth)
                {
                    int requestdepth = (int)RequestDepth;
                    ParaObjects.ArticleFolder ArticleFolder = new ParaObjects.ArticleFolder();
                    //Customer = null;
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, Paraenums.ParatureEntity.ArticleFolder, ArticleFolderid);
                    if (ar.HasException == false)
                    {
                        ArticleFolder = xmlToObjectParser.ArticleParser.ArticleFolderParser.ArticleFolderFill(ar.xmlReceived, requestdepth, ParaCredentials);
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
                    ArticleFolder = ArticleFolderFillDetails(ArticleFolderid, ParaCredentials, Paraenums.RequestDepth.Standard);

                    return ArticleFolder;
                }
            }

        }

        /// <summary>
        /// Contains all the methods to access shared entities (like CSRs, SLAs, departments, etc)
        /// </summary>
        public partial class Entities
        {
            public partial class Timezone
            {
                /// <summary>
                /// Returns a Timezone object with all of its properties filled.
                /// </summary>
                /// <param name="Csrid">
                ///The Timezone id that you would like to get the details of. 
                ///Value Type: <see cref="Int64" />   (System.Int64)
                ///</param>
                /// <param name="ParaCredentials">
                /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
                /// </param>               
                public static ParaObjects.Timezone TimezoneGetDetails(Int64 TimezoneId, ParaCredentials ParaCredentials)
                {
                    ParaObjects.Timezone Timezone = new ParaObjects.Timezone();
                    Timezone = TimezoneFillDetails(TimezoneId, ParaCredentials);
                    return Timezone;
                }

                /// <summary>
                /// Returns an Timezone object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="TimezoneXML">
                /// The Timezone XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.Timezone TimezoneGetDetails(XmlDocument TimezoneXML)
                {
                    ParaObjects.Timezone Timezone = new ParaObjects.Timezone();
                    Timezone = xmlToObjectParser.TimezoneParser.TimezoneFill(TimezoneXML);

                    return Timezone;
                }

                /// <summary>
                /// Returns an Timezone list object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="TimezoneListXML">
                /// The Timezone List XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.TimezonesList TimezoneGetList(XmlDocument TimezoneListXML)
                {
                    ParaObjects.TimezonesList TimezonesList = new ParaObjects.TimezonesList();
                    TimezonesList = xmlToObjectParser.TimezoneParser.TimezonesFillList(TimezoneListXML);

                    TimezonesList.ApiCallResponse.xmlReceived = TimezoneListXML;

                    return TimezonesList;
                }
                /// <summary>
                /// Get the list of Timezones from within your Parature license.
                /// </summary>
                public static ParaObjects.TimezonesList TimezoneGetList(ParaCredentials ParaCredentials)
                {
                    return TimezoneFillList(ParaCredentials, new EntityQuery.TimezoneQuery());
                }

                /// <summary>
                /// Get the list of Timezones from within your Parature license.
                /// </summary>
                public static ParaObjects.TimezonesList TimezoneGetList(ParaCredentials ParaCredentials, EntityQuery.TimezoneQuery Query)
                {
                    return TimezoneFillList(ParaCredentials, Query);
                }
                /// <summary>
                /// Fills a Timezone List object.
                /// </summary>
                private static ParaObjects.TimezonesList TimezoneFillList(ParaCredentials ParaCredentials, EntityQuery.TimezoneQuery Query)
                {

                    ParaObjects.TimezonesList TimezoneList = new ParaObjects.TimezonesList();
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureEntity.Timezone, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        TimezoneList = xmlToObjectParser.TimezoneParser.TimezonesFillList(ar.xmlReceived);
                    }
                    TimezoneList.ApiCallResponse = ar;
                    return TimezoneList;
                }
                static ParaObjects.Timezone TimezoneFillDetails(Int64 TimezoneId, ParaCredentials ParaCredentials)
                {
                    ParaObjects.Timezone Timezone = new ParaObjects.Timezone();
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, Paraenums.ParatureEntity.Timezone, TimezoneId);
                    if (ar.HasException == false)
                    {
                        Timezone = xmlToObjectParser.TimezoneParser.TimezoneFill(ar.xmlReceived);
                    }
                    else
                    {

                        Timezone.TimezoneID = 0;
                    }

                    return Timezone;
                }
            }

            public partial class Status
            {
                /// <summary>
                /// Returns a Status object with all of its properties filled.
                /// </summary>
                /// <param name="Csrid">
                ///The Status id that you would like to get the details of. 
                ///Value Type: <see cref="Int64" />   (System.Int64)
                ///</param>
                /// <param name="ParaCredentials">
                /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
                /// </param>               
                public static ParaObjects.Status StatusGetDetails(Int64 StatusId, ParaCredentials ParaCredentials)
                {
                    ParaObjects.Status Status = new ParaObjects.Status();
                    Status = StatusFillDetails(StatusId, ParaCredentials);
                    return Status;
                }

                /// <summary>
                /// Returns an Status object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="StatusXML">
                /// The Status XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.Status StatusGetDetails(XmlDocument StatusXML)
                {
                    ParaObjects.Status Status = new ParaObjects.Status();
                    Status = xmlToObjectParser.StatusParser.StatusFill(StatusXML);

                    return Status;
                }

                /// <summary>
                /// Returns an Status list object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="StatusListXML">
                /// The Status List XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.StatusList StatusGetList(XmlDocument StatusListXML)
                {
                    ParaObjects.StatusList StatussList = new ParaObjects.StatusList();
                    StatussList = xmlToObjectParser.StatusParser.StatusFillList(StatusListXML);

                    StatussList.ApiCallResponse.xmlReceived = StatusListXML;

                    return StatussList;
                }

                /// <summary>
                /// Get the list of Statuss from within your Parature license.
                /// </summary>
                public static ParaObjects.StatusList StatusGetList(ParaCredentials ParaCredentials)
                {
                    return StatusFillList(ParaCredentials, new EntityQuery.StatusQuery());
                }

                /// <summary>
                /// Get the list of Statuss from within your Parature license.
                /// </summary>
                public static ParaObjects.StatusList StatusGetList(ParaCredentials ParaCredentials, EntityQuery.StatusQuery Query)
                {
                    return StatusFillList(ParaCredentials, Query);
                }
                /// <summary>
                /// Fills a Status List object.
                /// </summary>
                private static ParaObjects.StatusList StatusFillList(ParaCredentials ParaCredentials, EntityQuery.StatusQuery Query)
                {

                    ParaObjects.StatusList StatusList = new ParaObjects.StatusList();
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureEntity.status, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        StatusList = xmlToObjectParser.StatusParser.StatusFillList(ar.xmlReceived);
                    }
                    StatusList.ApiCallResponse = ar;


                    // Checking if the system needs to recursively call all of the data returned.
                    if (Query.RetrieveAllRecords)
                    {
                        bool continueCalling = true;
                        while (continueCalling)
                        {
                            ParaObjects.StatusList objectlist = new ParaObjects.StatusList();

                            if (StatusList.TotalItems > StatusList.Statuses.Count)
                            {
                                // We still need to pull data

                                // Getting next page's data
                                Query.PageNumber = Query.PageNumber + 1;

                                ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureEntity.status, Query.BuildQueryArguments());

                                objectlist = xmlToObjectParser.StatusParser.StatusFillList(ar.xmlReceived);

                                if (objectlist.Statuses.Count == 0)
                                {
                                    continueCalling = false;
                                }

                                StatusList.Statuses.AddRange(objectlist.Statuses);
                                StatusList.ResultsReturned = StatusList.Statuses.Count;
                                StatusList.PageNumber = Query.PageNumber;
                            }
                            else
                            {
                                // That is it, pulled all the items.
                                continueCalling = false;
                                StatusList.ApiCallResponse = ar;
                            }
                        }
                    }

                    return StatusList;
                }
                static ParaObjects.Status StatusFillDetails(Int64 StatusId, ParaCredentials ParaCredentials)
                {
                    ParaObjects.Status Status = new ParaObjects.Status();
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, Paraenums.ParatureEntity.status, StatusId);
                    if (ar.HasException == false)
                    {
                        Status = xmlToObjectParser.StatusParser.StatusFill(ar.xmlReceived);
                    }
                    else
                    {

                        Status.StatusID = 0;
                    }

                    return Status;
                }
            }

            public partial class Role
            {
                /// <summary>
                /// Returns a Role object with all of its properties filled.
                /// </summary>
                /// <param name="RoleID">
                /// The Role number that you would like to get the details of.
                /// Value Type: <see cref="Int64" />   (System.Int64)
                ///</param>
                /// <param name="ParaCredentials">
                /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
                /// </param>
                /// <returns></returns>
                public static ParaObjects.Role RoleGetDetails(Int64 RoleID, ParaCredentials ParaCredentials)
                {
                    ParaObjects.Role Role = new ParaObjects.Role();
                    Role = RoleFillDetails(RoleID, ParaCredentials);
                    return Role;
                }

                /// <summary>
                /// Returns an role object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="RoleXML">
                /// The Role XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.Role RoleGetDetails(XmlDocument RoleXML)
                {
                    ParaObjects.Role role = new ParaObjects.Role();
                    role = xmlToObjectParser.RoleParser.RoleFill(RoleXML);

                    return role;
                }

                /// <summary>
                /// Returns an role list object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="RoleListXML">
                /// The Role List XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.RolesList RolesGetList(XmlDocument RoleListXML)
                {
                    ParaObjects.RolesList rolesList = new ParaObjects.RolesList();
                    rolesList = xmlToObjectParser.RoleParser.RolesFillList(RoleListXML);

                    rolesList.ApiCallResponse.xmlReceived = RoleListXML;

                    return rolesList;
                }

                /// <summary>
                /// Get the List of Roles from within your Parature license
                /// </summary>
                /// <param name="ParaCredentials"></param>
                /// <param name="Query"></param>
                /// <returns></returns>
                public static ParaObjects.RolesList RolesGetList(ParaCredentials ParaCredentials, EntityQuery.RoleQuery Query, Paraenums.ParatureModule Module)
                {
                    return RoleFillList(ParaCredentials, Query, Module);
                }


                public static ParaObjects.RolesList RolesGetList(ParaCredentials ParaCredentials, Paraenums.ParatureModule Module)
                {
                    return RoleFillList(ParaCredentials, new EntityQuery.RoleQuery(), Module);
                }

                /// <summary>
                /// Fills a Role list object
                /// </summary>
                /// <param name="ParaCredentials"></param>
                /// <param name="Query"></param>
                /// <returns></returns>
                private static ParaObjects.RolesList RoleFillList(ParaCredentials ParaCredentials, EntityQuery.RoleQuery Query, Paraenums.ParatureModule Module)
                {
                    ParaObjects.RolesList RolesList = new ParaObjects.RolesList();
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectSecondLevelGetList(ParaCredentials, Module, Paraenums.ParatureEntity.role, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        RolesList = xmlToObjectParser.RoleParser.RolesFillList(ar.xmlReceived);
                    }
                    RolesList.ApiCallResponse = ar;

                    // Checking if the system needs to recursively call all of the data returned.
                    if (Query.RetrieveAllRecords)
                    {
                        bool continueCalling = true;
                        while (continueCalling)
                        {
                            ParaObjects.RolesList objectlist = new ParaObjects.RolesList();

                            if (RolesList.TotalItems > RolesList.Roles.Count)
                            {
                                // We still need to pull data

                                // Getting next page's data
                                Query.PageNumber = Query.PageNumber + 1;

                                ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureEntity.role, Query.BuildQueryArguments());

                                objectlist = xmlToObjectParser.RoleParser.RolesFillList(ar.xmlReceived);

                                if (objectlist.Roles.Count == 0)
                                {
                                    continueCalling = false;
                                }

                                RolesList.Roles.AddRange(objectlist.Roles);
                                RolesList.ResultsReturned = RolesList.Roles.Count;
                                RolesList.PageNumber = Query.PageNumber;
                            }
                            else
                            {
                                // That is it, pulled all the items.
                                continueCalling = false;
                                RolesList.ApiCallResponse = ar;
                            }
                        }
                    }

                    return RolesList;
                }

                static ParaObjects.Role RoleFillDetails(Int64 RoleID, ParaCredentials ParaCredentials)
                {
                    ParaObjects.Role Role = new ParaObjects.Role();
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, Paraenums.ParatureEntity.role, RoleID);
                    if (ar.HasException == false)
                    {
                        Role = xmlToObjectParser.RoleParser.RoleFill(ar.xmlReceived);
                    }
                    else
                    {
                        Role.RoleID = 0;
                    }
                    return Role;
                }
                /// <summary>
                /// Returns a Role object if it finds a Role matching the name you provide to it. Will return null if nothing is found.
                /// </summary>
                /// <param name="RoleName">
                /// The Role Name to look for.
                /// </param>
                /// <param name="ParaCredentials"></param>
                /// <param name="IgnoreCase">
                /// When looking for this Role, please indicate whether the search should be case sensitive or not.
                /// </param>
                /// <returns></returns>
                static ParaObjects.Role HelperRoleFindByName(string RoleName, ParaCredentials ParaCredentials, bool IgnoreCase, Paraenums.ParatureModule Module)
                {
                    ParaObjects.Role foundRole = new ParaObjects.Role();
                    EntityQuery.RoleQuery rq = new EntityQuery.RoleQuery();
                    rq.PageSize = 1000;
                    ParaObjects.RolesList oRole = new ParaObjects.RolesList();
                    oRole = ApiHandler.Entities.Role.RolesGetList(ParaCredentials, rq, Module);

                    string RoleToCompare = "";
                    string RoleDestRole = "";
                    if (IgnoreCase == true)
                    {
                        RoleToCompare = RoleName.ToLower();
                    }
                    bool found = false;
                    foreach (ParaObjects.Role role in oRole.Roles)
                    {
                        if (IgnoreCase == true)
                        {
                            RoleDestRole = role.Name.ToLower();
                        }
                        if (string.Compare(RoleToCompare, RoleDestRole) == 0)
                        {
                            found = true;
                            foundRole = role;
                            break;
                        }
                    }
                    if (found == true)
                    {
                        return foundRole;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            public partial class Sla
            {
                /// <summary>
                /// Returns an SLA object with all of its properties filled.
                /// </summary>
                /// <param name="SLAid">
                ///The SLA number that you would like to get the details of. 
                ///Value Type: <see cref="Int64" />   (System.Int64)
                ///</param>
                /// <param name="ParaCredentials">
                /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
                /// </param>               
                public static ParaObjects.Sla SLAGetDetails(Int64 SLAid, ParaCredentials ParaCredentials)
                {
                    ParaObjects.Sla Sla = new ParaObjects.Sla();
                    Sla = SlaFillDetails(SLAid, ParaCredentials);
                    return Sla;
                }

                /// <summary>
                /// Returns an sla object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="SlaXML">
                /// The Sla XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.Sla SLAGetDetails(XmlDocument SlaXML)
                {
                    ParaObjects.Sla sla = new ParaObjects.Sla();
                    sla = xmlToObjectParser.SlaParser.SlaFill(SlaXML);

                    return sla;
                }

                /// <summary>
                /// Get the list of SLAs from within your Parature license.
                /// </summary>
                public static ParaObjects.SlasList SLAsGetList(ParaCredentials ParaCredentials)
                {
                    return SlaFillList(ParaCredentials, new EntityQuery.SlaQuery());
                }

                /// <summary>
                /// Get the list of SLAs from within your Parature license.
                /// </summary>
                public static ParaObjects.SlasList SLAsGetList(ParaCredentials ParaCredentials, EntityQuery.SlaQuery Query)
                {
                    return SlaFillList(ParaCredentials, Query);
                }

                /// <summary>
                /// Returns an sla list object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="SlaListXML">
                /// The Sla List XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.SlasList SLAsGetList(XmlDocument SlaListXML)
                {
                    ParaObjects.SlasList slasList = new ParaObjects.SlasList();
                    slasList = xmlToObjectParser.SlaParser.SlasFillList(SlaListXML);

                    slasList.ApiCallResponse.xmlReceived = SlaListXML;

                    return slasList;
                }

                /// <summary>
                /// Fills a Sla list object.
                /// </summary>
                private static ParaObjects.SlasList SlaFillList(ParaCredentials ParaCredentials, EntityQuery.SlaQuery Query)
                {

                    ParaObjects.SlasList SlasList = new ParaObjects.SlasList();
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureEntity.Sla, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        SlasList = xmlToObjectParser.SlaParser.SlasFillList(ar.xmlReceived);
                    }
                    SlasList.ApiCallResponse = ar;

                    // Checking if the system needs to recursively call all of the data returned.
                    if (Query.RetrieveAllRecords)
                    {
                        bool continueCalling = true;
                        while (continueCalling)
                        {
                            ParaObjects.SlasList objectlist = new ParaObjects.SlasList();

                            if (SlasList.TotalItems > SlasList.Slas.Count)
                            {
                                // We still need to pull data

                                // Getting next page's data
                                Query.PageNumber = Query.PageNumber + 1;

                                ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureEntity.Sla, Query.BuildQueryArguments());

                                objectlist = xmlToObjectParser.SlaParser.SlasFillList(ar.xmlReceived);

                                if (objectlist.Slas.Count == 0)
                                {
                                    continueCalling = false;
                                }

                                SlasList.Slas.AddRange(objectlist.Slas);
                                SlasList.ResultsReturned = SlasList.Slas.Count;
                                SlasList.PageNumber = Query.PageNumber;
                            }
                            else
                            {
                                // That is it, pulled all the items.
                                continueCalling = false;
                                SlasList.ApiCallResponse = ar;
                            }
                        }
                    }


                    return SlasList;
                }
                static ParaObjects.Sla SlaFillDetails(Int64 Slaid, ParaCredentials ParaCredentials)
                {
                    ParaObjects.Sla Sla = new ParaObjects.Sla();
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, Paraenums.ParatureEntity.Sla, Slaid);
                    if (ar.HasException == false)
                    {
                        Sla = xmlToObjectParser.SlaParser.SlaFill(ar.xmlReceived);
                    }
                    else
                    {

                        Sla.SlaID = 0;
                    }

                    //Sla.ApiCallResponse = ar;
                    return Sla;
                }
                /// <summary>
                /// Returns an SLA object if it finds an Sla matching the name you provide to it. Will return null if nothing is found.
                /// </summary>
                /// <param name="SlaName">
                /// The Sla Name to look for.
                /// </param>
                /// <param name="ParaCredentials"></param>
                /// <param name="IgnoreCase">
                /// When looking for this SLA, please indicate whether the search should be case sensitive or not.
                /// </param>
                /// <returns></returns>
                static ParaObjects.Sla HelperSlaFindByName(string SlaName, ParaCredentials ParaCredentials, bool IgnoreCase)
                {
                    ParaObjects.Sla foundSla = new ParaObjects.Sla();
                    EntityQuery.SlaQuery sq = new EntityQuery.SlaQuery();
                    sq.PageSize = 1000;
                    ParaObjects.SlasList oSLA = new ParaObjects.SlasList();
                    oSLA = ApiHandler.Entities.Sla.SLAsGetList(ParaCredentials, sq);

                    string SlaToCompare = "";
                    string SlaDestSla = "";
                    if (IgnoreCase == true)
                    {
                        SlaToCompare = SlaName.ToLower();
                    }
                    bool found = false;
                    foreach (ParaObjects.Sla sla in oSLA.Slas)
                    {
                        if (IgnoreCase == true)
                        {
                            SlaDestSla = sla.Name.ToLower();
                        }
                        if (string.Compare(SlaToCompare, SlaDestSla) == 0)
                        {
                            found = true;
                            foundSla = sla;
                            break;
                        }
                    }
                    if (found == true)
                    {
                        return foundSla;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            public partial class Department
            {
                /// <summary>
                /// Returns a Department object with all of its properties filled.
                /// </summary>
                /// <param name="departmentid">
                ///The Department number that you would like to get the details of. 
                ///</param>
                /// <param name="ParaCredentials">
                /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
                /// </param>               
                public static ParaObjects.Department DepartmentGetDetails(Int64 departmentid, ParaCredentials ParaCredentials)
                {
                    ParaObjects.Department department = new ParaObjects.Department();
                    department = DepartmentFillDetails(departmentid, ParaCredentials);
                    return department;
                }

                /// <summary>
                /// Returns a department object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="DepartmentXML">
                /// The department XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.Department DepartmentGetDetails(XmlDocument DepartmentXML)
                {
                    ParaObjects.Department department = new ParaObjects.Department();
                    department = xmlToObjectParser.DepartmentParser.DepartmentFill(DepartmentXML);

                    return department;
                }

                /// <summary>
                /// Returns an Department list object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="DepartmentListXML">
                /// The Departments List XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.DepartmentsList DepartmentsGetList(XmlDocument DepartmentListXML)
                {
                    ParaObjects.DepartmentsList departmentslist = new ParaObjects.DepartmentsList();
                    departmentslist = xmlToObjectParser.DepartmentParser.DepartmentsFillList(DepartmentListXML);

                    departmentslist.ApiCallResponse.xmlReceived = DepartmentListXML;

                    return departmentslist;
                }

                /// <summary>
                /// Get the list of Departments from within your Parature license.
                /// </summary>
                public static ParaObjects.DepartmentsList DepartmentsGetList(ParaCredentials ParaCredentials, EntityQuery.DepartmentQuery Query)
                {
                    return DepartmentFillList(ParaCredentials, Query);
                }
                /// <summary>
                /// Fills a Departmentslist object.
                /// </summary>
                private static ParaObjects.DepartmentsList DepartmentFillList(ParaCredentials ParaCredentials, EntityQuery.DepartmentQuery Query)
                {

                    ParaObjects.DepartmentsList departmentsList = new ParaObjects.DepartmentsList();
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureEntity.Department, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        departmentsList = xmlToObjectParser.DepartmentParser.DepartmentsFillList(ar.xmlReceived);
                    }
                    departmentsList.ApiCallResponse = ar;
                    return departmentsList;
                }
                static ParaObjects.Department DepartmentFillDetails(Int64 Departmentid, ParaCredentials ParaCredentials)
                {
                    ParaObjects.Department department = new ParaObjects.Department();
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, Paraenums.ParatureEntity.Department, Departmentid);
                    if (ar.HasException == false)
                    {
                        department = xmlToObjectParser.DepartmentParser.DepartmentFill(ar.xmlReceived);
                    }
                    else
                    {
                        department.DepartmentID = 0;
                    }
                    department.ApiCallResponse = ar;

                    return department;
                }
            }

            public partial class CustomerStatus
            {
                /// <summary>
                /// Returns an Customer object with all of its properties filled.
                /// </summary>
                /// <param name="Customerid">
                ///The Customer number that you would like to get the details of. 
                ///Value Type: <see cref="Int64" />   (System.Int64)
                ///</param>
                /// <param name="ParaCredentials">
                /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
                /// </param>               
                public static ParaObjects.CustomerStatus CustomerStatusGetDetails(Int64 CustomerStatusId, ParaCredentials ParaCredentials)
                {
                    ParaObjects.CustomerStatus CustomerStatus = new ParaObjects.CustomerStatus();
                    CustomerStatus = CustomerStatusFillDetails(CustomerStatusId, ParaCredentials);
                    return CustomerStatus;
                }
                /// <summary>
                /// Get the list of Customers from within your Parature license.
                /// </summary>
                public static ParaObjects.CustomerStatusList CustomerStatusGetList(ParaCredentials ParaCredentials)
                {
                    return CustomerStatusFillList(ParaCredentials, new EntityQuery.CustomerStatusQuery());
                }

                /// <summary>
                /// Get the list of Customers from within your Parature license.
                /// </summary>
                public static ParaObjects.CustomerStatusList CustomerStatusGetList(ParaCredentials ParaCredentials, EntityQuery.CustomerStatusQuery Query)
                {
                    return CustomerStatusFillList(ParaCredentials, Query);
                }

                /// <summary>
                /// Returns an CustomerStatus object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="CustomerStatusXML">
                /// The CustomerStatus XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.CustomerStatus CustomerStatusGetDetails(XmlDocument CustomerStatusXML)
                {
                    ParaObjects.CustomerStatus CustomerStatus = new ParaObjects.CustomerStatus();
                    CustomerStatus = xmlToObjectParser.CustomerStatusParser.CustomerStatusFill(CustomerStatusXML);

                    CustomerStatus.ApiCallResponse.xmlReceived = CustomerStatusXML;
                    CustomerStatus.ApiCallResponse.Objectid = CustomerStatus.StatusID;

                    return CustomerStatus;
                }

                /// <summary>
                /// Returns an CustomerStatus list object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="CustomerStatusListXML">
                /// The CustomerStatus List XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.CustomerStatusList CustomerStatusGetList(XmlDocument CustomerStatusListXML)
                {
                    ParaObjects.CustomerStatusList CustomerStatussList = new ParaObjects.CustomerStatusList();
                    CustomerStatussList = xmlToObjectParser.CustomerStatusParser.CustomerStatusFillList(CustomerStatusListXML);

                    CustomerStatussList.ApiCallResponse.xmlReceived = CustomerStatusListXML;

                    return CustomerStatussList;
                }

                /// <summary>
                /// Fills a Sla list object.
                /// </summary>
                private static ParaObjects.CustomerStatusList CustomerStatusFillList(ParaCredentials ParaCredentials, EntityQuery.CustomerStatusQuery Query)
                {

                    ParaObjects.CustomerStatusList CustomerStatusList = new ParaObjects.CustomerStatusList();
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectSecondLevelGetList(ParaCredentials, Paraenums.ParatureModule.Customer, Paraenums.ParatureEntity.status, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        CustomerStatusList = xmlToObjectParser.CustomerStatusParser.CustomerStatusFillList(ar.xmlReceived);
                    }
                    CustomerStatusList.ApiCallResponse = ar;





                    return CustomerStatusList;
                }
                static ParaObjects.CustomerStatus CustomerStatusFillDetails(Int64 CustomerStatusId, ParaCredentials ParaCredentials)
                {
                    ParaObjects.CustomerStatus CustomerStatus = new ParaObjects.CustomerStatus();
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, Paraenums.ParatureEntity.CustomerStatus, CustomerStatusId);
                    if (ar.HasException == false)
                    {
                        CustomerStatus = xmlToObjectParser.CustomerStatusParser.CustomerStatusFill(ar.xmlReceived);
                    }
                    else
                    {

                        CustomerStatus.StatusID = 0;
                    }

                    return CustomerStatus;
                }
            }

            public partial class CsrStatus
            {
                /// <summary>
                /// Returns an Csr object with all of its properties filled.
                /// </summary>
                /// <param name="Csrid">
                ///The Csr number that you would like to get the details of. 
                ///Value Type: <see cref="Int64" />   (System.Int64)
                ///</param>
                /// <param name="ParaCredentials">
                /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
                /// </param>               
                public static ParaObjects.CsrStatus CsrStatusGetDetails(Int64 CsrStatusId, ParaCredentials ParaCredentials)
                {
                    ParaObjects.CsrStatus CsrStatus = new ParaObjects.CsrStatus();
                    CsrStatus = CsrStatusFillDetails(CsrStatusId, ParaCredentials);
                    return CsrStatus;
                }
                /// <summary>
                /// Get the list of Csrs from within your Parature license.
                /// </summary>
                public static ParaObjects.CsrStatusList CsrStatusGetList(ParaCredentials ParaCredentials)
                {
                    return CsrStatusFillList(ParaCredentials, new EntityQuery.CsrStatusQuery());
                }

                /// <summary>
                /// Get the list of Csrs from within your Parature license.
                /// </summary>
                public static ParaObjects.CsrStatusList CsrStatusGetList(ParaCredentials ParaCredentials, EntityQuery.CsrStatusQuery Query)
                {
                    return CsrStatusFillList(ParaCredentials, Query);
                }

                /// <summary>
                /// Returns an CsrStatus object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="CsrStatusXML">
                /// The CsrStatus XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.CsrStatus CsrStatusGetDetails(XmlDocument CsrStatusXML)
                {
                    ParaObjects.CsrStatus CsrStatus = new ParaObjects.CsrStatus();
                    CsrStatus = xmlToObjectParser.CsrStatusParser.CsrStatusFill(CsrStatusXML);

                    CsrStatus.ApiCallResponse.xmlReceived = CsrStatusXML;
                    CsrStatus.ApiCallResponse.Objectid = CsrStatus.StatusID;

                    return CsrStatus;
                }

                /// <summary>
                /// Returns an CsrStatus list object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="CsrStatusListXML">
                /// The CsrStatus List XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.CsrStatusList CsrStatusGetList(XmlDocument CsrStatusListXML)
                {
                    ParaObjects.CsrStatusList CsrStatussList = new ParaObjects.CsrStatusList();
                    CsrStatussList = xmlToObjectParser.CsrStatusParser.CsrStatusFillList(CsrStatusListXML);

                    CsrStatussList.ApiCallResponse.xmlReceived = CsrStatusListXML;

                    return CsrStatussList;
                }

                /// <summary>
                /// Fills a Sla list object.
                /// </summary>
                private static ParaObjects.CsrStatusList CsrStatusFillList(ParaCredentials ParaCredentials, EntityQuery.CsrStatusQuery Query)
                {

                    ParaObjects.CsrStatusList CsrStatusList = new ParaObjects.CsrStatusList();
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectSecondLevelGetList(ParaCredentials, Paraenums.ParatureModule.Csr, Paraenums.ParatureEntity.status, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        CsrStatusList = xmlToObjectParser.CsrStatusParser.CsrStatusFillList(ar.xmlReceived);
                    }
                    CsrStatusList.ApiCallResponse = ar;
                    return CsrStatusList;
                }
                static ParaObjects.CsrStatus CsrStatusFillDetails(Int64 CsrStatusId, ParaCredentials ParaCredentials)
                {
                    ParaObjects.CsrStatus CsrStatus = new ParaObjects.CsrStatus();
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, Paraenums.ParatureEntity.CsrStatus, CsrStatusId);
                    if (ar.HasException == false)
                    {
                        CsrStatus = xmlToObjectParser.CsrStatusParser.CsrStatusFill(ar.xmlReceived);
                    }
                    else
                    {

                        CsrStatus.StatusID = 0;
                    }

                    return CsrStatus;
                }
            }

            public partial class TicketStatus
            {
                /// <summary>
                /// Returns an Csr object with all of its properties filled.
                /// </summary>
                /// <param name="Csrid">
                ///The Csr number that you would like to get the details of. 
                ///Value Type: <see cref="Int64" />   (System.Int64)
                ///</param>
                /// <param name="ParaCredentials">
                /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
                /// </param>               
                public static ParaObjects.TicketStatus TicketStatusGetDetails(Int64 ticketStatusId, ParaCredentials ParaCredentials)
                {
                    ParaObjects.TicketStatus ticketStatus = new ParaObjects.TicketStatus();
                    ticketStatus = TicketStatusFillDetails(ticketStatusId, ParaCredentials);
                    return ticketStatus;
                }

                /// <summary>
                /// Get the list of Csrs from within your Parature license.
                /// </summary>
                public static ParaObjects.TicketStatusList TicketStatusGetList(ParaCredentials ParaCredentials)
                {
                    return TicketStatusFillList(ParaCredentials, new EntityQuery.TicketStatusQuery());
                }
                /// <summary>
                /// Get the list of Csrs from within your Parature license.
                /// </summary>
                public static ParaObjects.TicketStatusList TicketStatusGetList(ParaCredentials ParaCredentials, EntityQuery.TicketStatusQuery Query)
                {
                    return TicketStatusFillList(ParaCredentials, Query);
                }

                /// <summary>
                /// Returns an ticketStatus object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="TicketStatusXML">
                /// The TicketStatus XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.TicketStatus TicketStatusGetDetails(XmlDocument TicketStatusXML)
                {
                    ParaObjects.TicketStatus ticketStatus = new ParaObjects.TicketStatus();
                    ticketStatus = xmlToObjectParser.TicketStatusParser.TicketStatusFill(TicketStatusXML);

                    ticketStatus.ApiCallResponse.xmlReceived = TicketStatusXML;
                    ticketStatus.ApiCallResponse.Objectid = ticketStatus.StatusID;

                    return ticketStatus;
                }

                /// <summary>
                /// Returns an ticketStatus list object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="TicketStatusListXML">
                /// The TicketStatus List XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.TicketStatusList TicketStatusGetList(XmlDocument TicketStatusListXML)
                {
                    ParaObjects.TicketStatusList ticketStatussList = new ParaObjects.TicketStatusList();
                    ticketStatussList = xmlToObjectParser.TicketStatusParser.TicketStatusFillList(TicketStatusListXML);

                    ticketStatussList.ApiCallResponse.xmlReceived = TicketStatusListXML;

                    return ticketStatussList;
                }

                /// <summary>
                /// Fills a Sla list object.
                /// </summary>
                private static ParaObjects.TicketStatusList TicketStatusFillList(ParaCredentials ParaCredentials, EntityQuery.TicketStatusQuery Query)
                {

                    ParaObjects.TicketStatusList TicketStatusList = new ParaObjects.TicketStatusList();
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectSecondLevelGetList(ParaCredentials, Paraenums.ParatureModule.Ticket, Paraenums.ParatureEntity.status, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        TicketStatusList = xmlToObjectParser.TicketStatusParser.TicketStatusFillList(ar.xmlReceived);
                    }
                    TicketStatusList.ApiCallResponse = ar;

                    // Checking if the system needs to recursively call all of the data returned.
                    if (Query.RetrieveAllRecords)
                    {
                        bool continueCalling = true;
                        while (continueCalling)
                        {
                            ParaObjects.TicketStatusList objectlist = new ParaObjects.TicketStatusList();

                            if (TicketStatusList.TotalItems > TicketStatusList.TicketStatuses.Count)
                            {
                                // We still need to pull data

                                // Getting next page's data
                                Query.PageNumber = Query.PageNumber + 1;

                                ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureEntity.TicketStatus, Query.BuildQueryArguments());

                                objectlist = xmlToObjectParser.TicketStatusParser.TicketStatusFillList(ar.xmlReceived);

                                if (objectlist.TicketStatuses.Count == 0)
                                {
                                    continueCalling = false;
                                }

                                TicketStatusList.TicketStatuses.AddRange(objectlist.TicketStatuses);
                                TicketStatusList.ResultsReturned = TicketStatusList.TicketStatuses.Count;
                                TicketStatusList.PageNumber = Query.PageNumber;
                            }
                            else
                            {
                                // That is it, pulled all the items.
                                continueCalling = false;
                                TicketStatusList.ApiCallResponse = ar;
                            }
                        }
                    }


                    return TicketStatusList;
                }
                static ParaObjects.TicketStatus TicketStatusFillDetails(Int64 ticketStatusId, ParaCredentials ParaCredentials)
                {
                    ParaObjects.TicketStatus TicketStatus = new ParaObjects.TicketStatus();
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, Paraenums.ParatureEntity.TicketStatus, ticketStatusId);
                    if (ar.HasException == false)
                    {
                        TicketStatus = xmlToObjectParser.TicketStatusParser.TicketStatusFill(ar.xmlReceived);
                    }
                    else
                    {

                        TicketStatus.StatusID = 0;
                    }

                    return TicketStatus;
                }
            }

            public partial class Queue
            {
                /// <summary>
                /// Returns a Queue object with all of its properties filled.
                /// </summary>
                /// <param name="Csrid">
                ///The Queue id that you would like to get the details of. 
                ///Value Type: <see cref="Int64" />   (System.Int64)
                ///</param>
                /// <param name="ParaCredentials">
                /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
                /// </param>               
                public static ParaObjects.Queue QueueGetDetails(Int64 queueId, ParaCredentials ParaCredentials)
                {
                    ParaObjects.Queue queue = new ParaObjects.Queue();
                    queue = QueueFillDetails(queueId, ParaCredentials);
                    return queue;
                }

                /// <summary>
                /// Returns an queue object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="QueueXML">
                /// The Queue XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.Queue QueueGetDetails(XmlDocument QueueXML)
                {
                    ParaObjects.Queue queue = new ParaObjects.Queue();
                    queue = xmlToObjectParser.QueueParser.QueueFill(QueueXML);

                    return queue;
                }

                /// <summary>
                /// Returns an queue list object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="QueueListXML">
                /// The Queue List XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.QueueList QueueGetList(XmlDocument QueueListXML)
                {
                    ParaObjects.QueueList queuesList = new ParaObjects.QueueList();
                    queuesList = xmlToObjectParser.QueueParser.QueueFillList(QueueListXML);

                    queuesList.ApiCallResponse.xmlReceived = QueueListXML;

                    return queuesList;
                }

                /// <summary>
                /// Get the list of Queues from within your Parature license.
                /// </summary>
                public static ParaObjects.QueueList QueueGetList(ParaCredentials ParaCredentials)
                {
                    return QueueFillList(ParaCredentials, new EntityQuery.QueueQuery());
                }

                /// <summary>
                /// Get the list of Queues from within your Parature license.
                /// </summary>
                public static ParaObjects.QueueList QueueGetList(ParaCredentials ParaCredentials, EntityQuery.QueueQuery Query)
                {
                    return QueueFillList(ParaCredentials, Query);
                }
                /// <summary>
                /// Fills a Queue List object.
                /// </summary>
                private static ParaObjects.QueueList QueueFillList(ParaCredentials ParaCredentials, EntityQuery.QueueQuery Query)
                {

                    ParaObjects.QueueList QueueList = new ParaObjects.QueueList();
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureEntity.Queue, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        QueueList = xmlToObjectParser.QueueParser.QueueFillList(ar.xmlReceived);
                    }
                    QueueList.ApiCallResponse = ar;

                    // Checking if the system needs to recursively call all of the data returned.
                    if (Query.RetrieveAllRecords)
                    {
                        bool continueCalling = true;
                        while (continueCalling)
                        {
                            ParaObjects.QueueList objectlist = new ParaObjects.QueueList();

                            if (QueueList.TotalItems > QueueList.Queues.Count)
                            {
                                // We still need to pull data

                                // Getting next page's data
                                Query.PageNumber = Query.PageNumber + 1;

                                ar = ApiCallFactory.ObjectGetList(ParaCredentials, Paraenums.ParatureEntity.Queue, Query.BuildQueryArguments());

                                objectlist = xmlToObjectParser.QueueParser.QueueFillList(ar.xmlReceived);

                                if (objectlist.Queues.Count == 0)
                                {
                                    continueCalling = false;
                                }

                                QueueList.Queues.AddRange(objectlist.Queues);
                                QueueList.ResultsReturned = QueueList.Queues.Count;
                                QueueList.PageNumber = Query.PageNumber;
                            }
                            else
                            {
                                // That is it, pulled all the items.
                                continueCalling = false;
                                QueueList.ApiCallResponse = ar;
                            }
                        }
                    }

                    return QueueList;
                }
                static ParaObjects.Queue QueueFillDetails(Int64 queueId, ParaCredentials ParaCredentials)
                {
                    ParaObjects.Queue Queue = new ParaObjects.Queue();
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, Paraenums.ParatureEntity.Queue, queueId);
                    if (ar.HasException == false)
                    {
                        Queue = xmlToObjectParser.QueueParser.QueueFill(ar.xmlReceived);
                    }
                    else
                    {

                        Queue.QueueID = 0;
                    }

                    return Queue;
                }
            }

            public partial class ContactView
            {
                /// <summary>
                /// Returns an View list object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="ViewListXML">
                /// The View List XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.ContactViewList ViewGetList(XmlDocument ViewListXML)
                {
                    ParaObjects.ContactViewList ViewsList = new ParaObjects.ContactViewList();
                    ViewsList = xmlToObjectParser.CustomerViewParser.ViewFillList(ViewListXML);

                    ViewsList.ApiCallResponse.xmlReceived = ViewListXML;

                    return ViewsList;
                }

                /// <summary>
                /// Get the list of Views from within your Parature license.
                /// </summary>
                public static ParaObjects.ContactViewList ViewGetList(ParaCredentials ParaCredentials, EntityQuery.ViewQuery Query)
                {
                    return ViewFillList(ParaCredentials, Query);
                }

                /// <summary>
                /// Fills a View List object.
                /// </summary>
                private static ParaObjects.ContactViewList ViewFillList(ParaCredentials ParaCredentials, EntityQuery.ViewQuery Query)
                {

                    ParaObjects.ContactViewList ViewList = new ParaObjects.ContactViewList();
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectSecondLevelGetList(ParaCredentials, Paraenums.ParatureModule.Customer, Paraenums.ParatureEntity.view, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        ViewList = xmlToObjectParser.CustomerViewParser.ViewFillList(ar.xmlReceived);
                    }
                    ViewList.ApiCallResponse = ar;
                    return ViewList;
                }
            }

            public partial class AccountView
            {
                /// <summary>
                /// Returns an View list object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="ViewListXML">
                /// The View List XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.AccountViewList ViewGetList(XmlDocument ViewListXML)
                {
                    ParaObjects.AccountViewList ViewsList = new ParaObjects.AccountViewList();
                    ViewsList = xmlToObjectParser.AccountViewParser.ViewFillList(ViewListXML);

                    ViewsList.ApiCallResponse.xmlReceived = ViewListXML;

                    return ViewsList;
                }

                /// <summary>
                /// Get the list of Views from within your Parature license.
                /// </summary>
                public static ParaObjects.AccountViewList ViewGetList(ParaCredentials ParaCredentials, EntityQuery.ViewQuery Query)
                {
                    return ViewFillList(ParaCredentials, Query);
                }

                /// <summary>
                /// Fills a View List object.
                /// </summary>
                private static ParaObjects.AccountViewList ViewFillList(ParaCredentials ParaCredentials, EntityQuery.ViewQuery Query)
                {

                    ParaObjects.AccountViewList ViewList = new ParaObjects.AccountViewList();
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectSecondLevelGetList(ParaCredentials, Paraenums.ParatureModule.Account, Paraenums.ParatureEntity.view, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        ViewList = xmlToObjectParser.AccountViewParser.ViewFillList(ar.xmlReceived);
                    }
                    ViewList.ApiCallResponse = ar;
                    return ViewList;
                }
            }

            public partial class TicketView
            {
                /// <summary>
                /// Returns an View list object from a XML Document. No calls to the APIs are made when calling this method.
                /// </summary>
                /// <param name="ViewListXML">
                /// The View List XML, is should follow the exact template of the XML returned by the Parature APIs.
                /// </param>
                public static ParaObjects.TicketViewList ViewGetList(XmlDocument ViewListXML)
                {
                    ParaObjects.TicketViewList ViewsList = new ParaObjects.TicketViewList();
                    ViewsList = xmlToObjectParser.TicketViewParser.ViewFillList(ViewListXML);

                    ViewsList.ApiCallResponse.xmlReceived = ViewListXML;

                    return ViewsList;
                }

                /// <summary>
                /// Get the list of Views from within your Parature license.
                /// </summary>
                public static ParaObjects.TicketViewList ViewGetList(ParaCredentials ParaCredentials, EntityQuery.ViewQuery Query)
                {
                    return ViewFillList(ParaCredentials, Query);
                }

                /// <summary>
                /// Fills a View List object.
                /// </summary>
                private static ParaObjects.TicketViewList ViewFillList(ParaCredentials ParaCredentials, EntityQuery.ViewQuery Query)
                {

                    ParaObjects.TicketViewList ViewList = new ParaObjects.TicketViewList();
                    ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
                    ar = ApiCallFactory.ObjectSecondLevelGetList(ParaCredentials, Paraenums.ParatureModule.Ticket, Paraenums.ParatureEntity.view, Query.BuildQueryArguments());
                    if (ar.HasException == false)
                    {
                        ViewList = xmlToObjectParser.TicketViewParser.ViewFillList(ar.xmlReceived);
                    }
                    ViewList.ApiCallResponse = ar;
                    return ViewList;
                }
            }

        }

        /// <summary>
        /// Internal Method to run an Action, independently from the module.
        /// </summary>
        private static ParaObjects.ApiCallResponse ActionRun(Int64 Objectid, ParaObjects.Action Action, ParaCredentials ParaCredentials, Paraenums.ParatureModule Module)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc = xmlgenerator.ActionGenerateXML(Action, Module);
            ParaObjects.ApiCallResponse ar = new ParaObjects.ApiCallResponse();
            ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, Module, doc, Objectid);
            return ar;
        }


        static ParaObjects.Attachment UploadFile(Paraenums.ParatureModule Module, ParaCredentials ParaCredentials, System.Net.Mail.Attachment Attachment)
        {
            ParaObjects.Attachment attachment = new ParaObjects.Attachment();
            string postUrl = "";
            System.Xml.XmlDocument UploadUrlDoc = new System.Xml.XmlDocument();
            ParaObjects.ApiCallResponse PostUrlR = new ParaObjects.ApiCallResponse();
            PostUrlR = ApiCallFactory.FileUploadGetUrl(ParaCredentials, Module);
            UploadUrlDoc = PostUrlR.xmlReceived;
            postUrl = xmlToObjectParser.AttachmentParser.AttachmentGetUrlToPost(UploadUrlDoc);

            System.Xml.XmlDocument AttaDoc = new System.Xml.XmlDocument();
            ParaObjects.ApiCallResponse upresp = new ParaObjects.ApiCallResponse();
            upresp = ApiCallFactory.FilePerformUpload(postUrl, Attachment, ParaCredentials.Instanceid, ParaCredentials);

            AttaDoc = upresp.xmlReceived;

            attachment = xmlToObjectParser.AttachmentParser.AttachmentFill(AttaDoc);
            return attachment;
        }

        /// <summary>
        /// Internal method to handle the upload of a file to Parature.
        /// </summary>
        static ParaObjects.Attachment UploadFile(Paraenums.ParatureModule Module, ParaCredentials ParaCredentials, string text, string contentType, string FileName)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] bytes = encoding.GetBytes(text);
            return UploadFile(Module, ParaCredentials, bytes, contentType, FileName);
        }

        /// <summary>
        /// Internal method to handle the upload of a file to Parature.
        /// </summary>
        public static ParaObjects.Attachment UploadFile(Paraenums.ParatureModule Module, ParaCredentials ParaCredentials, Byte[] Attachment, String contentType, String FileName)
        {
            ParaObjects.Attachment attachment = new ParaObjects.Attachment();
            string postUrl = "";
            postUrl = xmlToObjectParser.AttachmentParser.AttachmentGetUrlToPost(ApiCallFactory.FileUploadGetUrl(ParaCredentials, Module).xmlReceived);

            System.Xml.XmlDocument AttaDoc = new System.Xml.XmlDocument();
            if (string.IsNullOrEmpty(postUrl) == false)
            {
                AttaDoc = ApiCallFactory.FilePerformUpload(postUrl, Attachment, contentType, FileName, ParaCredentials.Instanceid, ParaCredentials).xmlReceived;
                attachment = xmlToObjectParser.AttachmentParser.AttachmentFill(AttaDoc);
            }
            else
            {
                attachment = new ParaObjects.Attachment();
            }
            return attachment;
        }

        /// <summary>
        /// Checks whether it is possible to connect to the Parature APIs or not. 
        /// </summary>
        /// <param name="credentials">
        /// The credentials class you need to access the APIs.
        /// </param>
        /// <returns>
        /// Will return "true" if it was able to successfully connect to the Parature APIs, False if not. 
        ///</returns>
        public static bool TestApiConnection(ParaCredentials credentials)
        {
            ParaObjects.CsrsList CSRs = new ParaObjects.CsrsList();
            ModuleQuery.CsrQuery csrq = new ModuleQuery.CsrQuery();
            csrq.PageSize = 1;
            CSRs = ApiHandler.Csr.CsrsGetList(credentials, csrq);
            if (CSRs.Csrs.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static ParaObjects.ApiCallResponse TestApiCallResponse(ParaCredentials credentials)
        {
            ParaObjects.CsrsList CSRs = new ParaObjects.CsrsList();
            ModuleQuery.CsrQuery csrq = new ModuleQuery.CsrQuery();
            csrq.PageSize = 1;
            CSRs = ApiHandler.Csr.CsrsGetList(credentials, csrq);
            return CSRs.ApiCallResponse;
        }

        /// <summary>
        /// Returns the http response code, as returned by the Parature API servers. 
        /// </summary>
        /// <param name="credentials">
        /// The credentials class you need to access the APIs.
        /// </param>
        public static HttpStatusCode TestApiResponseCode(ParaCredentials credentials)
        {
            ParaObjects.CsrsList CSRs = new ParaObjects.CsrsList();
            ModuleQuery.CsrQuery csrq = new ModuleQuery.CsrQuery();
            csrq.PageSize = 1;
            CSRs = ApiHandler.Csr.CsrsGetList(credentials, csrq);

            return (HttpStatusCode)CSRs.ApiCallResponse.httpResponseCode;
        }
    }
}
