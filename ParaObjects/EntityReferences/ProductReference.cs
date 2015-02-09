using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParatureSDK.ParaObjects.EntityReferences
{
    public class ProductReference: EntityReference<Product>
    {
        public Product Entity
        {
            get { return base.Entity; }
            set { base.Entity = value; }
        }
    }
}
