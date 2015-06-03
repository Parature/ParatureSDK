using System;
using ParatureSDK.ModuleQuery;
using ParatureSDK.ParaObjects;

namespace ParatureSDK.ApiHandler.ApiMethods
{
    public abstract class FirstLevelApiMethods<TEntity, TQuery> : FirstLevelApiGetMethods<TEntity, TQuery> where TEntity : ParaEntity, new()
        where TQuery : ParaEntityQuery
    {
        /// <summary>
        /// Create a Parature Account. Requires an Object and a credentials object. Will return the Newly Created accountId. Returns 0 if the entity creation fails.
        /// </summary>
        public static ApiCallResponse Insert<TEntity>(TEntity entity, ParaCredentials paraCredentials) where TEntity : ParaEntity, new()
        {
            var doc = XmlGenerator.GenerateXml(entity);
            var ar = ApiCallFactory.ObjectCreateUpdate(paraCredentials, EnumTypeParser.Module<TEntity>(), doc, 0);
            entity.Id = ar.Id;
            return ar;
        }

        /// <summary>
        /// Update a Parature Account. Requires an Object and a credentials object.  Will return the updated accountId. Returns 0 if the entity update operation fails.
        /// </summary>
        public static ApiCallResponse Update<TEntity>(TEntity entity, ParaCredentials paraCredentials) where TEntity : ParaEntity, new()
        {
            var ar = ApiCallFactory.ObjectCreateUpdate(paraCredentials, EnumTypeParser.Module<TEntity>(),
                XmlGenerator.GenerateXml(entity),
                entity.Id);
            return ar;
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
            return ApiCallFactory.ObjectDelete(pc, EnumTypeParser.Module<TEntity>(), entityId, purge);
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
            return ApiCallFactory.ObjectDelete(pc, EnumTypeParser.Module<TEntity>(), entityId, false);
        }
    }
}
