using System;
using System.Collections;
using System.Threading;
using System.Xml;
using ParatureAPI.PagedData;
using ParatureAPI.ParaObjects;
using Action = ParatureAPI.ParaObjects.Action;

namespace ParatureAPI.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Ticket module.
    /// </summary>
    public class Ticket
    {
        /// <summary>
        /// Provides the Schema of the Ticket module.
        /// </summary>
        public static ParaObjects.Ticket TicketSchema(ParaCredentials ParaCredentials)
        {
            ParaObjects.Ticket Ticket = new ParaObjects.Ticket();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetSchema(ParaCredentials, ParaEnums.ParatureModule.Ticket);

            if (ar.HasException == false)
            {
                Ticket = XmlToObjectParser.TicketParser.TicketFill(ar.xmlReceived, 0, false, ParaCredentials);
            }
            Ticket.ApiCallResponse = ar;
            return Ticket;
        }

        /// <summary>
        /// Gets an empty object with the scheam (custom fields, if any).  This call will also try to create a dummy
        /// record in order to determine if any of the custom fields have special validation rules (e.g. email, phone, url)
        /// and set the "dataType" of the custom field accordingly.
        /// </summary> 
        static public ParaObjects.Ticket TicketSchemaWithCustomFieldTypes(ParaCredentials ParaCredentials)
        {
            ParaObjects.Ticket Ticket = TicketSchema(ParaCredentials);

            Ticket = (ParaObjects.Ticket)ApiCallFactory.ObjectCheckCustomFieldTypes(ParaCredentials, ParaEnums.ParatureModule.Ticket, Ticket);

            return Ticket;
        }

        /// <summary>
        /// Creates a Parature Ticket. Requires an Object and a credentials object. Will return the Newly Created Customerid. Returns 0 if the Customer creation fails.
        /// </summary>
        public static ApiCallResponse TicketInsert(ParaObjects.Ticket Ticket, ParaCredentials ParaCredentials)
        {
            ApiCallResponse ar = new ApiCallResponse();
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc = XmlGenerator.TicketGenerateXML(Ticket);
            ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, ParaEnums.ParatureModule.Ticket, doc, 0);
            Ticket.id = ar.Objectid;
            Ticket.uniqueIdentifier = ar.Objectid;
            return ar;
        }

        /// <summary>
        /// Updates a Parature Ticket. Requires an Object and a credentials object.  Will return the updated Ticketid. Returns 0 if the Customer update operation failed.
        /// </summary>
        public static ApiCallResponse TicketUpdate(ParaObjects.Ticket Ticket, ParaCredentials ParaCredentials)
        {
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, ParaEnums.ParatureModule.Ticket, XmlGenerator.TicketGenerateXML(Ticket), Ticket.id);
            return ar;
        }

        /// <summary>
        /// Provides the capability to delete a Ticket.
        /// </summary>
        /// <param name="Ticketid">
        /// The id of the Ticket to delete
        /// </param>
        /// <param name="purge">
        /// If purge is set to true, the ticket will be permanently deleted. Otherwise, it will just be 
        /// moved to the trash bin, so it will still be able to be restored from the service desk.
        ///</param>
        public static ApiCallResponse TicketDelete(Int64 Ticketid, ParaCredentials ParaCredentials, bool purge)
        {
            return ApiCallFactory.ObjectDelete(ParaCredentials, ParaEnums.ParatureModule.Ticket, Ticketid, purge);
        }

        /// <summary>
        /// Returns a Ticket object with all the properties of an customer.
        /// </summary>
        /// <param name="TicketNumber">
        ///The Ticket number that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="IncludeHistory">
        /// Indicates whether or not to return the Ticket action history. Please keep in mind that the action history for certain tickets 
        /// can be very large, and therefore might slow down the operation.
        /// </param>
        /// <param name="ParaCredentials">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>
        /// <param name="RequestDepth">
        /// For a simple Ticket request, please put 0. <br/>When Requesting a Ticket, there might be related objects linked to that Ticket: such as Customer, AssignedToCsr, etc. <br/>With a regular Ticket detail call, generally only the ID and names of the extra objects are loaded. 
        /// <example>For example, the call will return a Ticket.Customer object, but only the Name and ID values will be filled. All of the other properties of the Customer object will be empty. If you select RequestDepth=1, then we will go one level deeper into our request and will therefore retrieve the Customer's detail for you. Customers might be part of an account, so if you select RequestDepth=2, we will go to an even deeper level and return the full account object with all of its properties, etc.<br/> Please make sure you only request the depth you need, as this might make your request slower. </example>
        /// </param>
        public static ParaObjects.Ticket TicketGetDetails(Int64 TicketNumber, bool IncludeHistory, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
        {
            ParaObjects.Ticket Ticket = new ParaObjects.Ticket();
            Ticket = TicketFillDetails(TicketNumber, ParaCredentials, RequestDepth, true, IncludeHistory);
            return Ticket;
        }

        /// <summary>
        /// Returns a Ticket object with all of its details.
        /// </summary>
        /// <param name="IncludeHistory">
        /// Indicates whether or not to return the Ticket action history. Please keep in mind that the action history for certain tickets 
        /// can be very large, and therefore might slow down the operation.
        /// </param> 
        /// <param name="TicketNumber">
        ///The Ticket number that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="ParaCredentials">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>
        public static ParaObjects.Ticket TicketGetDetails(Int64 TicketNumber, bool IncludeHistory, ParaCredentials ParaCredentials)
        {

            ParaObjects.Ticket Ticket = new ParaObjects.Ticket();
            Ticket = TicketFillDetails(TicketNumber, ParaCredentials, ParaEnums.RequestDepth.Standard, true, IncludeHistory);
            return Ticket;
        }

        /// <summary>
        /// Returns an ticket object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="TicketXML">
        /// The Ticket XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaObjects.Ticket TicketGetDetails(XmlDocument TicketXML)
        {
            ParaObjects.Ticket ticket = new ParaObjects.Ticket();
            ticket = XmlToObjectParser.TicketParser.TicketFill(TicketXML, 0, true, null);
            ticket.FullyLoaded = true;

            ticket.ApiCallResponse.xmlReceived = TicketXML;
            ticket.ApiCallResponse.Objectid = ticket.id;

            ticket.IsDirty = false;
            return ticket;
        }

        /// <summary>
        /// Returns an ticket list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="TicketListXML">
        /// The Ticket List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static TicketsList TicketsGetList(XmlDocument TicketListXML)
        {
            TicketsList ticketsList = new TicketsList();
            ticketsList = XmlToObjectParser.TicketParser.TicketsFillList(TicketListXML, true, 0, null);

            ticketsList.ApiCallResponse.xmlReceived = TicketListXML;

            return ticketsList;
        }

        /// <summary>
        /// Provides you with the capability to list the 25 first Tickets returned by the APIs.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>
        public static TicketsList TicketsGetList(ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth)
        {
            return TicketsFillList(ParaCredentials, null, RequestDepth);
        }

        /// <summary>
        /// Provides you with the capability to list Tickets. This will only list the first 25 tickets returned by the Api.
        /// </summary>
        public static TicketsList TicketsGetList(ParaCredentials ParaCredentials)
        {
            return TicketsFillList(ParaCredentials, null, ParaEnums.RequestDepth.Standard);
        }

        /// <summary>
        /// Provides you with the capability to list Tickets, following criteria you would set
        /// by instantiating a ModuleQuery.CustomerQuery object
        /// </summary>
        public static TicketsList TicketsGetList(ParaCredentials ParaCredentials, ModuleQuery.TicketQuery Query)
        {
            return TicketsFillList(ParaCredentials, Query, ParaEnums.RequestDepth.Standard);
        }

        /// <summary>
        /// Provides you with the capability to list Tickets, following criteria you would set
        /// by instantiating a ModuleQuery.TicketQuery object.
        /// Provides with the capability to pull extra data, by setting the proper request depth. Please be aware that 
        /// this might considerably slow your request, due to the high volume of API calls needed, in case you require more than 
        /// the standard field depth.
        /// </summary>
        public static TicketsList TicketsGetList(ParaCredentials ParaCredentials, ModuleQuery.TicketQuery Query, ParaEnums.RequestDepth RequestDepth)
        {
            return TicketsFillList(ParaCredentials, Query, RequestDepth);
        }

        /// <summary>
        /// Fills an Ticket list object.
        /// </summary>
        private static TicketsList TicketsFillList(ParaCredentials ParaCredentials, ModuleQuery.TicketQuery Query, ParaEnums.RequestDepth RequestDepth)
        {
            int requestdepth = (int)RequestDepth;
            if (Query == null)
            {
                Query = new ModuleQuery.TicketQuery();
            }

            // Making a schema call and returning all custom fields to be included in the call.
            if (Query.IncludeAllCustomFields == true)
            {
                ParaObjects.Ticket objschem = new ParaObjects.Ticket();
                objschem = TicketSchema(ParaCredentials);
                Query.IncludeCustomField(objschem.CustomFields);
            }
            ApiCallResponse ar = new ApiCallResponse();
            TicketsList TicketsList = new TicketsList();

            if (Query.RetrieveAllRecords && Query.OptimizePageSize)
            {
                OptimizationResult rslt = ApiUtils.OptimizeObjectPageSize(TicketsList, Query, ParaCredentials, requestdepth, ParaEnums.ParatureModule.Ticket);
                ar = rslt.apiResponse;
                Query = (ModuleQuery.TicketQuery)rslt.Query;
                TicketsList = ((TicketsList)rslt.objectList);
                rslt = null;
            }
            else
            {
                ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Ticket, Query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    TicketsList = XmlToObjectParser.TicketParser.TicketsFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);
                }
                TicketsList.ApiCallResponse = ar;
            }

            // Checking if the system needs to recursively call all of the data returned.
            if (Query.RetrieveAllRecords && !ar.HasException)
            {
                // A flag variable to check if we need to make more calls
                if (Query.OptimizeCalls)
                {
                    System.Threading.Thread t;
                    ThreadPool.ObjectList instance = null;
                    int callsRequired = (int)Math.Ceiling((double)(TicketsList.TotalItems / (double)TicketsList.PageSize));
                    for (int i = 2; i <= callsRequired; i++)
                    {
                        //ApiCallFactory.waitCheck(ParaCredentials.Accountid);
                        Query.PageNumber = i;
                        //implement semaphore right here (in the thread pool instance to control the generation of threads
                        instance = new ThreadPool.ObjectList(ParaCredentials, ParaEnums.ParatureModule.Ticket, Query.BuildQueryArguments(), requestdepth);
                        t = new System.Threading.Thread(delegate() { instance.Go(TicketsList); });
                        t.Start();
                    }

                    while (TicketsList.TotalItems > TicketsList.Tickets.Count)
                    {
                        Thread.Sleep(500);
                    }

                    TicketsList.ResultsReturned = TicketsList.Tickets.Count;
                    TicketsList.PageNumber = callsRequired;
                }
                else
                {
                    bool continueCalling = true;
                    while (continueCalling)
                    {
                        TicketsList objectlist = new TicketsList();

                        if (TicketsList.TotalItems > TicketsList.Tickets.Count)
                        {
                            // We still need to pull data

                            // Getting next page's data
                            Query.PageNumber = Query.PageNumber + 1;

                            ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Ticket, Query.BuildQueryArguments());


                            if (ar.HasException == false)
                            {
                                objectlist = XmlToObjectParser.TicketParser.TicketsFillList(ar.xmlReceived, Query.MinimalisticLoad, requestdepth, ParaCredentials);

                                TicketsList.Tickets.AddRange(objectlist.Tickets);
                                TicketsList.ResultsReturned = TicketsList.Tickets.Count;
                                TicketsList.PageNumber = Query.PageNumber;
                            }
                            else
                            {
                                // There is an error processing request
                                TicketsList.ApiCallResponse = ar;
                                continueCalling = false;
                            }
                        }
                        else
                        {
                            // That is it, pulled all the items.
                            continueCalling = false;
                            TicketsList.ApiCallResponse = ar;
                        }
                    }
                }
            }

            return TicketsList;

        }

        internal static Attachment TicketAddAttachment(ParaCredentials ParaCredentials, Byte[] Attachment, string contentType, string FileName)
        {
            return ApiUtils.UploadFile(ParaEnums.ParatureModule.Ticket, ParaCredentials, Attachment, contentType, FileName);
        }
        internal static Attachment TicketAddAttachment(ParaCredentials ParaCredentials, string text, string contentType, string FileName)
        {
            return ApiUtils.UploadFile(ParaEnums.ParatureModule.Ticket, ParaCredentials, text, contentType, FileName);
        }

        static ParaObjects.Ticket TicketFillDetails(Int64 TicketNumber, ParaCredentials ParaCredentials, ParaEnums.RequestDepth RequestDepth, bool MinimalisticLoad, bool IncludeHistory)
        {
            int requestdepth = (int)RequestDepth;
            ParaObjects.Ticket Ticket = new ParaObjects.Ticket();
            //Ticket = null;
            ApiCallResponse ar = new ApiCallResponse();

            if (IncludeHistory == true)
            {
                ArrayList arl = new ArrayList();
                arl.Add("_history_=true");
                ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, ParaEnums.ParatureModule.Ticket, TicketNumber, arl);
            }
            else
            {
                ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, ParaEnums.ParatureModule.Ticket, TicketNumber);
            }


            if (ar.HasException == false)
            {
                Ticket = XmlToObjectParser.TicketParser.TicketFill(ar.xmlReceived, requestdepth, MinimalisticLoad, ParaCredentials);
                Ticket.FullyLoaded = true;
            }
            else
            {
                Ticket.FullyLoaded = false;
                Ticket.id = 0;
            }
            Ticket.ApiCallResponse = ar;
            Ticket.IsDirty = false;
            return Ticket;
        }

        /// <summary>
        /// Grabs a Ticket to the CSR that is making the call.
        /// </summary>
        /// <param name="Ticketid">
        /// The Ticket you would like to run this action on.
        /// </param>
        /// <param name="actionid">
        /// The id of action Grab in your license.
        /// </param>
        /// <param name="ParaCredentials">
        /// Your credentials object.
        /// </param>
        /// <returns></returns>
        public static ApiCallResponse ActionRunGrab(Int64 Ticketid, Int64 actionid, ParaCredentials ParaCredentials)
        {
            Action Action = new Action();
            Action.ActionID = actionid;
            Action.actionType = ParaEnums.ActionType.Grab;
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiUtils.ActionRun(Ticketid, Action, ParaCredentials, ParaEnums.ParatureModule.Ticket);
            return ar;
        }

        /// <summary>
        /// Assigns a Ticket to a specific CSR.
        /// </summary>
        /// <param name="Ticketid">
        /// The Ticket you would like to run this action on.
        /// </param>
        /// <param name="Action">
        /// The action object your would like to run on this ticket.
        /// </param>
        /// <param name="ParaCredentials">
        /// Your credentials object.
        /// </param>
        /// <param name="CsrID">
        /// The CSR you would like to assign this ticket to.
        /// </param>
        /// <returns></returns>
        public static ApiCallResponse ActionRunAssignToCsr(Int64 Ticketid, Action Action, Int64 CsrID, ParaCredentials ParaCredentials)
        {
            Action.actionType = ParaEnums.ActionType.Assign;
            Action.AssigntToCsrid = CsrID;
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiUtils.ActionRun(Ticketid, Action, ParaCredentials, ParaEnums.ParatureModule.Ticket);
            return ar;
        }

        /// <summary>
        /// Assigns a Ticket to a specific Queue.
        /// </summary>
        /// <param name="Ticketid">
        /// The Ticket you would like to run this action on.
        /// </param>
        /// <param name="Action">
        /// The action object your would like to run on this ticket.
        /// </param>
        /// <param name="ParaCredentials">
        /// Your credentials object.
        /// </param>
        /// <param name="QueueID">
        /// The Queue you would like to assign this ticket to.
        /// </param>
        /// <returns></returns>
        public static ApiCallResponse ActionRunAssignToQueue(Int64 Ticketid, Action Action, Int64 QueueID, ParaCredentials ParaCredentials)
        {
            Action.actionType = ParaEnums.ActionType.Assign_Queue;
            Action.AssignToQueueid = QueueID;
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiUtils.ActionRun(Ticketid, Action, ParaCredentials, ParaEnums.ParatureModule.Ticket);
            return ar;
        }

        /// <summary>
        /// Assigns a Ticket to a specific Queue.
        /// </summary>
        /// <param name="Ticketid">
        /// The Ticket you would like to run this action on.
        /// </param>
        /// <param name="Action">
        /// The action object your would like to run on this ticket.
        /// </param>
        /// <param name="ParaCredentials">
        /// Your credentials object.
        /// </param>
        /// <returns></returns>
        public static ApiCallResponse ActionRun(Int64 Ticketid, Action Action, ParaCredentials ParaCredentials)
        {
            Action.actionType = ParaEnums.ActionType.Other;
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiUtils.ActionRun(Ticketid, Action, ParaCredentials, ParaEnums.ParatureModule.Ticket);
            return ar;
        }
    }
}