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
        public static T EntityFill<T>(XmlDocument xmlDoc)
        {
            var serializer = new XmlSerializer(typeof(T));
            var xml = XDocument.Parse(xmlDoc.OuterXml);
            var entity = (T)serializer.Deserialize(xml.CreateReader());

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
        /// This does the same as the generic FillList method, but overwrites the XML to account for the changing schema
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

            return list;
        }

        static internal PagedData.PagedData ObjectFillList(XmlDocument xmlresp, Boolean minimalisticLoad, int requestdepth, ParaCredentials paraCredentials, ParaEnums.ParatureModule module)
        {
            switch (module)
            {
                case ParaEnums.ParatureModule.Ticket:
                    return FillList<Ticket>(xmlresp);
                case ParaEnums.ParatureModule.Account:
                    return FillList<Account>(xmlresp);
                case ParaEnums.ParatureModule.Customer:
                    return FillList<Customer>(xmlresp);
                case ParaEnums.ParatureModule.Download:
                    return FillList<Download>(xmlresp);
                case ParaEnums.ParatureModule.Article:
                    return FillList<Article>(xmlresp);
                case ParaEnums.ParatureModule.Product:
                    return FillList<Product>(xmlresp);
                case ParaEnums.ParatureModule.Asset:
                    return FillList<Asset>(xmlresp);
                case ParaEnums.ParatureModule.Chat:
                    return FillList<Chat>(xmlresp);
                default:
                    throw new Exception("Unknown Module For the Object Fill list");
            }
        }
    }
}
