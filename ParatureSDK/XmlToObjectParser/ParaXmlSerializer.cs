using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ParatureSDK.XmlToObjectParser
{
    /// <summary>
    /// Custom XML Serializer for updating/setting attributes due to quirks in deserializing the IEnumerable interface
    /// </summary>
    public class ParaXmlSerializer : XmlSerializer
    {
        /// <summary>
        /// Indicates that a class has special handling after deserializtion
        /// </summary>
        public interface IXmlDeserializationCallback
        {
            void OnXmlDeserialization(XDocument xml);
        }

        /// <summary>
        /// Custom Deserializer for all Parature Objects. 
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public object Deserialize(XDocument xml)
        {
            var result = Deserialize(xml.CreateReader());

            var deserializedCallback = result as IXmlDeserializationCallback;
            if (deserializedCallback != null)
            {
                deserializedCallback.OnXmlDeserialization(xml);
            }

            return result;
        }

        /// <summary>
        /// Type constructor
        /// </summary>
        /// <param name="type"></param>
        public ParaXmlSerializer(Type type)
            : base(type)
        {
        }
    }
}
