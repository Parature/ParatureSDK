using System;
using System.Collections;
using System.Xml;
using ParatureSDK.ParaObjects;
using ParatureSDK.Query.ModuleQuery;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler.ApiMethods
{
    public abstract class FirstLevelApiGetMethods<TEntity, TQuery>
        where TEntity : ParaEntity, new()
        where TQuery : ParaEntityQuery
    {
        /// <summary>
        /// Returns an entity object from an XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="xml">
        /// The entity XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static TEntity GetDetails(XmlDocument xml)
        {
            var entity = ParaEntityParser.EntityFill<TEntity>(xml);
            entity.FullyLoaded = true;

            entity.ApiCallResponse.XmlReceived = xml;
            entity.ApiCallResponse.Id = entity.Id;

            entity.IsDirty = false;
            return entity;
        }

        /// <summary>
        /// Returns an object with all the properties of the entity.
        /// </summary>
        /// <param name="entityId">
        ///The entity number that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="pc">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>
        public static TEntity GetDetails(Int64 entityId, ParaCredentials pc)
        {
            return GetDetails(entityId, pc, new ArrayList());
        }

        public static TEntity GetDetails(Int64 entityId, ParaCredentials pc, ArrayList queryStringParams)
        {
            var entity = ApiUtils.ApiGetEntity<TEntity>(pc, entityId,
                queryStringParams);

            return entity;
        }

        /// <summary>
        /// Will return the first 25 accounts returned by the APIs.
        /// </summary>            
        public static ParaEntityList<TEntity> GetList(ParaCredentials pc)
        {
            return ApiUtils.ApiGetEntityList<TEntity>(pc);
        }

        /// <summary>
        /// Provides you with the capability to list accounts, following criteria you would set
        /// by instantiating a ModuleQuery.AccountQuery object
        /// </summary>
        [Obsolete("To be removed in favor of ParaService.GetList in next major revision.")]
        public static ParaEntityList<TEntity> GetList(ParaCredentials pc, TQuery query)
        {
            // Making a schema call and returning all custom fields to be included in the call.
            if (query.IncludeAllCustomFields)
            {
                var objschem = Schema(pc);
                query.IncludeCustomField(objschem.CustomFields);
            }

            return ApiUtils.ApiGetEntityList<TEntity>(pc, query);
        }

        /// <summary>
        /// Returns an accounts list object from an XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="listXml">
        /// The Account List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<TEntity> GetList(XmlDocument listXml)
        {
            var entityList = ParaEntityParser.FillList<TEntity>(listXml);

            entityList.ApiCallResponse.XmlReceived = listXml;

            return entityList;
        }

        /// <summary>
        /// Gets an empty object with the scheam (custom fields, if any).
        /// </summary>            
        public static TEntity Schema(ParaCredentials pc)
        {
            var entity = new TEntity();
            var ar = ApiCallFactory.ObjectGetSchema<TEntity>(pc);

            if (ar.HasException == false)
            {
                var purgedSchema = ApiUtils.RemoveStaticFieldsNodes(ar.XmlReceived);
                entity = ParaEntityParser.EntityFill<TEntity>(purgedSchema);
            }

            entity.ApiCallResponse = ar;
            return entity;
        }
    }
}