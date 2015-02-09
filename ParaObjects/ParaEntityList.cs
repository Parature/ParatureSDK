using System.Collections.Generic;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects
{
    [XmlRoot("Entities")]
    public class ParaEntityList<T> : PagedData.PagedData
    {
        public static string ElemName = typeof (T).Name;

        public List<T> Data = new List<T>();
    }
}
