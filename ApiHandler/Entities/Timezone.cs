using System;
using System.Xml;
using ParatureSDK.EntityQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler.Entities
{
    public class Timezone
    {
        /// <summary>
        /// Returns a Timezone object with all of its properties filled.
        /// </summary>
        /// <param name="Csrid">
        ///The Timezone id that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="creds">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>               
        public static ParaObjects.Timezone GetDetails(Int64 id, ParaCredentials creds)
        {
            ParaObjects.Timezone Timezone = new ParaObjects.Timezone();
            Timezone = FillDetails(id, creds);
            return Timezone;
        }

        /// <summary>
        /// Returns an Timezone object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="xml">
        /// The Timezone XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaObjects.Timezone GetDetails(XmlDocument xml)
        {
            ParaObjects.Timezone Timezone = new ParaObjects.Timezone();
            Timezone = ParaEntityParser.EntityFill<ParaObjects.Timezone>(xml);

            return Timezone;
        }

        /// <summary>
        /// Returns an Timezone list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="listXml">
        /// The Timezone List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<ParaObjects.Timezone> GetList(XmlDocument listXml)
        {
            var TimezonesList = new ParaEntityList<ParaObjects.Timezone>();
            TimezonesList = ParaEntityParser.FillList<ParaObjects.Timezone>(listXml);

            TimezonesList.ApiCallResponse.XmlReceived = listXml;

            return TimezonesList;
        }
        /// <summary>
        /// Get the list of Timezones from within your Parature license.
        /// </summary>
        public static ParaEntityList<ParaObjects.Timezone> GetList(ParaCredentials creds)
        {
            return FillList(creds, new TimezoneQuery());
        }

        /// <summary>
        /// Get the list of Timezones from within your Parature license.
        /// </summary>
        public static ParaEntityList<ParaObjects.Timezone> GetList(ParaCredentials creds, TimezoneQuery query)
        {
            return FillList(creds, query);
        }
        /// <summary>
        /// Fills a Timezone List object.
        /// </summary>
        private static ParaEntityList<ParaObjects.Timezone> FillList(ParaCredentials creds, TimezoneQuery query)
        {

            var TimezoneList = new ParaEntityList<ParaObjects.Timezone>();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetList(creds, ParaEnums.ParatureEntity.Timezone, query.BuildQueryArguments());
            if (ar.HasException == false)
            {
                TimezoneList = ParaEntityParser.FillList<ParaObjects.Timezone>(ar.XmlReceived);
            }
            TimezoneList.ApiCallResponse = ar;
            return TimezoneList;
        }

        static ParaObjects.Timezone FillDetails(Int64 id, ParaCredentials creds)
        {
            ParaObjects.Timezone Timezone = new ParaObjects.Timezone();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetDetail(creds, ParaEnums.ParatureEntity.Timezone, id);
            if (ar.HasException == false)
            {
                Timezone = ParaEntityParser.EntityFill<ParaObjects.Timezone>(ar.XmlReceived);
            }
            else
            {

                Timezone.Id = 0;
            }

            return Timezone;
        }
    }
}