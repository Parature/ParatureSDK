using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects.EntityReferences
{
    public class ProductReference: EntityReference<Product>
    {
        [XmlElement("Product")]
        public Product Product
        {
            get { return base.Entity; }
            set { base.Entity = value; }
        }
    }
}
