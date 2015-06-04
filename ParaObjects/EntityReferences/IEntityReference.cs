using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParatureSDK.ParaObjects.EntityReferences
{
    public interface IEntityReference
    {
        ParaEntityBaseProperties GetEntity();
    }
}
