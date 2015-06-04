using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParatureSDK.ParaObjects.EntityReferences
{
    public class DepartmentReference : EntityReference<Department>
    {
        public Department Department
        {
            get { return base.Entity; }
            set { base.Entity = value; }
        }
    }
}
