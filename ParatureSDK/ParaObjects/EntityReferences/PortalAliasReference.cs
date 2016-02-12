using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects.EntityReferences
{
    public class PortalAliasReference : EntityReference<PortalAlias>
    {
        [XmlElement("Portal_Alias")]
        public PortalAlias PortalAlias
        {
            get { return base.Entity; }
            set { base.Entity = value; }
        }
    }
}
