using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects.EntityReferences
{
    public class RoleReference
    {
        [XmlIgnore]
        public Role Role { get; set; }
    }
}
