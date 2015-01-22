using System;
using System.Xml;
using ParatureAPI.ParaObjects;
using ParatureAPI.XmlToObjectParser;

namespace ParatureAPI.ApiHandler.Entities
{
    public partial class Status
    {
        /// <summary>
        /// Returns a Status object with all of its properties filled.
        /// </summary>
        /// <param name="Csrid">
        ///The Status id that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="ParaCredentials">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>               
        public static ParaObjects.Status StatusGetDetails(Int64 StatusId, ParaCredentials ParaCredentials)
        {
            ParaObjects.Status Status = new ParaObjects.Status();
            Status = StatusFillDetails(StatusId, ParaCredentials);
            return Status;
        }

        /// <summary>
        /// Returns an Status object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="StatusXML">
        /// The Status XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaObjects.Status StatusGetDetails(XmlDocument StatusXML)
        {
            ParaObjects.Status Status = new ParaObjects.Status();
            Status = StatusParser.StatusFill(StatusXML);

            return Status;
        }

        /// <summary>
        /// Returns an Status list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="StatusListXML">
        /// The Status List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<ParaObjects.Status> StatusGetList(XmlDocument StatusListXML)
        {
            var StatussList = new ParaEntityList<ParaObjects.Status>();
            StatussList = StatusParser.StatusFillList(StatusListXML);

            StatussList.ApiCallResponse.xmlReceived = StatusListXML;

            return StatussList;
        }

        /// <summary>
        /// Get the list of Statuss from within your Parature license.
        /// </summary>
        public static ParaEntityList<ParaObjects.Status> StatusGetList(ParaCredentials ParaCredentials)
        {
            return StatusFillList(ParaCredentials, new EntityQuery.StatusQuery());
        }

        /// <summary>
        /// Get the list of Statuss from within your Parature license.
        /// </summary>
        public static ParaEntityList<ParaObjects.Status> StatusGetList(ParaCredentials ParaCredentials, EntityQuery.StatusQuery Query)
        {
            return StatusFillList(ParaCredentials, Query);
        }
        /// <summary>
        /// Fills a Status List object.
        /// </summary>
        private static ParaEntityList<ParaObjects.Status> StatusFillList(ParaCredentials ParaCredentials, EntityQuery.StatusQuery Query)
        {

            var StatusList = new ParaEntityList<ParaObjects.Status>();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureEntity.status, Query.BuildQueryArguments());
            if (ar.HasException == false)
            {
                StatusList = StatusParser.StatusFillList(ar.xmlReceived);
            }
            StatusList.ApiCallResponse = ar;


            // Checking if the system needs to recursively call all of the data returned.
            if (Query.RetrieveAllRecords)
            {
                bool continueCalling = true;
                while (continueCalling)
                {
                    var objectlist = new ParaEntityList<ParaObjects.Status>();

                    if (StatusList.TotalItems > StatusList.Data.Count)
                    {
                        // We still need to pull data

                        // Getting next page's data
                        Query.PageNumber = Query.PageNumber + 1;

                        ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureEntity.status, Query.BuildQueryArguments());

                        objectlist = StatusParser.StatusFillList(ar.xmlReceived);

                        if (objectlist.Data.Count == 0)
                        {
                            continueCalling = false;
                        }

                        StatusList.Data.AddRange(objectlist.Data);
                        StatusList.ResultsReturned = StatusList.Data.Count;
                        StatusList.PageNumber = Query.PageNumber;
                    }
                    else
                    {
                        // That is it, pulled all the items.
                        continueCalling = false;
                        StatusList.ApiCallResponse = ar;
                    }
                }
            }

            return StatusList;
        }
        static ParaObjects.Status StatusFillDetails(Int64 StatusId, ParaCredentials ParaCredentials)
        {
            ParaObjects.Status Status = new ParaObjects.Status();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, ParaEnums.ParatureEntity.status, StatusId);
            if (ar.HasException == false)
            {
                Status = StatusParser.StatusFill(ar.xmlReceived);
            }
            else
            {

                Status.StatusID = 0;
            }

            return Status;
        }
    }
}