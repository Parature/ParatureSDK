using System;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects.EntityReferences
{
    public class EntityReference<T> : IEntityReference 
        where T : ParaEntityBaseProperties
    {
        [XmlIgnore]
        public T Entity { get; set; }

        public ParaEntityBaseProperties GetEntity()
        {
            return Entity;
        }
    }
}
