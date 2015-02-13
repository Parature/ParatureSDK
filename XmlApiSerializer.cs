using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ParatureSDK.ParaObjects;

namespace ParatureSDK
{
    public class XmlApiSerializer
    {
        public static string Serialize(Object ent)
        {
            var serializer = new XmlSerializer(ent.GetType());

            using (var stringWriter = new XmlStringWriterUtf8())
            {
                serializer.Serialize(stringWriter, ent);
                return stringWriter.ToString();
            }
        }

        public static string SerializeList(IEnumerable<ParaEntity> list)
        {
            var attributes = new XmlAttributes { XmlRoot = new XmlRootAttribute("Entities") };
            var type = list.GetType();

            var attributeOverrides = new XmlAttributeOverrides();
            attributeOverrides.Add(type, attributes);

            var serializer = new XmlSerializer(
                type,
                attributeOverrides
            );

            using (var stringWriter = new XmlStringWriterUtf8())
            {
                serializer.Serialize(stringWriter, list);
                return stringWriter.ToString();
            }
        }
    }

    //Override the string builder class so we get UTF8.
    internal class XmlStringWriterUtf8 : StringWriter
    {
        public override Encoding Encoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }
    }
}
