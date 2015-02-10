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
            //Generate the paged data parsed object. Data prop will be empty
            var list = EntityFill<ParaEntityList<TEnt>>(xmlDoc);
            var docNode = xmlDoc.DocumentElement;

            //Fill the list of entities
            foreach (XmlNode xn in docNode.ChildNodes)
            {
                var xDoc = new XmlDocument();
                xDoc.LoadXml(xn.OuterXml);
                list.Data.Add(EntityFill<TEnt>(xDoc));
            }

            return list;
        }

        static internal PagedData.PagedData ObjectFillList(XmlDocument xmlresp, Boolean MinimalisticLoad, int requestdepth, ParaCredentials ParaCredentials, ParaEnums.ParatureModule module)
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
                    //case Paraenums.ParatureModule.Chat:
                    //    return ChatParser.ChatsFillList(xmlresp, false, false, requestdepth, ParaCredentials);
                default:
                    throw new Exception("Unknown Module For the Object Fill list");
            }
        }
    }
}
