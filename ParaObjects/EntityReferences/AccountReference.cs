using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects.EntityReferences
{
    class AccountReference: EntityReference<Account>
    {
        [XmlElement("Account")]
        public Account Entity
        {
            get { return base.Entity; }
            set { base.Entity = value; }
        }
    }
}
