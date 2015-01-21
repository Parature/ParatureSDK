using System;
using System.Xml;
using ParatureAPI.ParaObjects;
using ParatureAPI.XmlToObjectParser;

namespace ParatureAPI.ApiHandler.Entities
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
            queue = QueueParser.QueueFill(queueXml);

            return queue;
        }

        /// <summary>
        /// Returns an queue list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="queueListXml">
        /// The Queue List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static QueueList QueueGetList(XmlDocument queueListXml)
        {
            QueueList queuesList = new QueueList();
            queuesList = QueueParser.QueueFillList(queueListXml);

            queuesList.ApiCallResponse.xmlReceived = queueListXml;

            return queuesList;
        }

        /// <summary>
        /// Get the list of Queues from within your Parature license.
        /// </summary>
        public static QueueList QueueGetList(ParaCredentials paraCredentials)
        {
            return QueueFillList(paraCredentials, new EntityQuery.QueueQuery());
        }

        /// <summary>
        /// Get the list of Queues from within your Parature license.
        /// </summary>
        public static QueueList QueueGetList(ParaCredentials paraCredentials, EntityQuery.QueueQuery query)
        {
            return QueueFillList(paraCredentials, query);
        }
        /// <summary>
        /// Fills a Queue List object.
        /// </summary>
        private static QueueList QueueFillList(ParaCredentials paraCredentials, EntityQuery.QueueQuery query)
        {

            QueueList QueueList = new QueueList();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetList(paraCredentials, ParaEnums.ParatureEntity.Queue, query.BuildQueryArguments());
            if (ar.HasException == false)
            {
                QueueList = QueueParser.QueueFillList(ar.xmlReceived);
            }
            QueueList.ApiCallResponse = ar;

            // Checking if the system needs to recursively call all of the data returned.
            if (query.RetrieveAllRecords)
            {
                bool continueCalling = true;
                while (continueCalling)
                {
                    QueueList objectlist = new QueueList();

                    if (QueueList.TotalItems > QueueList.Queues.Count)
                    {
                        // We still need to pull data

                        // Getting next page's data
                        query.PageNumber = query.PageNumber + 1;

                        ar = ApiCallFactory.ObjectGetList(paraCredentials, ParaEnums.ParatureEntity.Queue, query.BuildQueryArguments());

                        objectlist = QueueParser.QueueFillList(ar.xmlReceived);

                        if (objectlist.Queues.Count == 0)
                        {
                            continueCalling = false;
                        }

                        QueueList.Queues.AddRange(objectlist.Queues);
                        QueueList.ResultsReturned = QueueList.Queues.Count;
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
                Queue = QueueParser.QueueFill(ar.xmlReceived);
            }
            else
            {

                Queue.QueueID = 0;
            }

            return Queue;
        }
    }
}