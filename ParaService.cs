using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ParatureSDK.ApiHandler;
using ParatureSDK.ParaObjects;
using ParatureSDK.Query.ModuleQuery;
using ParatureSDK.XmlToObjectParser;
using System.Collections;
using ParatureSDK.Query;
using ParatureSDK.Query.EntityQuery;

namespace ParatureSDK
{
    public class ParaService
    {
        public readonly ParaCredentials Credentials = null;

        public ParaService(ParaCredentials credentials)
        {
            if (credentials == null)
            {
                throw new ArgumentNullException("credentials");
            }

            Credentials = credentials;
        }

        /// <summary>
        /// Returns a view object with all of its properties filled.
        /// </summary>
        /// <param name="id">
        /// The view number that you would like to get the details of.
        /// Value Type: <see cref="long" />   (System.Int64)
        ///</param>
        /// <returns></returns>
        public TEntity Get<TEntity>(long id) where TEntity : ParaEntity, new()
        {
            return ApiUtils.ApiGetEntity<TEntity>(Credentials, id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entityId"></param>
        /// <param name="pc"></param>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public TEntity GetDetails<TEntity>(long entityId, ArrayList queryString) where TEntity : ParaEntity, new()
        {
            return ApiUtils.ApiGetEntity<TEntity>(Credentials, entityId, queryString);
        }

        /// <summary>
        /// Returns an view list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to return</typeparam>
        /// <param name="xml">The view List XML; it should follow the exact template of the XML returned by the Parature APIs.</param>
        /// <returns></returns>
        public ParaEntityList<TEntity> GetList<TEntity>(XmlDocument xml) where TEntity : ParaEntityBaseProperties, new()
        {
            var list = ParaEntityParser.FillList<TEntity>(xml);

            list.ApiCallResponse.XmlReceived = xml;

            return list;
        }

        public ParaEntityList<TEntity> GetList<TEntity>(ParaEntityQuery query)
            where TEntity : ParaEntity, new()
        {
            if (!(query.QueryTargetType is TEntity))
            {
                throw new ArgumentException("Inavlid query type for the requested entity result type", "query");
            }

            if (query.IncludeAllCustomFields)
            {
                var objschem = Create<TEntity>();
                query.IncludeCustomField(objschem.CustomFields);
            }

            return ApiUtils.ApiGetEntityList<TEntity>(Credentials, query);
        }

        public ParaEntityList<TFolder> GetList<TFolder>(FolderQuery query)
            where TFolder : Folder, new()
        {
            var folderList = new ParaEntityList<TFolder>();
            var ar = ApiCallFactory.ObjectGetList<TFolder>(Credentials, query.BuildQueryArguments());
            if (ar.HasException == false)
            {
                folderList = ParaEntityParser.FillList<TFolder>(ar.XmlReceived);
            }
            folderList.ApiCallResponse = ar;

            // Checking if the system needs to recursively call all of the data returned.
            if (query.RetrieveAllRecords)
            {
                bool continueCalling = true;
                while (continueCalling)
                {
                    if (folderList.TotalItems > folderList.Data.Count)
                    {
                        // We still need to pull data

                        // Getting next page's data
                        query.PageNumber = query.PageNumber + 1;

                        ar = ApiCallFactory.ObjectGetList<TFolder>(Credentials, query.BuildQueryArguments());

                        var objectlist = ParaEntityParser.FillList<TFolder>(ar.XmlReceived);

                        if (objectlist.Data.Count == 0)
                        {
                            continueCalling = false;
                        }

                        folderList.Data.AddRange(objectlist.Data);
                        folderList.ResultsReturned = folderList.Data.Count;
                        folderList.PageNumber = query.PageNumber;
                    }
                    else
                    {
                        // That is it, pulled all the items.
                        continueCalling = false;
                        folderList.ApiCallResponse = ar;
                    }
                }
            }

            return folderList;
        }

        /// <summary>
        /// Get the List of views from within your Parature license
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public ParaEntityList<TEntity> GetList<TModule,TEntity>(ParaEntityQuery query)
            where TEntity : ParaEntityBaseProperties, new()
            where TModule : ParaEntity
        {
            if (!(query.QueryTargetType is TEntity))
            {
                throw new ArgumentException("Inavlid query type for the requested entity result type", "query");
            }

            return ApiUtils.ApiGetEntityList<TModule, TEntity>(Credentials, query);
        }

        /// <summary>
        /// Create a new entity object. This object is not saved to the server until you call Insert with it.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public TEntity Create<TEntity>() where TEntity : ParaEntity, new()
        {
            var entity = new TEntity();
            var ar = ApiCallFactory.ObjectGetSchema<TEntity>(Credentials);

            if (ar.HasException == false)
            {
                var purgedSchema = ApiUtils.RemoveStaticFieldsNodes(ar.XmlReceived);
                entity = ParaEntityParser.EntityFill<TEntity>(purgedSchema);
            }

            entity.ApiCallResponse = ar;
            return entity;
        }

        /// <summary>
        /// Adds or updates the entity on the server.
        /// </summary>
        /// <param name="entity">The entity to save</param>
        /// <returns></returns>
        public ApiCallResponse Insert(IMutableEntity entity)
        {
            var pe = entity as ParaEntity;

            if (pe == null)
            {
                throw new ArgumentException("You can only call this function on a ParaEntity-derived object.", "entity");
            }

            return ApiCallFactory.ObjectCreateUpdate(Credentials, pe.GetType().Name, XmlGenerator.GenerateXml(pe), pe.Id);
        }

        /// <summary>
        /// Adds or updates the entity on the server.
        /// </summary>
        /// <param name="entity">The entity to save</param>
        /// <returns></returns>
        public ApiCallResponse Update(IMutableEntity entity)
        {
            var pe = entity as ParaEntity;

            if (pe == null)
            {
                throw new ArgumentException("You can only call this function on a ParaEntity-derived object.", "entity");
            }

            return ApiCallFactory.ObjectCreateUpdate(Credentials, pe.GetType().Name, XmlGenerator.GenerateXml(pe), pe.Id);
        }

        /// <summary>
        /// Deletes the entity from the server.
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        /// <returns></returns>
        public ApiCallResponse Delete(IMutableEntity entity)
        {
            var pe = entity as ParaEntity;

            if (pe == null)
            {
                throw new ArgumentException("You can only call this function on a ParaEntity-derived object.", "entity");
            }
            return ApiCallFactory.ObjectDelete(Credentials, pe.GetType().ToString(), pe.Id, false);
        }
    }
}
