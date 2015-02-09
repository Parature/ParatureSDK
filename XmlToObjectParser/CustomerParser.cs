using System;
using System.Xml;
using ParatureSDK.ParaHelper;
using ParatureSDK.ParaObjects;
using Account = ParatureSDK.ApiHandler.Account;

namespace ParatureSDK.XmlToObjectParser
{
    /// <summary>
    /// This class helps parse raw XML responses returned from the server to hard typed Customer objects that you can use for further processing.
    /// </summary>
    internal class CustomerParser
    {
        /// <summary>
        /// This methods requires a Customer xml file and returns a customer object. It should only by used for a retrieve operation.
        /// </summary>
        static internal ParaObjects.Customer CustomerFill(XmlDocument xmlresp, int requestdepth, bool includeAllCustomFields, ParaCredentials ParaCredentials)
        {
            ParaObjects.Customer Customer = new ParaObjects.Customer();
            XmlNode CustomerNode = xmlresp.DocumentElement;

            // Setting up the request level for all child items of an account.
            int childDepth = 0;
            if (requestdepth > 0)
            {
                childDepth = requestdepth - 1;
            }
            Customer = ParaEntityParser.EntityFill<ParaObjects.Customer>(xmlresp);
            Customer.FullyLoaded = true;
            return Customer;
        }

        /// <summary>
        /// This methods requires a Customer list xml file and returns an CustomersList oject. It should only by used for a List operation.
        /// </summary>
        static internal ParaEntityList<ParaObjects.Customer> CustomersFillList(XmlDocument xmlresp, Boolean MinimalisticLoad, int requestdepth, ParaCredentials ParaCredentials)
        {
            var CustomersList = new ParaEntityList<ParaObjects.Customer>();
            XmlNode DocNode = xmlresp.DocumentElement;

            // Setting up the request level for all child items of an account.
            int childDepth = 0;
            if (requestdepth > 0)
            {
                childDepth = requestdepth - 1;
            }


            CustomersList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());


            if (DocNode.Attributes["page-size"] != null)
            {
                // If this is a "TotalOnly" request, there are no other attributes than "Total"

                CustomersList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                CustomersList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                CustomersList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
            }



            foreach (XmlNode xn in DocNode.ChildNodes)
            {
                var xDoc = new XmlDocument();
                xDoc.LoadXml(xn.OuterXml);
                //CustomersList.Data.Add(CustomerFillNode(xn, childDepth, MinimalisticLoad, ParaCredentials));
                CustomersList.Data.Add(ParaEntityParser.EntityFill<Customer>(xDoc));
            }
            return CustomersList;
        }
    }
}