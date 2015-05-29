using System;
using System.Collections;
using System.Threading;
using System.Xml;
using ParatureSDK.EntityQuery;
using ParatureSDK.ModuleQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;
using Action = ParatureSDK.ParaObjects.Action;

namespace ParatureSDK.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Ticket module.
    /// </summary>
    public static class Ticket
    {
        /// <summary>
        /// Provides the Schema of the Ticket module.
        /// </summary>
        public static ParaObjects.Ticket Schema(ParaCredentials pc)
        {
            var ticket = new ParaObjects.Ticket();
            var ar = ApiCallFactory.ObjectGetSchema(pc, ParaEnums.ParatureModule.Ticket);

            if (ar.HasException == false)
            {
                var purgedSchema = ApiUtils.RemoveStaticFieldsNodes(ar.XmlReceived);
                ticket = ParaEntityParser.EntityFill<ParaObjects.Ticket>(purgedSchema);
            }
            ticket.ApiCallResponse = ar;
            return ticket;
        }

        /// <summary>
        /// Gets an empty object with the scheam (custom fields, if any).  This call will also try to create a dummy
        /// record in order to determine if any of the custom fields have special validation rules (e.g. email, phone, url)
        /// and set the "dataType" of the custom field accordingly.
        /// </summary> 
        static public ParaObjects.Ticket SchemaWithCustomFieldTypes(ParaCredentials pc)
        {
            ParaObjects.Ticket Ticket = Schema(pc);

            Ticket = (ParaObjects.Ticket)ApiCallFactory.ObjectCheckCustomFieldTypes(pc, ParaEnums.ParatureModule.Ticket, Ticket);

            return Ticket;
        }

        /// <summary>
        /// Creates a Parature Ticket. Requires an Object and a credentials object. Will return the Newly Created Customerid. Returns 0 if the Customer creation fails.
        /// </summary>
        public static ApiCallResponse Insert(ParaObjects.Ticket ticket, ParaCredentials pc)
        {
            ApiCallResponse ar = new ApiCallResponse();
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc = XmlGenerator.GenerateXml(ticket);
            ar = ApiCallFactory.ObjectCreateUpdate(pc, ParaEnums.ParatureModule.Ticket, doc, 0);
            ticket.Id = ar.Id;
            return ar;
        }

        /// <summary>
        /// Updates a Parature Ticket. Requires an Object and a credentials object.  Will return the updated ticketId. Returns 0 if the Customer update operation failed.
        /// </summary>
        public static ApiCallResponse Update(ParaObjects.Ticket ticket, ParaCredentials pc)
        {
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectCreateUpdate(pc, ParaEnums.ParatureModule.Ticket, XmlGenerator.GenerateXml(ticket), ticket.Id);
            return ar;
        }

        /// <summary>
        /// Provides the capability to delete a Ticket.
        /// </summary>
        /// <param name="ticketId">
        /// The id of the Ticket to delete
        /// </param>
        /// <param name="purge">
        /// If purge is set to true, the ticket will be permanently deleted. Otherwise, it will just be 
        /// moved to the trash bin, so it will still be able to be restored from the service desk.
        ///</param>
        public static ApiCallResponse Delete(Int64 ticketId, ParaCredentials pc, bool purge)
        {
            return ApiCallFactory.ObjectDelete(pc, ParaEnums.ParatureModule.Ticket, ticketId, purge);
        }

        /// <summary>
        /// Returns a Ticket object with all the properties of an customer.
        /// </summary>
        /// <param name="ticketNumber">
        ///The Ticket number that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="includeHistory">
        /// Indicates whether or not to return the Ticket action history. Please keep in mind that the action history for certain tickets 
        /// can be very large, and therefore might slow down the operation.
        /// </param>
        /// <param name="pc">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>
        /// <param name="requestDepth">
        /// For a simple Ticket request, please put 0. <br/>When Requesting a Ticket, there might be related objects linked to that Ticket: such as Customer, AssignedToCsr, etc. <br/>With a regular Ticket detail call, generally only the ID and names of the extra objects are loaded. 
        /// <example>For example, the call will return a Ticket.Customer object, but only the Name and ID values will be filled. All of the other properties of the Customer object will be empty. If you select RequestDepth=1, then we will go one level deeper into our request and will therefore retrieve the Customer's detail for you. Customers might be part of an account, so if you select RequestDepth=2, we will go to an even deeper level and return the full account object with all of its properties, etc.<br/> Please make sure you only request the depth you need, as this might make your request slower. </example>
        /// </param>
        public static ParaObjects.Ticket GetDetails(Int64 ticketNumber, bool includeHistory, ParaCredentials pc, ParaEnums.RequestDepth requestDepth)
        {
            ParaObjects.Ticket Ticket = new ParaObjects.Ticket();
            Ticket = FillDetails(ticketNumber, pc, includeHistory);
            return Ticket;
        }

        /// <summary>
        /// Returns a Ticket object with all of its details.
        /// </summary>
        /// <param name="includeHistory">
        /// Indicates whether or not to return the Ticket action history. Please keep in mind that the action history for certain tickets 
        /// can be very large, and therefore might slow down the operation.
        /// </param> 
        /// <param name="ticketNumber">
        ///The Ticket number that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="pc">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>
        public static ParaObjects.Ticket GetDetails(Int64 ticketNumber, bool includeHistory, ParaCredentials pc)
        {

            ParaObjects.Ticket Ticket = new ParaObjects.Ticket();
            Ticket = FillDetails(ticketNumber, pc, includeHistory);
            return Ticket;
        }

        /// <summary>
        /// Returns an ticket object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="ticketXml">
        /// The Ticket XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaObjects.Ticket GetDetails(XmlDocument ticketXml)
        {
            ParaObjects.Ticket ticket = new ParaObjects.Ticket();
            ticket = ParaEntityParser.EntityFill<ParaObjects.Ticket>(ticketXml);
            ticket.FullyLoaded = true;

            ticket.ApiCallResponse.XmlReceived = ticketXml;
            ticket.ApiCallResponse.Id = ticket.Id;

            ticket.IsDirty = false;
            return ticket;
        }

        /// <summary>
        /// Returns an ticket list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="ticketListXml">
        /// The Ticket List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<ParaObjects.Ticket> GetList(XmlDocument ticketListXml)
        {
            var ticketsList = new ParaEntityList<ParaObjects.Ticket>();
            ticketsList = ParaEntityParser.FillList<ParaObjects.Ticket>(ticketListXml);

            ticketsList.ApiCallResponse.XmlReceived = ticketListXml;

            return ticketsList;
        }

        /// <summary>
        /// Provides you with the capability to list the 25 first Tickets returned by the APIs.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>
        public static ParaEntityList<ParaObjects.Ticket> GetList(ParaCredentials pc, ParaEnums.RequestDepth requestDepth)
        {
            return FillList(pc, null, requestDepth);
        }

        /// <summary>
        /// Provides you with the capability to list Tickets. This will only list the first 25 tickets returned by the Api.
        /// </summary>
        public static ParaEntityList<ParaObjects.Ticket> GetList(ParaCredentials pc)
        {
            return FillList(pc, null, ParaEnums.RequestDepth.Standard);
        }

        /// <summary>
        /// Provides you with the capability to list Tickets, following criteria you would set
        /// by instantiating a ModuleQuery.CustomerQuery object
        /// </summary>
        public static ParaEntityList<ParaObjects.Ticket> GetList(ParaCredentials pc, ParaEntityQuery query)
        {
            return FillList(pc, query, ParaEnums.RequestDepth.Standard);
        }

        /// <summary>
        /// Provides you with the capability to list Tickets, following criteria you would set
        /// by instantiating a ModuleQuery.TicketQuery object.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>
        public static ParaEntityList<ParaObjects.Ticket> GetList(ParaCredentials pc, ParaEntityQuery query, ParaEnums.RequestDepth requestDepth)
        {
            return FillList(pc, query, requestDepth);
        }

        /// <summary>
        /// Fills an Ticket list object.
        /// </summary>
        private static ParaEntityList<ParaObjects.Ticket> FillList(ParaCredentials pc, ParaEntityQuery query, ParaEnums.RequestDepth requestDepth)
        {
            var requestdepth = (int)requestDepth;
            if (query == null)
            {
                query = new TicketQuery();
            }

            // Making a schema call and returning all custom fields to be included in the call.
            if (query.IncludeAllCustomFields)
            {
                var objschem = Schema(pc);
                query.IncludeCustomField(objschem.CustomFields);
            }
            ApiCallResponse ar;
            var ticketsList = new ParaEntityList<ParaObjects.Ticket>();

            if (query.RetrieveAllRecords && query.OptimizePageSize)
            {
                var rslt = ApiUtils.OptimizeObjectPageSize(ticketsList, query, pc, ParaEnums.ParatureModule.Ticket);
                ar = rslt.apiResponse;
                query = (TicketQuery)rslt.Query;
                ticketsList = ((ParaEntityList<ParaObjects.Ticket>)rslt.objectList);
            }
            else
            {
                ar = ApiCallFactory.ObjectGetList(pc, ParaEnums.ParatureModule.Ticket, query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    ticketsList = ParaEntityParser.FillList<ParaObjects.Ticket>(ar.XmlReceived);
                }
                ticketsList.ApiCallResponse = ar;
            }

            // Checking if the system needs to recursively call all of the data returned.
            if (query.RetrieveAllRecords && !ar.HasException)
            {
                // A flag variable to check if we need to make more calls
                if (query.OptimizeCalls)
                {
                    var callsRequired = (int)Math.Ceiling((double)(ticketsList.TotalItems / (double)ticketsList.PageSize));
                    for (var i = 2; i <= callsRequired; i++)
                    {
                        query.PageNumber = i;
                        //implement semaphore right here (in the thread pool instance to control the generation of threads
                        var instance = new ThreadPool.ObjectList(pc, ParaEnums.ParatureModule.Ticket, query.BuildQueryArguments());
                        var t = new Thread(() => instance.Go(ticketsList));
                        t.Start();
                    }

                    while (ticketsList.TotalItems > ticketsList.Data.Count)
                    {
                        Thread.Sleep(500);
                    }

                    ticketsList.ResultsReturned = ticketsList.Data.Count;
                    ticketsList.PageNumber = callsRequired;
                }
                else
                {
                    var continueCalling = true;
                    while (continueCalling)
                    {
                        if (ticketsList.TotalItems > ticketsList.Data.Count)
                        {
                            // We still need to pull data

                            // Getting next page's data
                            query.PageNumber = query.PageNumber + 1;

                            ar = ApiCallFactory.ObjectGetList(pc, ParaEnums.ParatureModule.Ticket, query.BuildQueryArguments());
                            if (ar.HasException == false)
                            {
                                var objectlist = ParaEntityParser.FillList<ParaObjects.Ticket>(ar.XmlReceived);

                                ticketsList.Data.AddRange(objectlist.Data);
                                ticketsList.ResultsReturned = ticketsList.Data.Count;
                                ticketsList.PageNumber = query.PageNumber;
                            }
                            else
                            {
                                // There is an error processing request
                                ticketsList.ApiCallResponse = ar;
                                continueCalling = false;
                            }
                        }
                        else
                        {
                            // That is it, pulled all the items.
                            continueCalling = false;
                            ticketsList.ApiCallResponse = ar;
                        }
                    }
                }
            }

            return ticketsList;

        }

        internal static Attachment AddAttachment(ParaCredentials pc, Byte[] attachment, string contentType, string fileName)
        {
            return ApiUtils.UploadFile(ParaEnums.ParatureModule.Ticket, pc, attachment, contentType, fileName);
        }
        
        internal static Attachment AddAttachment(ParaCredentials pc, string text, string contentType, string fileName)
        {
            return ApiUtils.UploadFile(ParaEnums.ParatureModule.Ticket, pc, text, contentType, fileName);
        }

        private static ParaObjects.Ticket FillDetails(Int64 ticketNumber, ParaCredentials pc, bool includeHistory)
        {
            var ticket = new ParaObjects.Ticket();
            ApiCallResponse ar = new ApiCallResponse();

            if (includeHistory == true)
            {
                ArrayList arl = new ArrayList();
                arl.Add("_history_=true");
                ar = ApiCallFactory.ObjectGetDetail(pc, ParaEnums.ParatureModule.Ticket, ticketNumber, arl);
            }
            else
            {
                ar = ApiCallFactory.ObjectGetDetail<ParaObjects.Ticket>(pc, ParaEnums.ParatureModule.Ticket, ticketNumber);
            }


            if (ar.HasException == false)
            {
                ticket = ParaEntityParser.EntityFill<ParaObjects.Ticket>(ar.XmlReceived);
                ticket.FullyLoaded = true;
            }
            else
            {
                ticket.FullyLoaded = false;
                ticket.Id = 0;
            }
            ticket.ApiCallResponse = ar;
            ticket.IsDirty = false;
            return ticket;
        }

        /// <summary>
        /// Assigns a Ticket to a specific Queue.
        /// </summary>
        /// <param name="ticketId">
        /// The Ticket you would like to run this action on.
        /// </param>
        /// <param name="action">
        /// The action object your would like to run on this ticket.
        /// </param>
        /// <param name="pc">
        /// Your credentials object.
        /// </param>
        /// <returns></returns>
        public static ApiCallResponse ActionRun(Int64 ticketId, Action action, ParaCredentials pc)
        {
            var ar = ApiUtils.ActionRun(ticketId, action, pc, ParaEnums.ParatureModule.Ticket);
            return ar;
        }

        public static class Status
        {
            private static ParaEnums.ParatureEntity _entityType = ParaEnums.ParatureEntity.status;
            private static ParaEnums.ParatureModule _moduleType = ParaEnums.ParatureModule.Ticket;

            /// <summary>
            /// Returns a Status object with all of its properties filled.
            /// </summary>
            /// <param name="id">
            /// The Status number that you would like to get the details of.
            /// Value Type: <see cref="Int64" />   (System.Int64)
            ///</param>
            /// <param name="creds">
            /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            /// </param>
            /// <returns></returns>
            public static ParaObjects.Status GetDetails(Int64 id, ParaCredentials creds)
            {
                var status = ApiUtils.ApiGetEntity<ParaObjects.Status>(creds, _entityType, id);
                return status;
            }

            /// <summary>
            /// Returns an Status object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="xml">
            /// The Status XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.Status GetDetails(XmlDocument xml)
            {
                var status = ParaEntityParser.EntityFill<ParaObjects.Status>(xml);

                return status;
            }

            /// <summary>
            /// Returns an Status list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="listXml">
            /// The Status List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaEntityList<ParaObjects.Status> GetList(XmlDocument listXml)
            {
                var statusList = ParaEntityParser.FillList<ParaObjects.Status>(listXml);

                statusList.ApiCallResponse.XmlReceived = listXml;

                return statusList;
            }

            /// <summary>
            /// Get the List of Statuss from within your Parature license
            /// </summary>
            /// <param name="creds"></param>
            /// <param name="query"></param>
            /// <returns></returns>
            public static ParaEntityList<ParaObjects.Status> GetList(ParaCredentials creds, StatusQuery query)
            {
                return ApiUtils.ApiGetEntityList<ParaObjects.Status>(creds, query, _moduleType, _entityType);
            }

            public static ParaEntityList<ParaObjects.Status> GetList(ParaCredentials creds)
            {
                return ApiUtils.ApiGetEntityList<ParaObjects.Status>(creds, new StatusQuery(), _moduleType, _entityType);
            }
        }

        public static class View
        {
            private static ParaEnums.ParatureEntity _entityType = ParaEnums.ParatureEntity.view;
            private static ParaEnums.ParatureModule _moduleType = ParaEnums.ParatureModule.Ticket;

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
            public static ParaObjects.View GetDetails(Int64 id, ParaCredentials creds)
            {
                var entity = ApiUtils.ApiGetEntity<ParaObjects.View>(creds, _entityType, id);
                return entity;
            }

            /// <summary>
            /// Returns an view object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="xml">
            /// The view XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.View GetDetails(XmlDocument xml)
            {
                var entity = ParaEntityParser.EntityFill<ParaObjects.View>(xml);

                return entity;
            }

            /// <summary>
            /// Returns an view list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="listXml">
            /// The view List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaEntityList<ParaObjects.View> GetList(XmlDocument listXml)
            {
                var list = ParaEntityParser.FillList<ParaObjects.View>(listXml);

                list.ApiCallResponse.XmlReceived = listXml;

                return list;
            }

            /// <summary>
            /// Get the List of views from within your Parature license
            /// </summary>
            /// <param name="creds"></param>
            /// <param name="query"></param>
            /// <returns></returns>
            public static ParaEntityList<ParaObjects.View> GetList(ParaCredentials creds, ViewQuery query)
            {
                return ApiUtils.ApiGetEntityList<ParaObjects.View>(creds, query, _moduleType, _entityType);
            }

            public static ParaEntityList<ParaObjects.View> GetList(ParaCredentials creds)
            {
                return ApiUtils.ApiGetEntityList<ParaObjects.View>(creds, new ViewQuery(), _moduleType, _entityType);
            }
        }
    }
}