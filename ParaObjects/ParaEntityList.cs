using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParatureAPI.ParaObjects
{
    public class ParaEntityList<T> : PagedData.PagedData
    {
        public List<T> Data = new List<T>();
    }
}
