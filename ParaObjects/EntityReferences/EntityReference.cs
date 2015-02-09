using System;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects.EntityReferences
{
    public class EntityReference<T> where T : ParaEntity
    {
        [XmlIgnore]
        public T Entity { get; set; }
    }
}
