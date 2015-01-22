using System;
using System.Xml;
using ParatureAPI.ParaObjects;
using ParatureAPI.XmlToObjectParser;

namespace ParatureAPI.ApiHandler.Entities
{
    public class CsrStatus
    {
        ///  <summary>
        ///  Returns an Csr object with all of its properties filled.
        ///  </summary>
        ///  <param name="Csrid">
        /// The Csr number that you would like to get the details of. 
        /// Value Type: <see cref="Int64" />   (System.Int64)
        /// </param>
        /// <param name="csrStatusId"></param>
        /// <param name="paraCredentials">
        ///  The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        ///  </param>               
        public static ParaObjects.CsrStatus CsrStatusGetDetails(Int64 csrStatusId, ParaCredentials paraCredentials)
        {
            ParaObjects.CsrStatus CsrStatus = new ParaObjects.CsrStatus();
            CsrStatus = CsrStatusFillDetails(csrStatusId, paraCredentials);
            return CsrStatus;
        }
               
        /// <summary>
        /// Get the list of Csrs from within your Parature license.
        /// </summary>
        public static ParaEntityList<ParaObjects.CsrStatus> CsrStatusGetList(ParaCredentials paraCredentials)
        {
            return CsrStatusFillList(paraCredentials, new EntityQuery.CsrStatusQuery());
        }

        /// <summary>
        /// Get the list of Csrs from within your Parature license.
        /// </summary>
        public static ParaEntityList<ParaObjects.CsrStatus> CsrStatusGetList(ParaCredentials paraCredentials, EntityQuery.CsrStatusQuery query)
        {
            return CsrStatusFillList(paraCredentials, query);
        }

        /// <summary>
        /// Returns an CsrStatus object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="csrStatusXml">
        /// The CsrStatus XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaObjects.CsrStatus CsrStatusGetDetails(XmlDocument csrStatusXml)
        {
            ParaObjects.CsrStatus CsrStatus = new ParaObjects.CsrStatus();
            CsrStatus = CsrStatusParser.CsrStatusFill(csrStatusXml);

            CsrStatus.ApiCallResponse.xmlReceived = csrStatusXml;
            CsrStatus.ApiCallResponse.Objectid = CsrStatus.StatusID;

            return CsrStatus;
        }

        /// <summary>
        /// Returns an CsrStatus list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="csrStatusListXml">
        /// The CsrStatus List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<ParaObjects.CsrStatus> CsrStatusGetList(XmlDocument csrStatusListXml)
        {
            var CsrStatussList = new ParaEntityList<ParaObjects.CsrStatus>();
            CsrStatussList = CsrStatusParser.CsrStatusFillList(csrStatusListXml);

            CsrStatussList.ApiCallResponse.xmlReceived = csrStatusListXml;

            return CsrStatussList;
        }

        /// <summary>
        /// Fills a Sla list object.
        /// </summary>
        private static ParaEntityList<ParaObjects.CsrStatus> CsrStatusFillList(ParaCredentials paraCredentials, EntityQuery.CsrStatusQuery query)
        {

            var CsrStatusList = new ParaEntityList<ParaObjects.CsrStatus>();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectSecondLevelGetList(paraCredentials, ParaEnums.ParatureModule.Csr, ParaEnums.ParatureEntity.status, query.BuildQueryArguments());
            if (ar.HasException == false)
            {
                CsrStatusList = CsrStatusParser.CsrStatusFillList(ar.xmlReceived);
            }
            CsrStatusList.ApiCallResponse = ar;
            return CsrStatusList;
        }

        private static ParaObjects.CsrStatus CsrStatusFillDetails(Int64 csrStatusId, ParaCredentials paraCredentials)
        {
            ParaObjects.CsrStatus CsrStatus = new ParaObjects.CsrStatus();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetDetail(paraCredentials, ParaEnums.ParatureEntity.CsrStatus, csrStatusId);
            if (ar.HasException == false)
            {
                CsrStatus = CsrStatusParser.CsrStatusFill(ar.xmlReceived);
            }
            else
            {

                CsrStatus.StatusID = 0;
            }

            return CsrStatus;
        }
    }
}