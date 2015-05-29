using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ParatureSDK.ModuleQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler
{
    public abstract class FirstLevelApiHandler<T> where T: ParaEntity, new()
    {
        internal static ParaEnums.ParatureModule _module;

        /// <summary>
        /// Create a Parature Account. Requires an Object and a credentials object. Will return the Newly Created accountId. Returns 0 if the entity creation fails.
        /// </summary>
        public static ApiCallResponse Insert(T entity, ParaCredentials paraCredentials)
        {
            var doc = XmlGenerator.GenerateXml(entity);
            var ar = ApiCallFactory.ObjectCreateUpdate(paraCredentials, _module, doc, 0);
            entity.Id = ar.Id;
            return ar;
        }

        /// <summary>
        /// Update a Parature Account. Requires an Object and a credentials object.  Will return the updated accountId. Returns 0 if the entity update operation fails.
        /// </summary>
        public static ApiCallResponse Update(T entity, ParaCredentials paraCredentials)
        {
            var ar = ApiCallFactory.ObjectCreateUpdate(paraCredentials, _module, XmlGenerator.GenerateXml(entity), entity.Id);
            return ar;
        }

        /// <summary>
        /// Returns an entity object from an XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="xml">
        /// The entity XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static T GetDetails(XmlDocument xml)
        {
            var entity = ParaEntityParser.EntityFill<T>(xml);
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
        public static T GetDetails(Int64 entityId, ParaCredentials pc)
        {
            return GetDetails(entityId, pc, new ArrayList());
        }

        public static T GetDetails(Int64 entityId, ParaCredentials pc, ArrayList queryStringParams)
        {
            var entity = ApiUtils.ApiGetEntity<T>(pc, _module, entityId, queryStringParams);

            return entity;
        }

        /// <summary>
        /// Will return the first 25 accounts returned by the APIs.
        /// </summary>            
        public static ParaEntityList<T> GetList(ParaCredentials pc)
        {
            return ApiUtils.ApiGetEntityList<T>(pc, _module);
        }

        /// <summary>
        /// Provides you with the capability to list accounts, following criteria you would set
        /// by instantiating a ModuleQuery.AccountQuery object
        /// </summary>
        public static ParaEntityList<T> GetList(ParaCredentials pc, ParaEntityQuery query)
        {
            // Making a schema call and returning all custom fields to be included in the call.
            if (query.IncludeAllCustomFields)
            {
                var objschem = Schema(pc);
                query.IncludeCustomField(objschem.CustomFields);
            }

            return ApiUtils.ApiGetEntityList<T>(pc, _module, query);
        }

        /// <summary>
        /// Returns an accounts list object from an XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="listXml">
        /// The Account List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<T> GetList(XmlDocument listXml)
        {
            var accountsList = ParaEntityParser.FillList<T>(listXml);

            accountsList.ApiCallResponse.XmlReceived = listXml;

            return accountsList;
        }

        /// <summary>
        /// Gets an empty object with the scheam (custom fields, if any).
        /// </summary>            
        public static T Schema(ParaCredentials pc)
        {
            var entity = new T();
            var ar = ApiCallFactory.ObjectGetSchema(pc, _module);

            if (ar.HasException == false)
            {
                var purgedSchema = ApiUtils.RemoveStaticFieldsNodes(ar.XmlReceived);
                entity = ParaEntityParser.EntityFill<T>(purgedSchema);
            }

            entity.ApiCallResponse = ar;
            return entity;
        }

        /// <summary>
        /// Gets an empty object with the scheam (custom fields, if any).  This call will also try to create a dummy
        /// record in order to determine if any of the custom fields have special validation rules (e.g. email, phone, url)
        /// and set the "dataType" of the custom field accordingly.
        /// </summary> 
        static public T SchemaWithCustomFieldTypes(ParaCredentials pc)
        {
            var entity = Schema(pc);

            entity = (T)ApiCallFactory.ObjectCheckCustomFieldTypes(pc, _module, entity);

            return entity;
        }

        ///  <summary>
        ///  Provides the capability to delete an Account.
        ///  </summary>
        ///  <param name="entityId">
        ///  The id of the Account to delete
        ///  </param>
        /// <param name="pc"></param>
        /// <param name="purge">
        ///  If purge is set to true, the entity will be permanently deleted. Otherwise, it will just be 
        ///  moved to the trash bin, so it will still be able to be restored from the service desk.
        /// </param>
        public static ApiCallResponse Delete(Int64 entityId, ParaCredentials pc, bool purge)
        {
            return ApiCallFactory.ObjectDelete(pc, _module, entityId, purge);
        }

        ///  <summary>
        ///  Provides the capability to delete an Account.
        ///  </summary>
        ///  <param name="entityId">
        ///  The id of the Account to delete
        ///  </param>
        /// <param name="pc"></param>
        public static ApiCallResponse Delete(Int64 entityId, ParaCredentials pc)
        {
            return ApiCallFactory.ObjectDelete(pc, _module, entityId, false);
        }
    }
}
