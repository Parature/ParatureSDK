using System;
using System.Collections;
using System.Xml;
using ParatureSDK.ModuleQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Csr module.
    /// </summary>
    public class Csr
    {
        /// <summary>
        /// Provides the Schema of the CSR module.
        /// </summary>
        public static ParaObjects.Csr Schema(ParaCredentials ParaCredentials)
        {
            ParaObjects.Csr Csr = new ParaObjects.Csr();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetSchema(ParaCredentials, ParaEnums.ParatureModule.Csr);

            if (ar.HasException == false)
            {
                Csr = ParaEntityParser.EntityFill<ParaObjects.Csr>(ar.XmlReceived);
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
        public static ApiCallResponse Delete(Int64 CsrId, ParaCredentials ParaCredentials)
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
        public static ParaObjects.Csr GetDetails(Int64 Csrid, ParaCredentials ParaCredentials)
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
        public static ParaObjects.Csr GetDetails(XmlDocument CsrXML)
        {
            ParaObjects.Csr csr = new ParaObjects.Csr();
            csr = ParaEntityParser.EntityFill<ParaObjects.Csr>(CsrXML);

            return csr;
        }

        /// <summary>
        /// Creates a Parature CSR. Requires an Object and a credentials object. Will return the Newly Created CSR ID
        /// </summary>
        public static ApiCallResponse Insert(ParaObjects.Csr Csr, ParaCredentials ParaCredentials)
        {
            ApiCallResponse ar = new ApiCallResponse();
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc = XmlGenerator.GenerateXml(Csr);
            ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, ParaEnums.ParatureModule.Csr, doc, 0);
            Csr.Id = ar.Id;
            return ar;
        }

        /// <summary>
        /// Returns an csr list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="CsrListXML">
        /// The Csr List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<ParaObjects.Csr> GetList(XmlDocument CsrListXML)
        {
            var csrsList = new ParaEntityList<ParaObjects.Csr>();
            csrsList = ParaEntityParser.FillList<ParaObjects.Csr>(CsrListXML);

            csrsList.ApiCallResponse.XmlReceived = CsrListXML;

            return csrsList;
        }

        /// <summary>
        /// Get the list of Csrs from within your Parature license.
        /// </summary>
        public static ParaEntityList<ParaObjects.Csr> GetList(ParaCredentials ParaCredentials)
        {
            return FillList(ParaCredentials, new CsrQuery());
        }

        /// <summary>
        /// Updates a Parature Csr. Requires a Csr object and a ParaCredentials object.  Will return the updated Csrid
        /// </summary>
        public static ApiCallResponse Update(ParaObjects.Csr Csr, ParaCredentials ParaCredentials)
        {
            ApiCallResponse ar = new ApiCallResponse();

            ar = ApiCallFactory.ObjectCreateUpdate(ParaCredentials, ParaEnums.ParatureModule.Csr, XmlGenerator.GenerateXml(Csr), Csr.Id);


            return ar;
            //return 0;
        }

        /// <summary>
        /// Get the list of Csrs from within your Parature license.
        /// </summary>
        public static ParaEntityList<ParaObjects.Csr> GetList(ParaCredentials ParaCredentials, CsrQuery Query)
        {
            return FillList(ParaCredentials, Query);
        }
        /// <summary>
        /// Fills a Sla list object.
        /// </summary>
        private static ParaEntityList<ParaObjects.Csr> FillList(ParaCredentials ParaCredentials, CsrQuery Query)
        {

            var CsrsList = new ParaEntityList<ParaObjects.Csr>();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Csr, Query.BuildQueryArguments());
            if (ar.HasException == false)
            {
                CsrsList = ParaEntityParser.FillList<ParaObjects.Csr>(ar.XmlReceived);
            }

            CsrsList.ApiCallResponse = ar;


            // Checking if the system needs to recursively call all of the data returned.
            if (Query.RetrieveAllRecords) 
            {
                bool continueCalling = true;
                while (continueCalling)
                {
                    var objectlist = new ParaEntityList<ParaObjects.Csr>();

                    if (CsrsList.TotalItems > CsrsList.Data.Count)
                    {
                        // We still need to pull data

                        // Getting next page's data
                        Query.PageNumber = Query.PageNumber + 1;

                        ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Csr, Query.BuildQueryArguments());

                        objectlist = ParaEntityParser.FillList<ParaObjects.Csr>(ar.XmlReceived);

                        if (objectlist.Data.Count == 0)
                        {
                            continueCalling = false;
                        }

                        CsrsList.Data.AddRange(objectlist.Data);
                        CsrsList.ResultsReturned = CsrsList.Data.Count;
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
            ar = ApiCallFactory.ObjectGetDetail<ParaObjects.Csr>(ParaCredentials, ParaEnums.ParatureModule.Csr, Csrid);
            if (ar.HasException == false)
            {
                Csr = ParaEntityParser.EntityFill<ParaObjects.Csr>(ar.XmlReceived);
            }
            else
            {

                Csr.Id = 0;
            }

            Csr.ApiCallResponse = ar;

            return Csr;
        }

        /// <summary>
        /// Contains all the methods needed to work with the Ticket statuses.
        /// </summary>
        public class CsrStatus
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
            public static ParaObjects.CsrStatus GetDetails(Int64 CsrStatusid, ParaCredentials ParaCredentials)
            {
                ParaObjects.CsrStatus CsrStatus = new ParaObjects.CsrStatus();
                CsrStatus = FillDetails(CsrStatusid, ParaCredentials);
                return CsrStatus;
            }

            /// <summary>
            /// Returns an CsrStatus object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="CsrStatusXML">
            /// The CsrStatus XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.CsrStatus GetDetails(XmlDocument CsrStatusXML)
            {
                ParaObjects.CsrStatus CsrStatus = new ParaObjects.CsrStatus();
                CsrStatus = ParaEntityParser.EntityFill<ParaObjects.CsrStatus>(CsrStatusXML);

                CsrStatus.ApiCallResponse.XmlReceived = CsrStatusXML;
                CsrStatus.ApiCallResponse.Id = CsrStatus.Id;

                return CsrStatus;
            }

            /// <summary>
            /// Returns an CsrStatus list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="CsrStatusListXML">
            /// The CsrStatus List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaEntityList<ParaObjects.CsrStatus> GetList(XmlDocument CsrStatusListXML)
            {
                var CsrStatussList = new ParaEntityList<ParaObjects.CsrStatus>();
                CsrStatussList = ParaEntityParser.FillList<ParaObjects.CsrStatus>(CsrStatusListXML);

                CsrStatussList.ApiCallResponse.XmlReceived = CsrStatusListXML;

                return CsrStatussList;
            }

            /// <summary>
            /// Provides you with the capability to list statuses
            /// </summary>
            public static ParaEntityList<ParaObjects.CsrStatus> GetList(ParaCredentials ParaCredentials)
            {
                return FillList(ParaCredentials);
            }


            /// <summary>
            /// Fills an Csr Status object.
            /// </summary>
            private static ParaEntityList<ParaObjects.CsrStatus> FillList(ParaCredentials ParaCredentials)
            {

                var CsrStatusList = new ParaEntityList<ParaObjects.CsrStatus>();
                //DownloadsList = null;
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectSecondLevelGetList(ParaCredentials, ParaEnums.ParatureModule.Csr, ParaEnums.ParatureEntity.status, new ArrayList(0));
                if (ar.HasException == false)
                {
                    CsrStatusList = ParaEntityParser.FillList<ParaObjects.CsrStatus>(ar.XmlReceived);
                }
                CsrStatusList.ApiCallResponse = ar;
                return CsrStatusList;
            }

            private static ParaObjects.CsrStatus FillDetails(Int64 CsrStatusid, ParaCredentials ParaCredentials)
            {

                ParaObjects.CsrStatus CsrStatus = new ParaObjects.CsrStatus();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, ParaEnums.ParatureEntity.CsrStatus, CsrStatusid);
                if (ar.HasException == false)
                {
                    CsrStatus = ParaEntityParser.EntityFill<ParaObjects.CsrStatus>(ar.XmlReceived);
                }
                else
                {
                    CsrStatus.Id = 0;
                }

                CsrStatus.ApiCallResponse = ar;
                return CsrStatus;
            }

        }
    }
}