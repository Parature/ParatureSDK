using System;
using System.Xml;
using ParatureSDK.EntityQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler.Entities
{
    public class TicketStatus
    {
        ///  <summary>
        ///  Returns an Csr object with all of its properties filled.
        ///  </summary>
        /// <param name="ticketStatusId"></param>
        /// <param name="paraCredentials">
        ///  The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        ///  </param>               
        public static ParaObjects.TicketStatus TicketStatusGetDetails(Int64 ticketStatusId, ParaCredentials paraCredentials)
        {
            ParaObjects.TicketStatus ticketStatus = new ParaObjects.TicketStatus();
            ticketStatus = TicketStatusFillDetails(ticketStatusId, paraCredentials);
            return ticketStatus;
        }

        /// <summary>
        /// Get the list of Csrs from within your Parature license.
        /// </summary>
        public static ParaEntityList<ParaObjects.TicketStatus> TicketStatusGetList(ParaCredentials paraCredentials)
        {
            return TicketStatusFillList(paraCredentials, new TicketStatusQuery());
        }
        /// <summary>
        /// Get the list of Csrs from within your Parature license.
        /// </summary>
        public static ParaEntityList<ParaObjects.TicketStatus> TicketStatusGetList(ParaCredentials paraCredentials, TicketStatusQuery query)
        {
            return TicketStatusFillList(paraCredentials, query);
        }

        /// <summary>
        /// Returns an ticketStatus object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="ticketStatusXml">
        /// The TicketStatus XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaObjects.TicketStatus TicketStatusGetDetails(XmlDocument ticketStatusXml)
        {
            ParaObjects.TicketStatus ticketStatus = new ParaObjects.TicketStatus();
            ticketStatus = TicketStatusParser.TicketStatusFill(ticketStatusXml);

            ticketStatus.ApiCallResponse.xmlReceived = ticketStatusXml;
            ticketStatus.ApiCallResponse.Objectid = ticketStatus.Id;

            return ticketStatus;
        }

        /// <summary>
        /// Returns an ticketStatus list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="ticketStatusListXml">
        /// The TicketStatus List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<ParaObjects.TicketStatus> TicketStatusGetList(XmlDocument ticketStatusListXml)
        {
            var ticketStatussList = new ParaEntityList<ParaObjects.TicketStatus>();
            ticketStatussList = TicketStatusParser.TicketStatusFillList(ticketStatusListXml);

            ticketStatussList.ApiCallResponse.xmlReceived = ticketStatusListXml;

            return ticketStatussList;
        }

        /// <summary>
        /// Fills a Sla list object.
        /// </summary>
        private static ParaEntityList<ParaObjects.TicketStatus> TicketStatusFillList(ParaCredentials paraCredentials, TicketStatusQuery query)
        {

            var TicketStatusList = new ParaEntityList<ParaObjects.TicketStatus>();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectSecondLevelGetList(paraCredentials, ParaEnums.ParatureModule.Ticket, ParaEnums.ParatureEntity.status, query.BuildQueryArguments());
            if (ar.HasException == false)
            {
                TicketStatusList = TicketStatusParser.TicketStatusFillList(ar.xmlReceived);
            }
            TicketStatusList.ApiCallResponse = ar;

            // Checking if the system needs to recursively call all of the data returned.
            if (query.RetrieveAllRecords)
            {
                bool continueCalling = true;
                while (continueCalling)
                {
                    var objectlist = new ParaEntityList<ParaObjects.TicketStatus>();

                    if (TicketStatusList.TotalItems > TicketStatusList.Data.Count)
                    {
                        // We still need to pull data

                        // Getting next page's data
                        query.PageNumber = query.PageNumber + 1;

                        ar = ApiCallFactory.ObjectGetList(paraCredentials, ParaEnums.ParatureEntity.TicketStatus, query.BuildQueryArguments());

                        objectlist = TicketStatusParser.TicketStatusFillList(ar.xmlReceived);

                        if (objectlist.Data.Count == 0)
                        {
                            continueCalling = false;
                        }

                        TicketStatusList.Data.AddRange(objectlist.Data);
                        TicketStatusList.ResultsReturned = TicketStatusList.Data.Count;
                        TicketStatusList.PageNumber = query.PageNumber;
                    }
                    else
                    {
                        // That is it, pulled all the items.
                        continueCalling = false;
                        TicketStatusList.ApiCallResponse = ar;
                    }
                }
            }


            return TicketStatusList;
        }

        private static ParaObjects.TicketStatus TicketStatusFillDetails(Int64 ticketStatusId, ParaCredentials paraCredentials)
        {
            ParaObjects.TicketStatus TicketStatus = new ParaObjects.TicketStatus();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetDetail(paraCredentials, ParaEnums.ParatureEntity.TicketStatus, ticketStatusId);
            if (ar.HasException == false)
            {
                TicketStatus = TicketStatusParser.TicketStatusFill(ar.xmlReceived);
            }
            else
            {

                TicketStatus.Id = 0;
            }

            return TicketStatus;
        }
    }
}