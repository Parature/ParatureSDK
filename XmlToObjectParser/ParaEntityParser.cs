using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using ParatureSDK.Fields;
using ParatureSDK.ParaObjects;

namespace ParatureSDK.XmlToObjectParser
{
    public class ParaEntityParser
    {
        public static Download EntityFillDownload(XmlDocument xmlDoc)
        {
            var xDoc = new XmlDocument();
            xDoc.LoadXml(xmlDoc.OuterXml);
            var hasMultipleDownloadFolders = ApiHandler.Download.HasMultipleFoldersAndConvert(xDoc);
            var dl = EntityFill<Download>(xDoc);
            dl.MultipleFolders = hasMultipleDownloadFolders;

            return dl;
        }

        public static T EntityFill<T>(XmlDocument xmlDoc)
        {
            var serializer = new ParaXmlSerializer(typeof(T));
            var xml = XDocument.Parse(xmlDoc.OuterXml);
            var entity = (T)serializer.Deserialize(xml);

            return entity;
        }

        public static ParaEntityList<TEnt> FillList<TEnt>(XmlDocument xmlDoc)
        {
            //Generate the paged data parsed object.
            var list = EntityFill<ParaEntityList<TEnt>>(xmlDoc);
            var docNode = xmlDoc.DocumentElement;

            //Resulting in duplicates due to proper deserialization of the "Data" property
            /*
            foreach (XmlNode xn in docNode.ChildNodes)
            {
                var xDoc = new XmlDocument();
                xDoc.LoadXml(xn.OuterXml);
                list.Data.Add(EntityFill<TEnt>(xDoc));
            }
            */

            return list;
        }

        /// <summary>
        /// Download module can have a config that changes the schema... 
        /// This does the same as the generic ApiGetEntityList method, but overwrites the XML to account for the changing schema
        /// Specific node is "Folder" vs "Folders"
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static ParaEntityList<Download> FillListDownload(XmlDocument xmlDoc)
        {
            //Generate the paged data parsed object. Data prop will be empty
            var list = new ParaEntityList<Download>();
            var docNode = xmlDoc.DocumentElement;

            //Fill the list of entities
            foreach (XmlNode xn in docNode.ChildNodes)
            {
                var xDoc = new XmlDocument();
                xDoc.LoadXml(xn.OuterXml);
                var hasMultipleDownloadFolders = ApiHandler.Download.HasMultipleFoldersAndConvert(xDoc);
                var dl = EntityFill<Download>(xDoc);
                dl.MultipleFolders = hasMultipleDownloadFolders;

                list.Data.Add(dl);
            }

            using (var nodeReader = new XmlNodeReader(xmlDoc))
            {
                //We need to manually parse total and results for PagedData object
                nodeReader.MoveToContent();
                var xml = XDocument.Load(nodeReader);

                var totalAttr = xml.Root.Attribute("total");
                list.TotalItems = totalAttr != null ? int.Parse(totalAttr.Value) : 0;

                var resultsAttr = xml.Root.Attribute("results");
                list.ResultsReturned = resultsAttr != null ? int.Parse(resultsAttr.Value) : 0;

                var pageAttr = xml.Root.Attribute("page");
                list.PageNumber = pageAttr != null ? int.Parse(pageAttr.Value) : 0;

                var pageSizeAttr = xml.Root.Attribute("page-size");
                list.PageSize = pageSizeAttr != null ? int.Parse(pageSizeAttr.Value) : 0;
            }

            return list;
        }
    }
}
