using System;
using System.Xml;
using ParatureSDK.EntityQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler.Entities
{
    public class Sla
    {
        /// <summary>
        /// Returns an SLA object with all of its properties filled.
        /// </summary>
        /// <param name="slaId">
        ///The SLA number that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="paraCredentials">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>               
        public static ParaObjects.Sla SLAGetDetails(Int64 slaId, ParaCredentials paraCredentials)
        {
            ParaObjects.Sla Sla = new ParaObjects.Sla();
            Sla = SlaFillDetails(slaId, paraCredentials);
            return Sla;
        }

        /// <summary>
        /// Returns an sla object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="slaXml">
        /// The Sla XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaObjects.Sla SLAGetDetails(XmlDocument slaXml)
        {
            ParaObjects.Sla sla = new ParaObjects.Sla();
            sla = SlaParser.SlaFill(slaXml);

            return sla;
        }

        /// <summary>
        /// Get the list of SLAs from within your Parature license.
        /// </summary>
        public static ParaEntityList<ParaObjects.Sla> SLAsGetList(ParaCredentials paraCredentials)
        {
            return SlaFillList(paraCredentials, new SlaQuery());
        }

        /// <summary>
        /// Get the list of SLAs from within your Parature license.
        /// </summary>
        public static ParaEntityList<ParaObjects.Sla> SLAsGetList(ParaCredentials paraCredentials, SlaQuery query)
        {
            return SlaFillList(paraCredentials, query);
        }

        /// <summary>
        /// Returns an sla list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="slaListXml">
        /// The Sla List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<ParaObjects.Sla> SLAsGetList(XmlDocument slaListXml)
        {
            var slasList = new ParaEntityList<ParaObjects.Sla>();
            slasList = SlaParser.SlasFillList(slaListXml);

            slasList.ApiCallResponse.xmlReceived = slaListXml;

            return slasList;
        }

        /// <summary>
        /// Fills a Sla list object.
        /// </summary>
        private static ParaEntityList<ParaObjects.Sla> SlaFillList(ParaCredentials paraCredentials, SlaQuery query)
        {

            var SlasList = new ParaEntityList<ParaObjects.Sla>();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetList(paraCredentials, ParaEnums.ParatureEntity.Sla, query.BuildQueryArguments());
            if (ar.HasException == false)
            {
                SlasList = SlaParser.SlasFillList(ar.xmlReceived);
            }
            SlasList.ApiCallResponse = ar;

            // Checking if the system needs to recursively call all of the data returned.
            if (query.RetrieveAllRecords)
            {
                bool continueCalling = true;
                while (continueCalling)
                {
                    var objectlist = new ParaEntityList<ParaObjects.Sla>();

                    if (SlasList.TotalItems > SlasList.Data.Count)
                    {
                        // We still need to pull data

                        // Getting next page's data
                        query.PageNumber = query.PageNumber + 1;

                        ar = ApiCallFactory.ObjectGetList(paraCredentials, ParaEnums.ParatureEntity.Sla, query.BuildQueryArguments());

                        objectlist = SlaParser.SlasFillList(ar.xmlReceived);

                        if (objectlist.Data.Count == 0)
                        {
                            continueCalling = false;
                        }

                        SlasList.Data.AddRange(objectlist.Data);
                        SlasList.ResultsReturned = SlasList.Data.Count;
                        SlasList.PageNumber = query.PageNumber;
                    }
                    else
                    {
                        // That is it, pulled all the items.
                        continueCalling = false;
                        SlasList.ApiCallResponse = ar;
                    }
                }
            }


            return SlasList;
        }

        private static ParaObjects.Sla SlaFillDetails(Int64 slaId, ParaCredentials paraCredentials)
        {
            ParaObjects.Sla Sla = new ParaObjects.Sla();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetDetail(paraCredentials, ParaEnums.ParatureEntity.Sla, slaId);
            if (ar.HasException == false)
            {
                Sla = SlaParser.SlaFill(ar.xmlReceived);
            }
            else
            {

                Sla.Id = 0;
            }

            //Sla.ApiCallResponse = ar;
            return Sla;
        }
    }
}