using System;
using System.Threading;
using System.Xml;
using ParatureAPI.PagedData;
using ParatureAPI.ParaObjects;
using ParatureAPI.XmlToObjectParser;

namespace ParatureAPI.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Account module.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Create a Parature Account. Requires an Object and a credentials object. Will return the Newly Created Accountid. Returns 0 if the account creation fails.
        /// </summary>
        public static ApiCallResponse AccountInsert(ParaObjects.Account account, ParaCredentials paraCredentials)
        {
            var ar = new ApiCallResponse();
            var doc = new XmlDocument();
            doc = XmlGenerator.AccountGenerateXml(account);
            ar = ApiCallFactory.ObjectCreateUpdate(paraCredentials, ParaEnums.ParatureModule.Account, doc, 0);
            account.Id = ar.Objectid;
            account.uniqueIdentifier = ar.Objectid;
            return ar;
        }

        /// <summary>
        /// Update a Parature Account. Requires an Object and a credentials object.  Will return the updated Accountid. Returns 0 if the account update operation fails.
        /// </summary>
        public static ApiCallResponse AccountUpdate(ParaObjects.Account account, ParaCredentials paraCredentials)
        {
            var ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectCreateUpdate(paraCredentials, ParaEnums.ParatureModule.Account, XmlGenerator.AccountGenerateXml(account), account.Id);
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
        public static ParaObjects.Account AccountGetDetails(Int64 accountid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
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
            account = AccountParser.AccountFill(AccountXML, 0, true, null);
            account.FullyLoaded = true;

            account.ApiCallResponse.xmlReceived = AccountXML;
            account.ApiCallResponse.Objectid = account.Id;

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
            account = AccountFillDetails(accountid, ParaCredentials, ParaEnums.RequestDepth.Standard, true);

            return account;

        }

        /// <summary>
        /// Provides you with the capability to list accounts, following criteria you would set
        /// by instantiating a ModuleQuery.AccountQuery object
        /// </summary>
        public static AccountsList AccountsGetList(ParaCredentials ParaCredentials, ModuleQuery.AccountQuery Query)
        {
            return AccountsFillList(ParaCredentials, Query, ParaEnums.RequestDepth.Standard);
        }

        /// <summary>
        /// Returns an accounts list object from an XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="AccountListXML">
        /// The Account List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static AccountsList AccountsGetList(XmlDocument AccountListXML)
        {
            AccountsList accountsList = new AccountsList();
            accountsList = AccountParser.AccountsFillList(AccountListXML, true, 0, null);

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
        public static AccountsList AccountsGetList(ParaCredentials ParaCredentials, ModuleQuery.AccountQuery Query, ParaEnums.RequestDepth RequestDepth)
        {
            return AccountsFillList(ParaCredentials, Query, RequestDepth);
        }

        /// <summary>
        /// Will return the first 25 accounts returned by the APIs.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>  
        public static AccountsList AccountsGetList(ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
        {
            return AccountsFillList(ParaCredentials, null, RequestDepth);
        }
        /// <summary>
        /// Will return the first 25 accounts returned by the APIs.
        /// </summary>            
        public static AccountsList AccountsGetList(ParaCredentials ParaCredentials)
        {
            return AccountsFillList(ParaCredentials, null, ParaEnums.RequestDepth.Standard);
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
        public static ApiCallResponse AccountDelete(Int64 Accountid, ParaCredentials ParaCredentials, bool purge)
        {
            return ApiCallFactory.ObjectDelete(ParaCredentials, ParaEnums.ParatureModule.Account, Accountid, purge);
        }


        /// <summary>
        /// Fills an account list object.
        /// </summary>
        private static AccountsList AccountsFillList(ParaCredentials ParaCredentials, ModuleQuery.AccountQuery Query, ParaEnums.RequestDepth RequestDepth)
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
                objschem = AccountSchema(ParaCredentials);
                Query.IncludeCustomField(objschem.CustomFields);
            }
            ApiCallResponse ar = new ApiCallResponse();
            AccountsList AccountsList = new AccountsList();

            if (Query.RetrieveAllRecords && Query.OptimizePageSize)
            {
                OptimizationResult rslt = ApiUtils.OptimizeObjectPageSize(AccountsList, Query, ParaCredentials, requestdepth, ParaEnums.ParatureModule.Account);
                ar = rslt.apiResponse;
                Query = (ModuleQuery.AccountQuery)rslt.Query;
                AccountsList = ((AccountsList)rslt.objectList);
                rslt = null;
            }
            else
            {
                ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Account, Query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    AccountsList = AccountParser.AccountsFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);
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
                        instance = new ThreadPool.ObjectList(ParaCredentials, ParaEnums.ParatureModule.Account, Query.BuildQueryArguments(), requestdepth);
                        t = new System.Threading.Thread(delegate() { instance.Go(AccountsList); });
                        t.Start();
                    }

                    while (AccountsList.TotalItems > AccountsList.Accounts.Count)
                    {
                        Thread.Sleep(500);
                    }

                    AccountsList.ResultsReturned = AccountsList.Accounts.Count;
                    AccountsList.PageNumber = callsRequired;
                }
                else
                {
                    bool continueCalling = true;
                    while (continueCalling)
                    {
                        AccountsList objectlist = new AccountsList();

                        if (AccountsList.TotalItems > AccountsList.Accounts.Count)
                        {
                            // We still need to pull data

                            // Getting next page's data
                            Query.PageNumber = Query.PageNumber + 1;

                            ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Account, Query.BuildQueryArguments());
                            if (ar.HasException==false)
                            {
                                objectlist = AccountParser.AccountsFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);
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

        private static ParaObjects.Account AccountFillDetails(Int64 accountid, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth, bool MinimalisticLoad)
        {
            int requestdepth = (int)RequestDepth;
            ParaObjects.Account account = new ParaObjects.Account();
            //account = null;
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, ParaEnums.ParatureModule.Account, accountid);
            if (ar.HasException == false)
            {
                account = AccountParser.AccountFill(ar.xmlReceived, requestdepth, MinimalisticLoad, ParaCredentials);
                account.FullyLoaded = true;
            }
            else
            {
                account.FullyLoaded = false;
                account.Id = 0;
            }
            account.ApiCallResponse = ar;
            account.IsDirty = false;
            return account;
        }

        /// <summary>
        /// Gets an empty object with the scheam (custom fields, if any).
        /// </summary>            
        public static ParaObjects.Account AccountSchema(ParaCredentials ParaCredentials)
        {
            ParaObjects.Account account = new ParaObjects.Account();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetSchema(ParaCredentials, ParaEnums.ParatureModule.Account);

            if (ar.HasException == false)
            {
                account = AccountParser.AccountFill(ar.xmlReceived, 0, false, ParaCredentials);

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

            account = (ParaObjects.Account)ApiCallFactory.ObjectCheckCustomFieldTypes(ParaCredentials, ParaEnums.ParatureModule.Account, account);

            return account;
        }          
    }
}