using System.Collections.Generic;

namespace ParatureSDK.ParaObjects
{
    public class ParaEntityList<T> : PagedData.PagedData
    {
        public List<T> Data = new List<T>();
    }
}
