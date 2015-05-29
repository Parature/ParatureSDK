using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using ParatureSDK.ModuleQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler
{
    internal class ApiRequest
    {
        /// <summary>
        /// Create a Parature Account. Requires an Object and a credentials object. Will return the Newly Created accountId. Returns 0 if the entity creation fails.
        /// </summary>
        public static ApiCallResponse Insert<T>(T entity, ParaCredentials paraCredentials) where T: ParaEntity
        {
            var doc = XmlGenerator.GenerateXml(entity);
            var ar = ApiCallFactory.ObjectCreateUpdate<T>(paraCredentials, doc, 0);
            entity.Id = ar.Id;
            return ar;
        }

        /// <summary>
        /// Update a Parature Account. Requires an Object and a credentials object.  Will return the updated accountId. Returns 0 if the entity update operation fails.
        /// </summary>
        public static ApiCallResponse Update<T>(T entity, ParaCredentials paraCredentials) where T: ParaEntity
        {
            var ar = ApiCallFactory.ObjectCreateUpdate(paraCredentials, ParaEnums.ParatureModule.Account, XmlGenerator.GenerateXml(entity), entity.Id);
            return ar;
        }

        /// <summary>
        /// Returns an entity object with all the properties of an entity.
        /// </summary>
        /// <param name="id">
        ///The entity number that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="pc">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>
        /// <param name="requestDepth">
        /// For a simple entity request, please put 0. <br/>When Requesting an entity, there might be related objects linked to that Account: such as products, viewable accounts, etc. <br/>With a regular Account detail call, generally only the ID and names of the extra objects are loaded. 
        /// <example>For example, the call will return an Account.Product object, but only the Name and ID values will be filled. All of the other properties of the product object will be empty. If you select RequestDepth=1, then we will go one level deeper into our request and will therefore retrieve the product's detail for you. Products might have assets linked to them, so if you select RequestDepth=2, we will go to an even deeped level and return all the assets properties for that product, etc.<br/> Please make sure you only request the depth you need, as this might make your request slower. </example>
        /// </param>
        public static T GetDetails<T>(Int64 id, ParaCredentials pc, ParaEnums.RequestDepth requestDepth) where T: ParaEntity
        {
            var entity = FillDetails<T>(id, pc);
            return entity;
        }

        /// <summary>
        /// Returns an entity object from an XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="accountXml">
        /// The Account XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static T GetDetails<T>(XmlDocument accountXml) where T: ParaEntity
        {
            var account = ParaEntityParser.EntityFill<T>(accountXml);
            account.FullyLoaded = true;

            account.ApiCallResponse.XmlReceived = accountXml;
            account.ApiCallResponse.Id = account.Id;

            account.IsDirty = false;
            return account;
        }

        /// <summary>
        /// Returns an entity object with all the properties of an entity.
        /// </summary>
        /// <param name="id">
        ///The entity number that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="pc">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>
        public static T GetDetails<T>(Int64 id, ParaCredentials pc) where T: ParaEntity
        {
            var entity = FillDetails<T>(id, pc);

            return entity;
        }

        /// <summary>
        /// Provides you with the capability to list accounts, following criteria you would set
        /// by instantiating a ModuleQuery.AccountQuery object
        /// </summary>
        public static ParaEntityList<T> GetList<T>(ParaCredentials pc, ParaEntityQuery query) where T: ParaEntity, new()
        {
            return FillList<T>(pc, query, ParaEnums.RequestDepth.Standard);
        }

        /// <summary>
        /// Returns an accounts list object from an XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="xml">
        /// The Account List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<T> GetList<T>(XmlDocument xml)
        {
            var entityList = ParaEntityParser.FillList<T>(xml);

            entityList.ApiCallResponse.XmlReceived = xml;

            return entityList;
        }

        /// <summary>
        /// Provides you with the capability to list accounts, following criteria you would set
        /// by instantiating a ModuleQuery.AccountQuery object.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>
        public static ParaEntityList<T> GetList<T>(ParaCredentials pc, ParaEntityQuery query, ParaEnums.RequestDepth requestDepth) where T: ParaEntity, new()
        {
            return FillList<T>(pc, query, requestDepth);
        }

        /// <summary>
        /// Will return the first 25 accounts returned by the APIs.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>  
        public static ParaEntityList<T> GetList<T>(ParaCredentials pc, ParaEnums.RequestDepth requestDepth) where T: ParaEntity, new()
        {
            return FillList<T>(pc, null, requestDepth);
        }
        /// <summary>
        /// Will return the first 25 accounts returned by the APIs.
        /// </summary>            
        public static ParaEntityList<T> GetList<T>(ParaCredentials pc) where T: ParaEntity, new()
        {
            return FillList<T>(pc, null, ParaEnums.RequestDepth.Standard);
        }

        ///  <summary>
        ///  Provides the capability to delete an Account.
        ///  </summary>
        ///  <param name="accountId">
        ///  The id of the Account to delete
        ///  </param>
        /// <param name="pc"></param>
        /// <param name="purge">
        ///  If purge is set to true, the entity will be permanently deleted. Otherwise, it will just be 
        ///  moved to the trash bin, so it will still be able to be restored from the service desk.
        /// </param>
        public static ApiCallResponse Delete<T>(Int64 accountId, ParaCredentials pc, bool purge) where T: ParaEntity
        {
            return ApiCallFactory.ObjectDelete<T>(pc, accountId, purge);
        }

        /// <summary>
        /// Fills an entity list object.
        /// </summary>
        private static ParaEntityList<T> FillList<T>(ParaCredentials pc, ParaEntityQuery query, ParaEnums.RequestDepth requestDepth) where T: ParaEntity, new()
        {
            var requestdepth = (int)requestDepth;
            if (query == null)
            {
                query = new AccountQuery();
            }

            // Making a schema call and returning all custom fields to be included in the call.
            if (query.IncludeAllCustomFields)
            {
                var objschem = Schema<T>(pc);
                query.IncludeCustomField(objschem.CustomFields);
            }
            ApiCallResponse ar;
            var entityList = new ParaEntityList<T>();

            if (query.RetrieveAllRecords && query.OptimizePageSize)
            {
                var rslt = ApiUtils.OptimizeObjectPageSize(entityList, query, pc, ParaEnums.ParatureModule.Account);
                ar = rslt.apiResponse;
                query = (AccountQuery)rslt.Query;
                entityList = ((ParaEntityList<T>)rslt.objectList);
            }
            else
            {
                ar = ApiCallFactory.ObjectGetList<T>(pc, query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    entityList = ParaEntityParser.FillList<T>(ar.XmlReceived);
                }
                entityList.ApiCallResponse = ar;
            }

            // Checking if the system needs to recursively call all of the data returned.
            if (query.RetrieveAllRecords && !ar.HasException)
            {
                // A flag variable to check if we need to make more calls
                if (query.OptimizeCalls)
                {
                    var callsRequired = (int)Math.Ceiling((double)(entityList.TotalItems / (double)entityList.PageSize));
                    for (var i = 2; i <= callsRequired; i++)
                    {
                        query.PageNumber = i;
                        //implement semaphore right here (in the thread pool instance to control the generation of threads
                        var instance = new ThreadPool.ObjectList(pc, ParaEnums.ParatureModule.Account, query.BuildQueryArguments());
                        var t = new Thread(() => instance.Go<T>(entityList));
                        t.Start();
                    }

                    while (entityList.TotalItems > entityList.Data.Count)
                    {
                        Thread.Sleep(500);
                    }

                    entityList.ResultsReturned = entityList.Data.Count;
                    entityList.PageNumber = callsRequired;
                }
                else
                {
                    var continueCalling = true;
                    while (continueCalling)
                    {
                        if (entityList.TotalItems > entityList.Data.Count)
                        {
                            // We still need to pull data

                            // Getting next page's data
                            query.PageNumber = query.PageNumber + 1;

                            ar = ApiCallFactory.ObjectGetList(pc, ParaEnums.ParatureModule.Account, query.BuildQueryArguments());
                            if (ar.HasException == false)
                            {
                                var objectlist = ParaEntityParser.FillList<T>(ar.XmlReceived);
                                entityList.Data.AddRange(objectlist.Data);
                                entityList.ResultsReturned = entityList.Data.Count;
                                entityList.PageNumber = query.PageNumber;
                            }
                            else
                            {
                                // There is an error processing request
                                entityList.ApiCallResponse = ar;
                                continueCalling = false;
                            }
                        }
                        else
                        {
                            // That is it, pulled all the items.
                            continueCalling = false;
                            entityList.ApiCallResponse = ar;
                        }
                    }
                }
            }

            return entityList;
        }

        private static T FillDetails<T>(Int64 accountid, ParaCredentials pc) where T: ParaEntity
        {
            T entity = default(T);
            var ar = ApiCallFactory.ObjectGetDetail<T>(pc, ParaEnums.ParatureModule.Account, accountid);
            if (ar.HasException == false)
            {
                entity = ParaEntityParser.EntityFill<T>(ar.XmlReceived);
                entity.FullyLoaded = true;
            }
            else
            {
                entity.FullyLoaded = false;
                entity.Id = 0;
            }
            entity.ApiCallResponse = ar;
            entity.IsDirty = false;
            return entity;
        }

        /// <summary>
        /// Gets an empty object with the scheam (custom fields, if any).
        /// </summary>            
        public static T Schema<T>(ParaCredentials pc) where T: ParaEntity, new ()
        {
            var account = new T();
            var ar = ApiCallFactory.ObjectGetSchema<T>(pc, ParaEnums.ParatureModule.Account);

            if (ar.HasException == false)
            {
                account = ParaEntityParser.EntityFill<T>(ar.XmlReceived);
            }

            account.ApiCallResponse = ar;
            return account;
        }

        /// <summary>
        /// Gets an empty object with the scheam (custom fields, if any).  This call will also try to create a dummy
        /// record in order to determine if any of the custom fields have special validation rules (e.g. email, phone, url)
        /// and set the "dataType" of the custom field accordingly.
        /// </summary> 
        static public T SchemaWithCustomFieldTypes<T>(ParaCredentials pc) where T: ParaEntity, new()
        {
            var entity = Schema<T>(pc);

            entity = ApiCallFactory.ObjectCheckCustomFieldTypes<T>(pc, entity);

            return entity;
        }          
    }
}
