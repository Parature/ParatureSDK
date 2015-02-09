using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects.EntityReferences
{
    class CustomerRoleReference: RoleReference
    {
        [XmlElement("CustomerRole")]
        public Role Role { 
            get { return base.Role; }
            set { base.Role = value; } 
        }
    }
}
