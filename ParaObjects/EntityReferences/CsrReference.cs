using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects.EntityReferences
{
    public class CsrReference : EntityReference<Csr>
    {
        [XmlElement("Csr")]
        public Csr Csr
        {
            get { return base.Entity; }
            set { base.Entity = value; }
        }
    }
}
