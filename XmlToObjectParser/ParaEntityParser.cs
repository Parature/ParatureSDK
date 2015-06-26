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
                list.TotalItems = int.Parse(xml.Root.Attribute("total").Value);
                list.ResultsReturned = int.Parse(xml.Root.Attribute("results").Value);
                list.PageNumber = int.Parse(xml.Root.Attribute("total").Value);
                list.PageSize = int.Parse(xml.Root.Attribute("page-size").Value);
            }

            return list;
        }
    }
}
