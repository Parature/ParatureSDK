using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects.EntityReferences
{
    public class AssetReference: EntityReference<Asset>
    {
        [XmlElement("Asset")]
        public Asset Entity
        {
            get { return base.Entity; }
            set { base.Entity = value; }
        }
    }
}
