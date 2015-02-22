using System;
using System.Xml;
using ParatureSDK.EntityQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler.Entities
{
    public class Queue
    {
        ///  <summary>
        ///  Returns a Queue object with all of its properties filled.
        ///  </summary>
        /// <param name="queueId"></param>
        /// <param name="paraCredentials">
        ///  The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        ///  </param>               
        public static ParaObjects.Queue QueueGetDetails(Int64 queueId, ParaCredentials paraCredentials)
        {
            ParaObjects.Queue queue = new ParaObjects.Queue();
            queue = QueueFillDetails(queueId, paraCredentials);
            return queue;
        }

        /// <summary>
        /// Returns an queue object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="queueXml">
        /// The Queue XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaObjects.Queue QueueGetDetails(XmlDocument queueXml)
        {
            ParaObjects.Queue queue = new ParaObjects.Queue();
            queue = ParaEntityParser.EntityFill<ParaObjects.Queue>(queueXml);

            return queue;
        }

        /// <summary>
        /// Returns an queue list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="queueListXml">
        /// The Queue List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<ParaObjects.Queue> QueueGetList(XmlDocument queueListXml)
        {
            var queuesList = new ParaEntityList<ParaObjects.Queue>();
            queuesList = ParaEntityParser.FillList<ParaObjects.Queue>(queueListXml);

            queuesList.ApiCallResponse.XmlReceived = queueListXml;

            return queuesList;
        }

        /// <summary>
        /// Get the list of Queues from within your Parature license.
        /// </summary>
        public static ParaEntityList<ParaObjects.Queue> QueueGetList(ParaCredentials paraCredentials)
        {
            return QueueFillList(paraCredentials, new QueueQuery());
        }

        /// <summary>
        /// Get the list of Queues from within your Parature license.
        /// </summary>
        public static ParaEntityList<ParaObjects.Queue> QueueGetList(ParaCredentials paraCredentials, QueueQuery query)
        {
            return QueueFillList(paraCredentials, query);
        }
        /// <summary>
        /// Fills a Queue List object.
        /// </summary>
        private static ParaEntityList<ParaObjects.Queue> QueueFillList(ParaCredentials paraCredentials, QueueQuery query)
        {

            var QueueList = new ParaEntityList<ParaObjects.Queue>();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetList(paraCredentials, ParaEnums.ParatureEntity.Queue, query.BuildQueryArguments());
            if (ar.HasException == false)
            {
                QueueList = ParaEntityParser.FillList<ParaObjects.Queue>(ar.XmlReceived);
            }
            QueueList.ApiCallResponse = ar;

            // Checking if the system needs to recursively call all of the data returned.
            if (query.RetrieveAllRecords)
            {
                bool continueCalling = true;
                while (continueCalling)
                {
                    var objectlist = new ParaEntityList<ParaObjects.Queue>();

                    if (QueueList.TotalItems > QueueList.Data.Count)
                    {
                        // We still need to pull data

                        // Getting next page's data
                        query.PageNumber = query.PageNumber + 1;

                        ar = ApiCallFactory.ObjectGetList(paraCredentials, ParaEnums.ParatureEntity.Queue, query.BuildQueryArguments());

                        objectlist = ParaEntityParser.FillList<ParaObjects.Queue>(ar.XmlReceived);

                        if (objectlist.Data.Count == 0)
                        {
                            continueCalling = false;
                        }

                        QueueList.Data.AddRange(objectlist.Data);
                        QueueList.ResultsReturned = QueueList.Data.Count;
                        QueueList.PageNumber = query.PageNumber;
                    }
                    else
                    {
                        // That is it, pulled all the items.
                        continueCalling = false;
                        QueueList.ApiCallResponse = ar;
                    }
                }
            }

            return QueueList;
        }

        private static ParaObjects.Queue QueueFillDetails(Int64 queueId, ParaCredentials paraCredentials)
        {
            ParaObjects.Queue Queue = new ParaObjects.Queue();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetDetail(paraCredentials, ParaEnums.ParatureEntity.Queue, queueId);
            if (ar.HasException == false)
            {
                Queue = ParaEntityParser.EntityFill<ParaObjects.Queue>(ar.XmlReceived);
            }
            else
            {

                Queue.QueueID = 0;
            }

            return Queue;
        }
    }
}