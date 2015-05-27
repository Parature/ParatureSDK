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
    /// Contains all the methods that allow you to interact with the Parature Account module.
    /// </summary>
    public static class Account
    {
        /// <summary>
        /// Create a Parature Account. Requires an Object and a credentials object. Will return the Newly Created accountId. Returns 0 if the account creation fails.
        /// </summary>
        public static ApiCallResponse Insert(ParaObjects.Account account, ParaCredentials paraCredentials)
        {
            var doc = XmlGenerator.GenerateXml(account);
            var ar = ApiCallFactory.ObjectCreateUpdate(paraCredentials, ParaEnums.ParatureModule.Account, doc, 0);
            account.Id = ar.Id;
            return ar;
        }

        /// <summary>
        /// Update a Parature Account. Requires an Object and a credentials object.  Will return the updated accountId. Returns 0 if the account update operation fails.
        /// </summary>
        public static ApiCallResponse Update(ParaObjects.Account account, ParaCredentials paraCredentials)
        {
            var ar = ApiCallFactory.ObjectCreateUpdate(paraCredentials, ParaEnums.ParatureModule.Account, XmlGenerator.GenerateXml(account), account.Id);
            return ar;
        }

        /// <summary>
        /// Returns an account object with all the properties of an account.
        /// </summary>
        /// <param name="accountid">
        ///The account number that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="pc">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>
        /// <param name="requestDepth">
        /// For a simple account request, please put 0. <br/>When Requesting an account, there might be related objects linked to that Account: such as products, viewable accounts, etc. <br/>With a regular Account detail call, generally only the ID and names of the extra objects are loaded. 
        /// <example>For example, the call will return an Account.Product object, but only the Name and ID values will be filled. All of the other properties of the product object will be empty. If you select RequestDepth=1, then we will go one level deeper into our request and will therefore retrieve the product's detail for you. Products might have assets linked to them, so if you select RequestDepth=2, we will go to an even deeped level and return all the assets properties for that product, etc.<br/> Please make sure you only request the depth you need, as this might make your request slower. </example>
        /// </param>
        public static ParaObjects.Account GetDetails(Int64 accountid, ParaCredentials pc, ParaEnums.RequestDepth requestDepth)
        {
            var account = FillDetails(accountid, pc, requestDepth, true);
            return account;
        }

        /// <summary>
        /// Returns an account object from an XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="accountXml">
        /// The Account XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaObjects.Account GetDetails(XmlDocument accountXml)
        {
            var account = ParaEntityParser.EntityFill<ParaObjects.Account>(accountXml);
            account.FullyLoaded = true;

            account.ApiCallResponse.XmlReceived = accountXml;
            account.ApiCallResponse.Id = account.Id;

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
        /// <param name="pc">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>
        public static ParaObjects.Account GetDetails(Int64 accountid, ParaCredentials pc)
        {
            var account = FillDetails(accountid, pc, ParaEnums.RequestDepth.Standard, true);

            return account;

        }

        /// <summary>
        /// Provides you with the capability to list accounts, following criteria you would set
        /// by instantiating a ModuleQuery.AccountQuery object
        /// </summary>
        public static ParaEntityList<ParaObjects.Account> GetList(ParaCredentials pc, ParaEntityQuery query)
        {
            return FillList(pc, query, ParaEnums.RequestDepth.Standard);
        }

        /// <summary>
        /// Returns an accounts list object from an XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="accountListXml">
        /// The Account List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<ParaObjects.Account> GetList(XmlDocument accountListXml)
        {
            var accountsList = ParaEntityParser.FillList<ParaObjects.Account>(accountListXml);

            accountsList.ApiCallResponse.XmlReceived = accountListXml;

            return accountsList;
        }

        /// <summary>
        /// Provides you with the capability to list accounts, following criteria you would set
        /// by instantiating a ModuleQuery.AccountQuery object.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>
        public static ParaEntityList<ParaObjects.Account> GetList(ParaCredentials pc, ParaEntityQuery query, ParaEnums.RequestDepth requestDepth)
        {
            return FillList(pc, query, requestDepth);
        }

        /// <summary>
        /// Will return the first 25 accounts returned by the APIs.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>  
        public static ParaEntityList<ParaObjects.Account> GetList(ParaCredentials pc, ParaEnums.RequestDepth requestDepth)
        {
            return FillList(pc, null, requestDepth);
        }
        /// <summary>
        /// Will return the first 25 accounts returned by the APIs.
        /// </summary>            
        public static ParaEntityList<ParaObjects.Account> GetList(ParaCredentials pc)
        {
            return FillList(pc, null, ParaEnums.RequestDepth.Standard);
        }

        ///  <summary>
        ///  Provides the capability to delete an Account.
        ///  </summary>
        ///  <param name="accountId">
        ///  The id of the Account to delete
        ///  </param>
        /// <param name="pc"></param>
        /// <param name="purge">
        ///  If purge is set to true, the account will be permanently deleted. Otherwise, it will just be 
        ///  moved to the trash bin, so it will still be able to be restored from the service desk.
        /// </param>
        public static ApiCallResponse Delete(Int64 accountId, ParaCredentials pc, bool purge)
        {
            return ApiCallFactory.ObjectDelete(pc, ParaEnums.ParatureModule.Account, accountId, purge);
        }

        /// <summary>
        /// Fills an account list object.
        /// </summary>
        private static ParaEntityList<ParaObjects.Account> FillList(ParaCredentials pc, ParaEntityQuery query, ParaEnums.RequestDepth requestDepth)
        {
            var requestdepth = (int)requestDepth;
            if (query == null)
            {
                query = new AccountQuery();
            }

            // Making a schema call and returning all custom fields to be included in the call.
            if (query.IncludeAllCustomFields)
            {
                var objschem = Schema(pc);
                query.IncludeCustomField(objschem.CustomFields);
            }
            ApiCallResponse ar;
            var accountsList = new ParaEntityList<ParaObjects.Account>();

            if (query.RetrieveAllRecords && query.OptimizePageSize)
            {
                OptimizationResult rslt = ApiUtils.OptimizeObjectPageSize(accountsList, query, pc, requestdepth, ParaEnums.ParatureModule.Account);
                ar = rslt.apiResponse;
                query = (AccountQuery)rslt.Query;
                accountsList = ((ParaEntityList<ParaObjects.Account>)rslt.objectList);
            }
            else
            {
                ar = ApiCallFactory.ObjectGetList(pc, ParaEnums.ParatureModule.Account, query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    accountsList = ParaEntityParser.FillList<ParaObjects.Account>(ar.XmlReceived);
                }
                accountsList.ApiCallResponse = ar;
            }

            // Checking if the system needs to recursively call all of the data returned.
            if (query.RetrieveAllRecords && !ar.HasException)
            {
                // A flag variable to check if we need to make more calls
                if (query.OptimizeCalls)
                {
                    var callsRequired = (int)Math.Ceiling((double)(accountsList.TotalItems / (double)accountsList.PageSize));
                    for (var i = 2; i <= callsRequired; i++)
                    {
                        query.PageNumber = i;
                        //implement semaphore right here (in the thread pool instance to control the generation of threads
                        var instance = new ThreadPool.ObjectList(pc, ParaEnums.ParatureModule.Account, query.BuildQueryArguments());
                        var t = new Thread(() => instance.Go(accountsList));
                        t.Start();
                    }

                    while (accountsList.TotalItems > accountsList.Data.Count)
                    {
                        Thread.Sleep(500);
                    }

                    accountsList.ResultsReturned = accountsList.Data.Count;
                    accountsList.PageNumber = callsRequired;
                }
                else
                {
                    var continueCalling = true;
                    while (continueCalling)
                    {
                        if (accountsList.TotalItems > accountsList.Data.Count)
                        {
                            // We still need to pull data

                            // Getting next page's data
                            query.PageNumber = query.PageNumber + 1;

                            ar = ApiCallFactory.ObjectGetList(pc, ParaEnums.ParatureModule.Account, query.BuildQueryArguments());
                            if (ar.HasException==false)
                            {
                                var objectlist = ParaEntityParser.FillList<ParaObjects.Account>(ar.XmlReceived);
                                accountsList.Data.AddRange(objectlist.Data);
                                accountsList.ResultsReturned = accountsList.Data.Count;
                                accountsList.PageNumber = query.PageNumber;
                            }
                            else
                            {
                                // There is an error processing request
                                accountsList.ApiCallResponse = ar;
                                continueCalling = false;
                            }
                        }
                        else
                        {
                            // That is it, pulled all the items.
                            continueCalling = false;
                            accountsList.ApiCallResponse = ar;
                        }
                    }
                }
            }

            return accountsList;
        }

        private static ParaObjects.Account FillDetails(Int64 accountid, ParaCredentials pc, ParaEnums.RequestDepth requestDepth, bool minimalisticLoad)
        {
            var requestdepth = (int)requestDepth;
            var account = new ParaObjects.Account();
            var ar = ApiCallFactory.ObjectGetDetail<ParaObjects.Account>(pc, ParaEnums.ParatureModule.Account, accountid);
            if (ar.HasException == false)
            {
                account = ParaEntityParser.EntityFill<ParaObjects.Account>(ar.XmlReceived);
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
        public static ParaObjects.Account Schema(ParaCredentials pc)
        {
            var account = new ParaObjects.Account();
            var ar = ApiCallFactory.ObjectGetSchema(pc, ParaEnums.ParatureModule.Account);

            if (ar.HasException == false)
            {
                var purgedSchema = ApiUtils.RemoveStaticFieldsNodes(ar.XmlReceived);
                account = ParaEntityParser.EntityFill<ParaObjects.Account>(purgedSchema);
            }

            account.ApiCallResponse = ar;
            return account;
        }

        /// <summary>
        /// Gets an empty object with the scheam (custom fields, if any).  This call will also try to create a dummy
        /// record in order to determine if any of the custom fields have special validation rules (e.g. email, phone, url)
        /// and set the "dataType" of the custom field accordingly.
        /// </summary> 
        static public ParaObjects.Account SchemaWithCustomFieldTypes(ParaCredentials pc)
        {
            var account = Schema(pc);

            account = (ParaObjects.Account)ApiCallFactory.ObjectCheckCustomFieldTypes(pc, ParaEnums.ParatureModule.Account, account);

            return account;
        }

        public static class View 
        {
            private static ParaEnums.ParatureEntity _entityType = ParaEnums.ParatureEntity.view;
            private static ParaEnums.ParatureModule _moduleType = ParaEnums.ParatureModule.Account;

            /// <summary>
            /// Returns a view object with all of its properties filled.
            /// </summary>
            /// <param name="id">
            /// The view number that you would like to get the details of.
            /// Value Type: <see cref="long" />   (System.Int64)
            ///</param>
            /// <param name="creds">
            /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            /// </param>
            /// <returns></returns>
            public static ParaObjects.AccountView GetDetails(Int64 id, ParaCredentials creds)
            {
                var entity = ApiUtils.FillDetails<ParaObjects.AccountView>(id, creds, _entityType);
                return entity;
            }

            /// <summary>
            /// Returns an view object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="xml">
            /// The view XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.AccountView GetDetails(XmlDocument xml)
            {
                var entity = ParaEntityParser.EntityFill<ParaObjects.AccountView>(xml);

                return entity;
            }

            /// <summary>
            /// Returns an view list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="listXml">
            /// The view List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaEntityList<ParaObjects.AccountView> GetList(XmlDocument listXml)
            {
                var list = ParaEntityParser.FillList<ParaObjects.AccountView>(listXml);

                list.ApiCallResponse.XmlReceived = listXml;

                return list;
            }

            /// <summary>
            /// Get the List of views from within your Parature license
            /// </summary>
            /// <param name="creds"></param>
            /// <param name="query"></param>
            /// <returns></returns>
            public static ParaEntityList<ParaObjects.AccountView> GetList(ParaCredentials creds, ViewQuery query, ParaEnums.ParatureModule module)
            {
                return ApiUtils.FillList<ParaObjects.AccountView>(creds, query, _entityType, _moduleType);
            }

            public static ParaEntityList<ParaObjects.AccountView> GetList(ParaCredentials creds, ParaEnums.ParatureModule module)
            {
                return ApiUtils.FillList<ParaObjects.AccountView>(creds, new ViewQuery(), _entityType, _moduleType);
            }
        }
    }
}