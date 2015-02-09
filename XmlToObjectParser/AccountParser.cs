using System;
using System.Collections.Generic;
using System.Xml;
using ParatureSDK.ParaHelper;
using ParatureSDK.ParaObjects;
using Account = ParatureSDK.ApiHandler.Account;

namespace ParatureSDK.XmlToObjectParser
{
    /// <summary>
    /// This class helps parse raw XML responses returned from the server to hard typed Account objects that you can use for further processing.
    /// </summary>
    internal class AccountParser
    {
        /// <summary>
        /// This methods requires an account xml file and returns an account object. It should only by used for a retrieve operation.
        /// </summary>
        static internal ParaObjects.Account AccountFill(XmlDocument xmlresp, int requestdepth, bool MinimalisticLoad, ParaCredentials ParaCredentials)
        {
            ParaObjects.Account account = new ParaObjects.Account();
            XmlNode AccountNode = xmlresp.DocumentElement;

            // Setting up the request level for all child items of an account.
            int childDepth = 0;
            if (requestdepth > 0)
            {
                childDepth = requestdepth - 1;
            }

            account = ParaEntityParser.EntityFill<ParaObjects.Account>(xmlresp);
            account.FullyLoaded = true;
            return account;
        }

        /// <summary>
        /// This methods requires an account list xml file and returns an AccountsList oject. It should only by used for a List operation.
        /// </summary>
        static internal ParaEntityList<ParaObjects.Account> AccountsFillList(XmlDocument xmlresp, Boolean MinimalisticLoad, int requestdepth, ParaCredentials ParaCredentials)
        {
            var AccountsList = new ParaEntityList<ParaObjects.Account>();
            XmlNode DocNode = xmlresp.DocumentElement;

            // Setting up the request level for all child items of an account.
            int childDepth = 0;
            if (requestdepth > 0)
            {
                childDepth = requestdepth - 1;
            }


            AccountsList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());

            if (DocNode.Attributes["page-size"] != null)
            {
                // If this is a "TotalOnly" request, there are no other attributes than "Total"
                AccountsList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                AccountsList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                AccountsList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
            }


            foreach (XmlNode xn in DocNode.ChildNodes)
            {
                var xDoc = new XmlDocument();
                xDoc.LoadXml(xn.OuterXml);
                AccountsList.Data.Add(ParaEntityParser.EntityFill<ParaObjects.Account>(xDoc));
                //AccountsList.Data.Add(AccountFillNode(xn, childDepth, MinimalisticLoad, ParaCredentials));
            }
            return AccountsList;
        }
    }
}