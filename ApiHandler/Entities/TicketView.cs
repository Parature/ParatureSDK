using System.Xml;
using ParatureSDK.EntityQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler.Entities
{
    public class TicketView
    {
        /// <summary>
        /// Returns an View list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="viewListXml">
        /// The View List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<ParaObjects.View> ViewGetList(XmlDocument viewListXml)
        {
            var viewsList = TicketViewParser.ViewFillList(viewListXml);

            viewsList.ApiCallResponse.xmlReceived = viewListXml;

            return viewsList;
        }

        /// <summary>
        /// Get the list of Views from within your Parature license.
        /// </summary>
        public static ParaEntityList<ParaObjects.View> ViewGetList(ParaCredentials paraCredentials, ViewQuery query)
        {
            return ViewFillList(paraCredentials, query);
        }

        /// <summary>
        /// Fills a View List object.
        /// </summary>
        private static ParaEntityList<ParaObjects.View> ViewFillList(ParaCredentials paraCredentials, ViewQuery query)
        {

            var viewList = new ParaEntityList<ParaObjects.View>();
            var ar = ApiCallFactory.ObjectSecondLevelGetList(paraCredentials, ParaEnums.ParatureModule.Ticket, ParaEnums.ParatureEntity.view, query.BuildQueryArguments());
            if (ar.HasException == false)
            {
                viewList = TicketViewParser.ViewFillList(ar.xmlReceived);
            }
            viewList.ApiCallResponse = ar;
            return viewList;
        }
    }
}