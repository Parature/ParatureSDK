using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParatureSDK.ParaObjects.EntityReferences
{
    public class ProductFolderReference : EntityReference<ProductFolder>
    {
        public ProductFolder ProductFolder
        {
            get { return base.Entity; }
            set { base.Entity = value; }
        }
    }
}
