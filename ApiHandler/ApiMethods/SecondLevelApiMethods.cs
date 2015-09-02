using System;
using System.Xml;
using ParatureSDK.ParaObjects;
using ParatureSDK.Query;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler.ApiMethods
{
    public abstract class SecondLevelApiMethods<TEntity, TQuery, TModule> 
        where TEntity : ParaEntityBaseProperties, new()
        where TQuery : ParaQuery, new()
        where TModule: ParaEntity
    {
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
        [Obsolete("To be removed in favor of ParaService.GetList in next major revision.")]
        public static TEntity GetDetails(Int64 id, ParaCredentials creds)
        {
            var entity = ApiUtils.ApiGetEntity<TEntity>(creds, id);
            return entity;
        }

        /// <summary>
        /// Returns an view object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="xml">
        /// The view XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        [Obsolete("To be removed in favor of ParaService.GetList in next major revision.")]
        public static TEntity GetDetails(XmlDocument xml)
        {
            var entity = ParaEntityParser.EntityFill<TEntity>(xml);

            return entity;
        }

        /// <summary>
        /// Returns an view list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="listXml">
        /// The view List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        [Obsolete("To be removed in favor of ParaService.GetList in next major revision.")]
        public static ParaEntityList<TEntity> GetList(XmlDocument listXml)
        {
            var list = ParaEntityParser.FillList<TEntity>(listXml);

            list.ApiCallResponse.XmlReceived = listXml;

            return list;
        }

        /// <summary>
        /// Get the List of views from within your Parature license
        /// </summary>
        /// <param name="creds"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        [Obsolete("To be removed in favor of ParaService.GetList in next major revision.")]
        public static ParaEntityList<TEntity> GetList(ParaCredentials creds, TQuery query)
        {
            return ApiUtils.ApiGetEntityList<TModule, TEntity>(creds, query);
        }

        [Obsolete("To be removed in favor of ParaService.GetList in next major revision.")]
        public static ParaEntityList<TEntity> GetList(ParaCredentials creds)
        {
            return ApiUtils.ApiGetEntityList<TModule, TEntity>(creds, new TQuery());
        }
    }
}
