using System;
using System.Xml;
using ParatureAPI.ParaObjects;

namespace ParatureAPI.XmlToObjectParser
{
    /// <summary>
    /// Handles all XML parsing logic needed for the Role object
    /// </summary>
    internal partial class TimezoneParser
    {
        /// <summary>
        /// This methods requires a Timezone xml file and returns a Timezone object. It should only by used for a retrieve operation.
        /// </summary>
        /// <param name="xmlresp"></param>
        /// <returns></returns>
        static internal Timezone TimezoneFill(XmlDocument xmlresp)
        {
            Timezone Timezone = new Timezone();
            XmlNode TimezoneNode = xmlresp.DocumentElement;
            Timezone = TimezoneFillNode(TimezoneNode);
            return Timezone;
        }
        /// <summary>
        /// This method requires a Timezone list xml file and returns a Timezone list object. It should only by used for a List operation.
        /// </summary>
        /// <param name="xmlresp"></param>
        /// <returns></returns>
        static internal TimezonesList TimezonesFillList(XmlDocument xmlresp)
        {
            TimezonesList TimezonesList = new TimezonesList();
            XmlNode DocNode = xmlresp.DocumentElement;

            TimezonesList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());

            if (DocNode.Attributes["page-size"] != null)
            {
                // If this is a "TotalOnly" request, there are no other attributes than "Total"
                TimezonesList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                TimezonesList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                TimezonesList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
            }

            foreach (XmlNode xn in DocNode.ChildNodes)
            {
                TimezonesList.Timezones.Add(TimezoneFillNode(xn));
            }
            return TimezonesList;
        }

        /// <summary>
        /// This method accepts a Timezone node and parses through the different items in it. it can be used to parse a Timezone node, whether the node is returned from a simple read, or as part of a list call.
        /// </summary>
        static internal Timezone TimezoneFillNode(XmlNode TimezoneNode)
        {

            Timezone Timezone = new Timezone();
            Timezone.TimezoneID = Int64.Parse(TimezoneNode.Attributes["id"].InnerText.ToString());

            foreach (XmlNode child in TimezoneNode.ChildNodes)
            {
                if (child.LocalName.Trim().ToLower() == "timezone")
                {
                    Timezone.Name = ParserUtils.NodeGetInnerText(child);
                }
                else if (child.LocalName.Trim().ToLower() == "abbreviation")
                {
                    Timezone.Abbreviation = ParserUtils.NodeGetInnerText(child);
                }
            }
            return Timezone;
        }
    }
}