using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects.EntityReferences
{
    public class EulaReference : EntityReference<Eula>
    {
        [XmlElement("Eula")]
        public Eula Eula
        {
            get { return Entity; }
            set { Entity = value; }
        }
    }
}
