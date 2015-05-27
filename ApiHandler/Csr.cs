using System;
using System.Collections;
using System.Xml;
using ParatureSDK.EntityQuery;
using ParatureSDK.ModuleQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Csr module.
    /// </summary>
    public static class Csr
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
                var purgedSchema = ApiUtils.RemoveStaticFieldsNodes(ar.XmlReceived);
                Csr = ParaEntityParser.EntityFill<ParaObjects.Csr>(purgedSchema);
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
        public static class Status
        {
            private static ParaEnums.ParatureEntity _entityType = ParaEnums.ParatureEntity.status;
            private static ParaEnums.ParatureModule _moduleType = ParaEnums.ParatureModule.Csr;

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
            public static ParaObjects.CsrStatus GetDetails(Int64 id, ParaCredentials creds)
            {
                var status = ApiUtils.FillDetails<ParaObjects.CsrStatus>(id, creds, _entityType);
                return status;
            }

            /// <summary>
            /// Returns an Status object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="xml">
            /// The Status XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.CsrStatus GetDetails(XmlDocument xml)
            {
                var status = ParaEntityParser.EntityFill<ParaObjects.CsrStatus>(xml);

                return status;
            }

            /// <summary>
            /// Returns an Status list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="listXml">
            /// The Status List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaEntityList<ParaObjects.CsrStatus> GetList(XmlDocument listXml)
            {
                var statusList = ParaEntityParser.FillList<ParaObjects.CsrStatus>(listXml);

                statusList.ApiCallResponse.XmlReceived = listXml;

                return statusList;
            }

            /// <summary>
            /// Get the List of Statuss from within your Parature license
            /// </summary>
            /// <param name="creds"></param>
            /// <param name="query"></param>
            /// <returns></returns>
            public static ParaEntityList<ParaObjects.CsrStatus> GetList(ParaCredentials creds, StatusQuery query, ParaEnums.ParatureModule module)
            {
                return ApiUtils.FillList<ParaObjects.CsrStatus>(creds, query, _entityType, _moduleType);
            }

            public static ParaEntityList<ParaObjects.CsrStatus> GetList(ParaCredentials creds, ParaEnums.ParatureModule module)
            {
                return ApiUtils.FillList<ParaObjects.CsrStatus>(creds, new StatusQuery(), _entityType, _moduleType);
            }

        }

        public static class Role
        {
            private static ParaEnums.ParatureEntity _entityType = ParaEnums.ParatureEntity.role;
            private static ParaEnums.ParatureModule _moduleType = ParaEnums.ParatureModule.Csr;

            /// <summary>
            /// Returns a Role object with all of its properties filled.
            /// </summary>
            /// <param name="id">
            /// The Role number that you would like to get the details of.
            /// Value Type: <see cref="Int64" />   (System.Int64)
            ///</param>
            /// <param name="creds">
            /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            /// </param>
            /// <returns></returns>
            public static ParaObjects.CsrRole GetDetails(Int64 id, ParaCredentials creds)
            {
                var entity = ApiUtils.FillDetails<CsrRole>(id, creds, _entityType);
                return entity;
            }

            /// <summary>
            /// Returns an role object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="xml">
            /// The Role XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.CsrRole GetDetails(XmlDocument xml)
            {
                var entity = ParaEntityParser.EntityFill<CsrRole>(xml);

                return entity;
            }

            /// <summary>
            /// Returns an role list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="listXml">
            /// The Role List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaEntityList<ParaObjects.CsrRole> GetList(XmlDocument listXml)
            {
                var list = ParaEntityParser.FillList<CsrRole>(listXml);

                list.ApiCallResponse.XmlReceived = listXml;

                return list;
            }

            /// <summary>
            /// Get the List of Roles from within your Parature license
            /// </summary>
            /// <param name="creds"></param>
            /// <param name="query"></param>
            /// <returns></returns>
            public static ParaEntityList<ParaObjects.CsrRole> GetList(ParaCredentials creds, RoleQuery query, ParaEnums.ParatureModule module)
            {
                return ApiUtils.FillList<CsrRole>(creds, query, _entityType, _moduleType);
            }

            public static ParaEntityList<ParaObjects.CsrRole> GetList(ParaCredentials creds, ParaEnums.ParatureModule module)
            {
                return ApiUtils.FillList<CsrRole>(creds, new RoleQuery(), _entityType, _moduleType);
            }
        }
    }
}