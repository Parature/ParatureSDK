using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ParatureSDK.EntityQuery;
using ParatureSDK.ModuleQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler
{
    public abstract class SecondLevelApiEntity<T> where T : ParaEntityBaseProperties, new()
    {
        public static ParaEnums.ParatureEntity _entityType;
        public static ParaEnums.ParatureModule _moduleType;

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
        public static T GetDetails(Int64 id, ParaCredentials creds)
        {
            var entity = ApiUtils.ApiGetEntity<T>(creds, _entityType, id);
            return entity;
        }

        /// <summary>
        /// Returns an view object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="xml">
        /// The view XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static T GetDetails(XmlDocument xml)
        {
            var entity = ParaEntityParser.EntityFill<T>(xml);

            return entity;
        }

        /// <summary>
        /// Returns an view list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="listXml">
        /// The view List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<T> GetList(XmlDocument listXml)
        {
            var list = ParaEntityParser.FillList<T>(listXml);

            list.ApiCallResponse.XmlReceived = listXml;

            return list;
        }

        /// <summary>
        /// Get the List of views from within your Parature license
        /// </summary>
        /// <param name="creds"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static ParaEntityList<T> GetList(ParaCredentials creds, ParaQuery query)
        {
            return ApiUtils.ApiGetEntityList<T>(creds, query, _moduleType, _entityType);
        }

        public static ParaEntityList<T> GetList(ParaCredentials creds)
        {
            return ApiUtils.ApiGetEntityList<T>(creds, new ViewQuery(), _moduleType, _entityType);
        }
    }
}
