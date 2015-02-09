using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects.EntityReferences
{
    class CustomerReference : EntityReference<Customer>
    {
        [XmlElement("Customer")]
        public Customer Entity
        {
            get { return base.Entity; }
            set { base.Entity = value; }
        }
    }
}
