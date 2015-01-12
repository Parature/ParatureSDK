using System;
using System.Collections;
using System.Xml;
using ParatureAPI.ParaObjects;

namespace ParatureAPI.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Csr module.
    /// </summary>
    public class Csr
    {
        /// <summary>
        /// Provides the Schema of the CSR module.
        /// </summary>
        public static ParaObjects.Csr CsrSchema(ParaCredentials ParaCredentials)
        {
            ParaObjects.Csr Csr = new ParaObjects.Csr();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetSchema(ParaCredentials, ParaEnums.ParatureModule.Csr);

            if (ar.HasException == false)
            {
                Csr = XmlToObjectParser.CsrParser.CsrFill(ar.xmlReceived);
            }
            Csr.ApiCallResponse = ar;
            Csr.IsDirty = false;
            return Csr;
        }

        /// <summary>
        /// Provides the capability to delete a CSR.
        /// </summary>
        /// <param name="CsrId">
        /// The id of the CSR to delete
        /// </param>
        public static ApiCallResponse CsrDelete(Int64 CsrId, ParaCredentials ParaCredentials)
        {
            return ApiCallFactory.ObjectDelete(ParaCredentials, ParaEnums.ParatureModule.Csr, CsrId, true);
        }

        /// <summary>
        /// Returns an Csr object with all of its properties filled.
        /// </summary>
        /// <param name="Csrid">
        ///The Csr number that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="ParaCredentials">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>               
        public static ParaObjects.Csr CsrGetDetails(Int64 Csrid, ParaCredentials ParaCredentials)
        {
            ParaObjects.Csr Csr = new ParaObjects.Csr();
            Csr = CsrFillDetails(Csrid, ParaCredentials);
            return Csr;
        }

        /// <summary>
        /// Returns an csr object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="CsrXML">
        /// The Csr XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaObjects.Csr CsrGetDetails(XmlDocument CsrXML)
        {
            ParaObjects.Csr csr = new ParaObjects.Csr();
            csr = XmlToObjectParser.CsrParser.CsrFill(CsrXML);

            return csr;
        }

        /// <summary>
        /// Creates a Parature CSR. Requires an Object and a credentials object. Will return the Newly Created CSR ID
        /// </summary>
        public static ApiCallResponse CsrInsert(ParaObjects.Csr Csr, ParaCredentials ParaCredentials)
        {
            ApiCallResponse ar = new ApiCallResponse();
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc = XmlGenerator.CsrGenerateXML(Csr);
            ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, ParaEnums.ParatureModule.Csr, doc, 0);
            Csr.CsrID = ar.Objectid;
            return ar;
        }

        /// <summary>
        /// Returns an csr list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="CsrListXML">
        /// The Csr List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static CsrsList CsrsGetList(XmlDocument CsrListXML)
        {
            CsrsList csrsList = new CsrsList();
            csrsList = XmlToObjectParser.CsrParser.CsrsFillList(CsrListXML);

            csrsList.ApiCallResponse.xmlReceived = CsrListXML;

            return csrsList;
        }

        /// <summary>
        /// Get the list of Csrs from within your Parature license.
        /// </summary>
        public static CsrsList CsrsGetList(ParaCredentials ParaCredentials)
        {
            return CsrFillList(ParaCredentials, new ModuleQuery.CsrQuery());
        }

        /// <summary>
        /// Updates a Parature Csr. Requires a Csr object and a ParaCredentials object.  Will return the updated Csrid
        /// </summary>
        public static ApiCallResponse CsrUpdate(ParaObjects.Csr Csr, ParaCredentials ParaCredentials)
        {
            ApiCallResponse ar = new ApiCallResponse();

            ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, ParaEnums.ParatureModule.Csr, XmlGenerator.CsrGenerateXML(Csr), Csr.CsrID);


            return ar;
            //return 0;
        }

        /// <summary>
        /// Get the list of Csrs from within your Parature license.
        /// </summary>
        public static CsrsList CsrsGetList(ParaCredentials ParaCredentials, ModuleQuery.CsrQuery Query)
        {
            return CsrFillList(ParaCredentials, Query);
        }
        /// <summary>
        /// Fills a Sla list object.
        /// </summary>
        private static CsrsList CsrFillList(ParaCredentials ParaCredentials, ModuleQuery.CsrQuery Query)
        {

            CsrsList CsrsList = new CsrsList();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Csr, Query.BuildQueryArguments());
            if (ar.HasException == false)
            {
                CsrsList = XmlToObjectParser.CsrParser.CsrsFillList(ar.xmlReceived);
            }

            CsrsList.ApiCallResponse = ar;


            // Checking if the system needs to recursively call all of the data returned.
            if (Query.RetrieveAllRecords) 
            {
                bool continueCalling = true;
                while (continueCalling)
                {
                    CsrsList objectlist = new CsrsList();

                    if (CsrsList.TotalItems > CsrsList.Csrs.Count)
                    {
                        // We still need to pull data

                        // Getting next page's data
                        Query.PageNumber = Query.PageNumber + 1;

                        ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Csr, Query.BuildQueryArguments());

                        objectlist = XmlToObjectParser.CsrParser.CsrsFillList(ar.xmlReceived);

                        if (objectlist.Csrs.Count == 0)
                        {
                            continueCalling = false;
                        }

                        CsrsList.Csrs.AddRange(objectlist.Csrs);
                        CsrsList.ResultsReturned = CsrsList.Csrs.Count;
                        CsrsList.PageNumber = Query.PageNumber;
                    }
                    else
                    {
                        // That is it, pulled all the items.
                        continueCalling = false;
                        CsrsList.ApiCallResponse = ar;
                    }
                }
            }

            return CsrsList;
        }

        private static ParaObjects.Csr CsrFillDetails(Int64 Csrid, ParaCredentials ParaCredentials)
        {
            ParaObjects.Csr Csr = new ParaObjects.Csr();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, ParaEnums.ParatureModule.Csr, Csrid);
            if (ar.HasException == false)
            {
                Csr = XmlToObjectParser.CsrParser.CsrFill(ar.xmlReceived);
            }
            else
            {

                Csr.CsrID = 0;
            }

            Csr.ApiCallResponse = ar;

            return Csr;
        }

        /// <summary>
        /// Contains all the methods needed to work with the Ticket statuses.
        /// </summary>
        public partial class CsrStatus
        {
            /// <summary>
            /// Returns a filled Csr status object.
            /// </summary>
            /// <param name="CsrStatusid">
            ///The Status id that you would like to get the details of. 
            ///Value Type: <see cref="Int64" />   (System.Int64)
            ///</param>
            /// <param name="ParaCredentials">
            /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            /// </param>                
            public static ParaObjects.CsrStatus CsrStatusGetDetails(Int64 CsrStatusid, ParaCredentials ParaCredentials)
            {
                ParaObjects.CsrStatus CsrStatus = new ParaObjects.CsrStatus();
                CsrStatus = CsrStatusFillDetails(CsrStatusid, ParaCredentials);
                return CsrStatus;
            }

            /// <summary>
            /// Returns an CsrStatus object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="CsrStatusXML">
            /// The CsrStatus XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.CsrStatus CsrStatusGetDetails(XmlDocument CsrStatusXML)
            {
                ParaObjects.CsrStatus CsrStatus = new ParaObjects.CsrStatus();
                CsrStatus = XmlToObjectParser.CsrStatusParser.CsrStatusFill(CsrStatusXML);

                CsrStatus.ApiCallResponse.xmlReceived = CsrStatusXML;
                CsrStatus.ApiCallResponse.Objectid = CsrStatus.StatusID;

                return CsrStatus;
            }

            /// <summary>
            /// Returns an CsrStatus list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="CsrStatusListXML">
            /// The CsrStatus List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static CsrStatusList CsrStatusGetList(XmlDocument CsrStatusListXML)
            {
                CsrStatusList CsrStatussList = new CsrStatusList();
                CsrStatussList = XmlToObjectParser.CsrStatusParser.CsrStatusFillList(CsrStatusListXML);

                CsrStatussList.ApiCallResponse.xmlReceived = CsrStatusListXML;

                return CsrStatussList;
            }

            /// <summary>
            /// Provides you with the capability to list statuses
            /// </summary>
            public static CsrStatusList CsrStatusGetList(ParaCredentials ParaCredentials)
            {
                return CsrStatusFillList(ParaCredentials);
            }


            /// <summary>
            /// Fills an Csr Status object.
            /// </summary>
            private static CsrStatusList CsrStatusFillList(ParaCredentials ParaCredentials)
            {

                CsrStatusList CsrStatusList = new CsrStatusList();
                //DownloadsList = null;
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectSecondLevelGetList(ParaCredentials, ParaEnums.ParatureModule.Csr, ParaEnums.ParatureEntity.status, new ArrayList(0));
                if (ar.HasException == false)
                {
                    CsrStatusList = XmlToObjectParser.CsrStatusParser.CsrStatusFillList(ar.xmlReceived);
                }
                CsrStatusList.ApiCallResponse = ar;
                return CsrStatusList;
            }

            static ParaObjects.CsrStatus CsrStatusFillDetails(Int64 CsrStatusid, ParaCredentials ParaCredentials)
            {

                ParaObjects.CsrStatus CsrStatus = new ParaObjects.CsrStatus();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, ParaEnums.ParatureEntity.CsrStatus, CsrStatusid);
                if (ar.HasException == false)
                {
                    CsrStatus = XmlToObjectParser.CsrStatusParser.CsrStatusFill(ar.xmlReceived);
                }
                else
                {
                    CsrStatus.StatusID = 0;
                }

                CsrStatus.ApiCallResponse = ar;
                return CsrStatus;
            }

        }
    }
}