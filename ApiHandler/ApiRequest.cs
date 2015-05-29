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
        /// Fills an entity list object.
        /// </summary>
        private static ParaEntityList<T> FillList<T>(ParaCredentials pc, ParaEntityQuery query,
            ParaEnums.RequestDepth requestDepth) where T : ParaEntity, new()
        {
            var requestdepth = (int) requestDepth;
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
                query = (AccountQuery) rslt.Query;
                entityList = ((ParaEntityList<T>) rslt.objectList);
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
                    var callsRequired =
                        (int) Math.Ceiling((double) (entityList.TotalItems/(double) entityList.PageSize));
                    for (var i = 2; i <= callsRequired; i++)
                    {
                        query.PageNumber = i;
                        //implement semaphore right here (in the thread pool instance to control the generation of threads
                        var instance = new ThreadPool.ObjectList(pc, ParaEnums.ParatureModule.Account,
                            query.BuildQueryArguments());
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

                            ar = ApiCallFactory.ObjectGetList(pc, ParaEnums.ParatureModule.Account,
                                query.BuildQueryArguments());
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

        private static T FillDetails<T>(Int64 accountid, ParaCredentials pc) where T : ParaEntity
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
        public static T Schema<T>(ParaCredentials pc) where T : ParaEntity, new()
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
        public static T SchemaWithCustomFieldTypes<T>(ParaCredentials pc) where T : ParaEntity, new()
        {
            var entity = Schema<T>(pc);

            entity = ApiCallFactory.ObjectCheckCustomFieldTypes<T>(pc, entity);

            return entity;
        }
    }
}