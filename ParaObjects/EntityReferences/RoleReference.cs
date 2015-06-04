using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects.EntityReferences
{
    public class RoleReference : EntityReference<Role>
    {
        [XmlIgnore]
        public Role Role
        {
            get { return base.Entity; }
            set { base.Entity = value; }
        }
    }
}
