using System;
using System.Xml;
using ParatureSDK.ParaHelper;
using ParatureSDK.ParaObjects;
using Account = ParatureSDK.ApiHandler.Account;
using Customer = ParatureSDK.ApiHandler.Customer;
using Product = ParatureSDK.ApiHandler.Product;

namespace ParatureSDK.XmlToObjectParser
{
    /// <summary>
    /// This class helps parse raw XML responses returned from the server to hard typed Asset objects that you can use for further processing.
    /// </summary>
    internal partial class AssetParser
    {
        /// <summary>
        /// This methods requires an Asset xml file and returns a product object. It should only by used for a retrieve operation.
        /// </summary>
        static internal ParaObjects.Asset AssetFill(XmlDocument xmlresp, Boolean MinimalisticLoad, int requestdepth, ParaCredentials ParaCredentials)
        {
            ParaObjects.Asset Asset = new ParaObjects.Asset();
            XmlNode AssetNode = xmlresp.DocumentElement;

            // Setting up the request level for all child items of an account.
            int childDepth = 0;
            if (requestdepth > 0)
            {
                childDepth = requestdepth - 1;
            }
            Asset = ParaEntityParser.EntityFill<ParaObjects.Asset>(xmlresp);
            Asset.FullyLoaded = true;
            return Asset;
        }

        /// <summary>
        /// This methods requires an Asset list xml file and returns an AssetsList oject. It should only by used for a List operation.
        /// </summary>
        static internal ParaEntityList<ParaObjects.Asset> AssetsFillList(XmlDocument xmlresp, Boolean MinimalisticLoad, int requestdepth, ParaCredentials ParaCredentials)
        {
            var AssetsList = new ParaEntityList<ParaObjects.Asset>();
            XmlNode DocNode = xmlresp.DocumentElement;

            // Setting up the request level for all child items of an account.
            int childDepth = 0;
            if (requestdepth > 0)
            {
                childDepth = requestdepth - 1;
            }


            AssetsList.TotalItems = Int32.Parse(DocNode.Attributes["total"].InnerText.ToString());

            if (DocNode.Attributes["page-size"] != null)
            {
                // If this is a "TotalOnly" request, there are no other attributes than "Total"
                AssetsList.PageNumber = Int32.Parse(DocNode.Attributes["page"].InnerText.ToString());
                AssetsList.PageSize = Int32.Parse(DocNode.Attributes["page-size"].InnerText.ToString());
                AssetsList.ResultsReturned = Int32.Parse(DocNode.Attributes["results"].InnerText.ToString());
            }



            foreach (XmlNode xn in DocNode.ChildNodes)
            {
                var xDoc = new XmlDocument();
                xDoc.LoadXml(xn.OuterXml);
                //AssetsList.Data.Add(AssetFillNode(xn, MinimalisticLoad, childDepth, ParaCredentials));
                AssetsList.Data.Add(ParaEntityParser.EntityFill<Asset>(xDoc));
            }
            return AssetsList;
        }
    }
}