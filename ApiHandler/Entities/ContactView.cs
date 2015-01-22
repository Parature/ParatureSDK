using System.Xml;
using ParatureAPI.ParaObjects;
using ParatureAPI.XmlToObjectParser;

namespace ParatureAPI.ApiHandler.Entities
{
    public class ContactView
    {
        /// <summary>
        /// Returns an View list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="viewListXml">
        /// The View List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ViewList ViewGetList(XmlDocument viewListXml)
        {
            var ViewsList = new ViewList();
            ViewsList = CustomerViewParser.ViewFillList(viewListXml);

            ViewsList.ApiCallResponse.xmlReceived = viewListXml;

            return ViewsList;
        }

        /// <summary>
        /// Get the list of Views from within your Parature license.
        /// </summary>
        public static ViewList ViewGetList(ParaCredentials paraCredentials, EntityQuery.ViewQuery query)
        {
            return ViewFillList(paraCredentials, query);
        }

        /// <summary>
        /// Fills a View List object.
        /// </summary>
        private static ViewList ViewFillList(ParaCredentials paraCredentials, EntityQuery.ViewQuery query)
        {

            var ViewList = new ViewList();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectSecondLevelGetList(paraCredentials, ParaEnums.ParatureModule.Customer, ParaEnums.ParatureEntity.view, query.BuildQueryArguments());
            if (ar.HasException == false)
            {
                ViewList = CustomerViewParser.ViewFillList(ar.xmlReceived);
            }
            ViewList.ApiCallResponse = ar;
            return ViewList;
        }
    }
}