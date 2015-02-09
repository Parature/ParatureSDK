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
    }
}
